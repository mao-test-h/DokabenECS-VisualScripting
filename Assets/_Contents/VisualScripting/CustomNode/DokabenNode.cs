using Unity.Mathematics;
using UnityEngine.VisualScripting;

namespace VisualScripting.CustomNode
{
    [Node]
    public static class DokabenNode
    {
        static readonly float[] AnimationTable =
            new float[]
            {
                1.5707963267948966f,
                1.4660765716752369f,
                1.3613568165555772f,
                1.2566370614359172f,
                1.1519173063162575f,
                1.0471975511965979f,
                0.9424777960769379f,
                0.8377580409572781f,
                0.7330382858376184f,
                0.6283185307179586f,
                0.5235987755982989f,
                0.4188790204786392f,
                0.31415926535897926f,
                0.2094395102393195f,
                0.10471975511965975f,
                0f,
            };

        public static float GetAnimationTable(int index) => AnimationTable[index];

        public static float4x4 CalcLogoMatrix(float4x4 mat, float3 pos, float3 scale, float sin, float cos)
        {
            // scale
            mat.c0.x = scale.x;
            mat.c1.y = scale.y;
            mat.c2.z = scale.z;

            // rotate
            var z = 0f;
            var halfY = -0.5f;
            //mat.c0.x = 1;
            mat.c3.w = 1;
            mat.c1.yz = new float2(cos, sin);
            mat.c2.yz = new float2(-sin, cos);
            mat.c3.yz = new float2(halfY - halfY * cos + z * sin, z - halfY * sin - z * cos);
            mat.c3.x = pos.x;
            mat.c3.yz += pos.yz;
            return mat;
        }

        // HACK: ECS VS上でCast出来ない?っぽいので拡張で対応..
        public static int FloatToInt(float val) => (int) val;
    }
}
