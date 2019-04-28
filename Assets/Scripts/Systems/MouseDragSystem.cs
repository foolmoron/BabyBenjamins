using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;

[Serializable]
public struct CanMouseDrag : IComponentData {
}

public class MouseDragSystem : JobComponentSystem
{
    [BurstCompile]
    struct MouseRaycastJob : IJob {
        
        [ReadOnly] public CollisionWorld CollisionWorld;
        [ReadOnly] public RaycastInput RaycastInput;
        public NativeArray<RaycastHit> Results;
        
        public void Execute() {
            CollisionWorld.CastRay(RaycastInput, out var hit);
            Results[0] = hit;
        }
    }

    Entity draggedEntity;
    bool dragging;

    UnityEngine.Camera camera;
    BuildPhysicsWorld buildPhysicsSystem;

    protected override void OnCreate() {
        buildPhysicsSystem = World.GetOrCreateSystem<BuildPhysicsWorld>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies) {
        if (!camera) {
            camera = UnityEngine.Camera.main;
        }
        if (!camera) {
            return inputDependencies;
        }
        if (UnityEngine.Input.GetMouseButton(0) && !dragging) {
            var results = new NativeArray<RaycastHit>(1, Allocator.TempJob);
            var job = new MouseRaycastJob {
                CollisionWorld = buildPhysicsSystem.PhysicsWorld.CollisionWorld,
                RaycastInput = new RaycastInput {
                    Ray = new Ray(camera.ScreenToWorldPoint(UnityEngine.Input.mousePosition).withZ(-25), new float3(0, 0, 100)),
                    Filter = new CollisionFilter { MaskBits = 1 << 0, CategoryBits = ~0u },
                },
                Results = results,
            };
            job.Schedule(JobHandle.CombineDependencies(buildPhysicsSystem.FinalJobHandle, inputDependencies)).Complete();
            if (results[0].Fraction > 0 && results[0].RigidBodyIndex < buildPhysicsSystem.PhysicsWorld.CollisionWorld.Bodies.Length) {
                draggedEntity = buildPhysicsSystem.PhysicsWorld.CollisionWorld.Bodies[results[0].RigidBodyIndex].Entity;
                EntityManager.AddComponentData(draggedEntity, new ForceToPosition { SpeedModifier = 10 });
                dragging = true;
                UnityEngine.Debug.Log(results[0].RigidBodyIndex + " - " + results[0].Position);
            }
            results.Dispose();
        } else if (UnityEngine.Input.GetMouseButton(0)) {
            var c = EntityManager.GetComponentData<ForceToPosition>(draggedEntity);
            c.Position = camera.ScreenToWorldPoint(UnityEngine.Input.mousePosition).withZ(0); // force to Z 0
            EntityManager.SetComponentData(draggedEntity, c);
        } else if (dragging) {
            EntityManager.RemoveComponent<ForceToPosition>(draggedEntity);
            dragging = false;
        }
        return inputDependencies;
    }
}