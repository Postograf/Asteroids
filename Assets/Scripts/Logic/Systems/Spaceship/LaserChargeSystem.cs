using Leopotam.EcsLite;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserChargeSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var shared = systems.GetShared<SharedData>();
        var lasers = world.Filter<LaserGunComponent>().End();
        var lasersPool = world.GetPool<LaserGunComponent>();

        foreach (var entity in lasers)
        {
            ref var laser = ref lasersPool.Get(entity);

            if (laser.Charge < Config.LaserMaxCharges)
            {
                laser.ChargeTime -= shared.DeltaTime;
                if (laser.ChargeTime <= 0)
                {
                    laser.Charge = Mathf.Min(laser.Charge + 1, Config.LaserMaxCharges);
                    laser.ChargeTime = Config.LaserChargeTime;
                }
            }
        }
    }
}
