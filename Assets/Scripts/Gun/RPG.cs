using UnityEngine;

public class RPG : Weapon
{
    public GameObject bulletPrefab;

    public override void Shoot(Vector2 aimDir)
    {
        Instantiate(bulletPrefab, firePoint.position, Quaternion.FromToRotation(Vector2.right, aimDir));
        // TODO: 粒子、音效、震动
    }
}
