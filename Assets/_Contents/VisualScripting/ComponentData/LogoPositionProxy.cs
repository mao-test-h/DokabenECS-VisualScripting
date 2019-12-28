using System;
using System.ComponentModel;
using Unity.Entities;
using UnityEngine;
using VisualScripting.Entities.Runtime;
using System.Collections.Generic;
[Serializable, ComponentEditor]
public struct LogoPosition : IComponentData
{
    [HideInInspector]
    public Unity.Mathematics.float3 Value;
}

[AddComponentMenu("Visual Scripting Components/LogoPosition")]
class LogoPositionProxy : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    [HideInInspector]
    public Unity.Mathematics.float3 Value;

    public void Convert(Unity.Entities.Entity entity, Unity.Entities.EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new LogoPosition { Value = Value });
    }

    public void DeclareReferencedPrefabs(List<UnityEngine.GameObject> referencedPrefabs)
    {
    }
}