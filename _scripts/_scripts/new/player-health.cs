using UnityEngine;
using System.Collections;

public class PlayerHealth : Health
{
    [SerializeField] private float knockbackForce = 10f;
    private Rigidbody2D rb;
    
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
    }
    
    public override bool TakeDamage(int damage)
    {
        if (base.TakeDamage(damage))
        {
            // Player died
            return true;
        }
        
        // Apply knockback in direction away from attacker
        if (rb != null)
        {
            Vector2 knockbackDirection = -transform.forward;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
        
        return false;
    }
    
    protected override void Die()
    {
        // Instead of destroying, handle game over
        // Could trigger game over UI or level restart
        Debug.Log("Player died!");
        // For now, we'll just restart the level after a short delay
        StartCoroutine(GameOverRoutine());
    }
    
    private System.Collections.IEnumerator GameOverRoutine()
    {
        // Add game over effects here
        yield return new WaitForSeconds(2f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
