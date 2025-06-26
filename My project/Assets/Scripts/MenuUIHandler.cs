using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;


#if UNITY_EDITOR
using UnityEditor;
#endif

[DefaultExecutionOrder(1000)] // Ensure this script runs after DataHolder is initialized
public class MenuUiHandler : MonoBehaviour
{
    public TextMeshProUGUI bestScoreText;
    public TMP_InputField playerNameInputField;

    [SerializeField] private Slider volumeSlider;

    private float bestScore;
    private string bestPlayer;
    private string currentPlayer;

    private string timerText;



    private void Awake()
    {
        if (DataHolder.Instance != null)
        {
            bestScore = DataHolder.Instance.bestScore;
            bestPlayer = DataHolder.Instance.bestPlayer;
            currentPlayer = DataHolder.Instance.currentPlayerName;

        }
        timerText = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(bestScore / 60), Mathf.FloorToInt(bestScore % 60));
        bestScoreText.text = "Best Score : " + bestPlayer + ": " + timerText;
        playerNameInputField.text = currentPlayer;
        volumeSlider.value = DataHolder.Instance.Volume; // Load the saved volume setting
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
   
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void StartGame()
    {
        if (!string.IsNullOrEmpty(playerNameInputField.text))
        {

            DataHolder.Instance.currentPlayerName = playerNameInputField.text;
            DataHolder.Instance.Volume = volumeSlider.value; // Save the volume setting
            DataHolder.Instance.Save(); // Save the current player name
            SceneManager.LoadScene(1);
            Time.timeScale = 1f; // Stop the game by setting the time scale to zero
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Debug.LogWarning("Player name cannot be empty. Please enter a name before starting the game.");
        }
    }

    public void QuitGame()
    {
        DataHolder.Instance.Save(); // Save the current state before quitting
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
                        Application.Quit(); // original code to quit Unity player
#endif

    }
}
