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
            // Kartýn rengini veya diðer görsel özelliklerini ayarlayabiliriz
            // Örneðin: kartRenderer.material.color = cardInfo.cardColor;
        }
    }

    public void SetVisibility(bool isVisible)
    {
        gameObject.SetActive(isVisible);

        if (isVisible)
        {
            SetOpacity(1.0f); // Tamamen görünür yap
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
