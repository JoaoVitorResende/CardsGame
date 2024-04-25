using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlacePoint : MonoBehaviour
{
    [SerializeField] private Card activeCard;

    [SerializeField] private bool isPlayerPoint;

    public void SetActiveCard(Card card) => activeCard = card;

    public Card GetActiveCard() => activeCard;

    public bool GetIsPlayerPoint() => isPlayerPoint;
}
