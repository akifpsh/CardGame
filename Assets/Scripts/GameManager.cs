using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Unity.MLAgents;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Arenas")]
    public Arena playerArena;
    public Arena opponentArena;
    public Arena sumoArena;

    [Header("Panels")]
    public SumoPanel sumoPanel;
    public TurnPanel turnPanel;
    public GameOverPanel gameOverPanel;

    [Header("Players")]
    public MonoBehaviour player1;
    public MonoBehaviour player2;

    [Header("Game Settings")]
    public float turnDuration = 10f;
    public float panelDisplayDuration = 3f;

    private bool player1CardPlayed = false;
    private bool player2CardPlayed = false;

    private int player1PlayedCardValue = 0;
    private int player2PlayedCardValue = 0;

    private string player1PlayedCard = "";
    private string player2PlayedCard = "";

    private bool gameOver = false; // GameOver bayraðý

    private void Awake()
    {
        sumoPanel.OpenPanel();
        sumoPanel.ClosePanel();
        turnPanel.OpenPanel();
        turnPanel.ClosePanel();
        gameOverPanel.OpenPanel();
        gameOverPanel.ClosePanel();
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (player1 == null || player2 == null)
        {
            return;
        }

        gameOverPanel.panel.SetActive(false); // Oyun sonu panelini baþlangýçta gizli tut

        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        while (!gameOver && playerArena.currentHealth > 0 && opponentArena.currentHealth > 0)
        {
            // Yeni tur baþlat
            yield return StartCoroutine(StartTurn());

            // Tur bitiþi
            yield return StartCoroutine(EndTurn());

            // Sumo panel kontrolü
            if (sumoArena.currentHealth <= 0)
            {
                yield return StartCoroutine(DisplaySumoPanel());
                sumoArena.ResetHealth(); // SumoArena canýný sýfýrlamak yerine yeniden yükle
            }

            // Turn panel kontrolü
            yield return StartCoroutine(DisplayTurnPanel());
            CheckGameOver();
        }

        if (gameOver)
        {
            gameOverPanel.OpenPanel(); // Oyun sonu panelini göster
            while (gameOverPanel.panel.activeSelf) // Oyun bittiðinde, gameOver paneli açýk kalýrken bekle
            {
                yield return null;
            }
        }
    }

    private IEnumerator StartTurn()
    {
        (player1 as IPlayer).OnTurnStart();
        (player2 as IPlayer).OnTurnStart();

        player1CardPlayed = false;
        player2CardPlayed = false;

        player1PlayedCardValue = 0;
        player2PlayedCardValue = 0;

        player1PlayedCard = "";
        player2PlayedCard = "";

        // Ajanlarýn karar vermesini saðla
        if (player1 is Agent agent1)
        {
            StartCoroutine(DelayedDecision(agent1));
        }
        if (player2 is Agent agent2)
        {
            StartCoroutine(DelayedDecision(agent2));
        }

        // Turn süresi boyunca veya iki kart da oynandýðýnda
        float elapsedTime = 0f;
        while (elapsedTime < turnDuration && (!player1CardPlayed || !player2CardPlayed))
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        (player1 as IPlayer).OnTurnEnd();
        (player2 as IPlayer).OnTurnEnd();
    }

    private IEnumerator DelayedDecision(Agent agent)
    {
        float delay = Random.Range(2f, 5f);
        yield return new WaitForSeconds(delay);
        agent.RequestDecision();
    }

    private IEnumerator EndTurn()
    {
        yield return null; // EndTurn artýk sadece boþ bir coroutine olarak kullanýlacak
    }

    private IEnumerator DisplaySumoPanel()
    {
        // Sumo arenaya saldýrý yapýlmýþ mý kontrol et
        bool player1AttackedSumo = player1PlayedCard == "Attack" && player1PlayedCardValue > 0 ;
        bool player2AttackedSumo = player2PlayedCard == "Attack" && player2PlayedCardValue > 0 ;

        // Sumo panelini açma kontrolü
        if (player1AttackedSumo)
        {
            if (player1 is Player)
            {
                sumoPanel.OpenPanel(player1 as Player);
            }
            else
            {
                int randomChoice = Random.Range(0, 2);
                (player1 as IPlayer).OnSumoChoiceMade(randomChoice);
            }
        }
        else if (player2AttackedSumo)
        {
            if (player2 is Player)
            {
                sumoPanel.OpenPanel(player2 as Player);
            }
            else
            {
                int randomChoice = Random.Range(0, 2);
                (player2 as IPlayer).OnSumoChoiceMade(randomChoice);
            }
        }
        else
        {
            Debug.Log("No valid sumo attack detected, sumo panel will not open.");
        }

        yield return new WaitForSeconds(panelDisplayDuration);
        sumoPanel.ClosePanel();
    }

    private IEnumerator DisplayTurnPanel()
    {
        // TurnPanel'i aç ve kartlarý göster
        turnPanel.OpenPanel();
        turnPanel.UpdateCards(player1PlayedCard, player2PlayedCard, player1PlayedCardValue, player2PlayedCardValue);
        yield return new WaitForSeconds(panelDisplayDuration);
        turnPanel.ClosePanel();
        CheckGameOver();
    }

    public void PlayerPlayedCard(string cardType, int cardValue)
    {
        player1CardPlayed = true;
        player1PlayedCard = cardType;
        player1PlayedCardValue = cardValue;
    }

    public void OpponentPlayedCard(string cardType, int cardValue)
    {
        player2CardPlayed = true;
        player2PlayedCard = cardType;
        player2PlayedCardValue = cardValue;
    }

    public void CheckGameOver()
    {
        if (playerArena.currentHealth <= 0 || opponentArena.currentHealth <= 0)
        {
            StopAllCoroutines();
            gameOver = true;

            if (player1 is Agent agent1)
            {
                agent1.AddReward(playerArena.currentHealth <= 0 ? -1.0f : 1.0f);
                agent1.EndEpisode();
            }
            if (player2 is Agent agent2)
            {
                agent2.AddReward(opponentArena.currentHealth <= 0 ? -1.0f : 1.0f);
                agent2.EndEpisode();
            }

            if (player1 is Player player1Instance)
            {
                player1Instance.cardsManager.ClearVisibleCards();
                gameOverPanel.OpenPanel();
            }
            else if (player2 is Player player2Instance)
            {
                player2Instance.cardsManager.ClearVisibleCards();
                gameOverPanel.OpenPanel();
            }
            else
            {
                StartCoroutine(RestartGameCoroutine());
            }
        }
    }

    private IEnumerator RestartGameCoroutine()
    {
        yield return new WaitForSeconds(2f); // Oyun sonu panelinin gösterilmesi için kýsa bir bekleme süresi
        RestartGame(); // Oyunu yeniden baþlat
    }

    public void RestartGame()
    {
        StopAllCoroutines(); // Tüm coroutine'leri durdur

        // Arenalarýn canlarýný sýfýrla
        playerArena.ResetHealth();
        opponentArena.ResetHealth();
        sumoArena.ResetHealth();

        // Diðer gerekli sýfýrlamalarý yap
        player1PlayedCardValue = 0;
        player2PlayedCardValue = 0;

        player1PlayedCard = "";
        player2PlayedCard = "";

        // Panel ve buton durumlarýný sýfýrla
        gameOverPanel.panel.SetActive(false);
        sumoPanel.panel.SetActive(false);
        turnPanel.panel.SetActive(false);

        // PlayerAgent ve CardAgent için sýfýrlama
        (player1 as Agent)?.EndEpisode();
        (player2 as Agent)?.EndEpisode();

        gameOver = false;

        // Oyunu yeniden baþlat
        StartCoroutine(GameLoop());
    }


    public void AgentPlayCard(IPlayer agent, int cardIndex, int targetIndex)
    {
        agent.PlayCard(cardIndex, targetIndex);
        agent.IsTurn = false;
    }

    public void AgentPlayCard(IPlayer agent, Card card, Arena arena)
    {
        int cardIndex = -1; // Bu deðiþkenin nasýl kullanýlacaðýný belirleyin
        if (Object.ReferenceEquals(agent, player1))
        {
            cardIndex = player1PlayedCard.CompareTo(player2PlayedCard);
        }
        else if (Object.ReferenceEquals(agent, player2))
        {
            cardIndex = player2PlayedCard.CompareTo(player1PlayedCard);
        }

        int targetIndex = 2; // Varsayýlan olarak sumoArena olarak belirleyin
        if (Object.ReferenceEquals(arena, playerArena))
        {
            targetIndex = 0;
        }
        else if (Object.ReferenceEquals(arena, opponentArena))
        {
            targetIndex = 1;
        }

        AgentPlayCard(agent, cardIndex, targetIndex);
    }
}

