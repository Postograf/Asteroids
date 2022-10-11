using Leopotam.EcsLite;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipInitSystem : IEcsInitSystem
{
    public void Init(IEcsSystems systems)
    {
        var logic = systems.GetShared<GameLogic>();

        var circle = new CircleCollider(Config.ShipColliderRadius);
        var collider = new ColliderComponent { Collider = circle };

        var size = new Vector2(Config.LaserWidth, logic.Field.Diagonal);
        var laser = new RectangularCollider(size);

        logic.Spawn(logic.Field.Center, Vector2.up, 0f)
            .Add(collider)
            .Add(new BulletGunComponent())
            .Add(new LaserGunComponent() { Collider = laser })
            .Add(new SpaceshipComponent())
            .End();
    }
}
