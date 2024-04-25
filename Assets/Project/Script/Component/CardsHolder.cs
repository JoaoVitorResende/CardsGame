using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsHolder : MonoBehaviour
{
    [SerializeField] private Transform minimalPosition;
    [SerializeField] private Transform maxPosition;
    [SerializeField] private List<Card> heldCards = new List<Card>();

    public static CardsHolder instance;

    private List<Vector3> cardPositions = new List<Vector3>();

    private void Awake() => instance = this;
    
    void Start() => SetCardPositionInHand();

    private void SetCardPositionInHand()
    {
        cardPositions.Clear();

        Vector3 distanceBetweenPoints = Vector3.zero;

        if(heldCards.Count > 1)
        {
            distanceBetweenPoints = (maxPosition.position - minimalPosition.position) / (heldCards.Count - 1);
        }

        for(int i = 0; i < heldCards.Count;i++)
        {
            cardPositions.Add(minimalPosition.position + (distanceBetweenPoints * i));
            heldCards[i].RecivePointToMove(cardPositions[i], minimalPosition.rotation);
            heldCards[i].SetCardLocation(true);
            heldCards[i].SetCardPosition(i);
        } 
    }

    public void RemoveCardFromHand(Card Card)
    {
        if(heldCards[Card.GetCardIdPosition()] == Card)
        {
            heldCards.RemoveAt(Card.GetCardIdPosition());
        }

        SetCardPositionInHand();
    }

    public void AddCardToHand(Card value)
    {
        heldCards.Add(value);
        SetCardPositionInHand();
    }

    public Vector3 GetCardsPositions(int value) => cardPositions[value];

    public Transform GetMinimalPosition() => minimalPosition;
}
