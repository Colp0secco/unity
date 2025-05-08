using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{

    public static PlayerController Instance;

    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float moveSpeed = 10;
    public Vector3 playerMoveDirection;
    public float playerMaxHealth = 10;
    public float playerHealth;

    public int experience;
    public int currentLevel;
    public int maxLevel;
    public List<int> playerLevels;

    public Weapon activeWeapon;

    private bool isImmune;
    [SerializeField]
    private float immunityDuration;
    [SerializeField]
    private float immunityTimer;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        for (int i = playerLevels.Count; i < maxLevel; i++)
        {
            playerLevels.Add(Mathf.CeilToInt(playerLevels[playerLevels.Count - 1] * 1.1f + 15));
        }
        playerHealth = playerMaxHealth;
        UiController.Instance.UpdateHealthSlider();
        UiController.Instance.UpdateExperienceSlider();
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        playerMoveDirection = new Vector3(inputX, inputY).normalized;

        animator.SetFloat("moveX", inputX);
        animator.SetFloat("moveY", inputY);

        if (playerMoveDirection == Vector3.zero)
        {
            animator.SetBool("moving", false);
        }
        else
        {
            animator.SetBool("moving", true);
        }

        if (immunityTimer > 0)
        {
            immunityTimer -= Time.deltaTime;
        } else
        {
            isImmune = false;
        }

    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(playerMoveDirection.x * moveSpeed,
            playerMoveDirection.y * moveSpeed);
    }

    public void TakeDamage(float damage)
    {
        if (!isImmune)
        {
            isImmune = true;
            immunityTimer = immunityDuration;
            playerHealth -= damage;
            UiController.Instance.UpdateHealthSlider();
            if (playerHealth <= 0)
            {
                gameObject.SetActive(false);
                GameManager.Instance.GameOver();
            }
        }
    }

    public void GetExperience(int experienceToGet)
    {
        experience += experienceToGet;
        UiController.Instance.UpdateExperienceSlider();
        if (experience >= playerLevels[currentLevel - 1])
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        experience -= playerLevels[currentLevel - 1];
        currentLevel++;
        UiController.Instance.UpdateExperienceSlider();
        UiController.Instance.levelUpButtons[0].AcrivateButton(activeWeapon);
        UiController.Instance.LevelUpPanelOpen();
    }

}
