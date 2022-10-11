using Leopotam.EcsLite;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpaceshipControl : MonoBehaviour
{
    [SerializeField] private View _view;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private float _laserDuration;
    private SpaceshipInputs _inputs;
    private Camera _camera;

    private void Awake()
    {
        _inputs = new SpaceshipInputs();

        _inputs.Game.BulletAttack.performed += BulletAttack;
        _inputs.Game.BulletAttack.canceled += BulletAttack;

        _inputs.Game.LaserAttack.performed += LaserAttack;

        _inputs.Game.MoveForward.performed += MoveForward;
        _inputs.Game.MoveForward.canceled += MoveForward;

        _camera = Camera.main;
        _inputs.Game.MouseTracking.performed += MouseTracking;
    }

    private void BulletAttack(InputAction.CallbackContext context)
    {
        var world = _view?.World;
        if (world == null) return;

        ref var bulletGun = ref world.GetPool<BulletGunComponent>().Get(_view.Entity);
        bulletGun.IsShooting = context.phase == InputActionPhase.Performed;
    }

    private void LaserAttack(InputAction.CallbackContext context)
    {
        var world = _view?.World;
        var transform = _view?.Transform;

        if (world == null) return;

        ref var laserGun = ref world.GetPool<LaserGunComponent>().Get(_view.Entity);
        laserGun.IsShooting = true;

        if (laserGun.Charge > 0)
        {
            var size = laserGun.Collider.Size;
            var start = transform.position;
            var end = transform.up * size.y;
            _lineRenderer.SetPositions(new Vector3[2] { start, end });
            _lineRenderer.widthCurve = new AnimationCurve(new Keyframe(0, size.x));
            _lineRenderer.enabled = true;
            Invoke(nameof(DisableLaser), _laserDuration);
        }
    }

    private void DisableLaser()
    {
        _lineRenderer.enabled = false;
    } 

    private void MoveForward(InputAction.CallbackContext context)
    {
        var world = _view?.World;
        if (world == null) return;

        ref var spaceship = ref world.GetPool<SpaceshipComponent>().Get(_view.Entity);
        spaceship.IsMoving = context.phase == InputActionPhase.Performed;
    }

    private void MouseTracking(InputAction.CallbackContext context)
    {
        var world = _view?.World;
        if (world == null) return;

        ref var transform = ref world.GetPool<TransformComponent>().Get(_view.Entity);
        Vector2 cameraPosition = _camera.ScreenToWorldPoint(context.ReadValue<Vector2>());
        transform.Direction = cameraPosition - transform.Position;
    }

    private void OnEnable()
    {
        _inputs.Enable();
    }

    private void OnDisable()
    {
        _inputs.Disable();
    }
}