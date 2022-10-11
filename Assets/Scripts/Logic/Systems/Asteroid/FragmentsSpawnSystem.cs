using Leopotam.EcsLite;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentsSpawnSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var logic = systems.GetShared<GameLogic>();

        var destroyingAsteroids = world
            .Filter<DestroyingComponent>()
            .Inc<AsteroidComponent>()
            .Inc<MovementComponent>()
            .Inc<TransformComponent>()
            .End();
        var transformPool = world.GetPool<TransformComponent>();
        var asteroidsPool = world.GetPool<AsteroidComponent>();
        var movementPool = world.GetPool<MovementComponent>();
        var colliderPool = world.GetPool<ColliderComponent>();

        foreach (var entity in destroyingAsteroids)
        {
            ref var asteroid = ref asteroidsPool.Get(entity);

            if (asteroid.FragmentsCount > 0)
            {
                var angle = 0f;
                for (int i = 0; i < asteroid.FragmentsCount; i++)
                {
                    angle = i % 2 == 0 ? angle + Config.SeparationAngle : angle * -1;

                    ref var transform = ref transformPool.Get(entity);
                    var position = transform.Position;
                    var direction = Quaternion.Euler(0, 0, angle) * transform.Direction;

                    ref var movement = ref movementPool.Get(entity);
                    var speed = movement.Velocity.magnitude * Config.FragmentSpeedMultiplyer;

                    var fragments = asteroid.FragmentsCount - Config.FragmentFragmentsDecreaser;
                    var component = new AsteroidComponent { FragmentsCount =  fragments };

                    ref var collider = ref colliderPool.Get(entity);
                    var circle = new CircleCollider(collider.Collider.Radius * Config.FragmentScale);
                    var fragmentCollider = new ColliderComponent { Collider = circle };

                    logic.Spawn(position, direction, speed)
                        .Add(component)
                        .Add(fragmentCollider)
                        .End();

                    angle *= angle < 0 ? -1 : 1;
                }
            }
        }
    }
}
