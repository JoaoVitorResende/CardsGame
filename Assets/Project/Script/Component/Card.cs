using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private CardsHolder cardsHolder;
    [SerializeField] private LayerMask whatIsDesktop;
    [SerializeField] private LayerMask placementPosition;
    [SerializeField] private Animator cardAnimator;
    [SerializeField] private bool isPlayer;
    [SerializeField] private CardPlacePoint assignedPlace;

    private bool isInHand;
    private bool isSelected;
    private bool isPressed;
    private Collider cardCollider;
    private int cardIdPosition;
    private Vector3 targetPoint;
    private Vector3 discardPoint = new Vector3 (8.56499958f, 0.397542864f, 1.37699997f);
    private Quaternion targetRotation;
    private Ray ray;
    private RaycastHit hit;
    private ChangeCardPosition changeCardPosition = new ChangeCardPosition();

    private void Awake() => cardsHolder = GameObject.Find("Hand").GetComponent<CardsHolder>();

    private void Start()
    {
        cardCollider = GetComponent<Collider>();
        CheckCardStartPosition();
    }
    
    private void Update()
    {
        MoveCardToTargetPoint();
        SelectedCardFunctions();
    }

    private void CheckCardStartPosition()
    {
        if(targetPoint == Vector3.zero)
            RecivePointToMove(transform.position, transform.rotation);
    }

    private void SelectedCardFunctions()
    {
        if (isSelected)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100f, whatIsDesktop))
                RecivePointToMove(hit.point + new Vector3(0f, 2f, 0f), Quaternion.identity);

            if (Input.GetMouseButtonDown(1))
                ReturnCardToHand();

            if (Input.GetMouseButtonDown(0) && !isPressed)
                CheckIfCardIsOnPlacePoint();
        }
        isPressed = false;
    }

    public void DamageCard(int damageValue)
    {
        int currentHealth = gameObject.GetComponent<CardUIComponents>().GetCardHealth();
        currentHealth -= damageValue;
        gameObject.GetComponent<CardUIComponents>().SetCardHealth(currentHealth);

        cardAnimator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            assignedPlace.SetActiveCard(null);
            RecivePointToMove(discardPoint, gameObject.transform.rotation);
            Destroy(gameObject,5f);
        }
    }

    private void CheckIfCardIsOnPlacePoint()
    {
        if (Physics.Raycast(ray, out hit, 100f, placementPosition) && BattleController.instance.GetBattlePhase() == TurnOrder.playerActive)
        {
            PlaceCardOnPoint();
            return;
        }
       
        ReturnCardToHand();
    }

    private void PlaceCardOnPoint()
    {
        CardPlacePoint SelectedPoint = hit.collider.GetComponent<CardPlacePoint>();

        if (SelectedPoint.GetActiveCard() == null && SelectedPoint.GetIsPlayerPoint())
        {
            CheckManaCost(SelectedPoint);
            return;
        }

        ReturnCardToHand();
    }

    private void CheckManaCost(CardPlacePoint value)
    {
        if(BattleController.instance.GetPlayerMana() >= GetComponent<CardUIComponents>().GetCardManaCost())
        {
            value.SetActiveCard(this);
            SetAssignedPlacePoint(value);
            RecivePointToMove(value.transform.position, Quaternion.identity);
            isInHand = false;
            isSelected = false;
            cardsHolder.RemoveCardFromHand(this);
            BattleController.instance.SpendPlayerMana(GetComponent<CardUIComponents>().GetCardManaCost());
            return;
        }

        PlayersStatusController.instance.SetChangedPlayerMana(true);
        ReturnCardToHand();
    }

    public void SetAssignedPlacePoint(CardPlacePoint placepoint)
    {
        assignedPlace = placepoint;
    }

    public void RecivePointToMove(Vector3 PointToMoveTo,Quaternion RotationToMove)
    {
        targetPoint = PointToMoveTo;
        targetRotation = RotationToMove;
    }

    private void ReturnCardToHand()
    {
        isSelected = false;
        cardCollider.enabled = true;
        RecivePointToMove(cardsHolder.GetCardsPositions(cardIdPosition), cardsHolder.GetMinimalPosition().rotation);
    }

    private void OnMouseOver()
    {
        if (isInHand && isPlayer)
            RecivePointToMove(cardsHolder.GetCardsPositions(cardIdPosition) + new Vector3(0f, 1f, .5f), Quaternion.identity);
    }

    private void OnMouseExit()
    {
        if (isInHand && isPlayer)
            RecivePointToMove(cardsHolder.GetCardsPositions(cardIdPosition), cardsHolder.GetMinimalPosition().rotation);
    }

    private void OnMouseDown()
    {
        if (isInHand && BattleController.instance.GetBattlePhase() == TurnOrder.playerActive && isPlayer)
        {
            isSelected = true;
            cardCollider.enabled = false;
            isPressed = true;
        }
    }

    private void MoveCardToTargetPoint() => changeCardPosition.MoveCardToPoint(transform.gameObject, targetPoint, targetRotation);

    public void AttackAnimation() => cardAnimator.SetTrigger("Attack");

    public void SetCardLocation(bool value) => isInHand = true;
    
    public void SetCardPosition(int value) => cardIdPosition = value;

    public int GetCardIdPosition() => cardIdPosition;

    public int GetCardAttack() => gameObject.GetComponent<CardUIComponents>().GetCardAttack();
}
