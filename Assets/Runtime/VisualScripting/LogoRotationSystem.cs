using Microsoft.CSharp;
using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using VisualScripting.CustomNode;

public class LogoRotationSystem : JobComponentSystem
{
    private Unity.Entities.EntityQuery LogoQuery;
    [BurstCompile]
    struct Update_LogoQuery_Job : IJobForEachWithEntity_ECC<Unity.Transforms.LocalToWorld, LogoPosition>
    {
        public float TimeTime;
        public void Execute(
            Unity.Entities.Entity LogoQueryEntity,
            int Update_LogoQuery_JobIdx,
            [ReadOnlyAttribute] ref Unity.Transforms.LocalToWorld LogoQueryLocalToWorld,
            [ReadOnlyAttribute] ref LogoPosition LogoQueryLogoPosition)
        {
            LogoQueryLocalToWorld.Value = DokabenNode.CalcLogoMatrix(
                LogoQueryLocalToWorld.Value,
                LogoQueryLogoPosition.Value,
                new float3(4.8F, 1.28F, 1F),
                math.sin(
                    DokabenNode.GetAnimationTable(
                        DokabenNode.FloatToInt(math.round(math.mul(((math.sin(TimeTime) + 1F) / 2F), 15F))))),
                math.cos(
                    DokabenNode.GetAnimationTable(
                        DokabenNode.FloatToInt(math.round(math.mul(((math.sin(TimeTime) + 1F) / 2F), 15F))))));
        }
    }

    protected override void OnCreate()
    {
        LogoQuery = GetEntityQuery(
            ComponentType.ReadOnly<Unity.Transforms.LocalToWorld>(),
            ComponentType.ReadOnly<LogoPosition>());
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        {
            inputDeps = JobForEachExtensions.Schedule(new Update_LogoQuery_Job()
            {
                TimeTime = Time.time
            }, LogoQuery, inputDeps);
        }

        return inputDeps;
    }
}