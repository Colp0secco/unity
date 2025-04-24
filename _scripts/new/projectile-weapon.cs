using UnityEngine;

public class ProjectileWeapon : Weapon
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 10f;
    
    protected override void PerformAttack(Vector2 direction)
    {
        GameObject projectileObj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Projectile projectile = projectileObj.GetComponent<Projectile>();
        
        if (projectile != null)
        {
            projectile.Initialize(direction, projectileSpeed, damage);
        }
    }
}
