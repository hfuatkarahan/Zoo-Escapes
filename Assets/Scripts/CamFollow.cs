using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform player;
    float yOffset, zOffset;

    void Start()
    {
        yOffset = transform.position.y - player.position.y;
        zOffset = transform.position.z - player.position.z;
    }


    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, player.position.y + yOffset, player.position.z + zOffset);
    }
}
