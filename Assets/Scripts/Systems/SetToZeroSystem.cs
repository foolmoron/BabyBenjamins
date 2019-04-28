using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using static Unity.Mathematics.math;
using quaternion = Unity.Mathematics.quaternion;

public class SetToZeroSystem : JobComponentSystem
{
    [BurstCompile]
    [RequireComponentTag(typeof(GravityTarget))]
    struct SetZToZeroJob : IJobForEach<Translation> {

        public void Execute(ref Translation targetPos) {
            targetPos.Value.z = 0;
        }
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDependencies) {
        var job = new SetZToZeroJob {
        }.Run(this, inputDependencies);
        return job;
    }
}