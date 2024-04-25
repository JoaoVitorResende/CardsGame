using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCardPosition : MonoBehaviour
{
    private float moveSpeed = 5f;
    private float rotationSpeed = 540f;

    public void MoveCardToPoint(GameObject card,Vector3 targetPoint, Quaternion targetRotation)
    {
        card.transform.position = Vector3.Lerp(card.transform.position, targetPoint, moveSpeed * Time.deltaTime);
        card.transform.rotation = Quaternion.RotateTowards(card.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        Vector3 forward = card.transform.TransformDirection(Vector3.forward) * 10;
    }
}
