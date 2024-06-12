using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Level
{
    public string level;
}

public class LevelSwitch : MonoBehaviour
{
    [SerializeField]

    public Level level = new Level();

    void Start()
    {
        // Any initialization if needed
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Save the level data
            SaveToJson();

            // Load the next scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(level.level);
        }
    }

    public void SaveToJson()
    {
        string currentLevel = JsonUtility.ToJson(level);
        string filePath = Application.persistentDataPath + "/levelData.json";
        Debug.Log("Saving to: " + filePath);
        System.IO.File.WriteAllText(filePath, currentLevel);
        Debug.Log("Saved level");
    }

}
