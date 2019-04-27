using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct GravitySource : IComponentData {
    public float Acceleration;
    public float MinRadius;
    public float MaxRadius;
}
