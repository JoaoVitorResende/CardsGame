using System;
using System.Collections;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public static BattleController instance;

    private int maxMana = 12;
    private TurnOrder turn;
    private int turnFase = 0;
    private bool isDamaged = false;
    private Player player = new Player();
    private Enemy enemy = new Enemy();
    private int currentPlayermaxMana;
    private int currentEnemymaxMana;

    private void Awake() => instance = this;

    private void Start() => StartFightersAttributes();

    public void AdvanceTurn()
    {
        turnFase++;

        if(turnFase >= Enum.GetValues(typeof(TurnOrder)).Length)
            turnFase = 0;

        SwitchTurnState(turnFase);
    }

    private void SwitchTurnState(int value)
    {
        switch (value)
        {
            case 0:
                turn = TurnOrder.playerActive;
                DeckController.instance.DrawCardNewPlayerPhase();
                Debug.Log("player puxa uma carta");
                break;

            case 1:
                turn = TurnOrder.playerCardAttacks;
                CardPointsController.instance.CardAttacks();
                Debug.Log("Player ataca");
                break;

            case 2:
                turn = TurnOrder.enemyActive;
                Debug.Log("inimigo começa a fazer algo");
                EnemyController.instance.StartAction();
                break;

            case 3:
                turn = TurnOrder.enemyCardAttacks;
                CardPointsController.instance.CardAttacks();
                Debug.Log("inimigo ataca");
                FillEnemyMana(2);
                break;
        }
    }

    private void StartFightersAttributes()
    {
        player.mana = 4;
        player.health = 30;
        enemy.health = 30;
        enemy.mana = 50;
    } 

    public void SpendEnemyMana(int value)
    {
        PlayersStatusController.instance.SetChangedEnemyMana(true);
        enemy.mana -= value;
        if (enemy.mana < 0)
            enemy.mana = 0;
    }

    public void SpendPlayerMana(int value)
    {
        PlayersStatusController.instance.SetManaHasChanged(true);
        player.mana -= value;
        if(player.mana < 0)
            player.mana = 0;
    }

    public void FillPlayerMana(int value)
    {
        if(player.mana <= maxMana)
        {
            PlayersStatusController.instance.SetManaHasChanged(true);
            player.mana += value;
        }

        AdvanceTurn();
    }

    private void FillEnemyMana(int value)
    {
        if (enemy.mana <= maxMana)
        {
            PlayersStatusController.instance.SetChangedEnemyMana(true);
            enemy.mana += value;
        }
    }

    public void DamageHealth(int value, string fighter) => WhichFighterGotDamage(value, fighter);

    private void WhichFighterGotDamage(int value,string fighter)
    {
        if (fighter.Equals("player"))
        {
            player.health = CheckHealth(player.health, value);
            DamageIndicator.instance.SetDamageGiven(value.ToString(),true);
            PlayersStatusController.instance.SetChangedPlayerHealth(true);
        }
        else
        {
            enemy.health = CheckHealth(enemy.health, value);
            DamageIndicator.instance.SetDamageGiven(value.ToString(),false);
            PlayersStatusController.instance.SetChangedEnemyHealth(true);
        }

        isDamaged = false;
    }

    private int CheckHealth(int health,int value)
    {
        if (health > 0 && !isDamaged)
        {
            health -= value;
            isDamaged = true;
            CheckHealth(health, value);
        }
        else
        {
            StartCoroutine("StartEndBattle");
        }

        return health;
    }

    IEnumerator StartEndBattle()
    {
        yield return new WaitForSeconds(2f);
        //game over ou
    }

    public TurnOrder GetBattlePhase() => turn;

    public int GetPlayerMana() => player.mana;

    public int GetPlayerHealth() => player.health;

    public int GetEnemyHealth() => enemy.health;

    public int GetEnemyMana() => enemy.mana;
}
