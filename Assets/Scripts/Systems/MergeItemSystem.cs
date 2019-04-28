﻿using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

[Serializable]
public struct ItemCoin1 : IComponentData {
}
[Serializable]
public struct ItemCoin5 : IComponentData {
}
[Serializable]
public struct ItemCoin25 : IComponentData {
}
[Serializable]
public struct ItemBill1 : IComponentData {
}
[Serializable]
public struct ItemBill20 : IComponentData {
}
[Serializable]
public struct ItemBill100 : IComponentData {
}
[Serializable]
public struct ItemCert1K : IComponentData {
}
[Serializable]
public struct ItemCert10K : IComponentData {
}
[Serializable]
public struct ItemCert100K : IComponentData {
}
[Serializable]
public struct ItemCert1M : IComponentData {
}
[Serializable]
public struct ItemCert10M : IComponentData {
}
[Serializable]
public struct ItemCert100M : IComponentData {
}
[Serializable]
public struct ItemCert1B : IComponentData {
}
[Serializable]
public struct ItemCert10B : IComponentData {
}
[Serializable]
public struct ItemCert100B : IComponentData {
}
[Serializable]
public struct ItemGold : IComponentData {
}

public class MergeItemSystem : JobComponentSystem
{
    public static Dictionary<Type, Entity> NextItemMap = new Dictionary<Type, Entity>();

    [BurstCompile]
    struct MergeCoin1Job : IJobForEachWithEntity<ItemCoin1, Translation> {

        [ReadOnly] public float MergeDistance;
        [ReadOnly] public Entity MergingEntity;
        [ReadOnly] public float3 MergingPos;
        public NativeArray<Entity> MergeResults;

        public void Execute(Entity entity, int index, [ReadOnly]ref ItemCoin1 item, [ReadOnly]ref Translation itemPos) {
            if (entity != MergingEntity && math.length(itemPos.Value - MergingPos) <= MergeDistance) {
                MergeResults[0] = entity;
            }
        }
    }
    [BurstCompile]
    struct MergeCoin5Job : IJobForEachWithEntity<ItemCoin5, Translation> {

        [ReadOnly] public float MergeDistance;
        [ReadOnly] public Entity MergingEntity;
        [ReadOnly] public float3 MergingPos;
        public NativeArray<Entity> MergeResults;

        public void Execute(Entity entity, int index, [ReadOnly]ref ItemCoin5 item, [ReadOnly]ref Translation itemPos) {
            if (entity != MergingEntity && math.length(itemPos.Value - MergingPos) <= MergeDistance) {
                MergeResults[0] = entity;
            }
        }
    }
    [BurstCompile]
    struct MergeCoin25Job : IJobForEachWithEntity<ItemCoin25, Translation> {

        [ReadOnly] public float MergeDistance;
        [ReadOnly] public Entity MergingEntity;
        [ReadOnly] public float3 MergingPos;
        public NativeArray<Entity> MergeResults;

        public void Execute(Entity entity, int index, [ReadOnly]ref ItemCoin25 item, [ReadOnly]ref Translation itemPos) {
            if (entity != MergingEntity && math.length(itemPos.Value - MergingPos) <= MergeDistance) {
                MergeResults[0] = entity;
            }
        }
    }
    [BurstCompile]
    struct MergeBill1Job : IJobForEachWithEntity<ItemBill1, Translation> {

        [ReadOnly] public float MergeDistance;
        [ReadOnly] public Entity MergingEntity;
        [ReadOnly] public float3 MergingPos;
        public NativeArray<Entity> MergeResults;

        public void Execute(Entity entity, int index, [ReadOnly]ref ItemBill1 item, [ReadOnly]ref Translation itemPos) {
            if (entity != MergingEntity && math.length(itemPos.Value - MergingPos) <= MergeDistance) {
                MergeResults[0] = entity;
            }
        }
    }

    BeginInitializationEntityCommandBufferSystem commandBufferSystem;
    Entity mergingEntity;
    bool merging;
    
