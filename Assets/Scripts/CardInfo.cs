using UnityEngine;

[CreateAssetMenu(menuName = "CardInfo")]
public class CardInfo : ScriptableObject
{
    public string type; // Kart�n t�r� (Attack, Heal vb.)
    public int value; // Kart�n de�eri
    public Color cardColor; // Kart�n rengi

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
