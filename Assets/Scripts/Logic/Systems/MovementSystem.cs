using Leopotam.EcsLite;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var shared = systems.GetShared<SharedData>();
        var field = shared.Logic.Field;
        var world = systems.GetWorld();
        var movables = world.Filter<MovementComponent>().Inc<TransformComponent>().End();
        var movementPool = world.GetPool<MovementComponent>();
        var trasformPool = world.GetPool<TransformComponent>();

        foreach (var entity in movables)
        {
            ref var movement = ref movementPool.Get(entity);
            ref var transform = ref trasformPool.Get(entity);
            transform.Position = field.MoveInside(
                transform.Position + movement.Velocity * shared.DeltaTime
            );
        }
    }
}
