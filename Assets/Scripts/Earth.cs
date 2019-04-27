using Unity.Entities;
using UnityEngine;

public class Earth : MonoBehaviour, IConvertGameObjectToEntity {

    [Range(0, 100)]
    public float GravityAcceleration = 10;
    public float MinRadius = 1;
    public float MaxRadius = 10;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        dstManager.AddComponentData(entity, new GravitySource { Acceleration = GravityAcceleration, MinRadius = MinRadius, MaxRadius = MaxRadius, });
    }
}
