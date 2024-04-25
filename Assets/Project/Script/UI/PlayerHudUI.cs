using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHudUI : MonoBehaviour
{
    [SerializeField] private TMP_Text playerManaText;
    [SerializeField] private TMP_Text playerHealthText;
    [SerializeField] private TMP_Text enemyHealthText;
    [SerializeField] private TMP_Text enemyManaText;
    [SerializeField] private GameObject manaWarningText;
    [SerializeField] private Button playerEndTurnButton;
    [SerializeField] private Button drawCardButton;

    private void FixedUpdate()
    {
        CheckIfPlayerHealthChanged();
        CheckIfEnemyHealthChanged();
        CheckIfEnemyManaChanged();
        CheckIfManaIsChanged();
        CheckBattlePhase();
    }

    private void CheckBattlePhase()
    {
        if(BattleController.instance.GetBattlePhase() == TurnOrder.playerActive && !playerEndTurnButton.interactable)
            ChangeButtonInteractivity();
    }

    private void CheckIfPlayerHealthChanged()
    {
        if (PlayersStatusController.instance.GetPlayerHealthChanged())
        {
            SetPlayerHealth();
            PlayersStatusController.instance.SetChangedPlayerHealth(false);
        }
    }

    private void CheckIfEnemyHealthChanged()
    {
        if (PlayersStatusController.instance.GetEnemyHealthChanged())
        {
            SetEnemyHealth();
            PlayersStatusController.instance.SetChangedEnemyHealth(false);
        }
    }

    private void CheckIfEnemyManaChanged()
    {
        if (PlayersStatusController.instance.GetEnemyManaChanged())
        {
            SetEnemyManaText();
            PlayersStatusController.instance.SetChangedEnemyMana(false);
        }
    }

    private void CheckIfManaIsChanged()
    {
        if (PlayersStatusController.instance.GetChangeMana())
        {
            SetPlayerManaText();
            PlayersStatusController.instance.SetManaHasChanged(false);
        }

        CheckIfNeedManaWarning();
    }

    private void CheckIfNeedManaWarning()
    {
        if(PlayersStatusController.instance.GetManaCondition())
            StartCoroutine("ShowWarning");
    }

    public void EndPlayerTurn()
    {
        ChangeButtonInteractivity();
        BattleController.instance.FillPlayerMana(2);
    }

    private void ChangeButtonInteractivity()
    {
        playerEndTurnButton.interactable = !playerEndTurnButton.interactable;
        drawCardButton.interactable = !drawCardButton.interactable;
    }

    public void DrawCard()
    {
        if (BattleController.instance.GetPlayerMana() >= 2)
            DeckController.instance.DrawCardForMana();
        else
            StartCoroutine("ShowWarning");
    }

    IEnumerator ShowWarning()
    {
        manaWarningText.SetActive(true);
        PlayersStatusController.instance.SetChangedPlayerMana(false);
        yield return new WaitForSeconds(2f);
        manaWarningText.SetActive(false);
    }

    private void SetPlayerManaText() => playerManaText.text = "Player Mana : " + BattleController.instance.GetPlayerMana().ToString();

    private void SetPlayerHealth() => playerHealthText.text = "Player Health: " + BattleController.instance.GetPlayerHealth().ToString();

    private void SetEnemyHealth() => enemyHealthText.text = "Enemy Health: " + BattleController.instance.GetEnemyHealth().ToString();

    private void SetEnemyManaText() => enemyManaText.text = "Enemy Mana: " + BattleController.instance.GetEnemyMana().ToString();
}
