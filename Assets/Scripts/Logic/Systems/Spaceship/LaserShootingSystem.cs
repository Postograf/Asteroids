using Leopotam.EcsLite;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEditor;

using UnityEngine;
using UnityEngine.UIElements;

public class LaserShootingSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var logic = systems.GetShared<SharedData>().Logic;
        Func<EcsWorld.Mask> genericMask = () => world.Filter<TransformComponent>();
        var laserSize = new Vector2(Config.LaserWidth, logic.Field.Diagonal);

        var colliders = genericMask().Inc<ColliderComponent>().End();
        var lasers = genericMask().Inc<LaserGunComponent>().End();

        var lasersPool = world.GetPool<LaserGunComponent>();
        var transfromPool = world.GetPool<TransformComponent>();
        var collidersPool = world.GetPool<ColliderComponent>();
        var deathPool = world.GetPool<DestroyingComponent>();

        foreach (var laserEntity in lasers)
        {
            ref var laser = ref lasersPool.Get(laserEntity);
            ref var laserTransform = ref transfromPool.Get(laserEntity);

            if (laser.IsShooting == false || laser.Charge == 0)
            {
                laser.IsShooting = false;
                continue;
            }

            laser.IsShooting = false;
            laser.Charge--;

            var rect = laser.Collider.Relocate(
                start: laserTransform.Position,
                direction: laserTransform.Direction
            );

            foreach (var colliderEntity in colliders)
            {
                if (colliderEntity == laserEntity)
                    continue;

                ref var collderTransform = ref transfromPool.Get(colliderEntity);
                ref var circle = ref collidersPool.Get(colliderEntity);

                var circleCenter = collderTransform.Position;
                var cornerToCenter = circleCenter - rect.Dots[0];
                var widthVector = rect.Dots[1] - rect.Dots[0];
                var widthDot = Vector2.Dot(widthVector, widthVector);
                var widthCircleDot = Vector2.Dot(cornerToCenter, widthVector);
                var heightVector = rect.Dots[rect.Dots.Length - 1] - rect.Dots[0];
                var heightDot = Vector2.Dot(heightVector, heightVector);
                var heightCircleDot = Vector2.Dot(cornerToCenter, heightVector);
                if (
                    Mathf.Clamp(heightCircleDot, 0, heightDot) == heightCircleDot
                    && Mathf.Clamp(widthCircleDot, 0, widthCircleDot) == widthCircleDot
                )
                {
                    deathPool.Add(colliderEntity);
                    continue;
                }

                for (int i = 0; i < rect.Edges.Length; i++)
                {
                    var edge = rect.Edges[i];
                    var perpendicular = rect.NormalizedPerpendiculars[i];
                    var perpendicularRadius = perpendicular * circle.Collider.Radius;
                    if (
                        SegmentsIntersection(
                            edge.First, edge.Second,
                            circleCenter, circleCenter + perpendicularRadius,
                            out var result
                        )
                    )
                    {
                        deathPool.Add(colliderEntity);
                        break;
                    }
                }
            }

            logic.DespawnDestroyingObjects(softDestroy: false);
        }
    }

    private bool SegmentsIntersection(
        Vector2 start1, Vector2 end1, Vector2 start2, Vector2 end2, out Vector2 result
    )
    {
        Debug.DrawLine(start1, end1, Color.blue, 100f);
        Debug.DrawLine(start2, end2, Color.yellow, 100f);

        var denominator =
            (start1.x - end1.x) * (start2.y - end2.y)
            - (start1.y - end1.y) * (start2.x - end2.x);

        if (denominator == 0)
        {
            result = Vector2.zero;
            return false;
        }

        var genericMult1 = start1.x * end1.y - start1.y * end1.x;
        var genericMult2 = start2.x * end2.y - start2.y * end2.x;

        result.x =
            (genericMult1 * (start2.x - end2.x) - (start1.x - end1.x) * genericMult2)
                                        / denominator;
        result.y =
            (genericMult1 * (start2.y - end2.y) - (start1.y - end1.y) * genericMult2)
                                        / denominator;

        Debug.DrawLine(result, result + Vector2.up * 0.1f, Color.blue, 100f);

        return PointInSegment(start1, end1, result) && PointInSegment(start2, end2, result);
    }

    private bool PointInSegment(Vector2 start, Vector2 end, Vector2 point)
    {
        var maxX = Mathf.Max(start.x, end.x);
        var minX = Mathf.Min(start.x, end.x);
        var maxY = Mathf.Max(start.y, end.y);
        var minY = Mathf.Min(start.y, end.y);
        return Mathf.Clamp(point.x, minX, maxX) == point.x
            && Mathf.Clamp(point.y, minY, maxY) == point.y;
    }
}