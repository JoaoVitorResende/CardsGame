using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour
{
    public static DeckController instance;

    [SerializeField] private List<CardScriptableObject> deckToUse = new List<CardScriptableObject>();

    [SerializeField] private GameObject cardToSpawn;

    private List<CardScriptableObject> activeCards = new List<CardScriptableObject>();

    private IEnumerator coroutine;

    private void Awake() => instance = this;

    private void Start()
    {
        SetupDeck(deckToUse);
        coroutine = DrawMultipleCards(5);
        StartCoroutine(coroutine);
    }

    public List<CardScriptableObject> SetupDeck(List<CardScriptableObject> deck)
    {
        activeCards.Clear();
        List<CardScriptableObject> tempDeck = new List<CardScriptableObject>();
        tempDeck.AddRange(deck);

        while(tempDeck.Count > 0)
        {
            SelectRamdomCard(tempDeck);
        }

        return activeCards;
    }

    private void SelectRamdomCard(List<CardScriptableObject> tempDeck)
    {
        int selected = Random.Range(0, tempDeck.Count);
        activeCards.Add(tempDeck[selected]);
        tempDeck.RemoveAt(selected);
    }

    private void DrawCardToHand()
    {
        if(activeCards.Count == 0)
            SetupDeck(deckToUse);

        CardsHolder.instance.AddCardToHand(CreateNewCardComponents(cardToSpawn, transform.position, transform.rotation).GetComponent<Card>());
    }

    public GameObject CreateNewCardComponents(GameObject newCardToSpawn,Vector3 newPosition, Quaternion newRotation, CardScriptableObject cardSO = null)
    {
        GameObject newCard = Instantiate(newCardToSpawn, newPosition, newRotation);
        newCard.GetComponent<CardUIComponents>().SetCardScriptableObject(SetCardScriptableObject());
        newCard.GetComponent<CardUIComponents>().SetupCard();
        activeCards.RemoveAt(0);
        return newCard;
    }

    private CardScriptableObject SetCardScriptableObject(CardScriptableObject cardSO = null)
    {
        if(cardSO == null)
            return activeCards[0];

        return cardSO;
    }

    public void DrawCardNewPlayerPhase() => DrawCardToHand();

    public void DrawCardForMana()
    {
        DrawCardToHand();
        BattleController.instance.SpendPlayerMana(2);
    }

    IEnumerator DrawMultipleCards(int value)
    {
        for (int i = 0; i < value; i++)
        {
            DrawCardToHand();
            yield return new WaitForSeconds(0.5f);
        }
    }
}
