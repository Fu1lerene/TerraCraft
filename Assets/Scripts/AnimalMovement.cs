using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.ScrollRect;

public class AnimalMovement : MonoBehaviour
{
    private AnimalStats anSt;
    private Vector3 movement;
    private SpriteRenderer sr;
    private float prevX;
    private float currX;
    private float timerType;
    private float timerTypeDelay = 1f;
    private GameObject player;
    private float eps = 2f;
    private float multSpeed = 1.0f;

    public MovementType movementType = MovementType.Stay;

    public enum MovementType
    {
        Stay,
        Wandering,
        RunAway
    }

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anSt = GetComponent<AnimalStats>();

        player = GetComponentInParent<CreatePlayer>().player;
    }

    void FixedUpdate()
    {
        FlipSprite();
        ChooseMovementTypeAndMoving();
    }

    private void ChooseMovementTypeAndMoving()
    {
        timerType += Time.deltaTime;

        SetMovementStay();

        SetMovementWandering();

        SetMovementRunAway();

        transform.Translate(movement);

    }

    private void SetMovementRunAway()
    {
        float dx = Mathf.Pow(player.transform.position.x - transform.position.x, 2);
        float dy = Mathf.Pow(player.transform.position.y - transform.position.y, 2);
        if (Mathf.Sqrt(dx + dy) < eps)
        {
            timerType = 0;
            timerTypeDelay = 3;
            movementType = MovementType.RunAway;
            multSpeed = 2;

            float movX = transform.position.x - player.transform.position.x;
            float movY = transform.position.y - player.transform.position.y;
            NormalizeMovement(movX, movY, out float movNormX, out float movNormY);
            movement = new Vector3(movNormX, movNormY, 0) * multSpeed * anSt.Speed * Time.deltaTime;
        }
    }

    private void SetMovementWandering()
    {
        if (timerType >= timerTypeDelay && (movementType == MovementType.Stay || movementType == MovementType.RunAway))
        {
            timerType = 0;
            timerTypeDelay = 3 + (2 * Random.value - 1); // время, которое овечка будет идти в случайном направлении
            movementType = MovementType.Wandering;
            multSpeed = 1;

            float movX = Random.value * 2 - 1;
            float movY = Random.value * 2 - 1;
            NormalizeMovement(movX, movY, out float movNormX, out float movNormY);
            movement = new Vector3(movNormX , movNormY, 0) * multSpeed * anSt.Speed * Time.deltaTime;
        }
    }

    private void NormalizeMovement(float movX, float movY, out float movNormX, out float movNormY)
    {
        movNormX = movX / Mathf.Sqrt(movX * movX + movY * movY);
        movNormY = movY / Mathf.Sqrt(movX * movX + movY * movY);
    }

    private void SetMovementStay()
    {
        if (timerType >= timerTypeDelay && movementType == MovementType.Wandering)
        {
            timerType = 0;
            timerTypeDelay = 1 + (2 * Random.value - 1); // время, которое овечка будет стоять на месте
            movementType = MovementType.Stay;
            movement = new Vector3(0, 0, 0);
        }
    }

    private void FlipSprite()
    {
        prevX = currX;
        currX = transform.position.x;
        if (prevX < currX)
            sr.flipX = true;
        if (prevX > currX)
            sr.flipX = false;
    }
}
