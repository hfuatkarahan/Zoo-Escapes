using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Move : MonoBehaviour
{
    public int Speed = 10, StarScore = 0;
    //public Transform leftPoint, midPoint, rightPoint;
    public int LeftPos, MidPos, RightPos;
    public bool atLeft, atRight, atMid = true, onMove = false;
    public Positions positions;
    private UIManager uiManager;

    void Awake()
    {
        uiManager = GameObject.Find("GameManager").GetComponent<UIManager>();
    }
    public enum Positions
    {
        onLeft,
        onMid,
        onRight
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, 0, 1) * Time.deltaTime * Speed);
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && !atLeft && !onMove)
        {
            if (atMid)
            {
                atLeft = true;
                atMid = false;
            }
            else if (atRight)
            {
                atRight = false;
                atMid = true;
            }
            if (positions == Positions.onMid)
            {
                positions = Positions.onLeft;
            }
            else if (positions == Positions.onRight)
            {
                positions = Positions.onMid;
            }
            //transform.position = new Vector3(transform.position.x - 3, transform.position.y, transform.position.z);
            transform.DOMoveX(transform.position.x - 3, 0.25f).SetEase(Ease.Linear).OnComplete(OnMoveToFalse);
            onMove = true;
        }
        if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && positions != Positions.onRight && !onMove)
        {
            if (atMid)
            {
                atRight = true;
                atMid = false;
            }
            else if (atLeft)
            {
                atLeft = false;
                atMid = true;
            }
            if (positions == Positions.onMid)
            {
                positions = Positions.onRight;
            }

            else if (positions == Positions.onLeft)
            {
                positions = Positions.onMid;
            }
            //transform.position = new Vector3(transform.position.x + 3, transform.position.y, transform.position.z);
            transform.DOMoveX(transform.position.x + 3, 0.25f).SetEase(Ease.Linear).OnComplete(OnMoveToFalse);
            onMove = true;
        }
    }

    public void OnMoveToFalse()
    {
        onMove = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Star"))
        {
            StarScore++;
            uiManager.inGamePanel.transform.Find("Score Text").GetComponent<TextMeshProUGUI>().text = "Score: " + StarScore.ToString();
            //other.gameObject.SetActive(false);
            other.GetComponent<MeshRenderer>().material.DOColor(Color.red, 1);
        }
        if (other.tag == "Finish")
        {
            Speed = 0;
            //gameObject.GetComponent<Animator>().enabled = false;
            uiManager.winPanel.SetActive(true);
        }
        
        if (other.CompareTag("CheckPoint"))
        {
            Speed *= 2;
            //transform.GetChild(1).DOLocalRotate(new Vector3(-90, 270, 90), 1);
        }
        if (other.tag == "Obstacle")
        {
            Speed = 0;
            uiManager.failPanel.SetActive(true);
        }

    }
}

