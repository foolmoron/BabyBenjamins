using Unity.Entities;
using UnityEngine;

public class ItemManager : Manager<ItemManager> {

    public GameObject ItemCoin1Prefab;
    public GameObject ItemCoin5Prefab;
    public GameObject ItemCoin25Prefab;
    public GameObject ItemBill1Prefab;
    public GameObject ItemBill20Prefab;
    public GameObject ItemBill100Prefab;
    public GameObject ItemCert1KPrefab;
    public GameObject ItemCert10KPrefab;
    public GameObject ItemCert100KPrefab;
    public GameObject ItemCert1MPrefab;
    public GameObject ItemCert10MPrefab;
    public GameObject ItemCert100MPrefab;
    public GameObject ItemCert1BPrefab;
    public GameObject ItemCert10BPrefab;
    public GameObject ItemCert100BPrefab;
    public GameObject ItemGoldPrefab;

    public Entity ItemCoin1Entity;
    public Entity ItemCoin5Entity;
    public Entity ItemCoin25Entity;
    public Entity ItemBill1Entity;
    public Entity ItemBill20Entity;
    public Entity ItemBill100Entity;
    public Entity ItemCert1KEntity;
    public Entity ItemCert10KEntity;
    public Entity ItemCert100KEntity;
    public Entity ItemCert1MEntity;
    public Entity ItemCert10MEntity;
    public Entity ItemCert100MEntity;
    public Entity ItemCert1BEntity;
    public Entity ItemCert10BEntity;
    public Entity ItemCert100BEntity;
    public Entity ItemGoldEntity;

    public void Awake() {
        var map = MergeItemSystem.NextItemMap;
        ItemCoin1Entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(ItemCoin1Prefab, World.Active);
        ItemCoin5Entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(ItemCoin5Prefab, World.Active);
        ItemCoin25Entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(ItemCoin25Prefab, World.Active);
        ItemBill1Entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(ItemBill1Prefab, World.Active);
        ItemBill20Entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(ItemBill20Prefab, World.Active);
        ItemBill100Entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(ItemBill100Prefab, World.Active);
        ItemCert1KEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(ItemCert1KPrefab, World.Active);
        ItemCert10KEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(ItemCert10KPrefab, World.Active);
        ItemCert100KEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(ItemCert100KPrefab, World.Active);
        ItemCert1MEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(ItemCert1MPrefab, World.Active);
        ItemCert10MEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(ItemCert10MPrefab, World.Active);
        ItemCert100MEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(ItemCert100MPrefab, World.Active);
        ItemCert1BEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(ItemCert1BPrefab, World.Active);
        ItemCert10BEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(ItemCert10BPrefab, World.Active);
        ItemCert100BEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(ItemCert100BPrefab, World.Active);
        ItemGoldEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(ItemGoldPrefab, World.Active);
        World.Active.EntityManager.AddComponentData(ItemCoin1Entity, new ItemCoin1());
        World.Active.EntityManager.AddComponentData(ItemCoin5Entity, new ItemCoin5());
        World.Active.EntityManager.AddComponentData(ItemCoin25Entity, new ItemCoin25());
        World.Active.EntityManager.AddComponentData(ItemBill1Entity, new ItemBill1());
        World.Active.EntityManager.AddComponentData(ItemBill20Entity, new ItemBill20());
        World.Active.EntityManager.AddComponentData(ItemBill100Entity, new ItemBill100());
        World.Active.EntityManager.AddComponentData(ItemCert1KEntity, new ItemCert1K());
        World.Active.EntityManager.AddComponentData(ItemCert10KEntity, new ItemCert10K());
        World.Active.EntityManager.AddComponentData(ItemCert100KEntity, new ItemCert100K());
        World.Active.EntityManager.AddComponentData(ItemCert1MEntity, new ItemCert1M());
        World.Active.EntityManager.AddComponentData(ItemCert10MEntity, new ItemCert10M());
        World.Active.EntityManager.AddComponentData(ItemCert100MEntity, new ItemCert100M());
        World.Active.EntityManager.AddComponentData(ItemCert1BEntity, new ItemCert1B());
        World.Active.EntityManager.AddComponentData(ItemCert10BEntity, new ItemCert10B());
        World.Active.EntityManager.AddComponentData(ItemCert100BEntity, new ItemCert100B());
        World.Active.EntityManager.AddComponentData(ItemGoldEntity, new ItemGold());
        map[typeof(ItemCoin1)] = ItemCoin5Entity;
        map[typeof(ItemCoin5)] = ItemCoin25Entity;
        map[typeof(ItemCoin25)] = ItemBill1Entity;
        map[typeof(ItemBill1)] = ItemBill20Entity;
        map[typeof(ItemBill20)] = ItemBill100Entity;
        map[typeof(ItemBill100)] = ItemCert1KEntity;
        map[typeof(ItemCert1K)] = ItemCert10KEntity;
        map[typeof(ItemCert10K)] = ItemCert100KEntity;
        map[typeof(ItemCert100K)] = ItemCert1MEntity;
        map[typeof(ItemCert1M)] = ItemCert10MEntity;
        map[typeof(ItemCert10M)] = ItemCert100MEntity;
        map[typeof(ItemCert100M)] = ItemCert1BEntity;
        map[typeof(ItemCert1B)] = ItemCert10BEntity;
        map[typeof(ItemCert10B)] = ItemCert100BEntity;
    }
}
