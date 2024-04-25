using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card", order = 1)]
public class CardScriptableObject : ScriptableObject
{
    [SerializeField] private int currentHealth;
    [SerializeField] private int attackPower;
    [SerializeField] private int manaCost;
    [SerializeField] private string cardName;
    [SerializeField] [TextArea] private string cardDescription;
    [SerializeField] [TextArea] private string cardLore;
    [SerializeField] private Sprite characterSprite;
    [SerializeField] private Sprite backgroundSprite;

    public int GetCurrentHealth() => currentHealth;

    public int GetAttackPower() => attackPower;

    public int GetManaCost() => manaCost;

    public string GetCardName() => cardName;

    public string GetCardDescription() => cardDescription;

    public string GetCardLore() => cardLore;

    public Sprite GetCharacterSprite() => characterSprite;

    public Sprite GetBackgroundSprite() => backgroundSprite;
    
}
