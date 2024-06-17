using UnityEngine;
using UnityEngine.UI;

public class Arena : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;
    public Text healthText;
    public Renderer arenaRenderer;

    private Color initialColor;
    private Color maxHealthColor;

    private void Start()
    {
        if (gameObject.CompareTag("SumoArena"))
        {
            maxHealthColor = Color.red; // Sumo Arena için kýrmýzý
        }
        else
        {
            maxHealthColor = Color.green; // Diðer arenalar için mavi
        }

        currentHealth = maxHealth;
        initialColor = Color.white;
        UpdateHealthUI();
    }

    public void ChangeHealth(int amount)
    {
        GameManager.Instance.CheckGameOver();
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        healthText.text = currentHealth.ToString();
        float healthRatio = (float)currentHealth / maxHealth;
        Color newColor = Color.Lerp(initialColor, maxHealthColor, healthRatio);
        arenaRenderer.material.color = newColor;
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }
}
