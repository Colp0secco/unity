using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] private string nextLevelName = "";
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // If next level name is provided, load that level
            if (!string.IsNullOrEmpty(nextLevelName))
            {
                SceneManager.LoadScene(nextLevelName);
            }
            else
            {
                // Otherwise regenerate the current level
                AbstractDungeonGenerator dungeonGenerator = FindObjectOfType<AbstractDungeonGenerator>();
                if (dungeonGenerator != null)
                {
                    dungeonGenerator.GenerateDungeon();
                }
            }
        }
    }
}
