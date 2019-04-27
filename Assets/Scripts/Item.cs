using Unity.Entities;
using UnityEngine;

public class Item : MonoBehaviour, IConvertGameObjectToEntity {
    
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        dstManager.AddComponentData(entity, new GravityTarget { });
    }
}
