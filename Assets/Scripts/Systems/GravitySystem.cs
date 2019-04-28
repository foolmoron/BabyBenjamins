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
public struct GravitySource : IComponentData {
    public float Acceleration;
    public float MinRadius;
    public float MaxRadius;
}

[Serializable]
public struct GravityTarget : IComponentData {

}

public class GravitySystem : JobComponentSystem
{
    [BurstCompile]
    struct GravitySystemJob : IJobForEach<GravityTarget, Translation, PhysicsVelocity> {

        public float dt;
        [ReadOnly] public GravitySource Source;
        [ReadOnly] public Translation SourcePos;

        public void Execute([ReadOnly]ref GravityTarget target, [ReadOnly]ref Translation targetPos, ref PhysicsVelocity targetVel) {
            var vectorToTarget = targetPos.Value - SourcePos.Value;
            var distToTarget = length(vectorToTarget);
            var dirToTarget = normalize(vectorToTarget);
            var force = -dirToTarget * Source.Acceleration * 1 / distToTarget;
            var distFactor = sign(max(0, distToTarget - Source.MinRadius)) + max(0, distToTarget - Source.MaxRadius) * max(0, distToTarget - Source.MaxRadius);
            targetVel.Linear += force * dt * distFactor;
        }
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDependencies) {
        var gravitySources = GetEntityQuery(ComponentType.ReadOnly<GravitySource>(), ComponentType.ReadOnly<Translation>()).ToEntityArray(Allocator.TempJob, out var queryJob);
        queryJob.Complete();

        var gravityJobs = new NativeArray<JobHandle>(gravitySources.Length, Allocator.Temp);
        for (int i = 0; i < gravityJobs.Length; i++) {
            gravityJobs[i] = new GravitySystemJob {
                dt = UnityEngine.Time.deltaTime,
                Source = EntityManager.GetComponentData<GravitySource>(gravitySources[i]),
                SourcePos = EntityManager.GetComponentData<Translation>(gravitySources[i]),
            }.Run(this, inputDependencies);
        }
        var gravityJob = JobHandle.CombineDependencies(gravityJobs);

        var disposeJob = new DisposeArrayJob { Array = gravitySources }.Schedule(gravityJob);

        return disposeJob;
    }
}