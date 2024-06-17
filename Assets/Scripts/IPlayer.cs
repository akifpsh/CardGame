public interface IPlayer
{
    bool IsTurn { get; set; }
    bool lastAttackedSumoArena { get; set; } // Bu satýrý ekleyin
    void OnTurnStart();
    void OnTurnEnd();
    void PlayCard(int cardIndex, int targetIndex);
    void OnSumoChoiceMade(int choice);
}
