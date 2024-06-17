using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour, IPlayer
{
    public CardsManager cardsManager;
    public Arena playerArena;
    public Arena opponentArena;
    public Arena sumoArena;
    public GameManager gameManager;

    public AudioClip cardSelectSound; // Kart se�ildi�inde oynat�lacak ses dosyas�
    private AudioSource audioSource;
    public bool IsTurn { get; set; }
    public bool lastAttackedSumoArena { get; set; } // Bu sat�r� ekleyin

    private GameObject selectedCard;
    private Vector3 originalPosition;
    public float shakeAmount = 0.1f;
    public float shakeDuration = 0.5f;
    private Coroutine shakeCoroutine;

    private void Start()
    {
        cardsManager = FindObjectOfType<CardsManager>();
        playerArena = ArenaManager.Instance.playerArena;
        opponentArena = ArenaManager.Instance.opponentArena;
        sumoArena = ArenaManager.Instance.sumoArena;
        gameManager = FindObjectOfType<GameManager>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (IsTurn && Input.GetMouseButtonDown(0))
        {
            HandleCardSelection();
            HandleCardPlay();
        }
    }

    private void HandleCardSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            var card = hit.collider.GetComponent<Card>();
            if (card != null && cardsManager.GetActiveCards().Contains(card.gameObject))
            {
                if (selectedCard != null)
                {
                    selectedCard.transform.localPosition = originalPosition; // �nceki kart� orijinal konumuna d�nd�r
                }

                selectedCard = card.gameObject;
                originalPosition = selectedCard.transform.localPosition; // Yeni kart�n orijinal konumunu kaydet

                if (shakeCoroutine != null)
                {
                    StopCoroutine(shakeCoroutine);
                }
                shakeCoroutine = StartCoroutine(ShakeCard(selectedCard));

                if (audioSource != null && cardSelectSound != null)
                {
                    audioSource.PlayOneShot(cardSelectSound);
                }
            }
        }
    }

    private IEnumerator ShakeCard(GameObject card)
    {
        float elapsed = 0.0f;
        while (elapsed < shakeDuration)
        {
            float y = Random.Range(-0.05f, 0.05f) * shakeAmount; // Sadece yukar� a�a�� hareket
            card.transform.localPosition = new Vector3(originalPosition.x, originalPosition.y + y, originalPosition.z);
            elapsed += Time.deltaTime;
            yield return new WaitForSeconds(0.1f); // Her titreme aras�nda bekleme s�resi ekliyoruz
        }
        card.transform.localPosition = originalPosition;
    }

    private void HandleCardPlay()
    {
        if (selectedCard != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                var arena = hit.collider.GetComponent<Arena>();
                if (arena != null)
                {
                    var cardComponent = selectedCard.GetComponent<Card>();
                    if (cardComponent.GetCardType() == "Attack" &&
                        (arena == opponentArena || arena == sumoArena))
                    {
                        PlayCard(cardComponent, arena);
                    }
                    else if (cardComponent.GetCardType() == "Heal" && arena == playerArena)
                    {
                        PlayCard(cardComponent, arena);
                    }
                }
            }
        }
    }

    private void PlayCard(Card card, Arena arena)
    {
        if (card.GetCardType() == "Attack")
        {
            arena.ChangeHealth(-card.GetCardValue());
            if (arena == sumoArena)
            {
                lastAttackedSumoArena = true;
            }
        }
        else if (card.GetCardType() == "Heal")
        {
            arena.ChangeHealth(card.GetCardValue());
        }

        cardsManager.CardPlayed(selectedCard);
        gameManager.PlayerPlayedCard(card.GetCardType(), card.GetCardValue());
        selectedCard = null;
        IsTurn = false; // Kart oynand�ktan sonra oyuncunun s�ras� biter
    }

    public void PlayCard(int cardIndex, int targetIndex)
    {
        // Bu metot, ajanlarla uyumluluk sa�lamak i�in bo� b�rak�labilir
    }

    public void OnSumoChoiceMade(int choice)
    {
    }

    public void OnTurnStart()
    {
        IsTurn = true;
    }

    public void OnTurnEnd()
    {
        IsTurn = false;
    }
}
