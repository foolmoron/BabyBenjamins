using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using static Unity.Mathematics.math;
using quaternion = Unity.Mathematics.quaternion;

public class ForceToZeroSystem : JobComponentSystem
{
    [BurstCompile]
    [RequireComponentTag(typeof(GravityTarget))]
    struct ForceToZeroJob : IJobForEach<Translation, PhysicsVelocity> {

        public float dt;

        public void Execute([ReadOnly]ref Translation targetPos, ref PhysicsVelocity targetVel) {
            var accelToZero = -targetPos.Value.z;
            targetVel.Linear.z += accelToZero * dt;
        }
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDependencies) {
        var job = new ForceToZeroJob {
            dt = UnityEngine.Time.deltaTime,
        }.Run(this, inputDependencies);
        return job;
    }
}