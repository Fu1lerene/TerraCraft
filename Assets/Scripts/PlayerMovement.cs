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
using UnityEditor.SearchService;


public class PlayerMovement : MonoBehaviour
{
    private bool isRunning = false;
    private float moveHorizontal;
    private float moveVertical;
    private Vector3 movement;
    private SpriteRenderer sr;
    private PlayerStats plSt;
    private float multSpeed = 1.0f;
    private Vector3 mousePos;


    void Start()
    {
        plSt = GetComponent<PlayerStats>();
        sr = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
        mousePos = Input.mousePosition;

        FlipSprite();
        Movement();
    }
    private void FixedUpdate()
    {
        movement = GetMovement(multSpeed);
        transform.Translate(movement); /// Перемещение
    }

    private void FlipSprite()
    {
        //if (mousePos.x >= 0 && mousePos.x < 550)
        //    sr.flipX = true;
        //if (mousePos.x > 550 && mousePos.x <= 1100)
        //    sr.flipX = false;


        /// Поворот спрайта
        if (moveHorizontal > 0)
        {
            sr.flipX = false;

        }
            
        if (moveHorizontal < 0)
        {
            sr.flipX = true;

        }     
    }

    private void Movement()
    {
        /// Движение с ускорением

        if (Input.GetKeyDown(KeyCode.LeftShift) && plSt.Stamina.Value > plSt.Stamina.MaxValue * 0.05 && movement != new Vector3(0, 0, 0))
            isRunning = true;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            isRunning = false;

        if (isRunning)
        {
            multSpeed = 2;
            plSt.Stamina.Value -= Time.deltaTime * plSt.Stamina.FallRate;
        }
        else
        {
            multSpeed = 1;
            plSt.Stamina.Value += Time.deltaTime * plSt.Stamina.RegenRate;
        }

        if (plSt.Stamina.Value <= 0)
        {
            isRunning = false;
        }

        if (plSt.Stamina.Value > plSt.Stamina.MaxValue)
        {
            plSt.Stamina.Value = plSt.Stamina.MaxValue;
        }
        if (plSt.Stamina.Value <= 0)
        {
            plSt.Stamina.Value = 0;
        }

        /// Нормализация скорости движения по диагонали
        if (moveHorizontal != 0 && moveVertical != 0)
        {
            movement /= Mathf.Sqrt(2);
        }
    }

    private Vector3 GetMovement(float multSpeed)
    {
        return new Vector3(moveHorizontal, moveVertical, 0f) * plSt.Speed * multSpeed * Time.deltaTime;
    }
}
