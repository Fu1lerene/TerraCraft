using Assets;
using Assets.Classes.Player;
using Assets.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using System.Threading;
using UnityEngine.UI;


public class PlayerMovement : MonoBehaviour
{
    public GameObject player;
    public GameObject entities;
    
    private bool isRunning = false;
    private float moveHorizontal;
    private float moveVertical;
    private Vector3 movement;
    private SpriteRenderer sr;
    private Player pl;
    private float multSpeed = 1.0f;


    void Start()
    {
        sr = player.GetComponent<SpriteRenderer>();
        pl = entities.GetComponent<CreatePlayer>().pl;
    }
    void Update()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        /// Поворот спрайта
        if (moveHorizontal > 0)
            sr.flipX = false;
        if (moveHorizontal < 0)
            sr.flipX = true;

        Movement();
    }

    private void Movement()
    {
        /// Движение с ускорением
        movement = GetMovement(multSpeed);
        if (Input.GetKeyDown(KeyCode.LeftShift) && pl.Stamina.Value > 5 && movement != new Vector3(0, 0, 0))
            isRunning = true;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            isRunning = false;

        if (isRunning)
        {
            multSpeed = 2;
            pl.Stamina.Value -= Time.deltaTime * pl.Stamina.FallRate;
        }
        else
        {
            multSpeed = 1;
            pl.Stamina.Value += Time.deltaTime * pl.Stamina.RegenRate;
        }

        if (pl.Stamina.Value <= 0)
        {
            isRunning = false;
        }

        if (pl.Stamina.Value > pl.Stamina.MaxValue)
        {
            pl.Stamina.Value = pl.Stamina.MaxValue;
        }
        if (pl.Stamina.Value <= 0)
        {
            pl.Stamina.Value = 0;
        }

        /// Нормализация скорости движения по диагонали
        if (moveHorizontal != 0 && moveVertical != 0)
        {
            movement /= Mathf.Sqrt(2);
        }

        player.transform.Translate(movement); /// Перемещение

        pl.X = player.transform.position.x;
        pl.Y = player.transform.position.y;
    }

    private Vector3 GetMovement(float multSpeed)
    {
        return new Vector3(moveHorizontal, moveVertical, 0f) * pl.Speed * multSpeed * Time.deltaTime;
    }
}
