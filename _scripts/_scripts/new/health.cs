using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] protected int maxHealth = 100;
    [SerializeField] protected float invulnerabilityTime = 0.5f;
    
    protected int currentHealth;
    protected bool isInvulnerable = false;
    protected float invulnerabilityTimer = 0f;
    
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    
    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }
    
    protected virtual void Update()
    {
        if (isInvulnerable)
        {
            invulnerabilityTimer -= Time.deltaTime;
            if (invulnerabilityTimer <= 0f)
            {
                isInvulnerable = false;
            }
        }
    }
    
    public virtual bool TakeDamage(int damage)
    {
        if (isInvulnerable) return false;
        
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            Die();
            return true;
        }
        
        StartInvulnerability();
        return false;
    }
    
    protected virtual void Die()
    {
        Destroy(gameObject);
    }
    
    protected void StartInvulnerability()
    {
        isInvulnerable = true;
        invulnerabilityTimer = invulnerabilityTime;
    }
    
    public virtual void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }
}
