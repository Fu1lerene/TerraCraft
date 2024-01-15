using Assets;
using Assets.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float moveHorizontal;
    private float moveVertical;
    private Vector3 movement;
    private int sizeCh = GenerateParams.SizeChunk;
    private int distLoad = GenerateParams.LoadingDistance;
    private int startCountChunks = GenerateParams.StartCountChunks;

    public GameObject player;
    public float speed = 10f;

    private void Awake()
    {
        player.transform.position = new Vector3((2 * startCountChunks + 1) * sizeCh / 2,
                                        (2 * startCountChunks + 1) * sizeCh / 2, 0);
    }

    void Start()
    {

    }
    void Update()
    {

        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movement = new Vector3(moveHorizontal, moveVertical, 0f) * speed * 2 * Time.deltaTime;
        }
        else
        {
            movement = new Vector3(moveHorizontal, moveVertical, 0f) * speed * Time.deltaTime;
        }
        if (moveHorizontal != 0 && moveVertical != 0)
        {
            movement /= Mathf.Sqrt(2);
        }

        player.transform.Translate(movement);

    }
}
