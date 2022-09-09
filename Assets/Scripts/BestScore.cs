using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class BestScore : MonoBehaviour
{
    public static BestScore Instance;
    public int bestScore;
    public string bestPlayer;
    public string playerName;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(Instance);
        LoadScore();
    }
    [System.Serializable]
    class SaveData
    {
        public int bestScore;
        public string bestPlayerName;
    }

    public void SaveScore()
    {
        SaveData data = new SaveData();
        data.bestScore = bestScore;
        data.bestPlayerName = bestPlayer;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestScore = data.bestScore;
            bestPlayer = data.bestPlayerName;
        }
    }
}
