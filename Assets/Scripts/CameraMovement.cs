using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Camera cam;
    public GameObject player;

    private int scrollSpeed = 10;
    private float camMin = 10;
    private float camMax = 300;



    void Start()
    {

    }

    void Update()
    {
        cam.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        cam.orthographicSize -= scroll * scrollSpeed;

        if (cam.orthographicSize < camMin)
            cam.orthographicSize = camMin;
        if (cam.orthographicSize > camMax)
            cam.orthographicSize = camMax;
    }
}
