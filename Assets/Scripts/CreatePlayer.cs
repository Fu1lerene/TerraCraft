using Assets.Classes;
using Assets.Classes.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlayer : MonoBehaviour
{
    public Player pl;
    private float hp = 100;
    private float speed = 2;
    private int startCountChunks = GenerateParams.StartCountChunks;
    private int sizeCh = GenerateParams.SizeChunk;

    public GameObject player;
    void Awake()
    {
        float x = (2 * startCountChunks + 1) * sizeCh / 2;
        float y = (2 * startCountChunks + 1) * sizeCh / 2;
        pl = new Player(x, y, hp, speed);
        player.transform.position = new Vector3(x, y, 0);
    }

    void Update()
    {
        
    }
}
