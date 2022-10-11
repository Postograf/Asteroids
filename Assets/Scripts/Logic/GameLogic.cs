using Codice.CM.Client.Differences;

using Leopotam.EcsLite;

using System;
using System.Collections.Generic;

using UnityEngine;

public class GameLogic
{
    private SharedData _updateData;
    private IEcsSystems _updateSystems;
    private SharedData _fixedData;
    private IEcsSystems _fixedUpdateSystems;
    private IEcsSystems _initSystems;
    private IEcsSystems _destroyingSystems;

    public event Action<int> Spawned;
    public event Action<IEnumerable<int>> Despawned;

    public Field Field { get; private set; }
    public EcsWorld World { get; private set; }

    public GameLogic()
    {
        World = new EcsWorld();
    }

    public void Init(Field field)
    {
        Field = field;

        _initSystems = new EcsSystems(World, this);
        _initSystems
            .Add(new SpaceshipInitSystem())
#if UNITY_EDITOR
            .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
            .Init();

        _updateData = new SharedData(this, Time.deltaTime);
        _updateSystems = new EcsSystems(World, _updateData);
        _updateSystems
            .Add(new BulletShootingSystem())
            .Add(new BulletReloadingSystem())
            .Add(new LaserShootingSystem())
            .Add(new LaserChargeSystem())
            .Add(new ForceSpaceshipSystem())
            .Add(new AsteroidsSpawnSystem())
            .Add(new UfoSpawnSystem())
            .Init();

        _fixedData = new SharedData(this, Time.fixedDeltaTime);
        _fixedUpdateSystems = new EcsSystems(World, _fixedData);
        _fixedUpdateSystems
            .Add(new UfoTargetingSystem())
            .Add(new BulletHurtSystem())
            .Add(new MovementSystem())
            .Add(new CollisionSystem())
            .Init();

        _destroyingSystems = new EcsSystems(World, this);
        _destroyingSystems
            .Add(new FragmentsSpawnSystem())
            .Init();
    }

    public void Update()
    {
        _updateData.DeltaTime = Time.deltaTime;
        _updateSystems.Run();
    }

    public void FixedUpdate()
    {
        _fixedData.DeltaTime = Time.fixedDeltaTime;
        _fixedUpdateSystems.Run();
    }

    public SpawnedObject Spawn(Vector2 position, Vector2 direction, float speed)
    {
        var entity = World.NewEntity();
        ref var transform = ref World.GetPool<TransformComponent>().Add(entity);
        ref var movement = ref World.GetPool<MovementComponent>().Add(entity);

        transform.Position = position;
        transform.Direction = direction;
        movement.Velocity = direction * speed;

        var spawned = new SpawnedObject(World, entity);
        spawned.Inited += (e) => Spawned?.Invoke(e);
        return spawned;
    }

    public void DespawnDestroyingObjects(bool softDestroy = true)
    {
        if (softDestroy) 
            _destroyingSystems.Run();

        var destroying = World.Filter<DestroyingComponent>().End();
        var destroyed = new List<int>();

        foreach (var entity in destroying)
        {
            destroyed.Add(entity);
            World.DelEntity(entity);
        }

        Despawned?.Invoke(destroyed);
    }
}