    protected override void OnCreate() {
        commandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        MouseDragSystem.OnDragStart += (entity, system) => {
            mergingEntity = entity;
            merging = true;
        };
        MouseDragSystem.OnDragEnd += (entity, system) => {
            merging = false;
        };
    }

    void ProcessMergeJob<TItem>(JobHandle mergeJob, NativeArray<Entity> results) {
        mergeJob.Complete();
        var entityToMerge = results[0];
        if (EntityManager.Exists(entityToMerge)) {
            var mergedEntity = EntityManager.Instantiate(NextItemMap[typeof(TItem)]);
            EntityManager.SetComponentData(mergedEntity, new Translation { Value = EntityManager.GetComponentData<Translation>(mergingEntity).Value });
            EntityManager.DestroyEntity(mergingEntity);
            EntityManager.DestroyEntity(entityToMerge);
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies) {
        if (!merging) {
            return inputDependencies;
        }
        var handle = inputDependencies;
        using (var results = new NativeArray<Entity>(1, Allocator.TempJob)) {
            if (EntityManager.HasComponent<ItemCoin1>(mergingEntity)) {
                handle = new MergeCoin1Job {
                    MergeDistance = 0.3f,
                    MergingEntity = mergingEntity,
                    MergingPos = EntityManager.GetComponentData<Translation>(mergingEntity).Value,
                    MergeResults = results,
                }.ScheduleSingle(this, inputDependencies);
                ProcessMergeJob<ItemCoin1>(handle, results);
            } else if (EntityManager.HasComponent<ItemCoin5>(mergingEntity)) {
                handle = new MergeCoin5Job {
                    MergeDistance = 0.4f,
                    MergingEntity = mergingEntity,
                    MergingPos = EntityManager.GetComponentData<Translation>(mergingEntity).Value,
                    MergeResults = results,
                }.ScheduleSingle(this, inputDependencies);
                ProcessMergeJob<ItemCoin5>(handle, results);
            } else if (EntityManager.HasComponent<ItemCoin25>(mergingEntity)) {
                handle = new MergeCoin25Job {
                    MergeDistance = 0.5f,
                    MergingEntity = mergingEntity,
                    MergingPos = EntityManager.GetComponentData<Translation>(mergingEntity).Value,
                    MergeResults = results,
                }.ScheduleSingle(this, inputDependencies);
                ProcessMergeJob<ItemCoin25>(handle, results);
            } else if (EntityManager.HasComponent<ItemBill1>(mergingEntity)) {
                handle = new MergeBill1Job {
                    MergeDistance = 0.75f,
                    MergingEntity = mergingEntity,
                    MergingPos = EntityManager.GetComponentData<Translation>(mergingEntity).Value,
                    MergeResults = results,
                }.ScheduleSingle(this, inputDependencies);
                ProcessMergeJob<ItemBill1>(handle, results);
            } else if (EntityManager.HasComponent<ItemBill20>(mergingEntity)) {

            } else if (EntityManager.HasComponent<ItemBill100>(mergingEntity)) {

            } else if (EntityManager.HasComponent<ItemCert1K>(mergingEntity)) {

            } else if (EntityManager.HasComponent<ItemCert10K>(mergingEntity)) {

            } else if (EntityManager.HasComponent<ItemCert100K>(mergingEntity)) {

            } else if (EntityManager.HasComponent<ItemCert1M>(mergingEntity)) {

            } else if (EntityManager.HasComponent<ItemCert10M>(mergingEntity)) {

            } else if (EntityManager.HasComponent<ItemCert100M>(mergingEntity)) {

            } else if (EntityManager.HasComponent<ItemCert1B>(mergingEntity)) {

            } else if (EntityManager.HasComponent<ItemCert10B>(mergingEntity)) {

            } else if (EntityManager.HasComponent<ItemCert100B>(mergingEntity)) {

            }
            commandBufferSystem.AddJobHandleForProducer(handle);
        }
        return handle;
    }
}