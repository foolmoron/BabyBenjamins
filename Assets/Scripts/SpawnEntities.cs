﻿using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnEntities : MonoBehaviour {

  public GameObject Prefab;
  public int Count = 1;
  public Vector3 PositionMin;
  public Vector3 PositionMax;
  public Vector3 RotationMin;
  public Vector3 RotationMax;

  Entity prefab;

  void Start() {
    prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(Prefab, World.Active);
  }

  void FixedUpdate() {
    var em = World.Active.EntityManager;
    for (int i = 0; i < 1; i++) {
      var e = em.Instantiate(prefab);
      em.SetComponentData(e, new Translation { Value = new Vector3(Mathf.Lerp(PositionMin.x, PositionMax.x, Random.value), Mathf.Lerp(PositionMin.y, PositionMax.y, Random.value), Mathf.Lerp(PositionMin.z, PositionMax.z, Random.value)) });
      em.SetComponentData(e, new Rotation { Value = Quaternion.Euler(Mathf.Lerp(RotationMin.x, RotationMax.x, Random.value), Mathf.Lerp(RotationMin.y, RotationMax.y, Random.value), Mathf.Lerp(RotationMin.z, RotationMax.z, Random.value)) });
    }
  }
}