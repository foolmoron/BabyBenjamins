using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

[BurstCompile]
struct DisposeArrayJob : IJob {
    [ReadOnly]
    [DeallocateOnJobCompletion]
    public NativeArray<Entity> Array;

    public void Execute() {
    }
}