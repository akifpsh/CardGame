using UnityEngine;
using UnityEngine.UI;

public class TurnPanel : MonoBehaviour
{
    public GameObject panel;
    public Text playerCardTypeText;
    public Text opponentCardTypeText;
    public Text playerCardValueText;
    public Text opponentCardValueText;

    private void Start()
    {
    }

    public void OpenPanel()
    {
        panel.SetActive(true);
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
    }

    public void UpdateCards(string playerCardType, string opponentCardType, int playerCardValue, int opponentCardValue)
    {
        playerCardTypeText.text = playerCardType;
        opponentCardTypeText.text = opponentCardType;
        playerCardValueText.text = playerCardValue.ToString();
        opponentCardValueText.text = opponentCardValue.ToString();
    }

}
