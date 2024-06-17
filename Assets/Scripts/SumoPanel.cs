using UnityEngine;
using UnityEngine.UI;

public class SumoPanel : MonoBehaviour
{
    public GameObject panel;
    public Button increaseHealthButton;
    public Button decreaseOpponentHealthButton;

    private Player currentPlayer;

    private void Start()
    {
        increaseHealthButton.onClick.AddListener(IncreaseHealth);
        decreaseOpponentHealthButton.onClick.AddListener(DecreaseOpponentHealth);
    }

    public void OpenPanel(Player player)
    {
        currentPlayer = player;
        panel.SetActive(true);
    }

    private void IncreaseHealth()
    {
        if (currentPlayer != null)
        {
            currentPlayer.playerArena.ChangeHealth(4);
        }
        ClosePanel();
    }

    private void DecreaseOpponentHealth()
    {
        if (currentPlayer != null)
        {
            currentPlayer.opponentArena.ChangeHealth(-4);
        }
        ClosePanel();
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
    }
    public void OpenPanel()
    {
        panel.SetActive(true);
    }
}
