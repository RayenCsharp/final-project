using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManeger : MonoBehaviour
{
    [SerializeField]private float timer = 0f; // Timer to keep track of the game time
    private string TimerText; // Text to display the timer in the UI

    public static float highestScore;
    private string bestPlayer;

    public static bool bossDefeated;

    [SerializeField] private GameObject enemyPrefab; // Prefab for the enemy to be spawned
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private List<GameObject> powerUpPrefabs; // List of power-up prefabs to be spawned

    [SerializeField]private int waveNumber = 1;
    [SerializeField]private int bossWave = 10;
    private int enemiesCount;
    private int powerUpsCount; // Counter for power-ups spawned

    [SerializeField] List<Transform> spawnPoints; // List of spawn points for enemies
    [SerializeField] private Transform centerSpawner;
    private bool bossSpawned;

    [SerializeField] private UiManeger uiManeger;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (DataHolder.Instance != null)
        {
            highestScore = DataHolder.Instance.bestScore;
            bestPlayer = DataHolder.Instance.bestPlayer;
        }
        SpawnEnemyWave(waveNumber);
        int randomPowerup = Random.Range(0, powerUpPrefabs.Count);
        Instantiate(powerUpPrefabs[randomPowerup], centerSpawner);
    }

    // Update is called once per frame
    void Update()
    {
        enemiesCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        powerUpsCount = GameObject.FindGameObjectsWithTag("PowerUp").Length;
        if (enemiesCount == 0)
        {
            waveNumber++;
            if (waveNumber == bossWave && !bossSpawned)
            {
                bossSpawned = true;
                Instantiate(bossPrefab, centerSpawner);
                int count = Mathf.Min(4, powerUpPrefabs.Count, spawnPoints.Count);
                for (int i = 0; i < count; i++)
                {
                    Instantiate(powerUpPrefabs[i], spawnPoints[i]);
                }
                return;
            }
            else if (waveNumber < bossWave)
            {
                SpawnEnemyWave(waveNumber);
            }

            if (powerUpsCount == 0)
            {
                int randomPowerup = Random.Range(0, powerUpPrefabs.Count);
                Instantiate(powerUpPrefabs[randomPowerup], centerSpawner);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

    }

    public string UpdateTimerText()
    {
        timer += Time.deltaTime; // Increment the timer by the time since the last frame
        TimerText = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(timer / 60), Mathf.FloorToInt(timer % 60));
        return TimerText; // Return the formatted timer text
    }

    void SpawnEnemyWave(int spawnWave)
    {
        for (int i = 0; i < spawnWave; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPosition());
        }
    }

    private Transform GenerateSpawnPosition()
    {
        int randomSpawn = Random.Range(0, 4); // Randomly select a spawn point
        return spawnPoints[randomSpawn]; // Return the selected spawn point
    }

    public void GameOver()
    {
        uiManeger.GameOver();
        AudioListener.pause = true;
        Time.timeScale = 0f; // Stop the game by setting the time scale to zero
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Results(timer);
        DataHolder.Instance.Save(); // Save the data when the game is over
    }

    void Results(float playerScore)
    {
        if ((playerScore < highestScore || highestScore == 0f) && bossDefeated)
        {
            highestScore = playerScore;
            DataHolder.Instance.bestScore = highestScore;
            bestPlayer = DataHolder.Instance.currentPlayerName;
            DataHolder.Instance.bestPlayer = bestPlayer;
        }
    }

    public void RestartScene()
    {
        GameManeger.bossDefeated = false;
        AudioListener.pause = false;
        SceneManager.LoadScene(0);
    }
}
