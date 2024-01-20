using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Camera cam;
    public GameObject entities;

    private GameObject player;
    private int scrollSpeed = 10;
    public float camMin = 3;
    public float camMax = 10;



    void Start()
    {

    }

    void Update()
    {
        player = entities.GetComponent<CreatePlayer>().player;
        cam.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        cam.orthographicSize -= scroll * scrollSpeed;

        if (cam.orthographicSize < camMin)
            cam.orthographicSize = camMin;
        if (cam.orthographicSize > camMax)
            cam.orthographicSize = camMax;
    }
}
