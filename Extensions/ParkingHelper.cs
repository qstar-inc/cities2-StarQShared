using Colossal.Mathematics;
using Game.Prefabs;
using Unity.Mathematics;

namespace StarQ.Shared.Extensions
{
    public static class ParkingHelper
    {
        public static int GetParkingSlotCount(Bezier4x3 bezier, ParkingLane parkingLane)
        {
            float slotInterval = GetSlotInterval(
                parkingLane,
                out float slotAngleRad,
                out float2 slotSize
            );
            if (slotInterval <= 0f)
                return 0;

            float length = MeasureBezierLength(bezier);
            float space = GetParkingSlotSpace(length, slotAngleRad, slotSize);

            return (int)math.floor((space + 0.01f) / slotInterval);
        }

        public static float GetSlotInterval(
            ParkingLane parkingLane,
            out float slotAngleRad,
            out float2 slotSize
        )
        {
            slotSize = math.select(parkingLane.m_SlotSize, 0f, parkingLane.m_SlotSize < 0.001f);
            slotAngleRad = math.radians(math.clamp(parkingLane.m_SlotAngle, 0f, 90f));

            float2 dir = new(math.cos(slotAngleRad), math.sin(slotAngleRad));

            if (dir.y < 0.001f)
            {
                // 0° parallel
                return slotSize.y;
            }
            else if (dir.x < 0.001f)
            {
                // 90° perpendicular
                return slotSize.x;
            }
            else
            {
                // Angled
                float2 candidate = slotSize / dir.yx;
                candidate = math.select(candidate, 0f, candidate < 0.001f);
                return (candidate.x < candidate.y) ? candidate.x : candidate.y;
            }
        }

        private static float GetParkingSlotSpace(float length, float slotAngleRad, float2 slotSize)
        {
            float space = length;

            if (slotAngleRad > 0.25f)
            {
                float2 dir = new(math.cos(slotAngleRad), math.sin(slotAngleRad));
                float slotDepth = math.min(math.dot(slotSize, dir), slotSize.y);
                space -= slotDepth * math.tan(1.5707964f - slotAngleRad);
            }

            return space;
        }

        private static float MeasureBezierLength(Bezier4x3 bezier, int steps = 16)
        {
            float length = 0f;
            float3 prev = EvaluateBezier(bezier, 0f);
            for (int i = 1; i <= steps; i++)
            {
                float t = (float)i / steps;
                float3 point = EvaluateBezier(bezier, t);
                length += math.distance(prev, point);
                prev = point;
            }
            return length;
        }

        private static float3 EvaluateBezier(Bezier4x3 bezier, float t)
        {
            float u = 1f - t;
            return u * u * u * bezier.a
                + 3f * u * u * t * bezier.b
                + 3f * u * t * t * bezier.c
                + t * t * t * bezier.d;
        }
    }
}
