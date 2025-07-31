using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    public Transform firePoint;

    [Header("Fire Settings")]
    public bool isAuto = false;
    public float fireRate = 0.2f;
    private float nextFireTime = 0f;

    public bool CanFire()
    {
        return Time.time >= nextFireTime;
    }

    public void TryFire(Vector2 aimDir)
    {
        if (CanFire())
        {
            Shoot(aimDir);
            nextFireTime = Time.time + fireRate;
        }
    }

    public abstract void Shoot(Vector2 aimDir);
}
