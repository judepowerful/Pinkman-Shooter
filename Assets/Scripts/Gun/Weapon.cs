using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    public Transform firePoint;

    [Header("Fire Settings")]
    public bool isAuto = false;
    public float fireRate = 0.2f;
    private float nextFireTime = 0f;
    public float recoilForce = 2f;
    public float recoilVisualDistance = 0.1f;
    protected WeaponAudioManager audioManager;
    

    protected virtual void Awake()
    {
        audioManager = GetComponent<WeaponAudioManager>();
    }

    public bool CanFire()
    {
        return Time.time >= nextFireTime;
    }

    public bool TryFire(Vector2 aimDir)
    {
        if (CanFire())
        {
            Shoot(aimDir);
            nextFireTime = Time.time + fireRate;
            return true;
        }
        return false;
    }

    public abstract void Shoot(Vector2 aimDir);
}
