using Leopotam.EcsLite;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletReloadingSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var shared = systems.GetShared<SharedData>();

        var bulletGuns = world.Filter<BulletGunComponent>().End();
        var gunsPool = world.GetPool<BulletGunComponent>();

        foreach (var entity in bulletGuns)
        {
            ref var gun = ref gunsPool.Get(entity);

            gun.Delay = Mathf.Max(gun.Delay - shared.DeltaTime, 0);
        }
    }
}
