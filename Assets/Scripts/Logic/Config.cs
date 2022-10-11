using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config
{
    //Asteroids
    public const float AsteroidColliderRadius = 1f;
    public const float FragmentScale = 0.5f;
    public const float AsteroidSpawnTime = 1f;
    public const int AsteroidFragments = 2;
    public const int FragmentFragmentsDecreaser = 2;
    public const float AsteroidSpeed = 1f;
    public const float FragmentSpeedMultiplyer = 1.5f;
    public const float SeparationAngle = 45f;

    //Ufos
    public const float UfoSpawnTime = 3f;
    public const float UfoSpeed = 2f;
    public const float UfoColliderRadius = 0.5f;

    //Bullets
    public const float BulletSpeed = 3f;
    public const float BulletColliderRadius = 0.25f;
    public const float BulletLifeTime = 3f;

    //BulletGun
    public const float BulletGunDelay = 0.4f;

    //LaserGun
    public const float LaserWidth = 0.5f;
    public const int LaserMaxCharges = 5;
    public const float LaserChargeTime = 2f;

    //SpaceShip
    public const float ShipAcceleration = 0.25f;
    public const float ShipMaxSpeed = 5f;
    public const float ShipColliderRadius = 0.5f;
}