using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManeger : MonoBehaviour
{
    [SerializeField] private Damageble playerDamageble; // Reference to the main menu UI
    [SerializeField] private Damageble bossDamageble; // Reference to the main menu UI
    [SerializeField] private GameManeger gameManeger; // Reference to the main menu UI
    [SerializeField] private TMP_Text playerHpText; // Reference to the main menu UI
    [SerializeField] private TMP_Text timerText; // Reference to the timer UI
    [SerializeField] private Image specialAttackIcon; // Reference to the special attack icon

    [SerializeField]private Image healIcon;
    [SerializeField]private Image doubleDamageIcon;
    [SerializeField]private Image immunityIcon;
    [SerializeField]private Image speedIcon;
    [SerializeField]private TMP_Text timer; // Text to display the double damage timer

    [SerializeField]private GameObject bossHpBar;
    [SerializeField]private Slider hpBar; // Reference to the boss HP text

    [SerializeField]private GameObject gameOverPanel; // Reference to the game over panel

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerHpText.text = $"HP: {playerDamageble.CurrentHealth}/{playerDamageble.MaxHealth}"; // Initialize the player HP text
        timerText.text = "00:00"; // Initialize the timer text
        gameOverPanel.SetActive(false); // Hide the game over panel at the start
    }

    // Update is called once per frame
    void Update()
    {
        playerHpText.text = $"HP: {playerDamageble.CurrentHealth}/{playerDamageble.MaxHealth}"; // Update the player HP text every frame
        timerText.text = gameManeger.UpdateTimerText(); // Update the timer text every frame

        if (bossDamageble == null)
        {
            GameObject bossObj = GameObject.FindGameObjectWithTag("Boss");
            if (bossObj != null)
            {
                bossDamageble = bossObj.GetComponent<Damageble>();
            }
        }

        if (bossDamageble != null && bossHpBar != null)
        {
            bossHpBar.SetActive(bossDamageble.IsAlive);

            // Make sure MaxHealth is not zero to avoid divide-by-zero error
            if (bossDamageble.MaxHealth > 0)
            {
                float healthRatio = (float)bossDamageble.CurrentHealth / bossDamageble.MaxHealth;
                hpBar.value = Mathf.Lerp(hpBar.value, healthRatio, Time.deltaTime * 5f);
            }
        }
    }

    public void ShowHeal()
    {
        Debug.Log("Heal called!");
        StartCoroutine(FlashIcon(healIcon, 1.5f));
    }

    public void ActivateDoubleDamage(float duration)
    {
        StartCoroutine(ShowIconWithTimer(doubleDamageIcon, timer, duration));
    }

    public void ActivateImmunity(float duration)
    {
        StartCoroutine(ShowIconWithTimer(immunityIcon, timer, duration));
    }

    public void ActivateSpeed(float duration)
    {
        StartCoroutine(ShowIconWithTimer(speedIcon, timer, duration));
    }

    private IEnumerator FlashIcon(Image icon, float duration)
    {
        icon.enabled = true;
        yield return new WaitForSeconds(duration);
        icon.enabled = false;
    }

    private IEnumerator ShowIconWithTimer(Image icon, TMP_Text timerText, float duration)
    {
        icon.enabled = true;
        float timeLeft = duration;
        while (timeLeft > 0)
        {
            timerText.text = Mathf.CeilToInt(timeLeft).ToString();
            timeLeft -= Time.deltaTime;
            yield return null;
        }
        timerText.text = "";
        icon.enabled = false;
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true); // Show the game over panel when the game is over
    }

    public void SpecialAttackReady()
    {
        specialAttackIcon.enabled = true; // Enable the special attack icon when the special attack is ready
    }

    public void SpecialAttackNotReady()
    {
        specialAttackIcon.enabled = false; // Disable the special attack icon when the special attack is used
    }
}
