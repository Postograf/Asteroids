using Leopotam.EcsLite;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoTargetingSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        Func<EcsWorld.Mask> generalMask = () => world.Filter<TransformComponent>();
        var spaceships = generalMask().Inc<SpaceshipComponent>().End();
        var ufos = generalMask().Inc<UfoComponent>().End();
        var transformsPool = world.GetPool<TransformComponent>();
        var movementPool = world.GetPool<MovementComponent>();

        foreach (var spaceship in spaceships)
        {
            ref var spaceshipTransform = ref transformsPool.Get(spaceship);
            foreach (var ufoEntity in ufos)
            {
                ref var ufoTransform = ref transformsPool.Get(ufoEntity);
                ref var ufoMovement = ref movementPool.Get(ufoEntity);

                var direction = (spaceshipTransform.Position - ufoTransform.Position).normalized;
                ufoMovement.Velocity = direction * Config.UfoSpeed;
            }

            break;
        }
    }
}
