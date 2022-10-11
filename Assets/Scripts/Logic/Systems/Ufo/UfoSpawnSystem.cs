using Leopotam.EcsLite;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoSpawnSystem : OverTimeSystem
{
    public UfoSpawnSystem()
    {
        _maxDelay = Config.UfoSpawnTime;
        _delay = _maxDelay;
    }

    protected override void RunOverTime(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var logic = systems.GetShared<SharedData>().Logic;
        var field = logic.Field;

        var position = field.RandomEdgePoint();

        var circle = new CircleCollider(Config.UfoColliderRadius);
        var collider = new ColliderComponent { Collider = circle };

        logic.Spawn(position, Vector2.zero, Config.UfoSpeed)
            .Add(new UfoComponent())
            .Add(collider)
            .End();
    }
}
