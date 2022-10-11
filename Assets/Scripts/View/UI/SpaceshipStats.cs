using System.Collections;
using System.Collections.Generic;
using System.Text;

using TMPro;

using UnityEngine;

public class SpaceshipStats : MonoBehaviour
{
    [SerializeField] private ObjectPool _pool;
    [SerializeField] private TMP_Text _statsText;

    private string _statsFormat;
    private bool _shipSpawned;
    private View _ship;

    private void Awake()
    {
        var builder = new StringBuilder();
        builder.Append("XY: ({0, 7: #0.00}, {1, 7: #0.00}) RT: {2, -3}° SP: {3, -3: #0.0} ");
        builder.Append("CH: {4, -2} CD: {5, -3:#0.0}");
        _statsFormat = builder.ToString();
        _pool.SpaceshipSpawned += (view) => OnShipStateChanged(view, true);
        _pool.SpaceshipDespawned += (view) => OnShipStateChanged(view, false);
    }

    private void OnShipStateChanged(View ship, bool isSpawned)
    {
        _ship = ship;
        _shipSpawned = isSpawned;
    }

    private void Update()
    {
        if (_shipSpawned)
        {
            var world = _ship.World;
            var entity = _ship.Entity;

            ref var transform = ref world.GetPool<TransformComponent>().Get(entity);
            ref var movement = ref world.GetPool<MovementComponent>().Get(entity);
            ref var laserGun = ref world.GetPool<LaserGunComponent>().Get(entity);

            var position = transform.Position;
            var rotation = Vector2.Angle(transform.Direction, Vector2.up);
            var speed = movement.Velocity.magnitude;
            var charge = laserGun.Charge;
            var chargeTime = laserGun.ChargeTime;
            _statsText.text = string.Format(
                _statsFormat,
                position.x, position.y, rotation, speed, charge, chargeTime
            );
        }
    }
}
