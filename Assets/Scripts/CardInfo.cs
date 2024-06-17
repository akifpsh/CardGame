using UnityEngine;

[CreateAssetMenu(menuName = "CardInfo")]
public class CardInfo : ScriptableObject
{
    public string type; // Kartýn türü (Attack, Heal vb.)
    public int value; // Kartýn deðeri
    public Color cardColor; // Kartýn rengi

    public int minValue = 1;
    public int maxValue = 3;

    private void OnEnable()
    {
        RandomizeValue();
    }

    private void RandomizeValue()
    {
        value = Random.Range(minValue, maxValue + 1);
    }
}
