using Microsoft.CSharp;
using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class DokabenInitializerSystem : ComponentSystem
{
    private Unity.Entities.EndSimulationEntityCommandBufferSystem m_EndFrameBarrier;
    private Unity.Entities.EntityQuery SpawnerQueryEnter;
    private Unity.Entities.EntityQuery SpawnerQueryTrackingQuery;
    public struct SpawnerQueryTracking : Unity.Entities.ISystemStateComponentData
    {
    }

    public struct GraphData : Unity.Entities.IComponentData
    {
        public Unity.Mathematics.float2 range;
    }

    protected override void OnCreate()
    {
        m_EndFrameBarrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        SpawnerQueryEnter = GetEntityQuery(
            ComponentType.ReadWrite<LogoSpawner>(),
            ComponentType.Exclude<SpawnerQueryTracking>());
        SpawnerQueryTrackingQuery = GetEntityQuery(ComponentType.ReadOnly<SpawnerQueryTracking>());
        EntityManager.CreateEntity(typeof(GraphData));
        SetSingleton(new GraphData { range = new float2(128F, -128F) });
    }

    protected override void OnUpdate()
    {
        Unity.Mathematics.Random rng = new Unity.Mathematics.Random();
        GraphData graphData = GetSingleton<GraphData>();
        {
            Entities.With(SpawnerQueryEnter).ForEach((
                Unity.Entities.Entity SpawnerQueryEntity,
                ref LogoSpawner SpawnerQueryEnterLogoSpawner) =>
            {
                rng.InitState((uint)Time.frameCount * 3889 ^ 1851936439 ^ (uint)SpawnerQueryEntity.Index * 7907);
                rng.InitState((uint)Time.frameCount * 3889 ^ 1851936439 ^ (uint)SpawnerQueryEntity.Index * 7907);
                rng.InitState((uint)Time.frameCount * 3889 ^ 1851936439 ^ (uint)SpawnerQueryEntity.Index * 7907);
                int Index = 0;
                for (; (Index < SpawnerQueryEnterLogoSpawner.InstanceCount); Index++)
                {
                    Unity.Entities.Entity entity = PostUpdateCommands.Instantiate(
                        SpawnerQueryEnterLogoSpawner.PrefabEntity);
                    PostUpdateCommands.SetComponent<LogoPosition>(
                        entity,
                        new LogoPosition { Value = new Unity.Mathematics.float3 { x = rng.NextFloat(
                            graphData.range.x,
                            graphData.range.y), y = rng.NextFloat(
                            graphData.range.x,
                            graphData.range.y), z = rng.NextFloat(
                            graphData.range.x,
                            graphData.range.y) } });
                }
            }

            );
        }

        PostUpdateCommands.AddComponent(SpawnerQueryEnter, typeof(SpawnerQueryTracking));
    }
}