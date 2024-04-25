using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardUIComponents : MonoBehaviour
{
    [SerializeField] private CardScriptableObject cardScriptableObject;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text attackText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private TMP_Text cardName;
    [SerializeField] private TMP_Text actionDescription;
    [SerializeField] private TMP_Text loreText;
    [SerializeField] private Image character;
    [SerializeField] private Image background;

    private int currentHealth;
    private int currentAttack;
    private int currentMana;
    
    private void Start() => ChangeCardsAtributes();
  
    private void ChangeCardsAtributes()
    {
        currentHealth = cardScriptableObject.GetCurrentHealth();
        currentAttack = cardScriptableObject.GetAttackPower();
        currentMana = cardScriptableObject.GetManaCost();
        ChangeCardText();
        ChangeCardSprite();
    }

    private void ChangeCardText()
    {
        UpdateCardDisplayAttributes();
        cardName.text = cardScriptableObject.GetCardName();
        actionDescription.text = cardScriptableObject.GetCardDescription();
        loreText.text = cardScriptableObject.GetCardLore();
    }

    private void ChangeCardSprite()
    {
        character.sprite = cardScriptableObject.GetCharacterSprite();
        background.sprite = cardScriptableObject.GetBackgroundSprite();
    }

    private void UpdateCardDisplayAttributes()
    {
        healthText.text = currentHealth.ToString();
        attackText.text = currentAttack.ToString();
        costText.text = currentMana.ToString();
    }

    public void SetCardScriptableObject(CardScriptableObject value) => cardScriptableObject = value;

    public CardScriptableObject GetCardScriptableObject() => cardScriptableObject;

    public int GetCardManaCost() => currentMana;

    public int GetCardHealth() => currentHealth;

    public int GetCardAttack() => currentAttack;

    public void SetCardHealth(int value)
    {
        currentHealth = value;
        UpdateCardDisplayAttributes();
    }

    public void SetupCard() => ChangeCardsAtributes();
}
