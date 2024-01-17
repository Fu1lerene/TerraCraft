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
    private float timer;
    private float timerDelay;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anSt = GetComponent<AnimalStats>();
        timerDelay = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        FlipSprite();

        Movement();

    }

    private void Movement()
    {
        timer += Time.deltaTime;
        if (timer > timerDelay)
        {
            timer = 0;
            movement = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0) * anSt.Speed * Time.deltaTime;
        }

        if (movement.x != 0 && movement.y != 0)
        {
            movement /= Mathf.Sqrt(2);
        }
        transform.Translate(movement);
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
