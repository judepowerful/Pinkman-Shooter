using UnityEngine;

[System.Serializable]
public class HitInfo
{
    public int damage = 1;
    public Vector2 knockbackDirection = Vector2.right;
    public float knockbackForce = 5f;
}
