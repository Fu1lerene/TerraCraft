using Assets.Classes;
using Assets.Classes.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlayer : MonoBehaviour
{
    private int startCountChunks = GenerateParams.StartCountChunks;
    private int sizeCh = GenerateParams.SizeChunk;

    public GameObject player;
    void Start()
    {
        float x = (2 * startCountChunks + 1) * sizeCh / 2;
        float y = (2 * startCountChunks + 1) * sizeCh / 2;
        player = Instantiate(player, new Vector3(x, y, 0), Quaternion.identity, transform);
    }

    void Update()
    {
        
    }
}
