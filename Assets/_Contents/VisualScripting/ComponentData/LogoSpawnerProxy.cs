using System;
using System.ComponentModel;
using Unity.Entities;
using UnityEngine;
using VisualScripting.Entities.Runtime;
using System.Collections.Generic;
[Serializable, ComponentEditor]
public struct LogoSpawner : IComponentData
{
    public Unity.Entities.Entity PrefabEntity;
    public int InstanceCount;
}

[AddComponentMenu("Visual Scripting Components/LogoSpawner")]
class LogoSpawnerProxy : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    public UnityEngine.GameObject PrefabEntity;
    public int InstanceCount;

    public void Convert(Unity.Entities.Entity entity, Unity.Entities.EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new LogoSpawner { PrefabEntity = conversionSystem.GetPrimaryEntity(PrefabEntity), InstanceCount = InstanceCount });
    }

    public void DeclareReferencedPrefabs(List<UnityEngine.GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(PrefabEntity);
    }
}