using UnityEngine;

public class Card : MonoBehaviour
{
    public CardInfo cardInfo; // Inspector'dan atanacak kart bilgileri

    private Renderer cardRenderer;

    private void Start()
    {
        cardRenderer = GetComponent<Renderer>();

        if (cardInfo != null)
        {
            // Kart�n rengini veya di�er g�rsel �zelliklerini ayarlayabiliriz
            // �rne�in: kartRenderer.material.color = cardInfo.cardColor;
        }
    }

    public void SetVisibility(bool isVisible)
    {
        gameObject.SetActive(isVisible);

        if (isVisible)
        {
            SetOpacity(1.0f); // Tamamen g�r�n�r yap
        }
    }

    public void SetOpacity(float opacity)
    {
        if (cardRenderer != null)
        {
            Color color = cardRenderer.material.color;
            color.a = opacity;
            cardRenderer.material.color = color;
        }
    }

    public string GetCardType()
    {
        return cardInfo != null ? cardInfo.type : "Unknown";
    }

    public int GetCardValue()
    {
        return cardInfo != null ? cardInfo.value : 0;
    }
}
