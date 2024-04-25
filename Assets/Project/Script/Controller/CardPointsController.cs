using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPointsController : MonoBehaviour
{
    public static CardPointsController instance;

    [SerializeField] private List<CardPlacePoint> playerCardPoints;
    [SerializeField] private List<CardPlacePoint> enemyCardPoints;

    private IEnumerator coroutine;
    private CardAttack cardAttack = new CardAttack();

    private void Awake() => instance = this;

    public void CardAttacks() => CardsAttack();
   
    private void CardsAttack()
    {
        if (BattleController.instance.GetBattlePhase() == TurnOrder.playerCardAttacks)
        {
            coroutine = UseCardsOnList(playerCardPoints.Count, true);
            StartCoroutine(coroutine);
        }

        if(BattleController.instance.GetBattlePhase() == TurnOrder.enemyCardAttacks)
        {
            coroutine = UseCardsOnList(enemyCardPoints.Count, false);
            StartCoroutine(coroutine);
        }
        BattleController.instance.AdvanceTurn();
    }

    IEnumerator UseCardsOnList(int cardsCount, bool value)
    {
        yield return new WaitForSeconds(.25f);

        for (int i = 0; i < cardsCount; i++)
        {
            if(value)
            {
                cardAttack.CheckIfPlayerPlaceIsNull(playerCardPoints[i], enemyCardPoints[i]);
            }
            else
            {
                cardAttack.CheckIfEnemyPlaceIsNull(playerCardPoints[i], enemyCardPoints[i]);
            }
           
            yield return new WaitForSeconds(.25f);
        }
    }

    public IEnumerator TimetoWait()
    {
        yield return new WaitForSeconds(.25f);
    }

    public List<CardPlacePoint> GetEnemyPlacePoints() => enemyCardPoints;
}
