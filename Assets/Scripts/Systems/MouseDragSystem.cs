using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;

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

    public static event Action<Entity, MouseDragSystem> OnDragStart = delegate { };
    public static event Action<Entity, MouseDragSystem> OnDragEnd = delegate { };

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
        if (!EntityManager.Exists(draggedEntity)) {
            dragging = false;
        }
        if (UnityEngine.Input.GetMouseButton(0) && !dragging) {
            var results = new NativeArray<RaycastHit>(1, Allocator.TempJob);
            new MouseRaycastJob {
                CollisionWorld = buildPhysicsSystem.PhysicsWorld.CollisionWorld,
                RaycastInput = new RaycastInput {
                    Ray = new Ray(camera.ScreenToWorldPoint(UnityEngine.Input.mousePosition).withZ(-25), new float3(0, 0, 100)),
                    Filter = new CollisionFilter { MaskBits = 1 << 0, CategoryBits = ~0u },
                },
                Results = results,
            }.Schedule(JobHandle.CombineDependencies(buildPhysicsSystem.FinalJobHandle, inputDependencies)).Complete();
            if (results[0].Fraction > 0 && results[0].RigidBodyIndex >= 0 && results[0].RigidBodyIndex < buildPhysicsSystem.PhysicsWorld.CollisionWorld.Bodies.Length) {
                draggedEntity = buildPhysicsSystem.PhysicsWorld.CollisionWorld.Bodies[results[0].RigidBodyIndex].Entity;
                if (EntityManager.Exists(draggedEntity)) {
                    EntityManager.AddComponentData(draggedEntity, new ForceToPosition {SpeedModifier = 10});
                    dragging = true;
                    OnDragStart(draggedEntity, this);
                }
            }
            results.Dispose();
        } else if (UnityEngine.Input.GetMouseButton(0)) {
            var c = EntityManager.GetComponentData<ForceToPosition>(draggedEntity);
            c.Position = camera.ScreenToWorldPoint(UnityEngine.Input.mousePosition).withZ(0); // force to Z 0
            EntityManager.SetComponentData(draggedEntity, c);
        } else if (dragging) {
            EntityManager.RemoveComponent<ForceToPosition>(draggedEntity);
            dragging = false;
            OnDragEnd(draggedEntity, this);
        }
        return inputDependencies;
    }
}