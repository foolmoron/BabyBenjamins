using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using static Unity.Mathematics.math;
using quaternion = Unity.Mathematics.quaternion;

[Serializable]
public struct ForceToPosition : IComponentData {
    public float3 Position;
    public float SpeedModifier;
}

public class ForceToPositionSystem : JobComponentSystem
{
    [BurstCompile]
    struct ForceToPositionJob : IJobForEach<Translation, ForceToPosition, PhysicsVelocity> {
        
        public void Execute([ReadOnly]ref Translation targetPos, [ReadOnly]ref ForceToPosition forceToPosition, ref PhysicsVelocity targetVel) {
            var vectorToMouse = (forceToPosition.Position - targetPos.Value).xy;
            targetVel.Linear = new float3(vectorToMouse * forceToPosition.SpeedModifier, 0);
        }
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDependencies) {
        var job = new ForceToPositionJob {
        }.Run(this, inputDependencies);
        return job;
    }
}