using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private float cameraX;
    [SerializeField] private float cameraY;
    [SerializeField] private float cameraZ;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x + cameraX,
            player.transform.position.y + cameraY,
            player.transform.position.z + cameraZ);
    }
}
