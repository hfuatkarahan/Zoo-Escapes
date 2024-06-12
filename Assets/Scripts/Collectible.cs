using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public CollectibleType cType;
    GameObject player;
    public bool collected;

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
        Magnet,
        Sneaker
    }

    private void Update()
    {
        if(cType == CollectibleType.Coin && player.GetComponent<Player>().isMagnetActive == true && collected == false)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < 6)
            {
                transform.DOMove(player.transform.position, 0.25f);
            }

            //if ((transform.position.z - transform.position.z) < 2)
            //{
            //    collected = true;
            //    transform.parent = player.transform;
            //    transform.DOLocalMove(Vector3.zero + new Vector3(0,1,0), 0.5f).OnComplete(StopDoTween);
            //}
        }
    }

    void StopDoTween()
    {
        transform.DOKill();
    }

}
