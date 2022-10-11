using Leopotam.EcsLite;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var logic = systems.GetShared<SharedData>().Logic;
        Func<EcsWorld.Mask> generalMask = () => world.Filter<TransformComponent>().Inc<ColliderComponent>();

        var collisions = generalMask().Exc<AsteroidComponent>().Exc<UfoComponent>().End();
        var colliders = generalMask().Exc<BulletComponent>().Exc<SpaceshipComponent>().End();
        var collidersPool = world.GetPool<ColliderComponent>();
        var transformPool = world.GetPool<TransformComponent>();
        var deathPool = world.GetPool<DestroyingComponent>();

        foreach (var collisionEntity in collisions)
        {
            ref var collision = ref collidersPool.Get(collisionEntity);
            ref var collisionTransform = ref transformPool.Get(collisionEntity);
            foreach (var colliderEntity in colliders)
            {
                ref var collider = ref collidersPool.Get(colliderEntity);
                ref var colliderTransform = ref transformPool.Get(colliderEntity);

                var collisionVector = colliderTransform.Position - collisionTransform.Position;
                var sqrDistance = collisionVector.sqrMagnitude;
                var radiusSum = collision.Collider.Radius + collider.Collider.Radius;
                var sqrRadiusSum = radiusSum * radiusSum;
                if (sqrDistance <= sqrRadiusSum)
                {
                    deathPool.Add(colliderEntity);
                    deathPool.Add(collisionEntity);
                    break;
                }
            }

            logic.DespawnDestroyingObjects();
        }
    }
}
