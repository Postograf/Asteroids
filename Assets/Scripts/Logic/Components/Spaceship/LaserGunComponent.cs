using UnityEngine;

public struct LaserGunComponent
{
    public int Charge { get; set; }
    public float ChargeTime { get; set; }
    public RectangularCollider Collider { get; set; }
    public bool IsShooting { get; set; }
}
