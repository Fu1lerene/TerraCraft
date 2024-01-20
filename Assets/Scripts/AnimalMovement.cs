using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalMovement : MonoBehaviour
{
    private AnimalStats anSt;
    private Vector3 movement;
    private SpriteRenderer sr;
    private float prevX;
    private float currX;
    private Vector3 prevPos;
    //private float timer;
    //private float timerDelay;

    private float timerType;
    private float timerTypeDelay = 1f;
    public string movementType;
    public GameObject player;
    private CreatePlayer crPl;
    private float eps = 2f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anSt = GetComponent<AnimalStats>();
        movementType = "stay";

        crPl = transform.parent.gameObject.GetComponent<CreatePlayer>();
        player = crPl.player;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FlipSprite();
        ChooseMovementTypeAndMoving();
    }

    private void ChooseMovementTypeAndMoving()
    {
        timerType += Time.deltaTime;
        if (timerType >= timerTypeDelay)
        {
            timerType = 0;
            float r1 = Random.value;
            if (movementType == "wandering")
            {
                timerTypeDelay = 1 + (2 * Random.value - 1); // врем€, которое овечка будет сто€ть на месте
                movementType = "stay";
                movement = new Vector3(0, 0, 0);
            }

            else if ((movementType == "stay") || (movementType == "run away"))
            {
                timerTypeDelay = 3 + (2 * Random.value - 1); // врем€, которое овечка будет идти в случайном направлении
                movementType = "wandering";
                float movX = Random.value * 2 - 1;
                float movY = Random.value * 2 - 1;
                float movNormX = movX / (Mathf.Sqrt(movX * movX + movY * movY));
                float movNormY = movY / (Mathf.Sqrt(movX * movX + movY * movY));
                movement = new Vector3(movNormX * anSt.Speed * Time.deltaTime, movNormY * anSt.Speed * Time.deltaTime, 0);
            }
        }

        if (Mathf.Sqrt(Mathf.Pow(player.transform.position.x - transform.position.x, 2) + Mathf.Pow(player.transform.position.y - transform.position.y, 2)) < eps)
        {
            timerType = 0;
            timerTypeDelay = 3;
            //Debug.Log("ќвца обкакалась от страха!");
            movementType = "run away";
            float movX = transform.position.x - player.transform.position.x;
            float movY = transform.position.y - player.transform.position.y;
            float movNormX = movX / (Mathf.Sqrt(movX * movX + movY * movY));
            float movNormY = movY / (Mathf.Sqrt(movX * movX + movY * movY));
            movement = new Vector3(movNormX * 2 * anSt.Speed * Time.deltaTime, movNormY * 2 * anSt.Speed * Time.deltaTime, 0);
        }

        transform.Translate(movement);
        
    }

    private void WanderingMovementType()
    {

    }

    private void StayMovementType()
    {
        
    }

    private void RunAwayMovementType()
    {
        //GameObject target = 
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
