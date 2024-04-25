using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private List<CardScriptableObject> deckToUse = new List<CardScriptableObject>();
    [SerializeField] private Transform cardSpawnPoint;
    [SerializeField] private GameObject cardToSpawn;
    [SerializeField] private int startHandSize;
    public AiType enemyAiType;
    public static EnemyController instance;
    private List<CardScriptableObject> activeCards = new List<CardScriptableObject>();
    private List<CardScriptableObject> cardsInHands = new List<CardScriptableObject>();
    [SerializeField] private CardScriptableObject selectedCard = null;

    private void Awake() => instance = this;

    private void Start()
    {
        DeckController.instance.SetupDeck(deckToUse);
        if (enemyAiType != AiType.placeFromDeck)
            SetupHand();
    }

    public void StartAction() => StartCoroutine("EnemyAction");

    private IEnumerator EnemyAction()
    {
        if (activeCards.Count == 0)
            DeckController.instance.SetupDeck(deckToUse);
        CreateCardPoints();
        yield return new WaitForSeconds(.5f);
        BattleController.instance.AdvanceTurn();
    }

    private void CreateCardPoints()
    {
        List<CardPlacePoint> cardPoints = new List<CardPlacePoint>();
        cardPoints.AddRange(CardPointsController.instance.GetEnemyPlacePoints());
        SelectRandonEnemyPlacePoint(cardPoints);
    }

    private void SelectRandonEnemyPlacePoint(List<CardPlacePoint> cardPoints)
    {
        int randomPoint = Random.Range(0, cardPoints.Count);
        CardPlacePoint selectedPoint = cardPoints[randomPoint];

        if (enemyAiType == AiType.placeFromDeck || enemyAiType == AiType.handRandomPlace)
        {
            cardPoints.Remove(selectedPoint);

            while (selectedPoint != null && cardPoints.Count > 0)
            {
                randomPoint = Random.Range(0, cardPoints.Count);
                selectedPoint = cardPoints[randomPoint];
                cardPoints.RemoveAt(randomPoint);
            }

            SwitchEnemyAi(selectedPoint);
        }
    }

    private void SwitchEnemyAi(CardPlacePoint selectedPoint)
    {
        switch (enemyAiType)
        {
            case AiType.placeFromDeck:
                InstanceNewEnemyCard(selectedPoint);
                break;
        }
    }

    private void InstanceNewEnemyCard(CardPlacePoint selectedPoint, CardScriptableObject cardSO = null)
    {
        if (selectedPoint.GetActiveCard() == null)
        {
            Card newCard = DeckController.instance.CreateNewCardComponents(cardToSpawn, cardSpawnPoint.position, cardSpawnPoint.rotation, SetCardScriptableObject()).GetComponent<Card>();
            newCard.RecivePointToMove(selectedPoint.transform.position, selectedPoint.transform.rotation);
            selectedPoint.SetActiveCard(newCard);
            newCard.SetAssignedPlacePoint(selectedPoint);
            BattleController.instance.SpendEnemyMana(newCard.GetComponent<CardUIComponents>().GetCardManaCost());
            PlayersStatusController.instance.SetChangedEnemyMana(true);
        }
    }

    private CardScriptableObject SetCardScriptableObject(CardScriptableObject cardSO = null)
    {
        if(cardSO == null)
        {
            return null;
        }
        return cardSO;
    }

    private void SetupHand()
    {
        for (int i = 0; i < startHandSize; i++)
        {
            if (activeCards.Count == 0)
            {
                DeckController.instance.SetupDeck(deckToUse);
            }
            cardsInHands.Add(activeCards[0]);
            activeCards.RemoveAt(0);
        }
    }

    public void SetupDeck()
    {
        activeCards.Clear();

        List<CardScriptableObject> tempDeck = new List<CardScriptableObject>();
        tempDeck.AddRange(deckToUse);

        int iterations = 0;
        while (tempDeck.Count > 0 && iterations < 500)
        {
            int selected = Random.Range(0, tempDeck.Count);
            activeCards.Add(tempDeck[selected]);
            tempDeck.RemoveAt(selected);
            iterations++;
        }
        SetupHand();
    }

    private CardScriptableObject SelectCardToPlay()
    {
        CardScriptableObject cardToPlay = null;
        List<CardScriptableObject> cardsToPlay = new List<CardScriptableObject>();

        foreach (CardScriptableObject card in cardsInHands)
        {
            if (card.GetManaCost() <= BattleController.instance.GetEnemyMana())
                cardsToPlay.Add(card);
        }

        if (cardsToPlay.Count > 0)
        {
            int selected = Random.Range(0, cardsToPlay.Count);
            cardToPlay = cardsToPlay[selected];
        }

        return cardToPlay;
    }
}
