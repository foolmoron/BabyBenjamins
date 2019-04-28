using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using static Unity.Mathematics.math;
using quaternion = Unity.Mathematics.quaternion;

[Serializable]
public struct HasValue : IComponentData {
    public long Value;
}

public class CountValueSystem : JobComponentSystem
{
    [BurstCompile]
    struct CountValueJob : IJob {

        [DeallocateOnJobCompletion]
        [ReadOnly] public NativeArray<Entity> Entities;
        [ReadOnly] public ComponentDataFromEntity<HasValue> HasValues;
        public NativeArray<long> Results;
        
        public void Execute() {
            Results[0] = 0;
            for (int i = 0; i < Entities.Length; i++) {
                Results[0] += HasValues[Entities[i]].Value;
            }
        }
    }

    public static event Action<long> OnNewCount = delegate { };
    
    protected override JobHandle OnUpdate(JobHandle inputDependencies) {
        var entities = GetEntityQuery(ComponentType.ReadOnly<HasValue>()).ToEntityArray(Allocator.TempJob, out var queryJob);
        queryJob.Complete();
        var results = new NativeArray<long>(1, Allocator.TempJob);
        var hasValues = GetComponentDataFromEntity<HasValue>(true);
        new CountValueJob {
            Entities = entities,
            HasValues = hasValues,
            Results = results,
        }.Schedule(inputDependencies).Complete();
        var count = results[0];
        OnNewCount(count);
        results.Dispose();
        return inputDependencies;
    }
}