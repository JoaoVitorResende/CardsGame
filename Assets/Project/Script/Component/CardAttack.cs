using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAttack : MonoBehaviour
{
    public void CheckIfPlayerPlaceIsNull(CardPlacePoint playerCardPlacePoint, CardPlacePoint enemyCardPlacePoint)
    {
        if (playerCardPlacePoint.GetActiveCard() != null)
            PlayerCardAttack(playerCardPlacePoint, enemyCardPlacePoint);
    }

    public void CheckIfEnemyPlaceIsNull(CardPlacePoint playerCardPlacePoint, CardPlacePoint enemyCardPlacePoint)
    {
        if (enemyCardPlacePoint.GetActiveCard() != null)
            EnemyCardAttack(playerCardPlacePoint, enemyCardPlacePoint);
    }

    private void PlayerCardAttack(CardPlacePoint playerCardPlacePoint, CardPlacePoint enemyCardPlacePoint)
    {
        if (enemyCardPlacePoint.GetActiveCard() != null)
        {
            enemyCardPlacePoint.GetActiveCard().DamageCard(playerCardPlacePoint.GetActiveCard().GetCardAttack());
        }
        else
        {
            BattleController.instance.DamageHealth(playerCardPlacePoint.GetActiveCard().GetCardAttack(),"enemy");
        }
        playerCardPlacePoint.GetActiveCard().AttackAnimation();
    }

    private void EnemyCardAttack(CardPlacePoint playerCardPlacePoint, CardPlacePoint enemyCardPlacePoint)
    {
        if (playerCardPlacePoint.GetActiveCard() != null)
        {
            playerCardPlacePoint.GetActiveCard().DamageCard(enemyCardPlacePoint.GetActiveCard().GetCardAttack());
        }
        else
        {
            BattleController.instance.DamageHealth(enemyCardPlacePoint.GetActiveCard().GetCardAttack(),"player");
        }
        enemyCardPlacePoint.GetActiveCard().AttackAnimation();
    }
}
