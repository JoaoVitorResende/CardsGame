using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageIndicator : MonoBehaviour
{
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject enemyDameIndicator;
    [SerializeField] private GameObject playerDameIndicator;
    private float lifeTime = 1f;
    private bool isDamageTaken = false;
    public static DamageIndicator instance;

    void Start()
    {
        instance = this;
    }

    private void Update()
    {
        MoveDamagePointsBasedOnTime();
    }

    private void MoveDamagePointsBasedOnTime()
    {
        if(isDamageTaken)
            GetComponent<RectTransform>().anchoredPosition += new Vector2(0f, -moveSpeed * Time.deltaTime);
    }

    public void SetDamageGiven(string damageValue, bool isPlayer)
    {
        damageText.text = damageValue;
        isDamageTaken = true;
        ChangeIndicatorPosition(isPlayer);
        GetComponent<CanvasGroup>().alpha = 1;
        StartCoroutine("DeactivateDamageIndicator");
    }

    private void ChangeIndicatorPosition(bool isPlayer)
    {
        if (isPlayer)
        {
            transform.position = playerDameIndicator.transform.position;
            return;
        }

        transform.position = enemyDameIndicator.transform.position;
    }

    private IEnumerator DeactivateDamageIndicator()
    {
        yield return new WaitForSeconds(lifeTime);
        RestartGameObjectConfiguration();
    }

    private void RestartGameObjectConfiguration()
    {
        isDamageTaken = false;
        GetComponent<CanvasGroup>().alpha = 0;
        return;
    }
}
