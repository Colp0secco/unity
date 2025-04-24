using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected int damage = 10;
    [SerializeField] protected float attackRate = 1f;
    [SerializeField] protected float attackRange = 5f;
    
    protected float nextAttackTime = 0f;
    
    public virtual bool Attack(Vector2 direction)
    {
        if (Time.time < nextAttackTime)
            return false;
            
        nextAttackTime = Time.time + (1f / attackRate);
        
        PerformAttack(direction);
        return true;
    }
    
    protected virtual void PerformAttack(Vector2 direction)
    {
        // Override in specific weapon implementations
    }
}
