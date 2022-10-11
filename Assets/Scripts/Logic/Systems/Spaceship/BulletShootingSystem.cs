using Leopotam.EcsLite;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShootingSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var logic = systems.GetShared<SharedData>().Logic;

        var bulletGuns = world.Filter<BulletGunComponent>().Inc<TransformComponent>().End();
        var gunsPool = world.GetPool<BulletGunComponent>();
        var transformPool = world.GetPool<TransformComponent>();

        foreach (var entity in bulletGuns)
        {
            ref var gun = ref gunsPool.Get(entity);

            if (gun.IsShooting && gun.Delay == 0)
            {
                ref var transform = ref transformPool.Get(entity);
                var bullet = new BulletComponent { LifeTime = Config.BulletLifeTime };
                var circle = new CircleCollider(Config.BulletColliderRadius);
                var collider = new ColliderComponent { Collider = circle };
                logic.Spawn(transform.Position, transform.Direction, Config.BulletSpeed)
                    .Add(collider)
                    .Add(bullet)
                    .End();

                gun.Delay = Config.BulletGunDelay;
            }
        }
    }
}
