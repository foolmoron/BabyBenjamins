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
    struct SetZToZeroJob : IJobForEachWithEntity<Translation> {

        public void Execute(Entity entity, int index, ref Translation targetPos) {
            targetPos.Value.z = index * 0.0001f;
        }
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDependencies) {
        var job = new SetZToZeroJob {
        }.Run(this, inputDependencies);
        return job;
    }
}