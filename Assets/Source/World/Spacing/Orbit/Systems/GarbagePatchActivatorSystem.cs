using System.IO;
using SpaceJunkyard.World.Astronomical;
using Unity.Collections;
using Unity.Entities;

namespace SpaceJunkyard.World.Spacing
{
    /// <summary>
    /// Takes responsibility over activation of garbage patches to enable spawning, also
    /// controls spacing logic between activated patches.
    /// </summary>
    public partial struct GarbagePatchActivatorSystem : ISystem
    {
        private NativeHashMap<Entity, NativeArray<RefRW<GarbagePatch>>> _patchesControl;

        public void OnCreate(ref SystemState state)
        {
            var configuration = SystemAPI.QueryBuilder().WithAll<RequestGarbagePatchActivatorCheck>()
                .Build();
            var spawners = SystemAPI.QueryBuilder().WithAll<AstronomicalBody>().Build();

            // initialize control map
            _patchesControl =
                new NativeHashMap<Entity, NativeArray<RefRW<GarbagePatch>>>(spawners.CalculateEntityCount(),
                    Allocator.Persistent);

            state.RequireForUpdate(configuration);
        }

        public void OnUpdate(ref SystemState state)
        {
            foreach (var garbagePatch in SystemAPI.Query<RefRW<GarbagePatch>>())
            {
                var gbPatch = garbagePatch.ValueRO;

                if (_patchesControl.ContainsKey(gbPatch.OrbitableBody))
                {
                    var controlArray = _patchesControl[gbPatch.OrbitableBody];
                    controlArray[gbPatch.PatchIndex] = garbagePatch;
                }
                else
                {
                    var controlArray = new NativeArray<RefRW<GarbagePatch>>(
                        gbPatch.PatchedOrbitConfiguration.PatchCount,
                        Allocator.Persistent);
                    controlArray[gbPatch.PatchIndex] = garbagePatch;
                    _patchesControl.Add(gbPatch.OrbitableBody, controlArray);
                }
            }

            foreach (var pair in _patchesControl)
            {
                if (pair.Value.Length == 0) continue;

                var configuration = pair.Value[0].ValueRO.PatchedOrbitConfiguration;
                var freeSpaces = CreateFreeSpacesCollection(ref pair.Value, configuration.PatchCount);

                // activate patches within contiguous spaces
                ActivateGarbagePatchesSpacing(ref freeSpaces, configuration.PerPatchOccupation, ref pair.Value);
            }

            RemoveRequest(ref state);
        }

        private void ActivateGarbagePatchesSpacing(ref NativeList<NativeList<int>> freeSpaces, int perPatchOccupation,
            ref NativeArray<RefRW<GarbagePatch>> garbagePatches)
        {
            foreach (var freeSpace in freeSpaces)
            {
                // no spaces
                if (freeSpace.Length == 0) continue;

                // available space isn't enough
                if (freeSpace.Length < perPatchOccupation) continue;

                // how many activations can fit within this section
                var possibleActivations = freeSpace.Length / perPatchOccupation;
                    
                // select a random number of activations
                var activations = UnityEngine.Random.Range(1, possibleActivations + 1);
                var emptySlots = freeSpace.Length - (activations * perPatchOccupation);
                var shiftIndex = 0;

                for (int j = 0; j < activations && shiftIndex < freeSpace.Length; j++)
                {
                    if (emptySlots > 0)
                    {
                        shiftIndex += UnityEngine.Random.Range(0, emptySlots);
                        emptySlots -= shiftIndex;
                    }
                        
                    // index to activate
                    var patchIndex = freeSpace[shiftIndex];
                    var patch = garbagePatches[patchIndex];
                        
                    // set origin  as active and the rest as occupied
                    patch.ValueRW.IsActive = true;

                    for (int k = 0; k < perPatchOccupation; k++)
                    {
                        garbagePatches[patchIndex + k].ValueRW.IsOccupied = true;
                    }
                        
                    // move index forward
                    shiftIndex += perPatchOccupation;
                }
            }
        }

        private NativeList<NativeList<int>> CreateFreeSpacesCollection(ref NativeArray<RefRW<GarbagePatch>> patches,
            int patchCount)
        {
            var freeSpaces = new NativeList<NativeList<int>>(1, Allocator.Temp);

            for (var index = 0; index < patches.Length; index++)
            {
                var currentPatch = patches[index].ValueRO;

                // register first free space entry
                if (!currentPatch.IsOccupied)
                {
                    if (freeSpaces.Length == 0)
                    {
                        freeSpaces.Add(new NativeList<int>(patchCount, Allocator.Temp));
                        freeSpaces[freeSpaces.Length - 1].Add(currentPatch.PatchIndex);
                    }
                    else
                    {
                        // get last collection of free spaces
                        var lastFreeSpace = freeSpaces[freeSpaces.Length - 1];
                        // check if we are next to this index
                        var previousIndex = lastFreeSpace[lastFreeSpace.Length - 1];

                        // we are next to each other, add our index
                        if (currentPatch.PatchIndex - previousIndex == 1)
                        {
                            lastFreeSpace.Add(currentPatch.PatchIndex);
                        }
                        else // we need to create a new collection of contiguity
                        {
                            freeSpaces.Add(new NativeList<int>(patchCount, Allocator.Temp));
                            freeSpaces[freeSpaces.Length - 1].Add(currentPatch.PatchIndex);
                        }
                    }
                }
            }

            return freeSpaces;
        }

        private void RemoveRequest(ref SystemState state)
        {
            var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (request, entity) in SystemAPI.Query<RefRO<RequestGarbagePatchActivatorCheck>>()
                         .WithEntityAccess())
            {
                // remove request
                entityCommandBuffer.RemoveComponent<RequestGarbagePatchActivatorCheck>(entity);
            }

            entityCommandBuffer.Playback(state.EntityManager);
        }
    }
}