using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //private GameObject player;
    private Move playerMove;
    public GameObject tapToStartPanel, winPanel, failPanel, inGamePanel;

    void Awake()
    {
        //player =GameObject.FindGameObjectWithTag("Player");
        //player.GetComponent<Move>().Speed = 0;
        playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<Move>();
        playerMove.Speed = 0;
    }

    public void TapToStart()
    {
        playerMove.Speed = 15;
        tapToStartPanel.SetActive(false);
        inGamePanel.SetActive(true);
    }
   

}
