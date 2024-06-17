using System.Collections.Generic;
using UnityEngine;

public class PlayerCardsManager : MonoBehaviour
{
    public GameObject[] cards; // Kart prefablarý
    public Transform[] cardPositions; // Kartlarýn yerleþtirileceði konumlarýn listesi
    public List<GameObject> visibleCards = new List<GameObject>(); // Görünür kartlar
    public List<GameObject> hiddenCards = new List<GameObject>(); // Gizli kartlar
    public Dictionary<GameObject, Transform> cardToPositionMap = new Dictionary<GameObject, Transform>(); // Kartlarýn pozisyonlarýný takip etmek için

    private void Start()
    {
        InitializeCards();
        DrawInitialCards();
    }

    public void InitializeCards()
    {
        if (cards == null || cards.Length == 0)
        {
            Debug.LogError($"{name} - Cards array is not assigned or empty in the inspector.");
            return;
        }

        foreach (GameObject card in cards)
        {
            card.gameObject.SetActive(false);
        }

        visibleCards.Clear();
        hiddenCards.Clear();
        cardToPositionMap.Clear();
    }

    public void DrawInitialCards()
    {
        List<int> selectedIndices = new List<int>();

        while (selectedIndices.Count < 3)
        {
            int randomIndex = Random.Range(0, cards.Length);
            if (!selectedIndices.Contains(randomIndex))
            {
                selectedIndices.Add(randomIndex);
            }
        }

        for (int i = 0; i < 3; i++)
        {
            GameObject selectedCard = cards[selectedIndices[i]];
            selectedCard.SetActive(true);

            Transform selectedPosition = GetRandomAvailableSpawnPoint();
            if (selectedPosition == null)
            {
                Debug.LogError($"{name} - No available spawn points.");
                return;
            }

            selectedCard.transform.position = selectedPosition.position;
            selectedCard.transform.rotation = selectedPosition.rotation;

            visibleCards.Add(selectedCard);
            cardToPositionMap[selectedCard] = selectedPosition;
        }

        for (int i = 0; i < cards.Length; i++)
        {
            if (!selectedIndices.Contains(i))
            {
                GameObject hiddenCard = cards[i];
                hiddenCard.SetActive(false);
                hiddenCards.Add(hiddenCard);
            }
        }
    }

    private Transform GetRandomAvailableSpawnPoint()
    {
        List<Transform> availablePositions = new List<Transform>(cardPositions);
        foreach (Transform position in cardToPositionMap.Values)
        {
            availablePositions.Remove(position);
        }

        if (availablePositions.Count == 0)
            return null;

        int randomIndex = Random.Range(0, availablePositions.Count);
        return availablePositions[randomIndex];
    }

    public void CardPlayed(GameObject playedCard)
    {
        if (playedCard == null)
        {
            Debug.LogError($"{name} - CardPlayed: playedCard is null.");
            return;
        }

        bool cardRemoved = visibleCards.Remove(playedCard);

        if (!cardRemoved)
        {
            Debug.LogError($"{name} - CardPlayed: Failed to remove playedCard from visibleCards.");
            return;
        }

        Transform previousPosition = cardToPositionMap[playedCard];
        cardToPositionMap.Remove(playedCard);
        playedCard.SetActive(false);
        hiddenCards.Add(playedCard);

        if (hiddenCards.Count > 0)
        {
            int randomIndex = Random.Range(0, hiddenCards.Count);
            GameObject newCard = hiddenCards[randomIndex];

            hiddenCards.RemoveAt(randomIndex);
            newCard.SetActive(true);
            newCard.transform.position = previousPosition.position;
            newCard.transform.rotation = previousPosition.rotation;
            visibleCards.Add(newCard);
            cardToPositionMap[newCard] = previousPosition;
        }
    }

    public List<GameObject> GetActiveCards()
    {
        return visibleCards;
    }

    public void UpdateCards()
    {
        foreach (var card in visibleCards)
        {
            if (!card.activeSelf)
            {
                card.SetActive(true);
                Transform spawnPoint = GetRandomAvailableSpawnPoint();
                if (spawnPoint != null)
                {
                    card.transform.position = spawnPoint.position;
                    card.transform.rotation = spawnPoint.rotation;
                }
            }
        }
    }

    public void ResetCards()
    {
        InitializeCards();
        DrawInitialCards();
    }
    public void ClearVisibleCards()
    {
        visibleCards.Clear();
    }
}
