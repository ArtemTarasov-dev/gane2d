using UnityEngine;
using UnityEngine.UI;

public class PoziomZdrowia : MonoBehaviour
{
    public int maxHealthPoints;
    public int currentHealthPoints;
    public Gracz selectedCharacter;
    public Image healthBar;
    public Text healthText;

    void Start()
    {
        if (selectedCharacter != null)
        {
            maxHealthPoints = selectedCharacter.maxHealthPoints;
            currentHealthPoints = selectedCharacter.healthPoints;
        }
        else
        {
            Debug.LogError("Selected character is not assigned.");
        }

        if (healthBar == null)
        {
            Debug.LogError("Health bar Image is not assigned.");
        }

        if (healthText == null)
        {
            Debug.LogError("Health text is not assigned.");
        }
    }

    void Update()
    {
        if (selectedCharacter != null && healthBar != null && healthText != null)
        {
            healthBar.fillAmount = (float)currentHealthPoints / maxHealthPoints;
            healthText.text = "HP: " + (healthBar.fillAmount * 100).ToString("0");
        }
    }

    public void UpdateHealthPoints(int healthPoints)
    {
        currentHealthPoints = healthPoints;
        if (healthBar != null && healthText != null)
        {
            healthBar.fillAmount = (float)currentHealthPoints / maxHealthPoints;
            healthText.text = "HP: " + (healthBar.fillAmount * 100).ToString("0");
        }
    }
}
