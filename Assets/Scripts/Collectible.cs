using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public CollectibleType cType;
    GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public enum CollectibleType
    {
        Star,
        DoubleXP,
        Shield,
        Coin,
        Magnet
    }

    private void Update()
    {
        if(cType == CollectibleType.Coin && player.GetComponent<Player>().isMagnetActive == true)
        {
            if (Vector3.Distance(player.transform.position, transform.position)<5)
            {
                transform.DOMove(player.transform.position, 0.5f);
            }
        }
    }
}
