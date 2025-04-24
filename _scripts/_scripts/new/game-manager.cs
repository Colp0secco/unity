using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private AbstractDungeonGenerator dungeonGenerator;
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int maxLevel = 10;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        if (dungeonGenerator == null)
        {
            dungeonGenerator = FindObjectOfType<AbstractDungeonGenerator>();
        }
        
        GenerateNewLevel();
    }
    
    public void GenerateNewLevel()
    {
        if (dungeonGenerator != null)
        {
            dungeonGenerator.GenerateDungeon();
        }
    }
    
    public void AdvanceToNextLevel()
    {
        currentLevel++;
        
        if (currentLevel > maxLevel)
        {
            // Game completed
            Debug.Log("Congratulations! You completed all levels!");
            // Show victory screen or restart game
        }
        else
        {
            GenerateNewLevel();
        }
    }
}
