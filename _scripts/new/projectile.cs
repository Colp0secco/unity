using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private int damage;
    private string targetTag = "Enemy"; // Default target
    
    public void Initialize(Vector2 direction, float speed, int damage, string targetTag = "Enemy")
    {
        this.direction = direction.normalized;
        this.speed = speed;
        this.damage = damage;
        this.targetTag = targetTag;
        
        // Rotate to face direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    
    private void Update()
    {
        transform.position += new Vector3(direction.x, direction.y, 0) * speed * Time.deltaTime;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            Health health = collision.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Wall") || collision.CompareTag("Prop"))
        {
            Destroy(gameObject);
        }
    }
    
    // Destroy after a certain time to prevent filling the scene
    private void Start()
    {
        Destroy(gameObject, 5f);
    }
}
