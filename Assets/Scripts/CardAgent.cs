using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class CardAgent : Agent, IPlayer
{
    public CardsManager opponentCardsManager;
    public OpponentCardsManager ownCardsManager;
    public Arena ownArena;
    public Arena opponentArena;
    public Arena sumoArena;
    public GameManager gameManager;

    public bool IsTurn { get; set; }
    public bool lastAttackedSumoArena { get; set; } // Bu satýrý ekleyin

    private GameObject selectedCard;

    public override void OnEpisodeBegin()
    {
        ownArena.ResetHealth();
        opponentArena.ResetHealth();
        sumoArena.ResetHealth();
        ownCardsManager.ResetCards();
        opponentCardsManager.ResetCards();
        IsTurn = true;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        foreach (var card in ownCardsManager.GetActiveCards())
        {
            var cardComponent = card.GetComponent<Card>();
            sensor.AddObservation(cardComponent.GetCardValue());
            sensor.AddObservation(cardComponent.GetCardType() == "Attack" ? 1 : 0);
        }

        foreach (var card in opponentCardsManager.GetActiveCards())
        {
            var cardComponent = card.GetComponent<Card>();
            sensor.AddObservation(cardComponent.GetCardValue());
            sensor.AddObservation(cardComponent.GetCardType() == "Attack" ? 1 : 0);
        }

        sensor.AddObservation(ownArena.currentHealth);
        sensor.AddObservation(opponentArena.currentHealth);
        sensor.AddObservation(sumoArena.currentHealth);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var discreteActions = actions.DiscreteActions;
        if (discreteActions.Length < 2)
        {
            return;
        }

        int cardIndex = discreteActions[0];
        int targetIndex = discreteActions[1];

        if (IsTurn)
        {
            PlayCard(cardIndex, targetIndex);
            IsTurn = false;
        }    
        
    }

    public void PlayCard(int cardIndex, int targetIndex)
    {
        var ownCards = ownCardsManager.GetActiveCards();

        if (cardIndex >= 0 && cardIndex < ownCards.Count)
        {
            selectedCard = ownCards[cardIndex]; // Seçilen kartý ayarla
            var cardComponent = selectedCard.GetComponent<Card>();
            string cardType = cardComponent.GetCardType();
            int cardValue = cardComponent.GetCardValue();

            Arena targetArena = null;

            if (cardType == "Heal")
            {
                targetArena = ownArena;
                if (ownArena.currentHealth == ownArena.maxHealth)
                {
                    AddReward(-0.1f); // Can doluyken heal kartý kullanýlýrsa ceza ver
                }
            }
            else if (cardType == "Attack")
            {
                targetArena = targetIndex == 0 ? opponentArena : sumoArena;
                if (targetArena == sumoArena)
                {
                    lastAttackedSumoArena = true; // Sumo arenayý hedef alýndý
                    AddReward(0.2f); // Sumo arenaya saldýrý yapýlýrsa ödül ver
                }
                if (targetArena == opponentArena)
                {
                    AddReward(0.3f);
                }
            }

            if (targetArena != null)
            {
                if (cardType == "Heal" && targetArena == ownArena)
                {
                    targetArena.ChangeHealth(cardValue);
                    gameManager.OpponentPlayedCard(cardType, cardValue);
                    AddReward(0.1f); // Heal kartý baþarýyla oynanýrsa ödül ver
                }
                else if (cardType == "Attack")
                {
                    targetArena.ChangeHealth(-cardValue);
                    gameManager.OpponentPlayedCard(cardType, cardValue);
                    AddReward(0.3f); // Attack kartý baþarýyla oynanýrsa ödül ver
                }

                ownCardsManager.CardPlayed(selectedCard);
                AddReward(0.1f); // Kart baþarýyla oynanýrsa ödül ver
            }
            else
            {
                AddReward(-0.1f);
            }
        }
        else
        {
            AddReward(-0.1f);
        }
    }

    public void OnTurnStart()
    {
        IsTurn = true;
    }

    public void OnTurnEnd()
    {
        IsTurn = false;
    }

    public void OnSumoChoiceMade(int choice)
    {
        if (choice == 0)
        {
            ownArena.ChangeHealth(4);
        }
        else if (choice == 1)
        {
            opponentArena.ChangeHealth(-4);
        }

        sumoArena.ResetHealth();
        IsTurn = true;
        AddReward(0.1f); // Sumo paneli doðru kullanýlýrsa ödül ver
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = Random.Range(0, ownCardsManager.GetActiveCards().Count);
        discreteActionsOut[1] = Random.Range(0, 3); // 0: opponentArena, 1: sumoArena
    }
}
