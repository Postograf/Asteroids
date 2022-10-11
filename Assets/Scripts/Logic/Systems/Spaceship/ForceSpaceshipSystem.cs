using Leopotam.EcsLite;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceSpaceshipSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var shared = systems.GetShared<SharedData>();

        var spaceships = world
            .Filter<SpaceshipComponent>()
            .Inc<MovementComponent>()
            .Inc<TransformComponent>()
            .End();
        var spaceshipsPool = world.GetPool<SpaceshipComponent>();
        var movementPool = world.GetPool<MovementComponent>();
        var transformPool = world.GetPool<TransformComponent>();

        foreach (var entity in spaceships)
        {
            ref var spaceship = ref spaceshipsPool.Get(entity);

            if (spaceship.IsMoving)
            {
                ref var movement = ref movementPool.Get(entity);
                ref var transform = ref transformPool.Get(entity);
                movement.Velocity += transform.Direction * Config.ShipAcceleration * shared.DeltaTime;
                movement.Velocity = Vector2.ClampMagnitude(movement.Velocity, Config.ShipMaxSpeed);
            }
        }
    }
}
