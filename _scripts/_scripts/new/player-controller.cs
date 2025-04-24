using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 aimDirection;
    private Weapon equippedWeapon;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        equippedWeapon = GetComponentInChildren<Weapon>();
        
        // Add a ProjectileWeapon component if no weapon is found
        if (equippedWeapon == null)
        {
            equippedWeapon = gameObject.AddComponent<ProjectileWeapon>();
        }
    }
    
    private void Update()
    {
        // Get movement input
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        
        // Normalize to prevent faster diagonal movement
        moveInput = moveInput.normalized;
        
        // Get aim direction (mouse position in screen space)
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        aimDirection = (worldPosition - transform.position).normalized;
        
        // Handle shooting
        if (Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.Space))
        {
            equippedWeapon.Attack(aimDirection);
        }
    }
    
    private void FixedUpdate()
    {
        // Apply movement
        rb.linearVelocity = moveInput * moveSpeed;
        
        // Rotate player to face mouse direction
        if (aimDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
