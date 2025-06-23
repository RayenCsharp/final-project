using System.IO;
using UnityEngine;

public class DataHolder : MonoBehaviour
{

    public static DataHolder Instance;

    public float bestScore;
    public string currentPlayerName;
    public string bestPlayer;
    public float Volume;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Load(); // Load data when the game starts
    }

    [System.Serializable]
    class SaveData
    {
        public float bestScore;
        public string currentPlayerName;
        public string bestPlayer;
        public float Volume;
    }

    public void Save()
    {
        SaveData data = new SaveData
        {
            bestScore = this.bestScore,
            currentPlayerName = this.currentPlayerName,
            bestPlayer = this.bestPlayer,
            Volume = this.Volume
        };
        string json = JsonUtility.ToJson(data);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void Load()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path); // Read the JSON data from the file
            SaveData data = JsonUtility.FromJson<SaveData>(json); // Deserialize the JSON data into a SaveData object
            bestScore = data.bestScore;
            currentPlayerName = data.currentPlayerName;
            bestPlayer = data.bestPlayer;
            Volume = data.Volume;
        }
        else
        {
            Debug.LogWarning("Save file not found at " + path);
        }
    }
}
