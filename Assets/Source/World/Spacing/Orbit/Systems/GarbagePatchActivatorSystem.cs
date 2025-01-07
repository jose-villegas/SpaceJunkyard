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
                var freeSpaces = CreateFreeSpacesCollection(ref pair.Value);
            }

            RemoveRequest(ref state);
        }

        private NativeList<NativeList<int>> CreateFreeSpacesCollection(ref NativeArray<RefRW<GarbagePatch>> patches)
        {
            var freeSpaces = new NativeList<NativeList<int>>(1, Allocator.Temp);
            
            for (var index = 0; index < patches.Length; index++)
            {
                var currentPatch = patches[index].ValueRO;
                var config = currentPatch.PatchedOrbitConfiguration;

                // register first free space entry
                if (!currentPatch.IsOccupied)
                {
                    if (freeSpaces.Length == 0)
                    {
                        freeSpaces.Add(new NativeList<int>(config.PatchCount, Allocator.Temp));
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
                            freeSpaces.Add(new NativeList<int>(config.PatchCount, Allocator.Temp));
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