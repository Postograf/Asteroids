using Leopotam.EcsLite;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameView : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private ObjectPool _pool;

    public GameLogic Logic { get; private set; }
    public EcsWorld World { get; private set; }

    private void Start()
    {
        var field = new Field(
            _camera.transform.position,
            _camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height))
            - _camera.ScreenToWorldPoint(Vector2.zero)
        );

        Logic = new GameLogic();
        World = Logic.World;
        Logic.Spawned += OnSpawn;
        Logic.Despawned += _pool.Despawn;
        
        Logic.Init(field);
    }

    private void OnSpawn(int entity)
    {
        ObjectType type;

        if (World.GetPool<SpaceshipComponent>().Has(entity)) type = ObjectType.Spaceship;
        else if (World.GetPool<AsteroidComponent>().Has(entity)) type = ObjectType.Asteroid;
        else if (World.GetPool<BulletComponent>().Has(entity)) type = ObjectType.Bullet;
        else type = ObjectType.Ufo;

        _pool.Spawn(type, World, entity);
    }

    private void Update()
    {
        Logic.Update();
    }

    private void FixedUpdate()
    {
        Logic.FixedUpdate();
    }

    public void Restart()
    {
        _pool.DespawnAll();
        Start();
    }
}
