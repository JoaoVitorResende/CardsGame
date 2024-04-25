using UnityEngine;

public class PlayersStatusController : MonoBehaviour
{
    public static PlayersStatusController instance;

    private bool isManaChanged;
    private bool isManaEnough;
    private bool isPlayerHealthChanged;
    private bool isEnemyHealthChanged;
    private bool isEnemyManaChanged;

    private void Awake() => instance = this;

    public void SetChangedPlayerMana(bool value) => isManaEnough = value;

    public void SetChangedPlayerHealth(bool value) => isPlayerHealthChanged = value;

    public void SetChangedEnemyHealth(bool value) => isEnemyHealthChanged = value;

    public void SetChangedEnemyMana(bool value) => isEnemyManaChanged = value;

    public void SetManaHasChanged(bool value) => isManaChanged = value;

    public bool GetChangeMana() => isManaChanged;

    public bool GetManaCondition() => isManaEnough;

    public bool GetPlayerHealthChanged() => isPlayerHealthChanged;

    public bool GetEnemyHealthChanged() => isEnemyHealthChanged;

    public bool GetEnemyManaChanged() => isEnemyManaChanged;

}
