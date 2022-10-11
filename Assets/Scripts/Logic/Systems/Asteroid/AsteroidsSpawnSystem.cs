using Leopotam.EcsLite;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidsSpawnSystem : OverTimeSystem
{
    public AsteroidsSpawnSystem() 
    {
        _maxDelay = Config.AsteroidSpawnTime;
        _delay = _maxDelay;
    }

    protected override void RunOverTime(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var logic = systems.GetShared<SharedData>().Logic;
        var field = logic.Field;

        var position = field.RandomEdgePoint();
        var randomAngle = Random.Range(0, 360f);
        var direction = Quaternion.Euler(0, 0, randomAngle) * Vector2.up;

        var fragments = Config.AsteroidFragments;
        var asteroid = new AsteroidComponent { FragmentsCount = fragments };

        var circle = new CircleCollider(Config.AsteroidColliderRadius);
        var collider = new ColliderComponent { Collider = circle };

        logic.Spawn(position, direction, Config.AsteroidSpeed)
            .Add(asteroid)
            .Add(collider)
            .End();
    }
}
