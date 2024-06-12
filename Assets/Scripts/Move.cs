using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public class Move : MonoBehaviour
{
    public int speed = 0, maxSpeed = 19;
    public float leftBorder = -2.75f, rightBorder = 2.75f;
    float transSpeed = 0.25f;
    public bool onLeft = false, mid = true, onRight = false;
    private Animator playerAnim;
    private GameObject myFoxBody;
    GameManager gameManager;
    public float jumpHeight = 2;

    void Awake()
    {
        playerAnim = transform.Find("Player").GetComponent<Animator>();
        myFoxBody = transform.Find("Player").gameObject;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void AnimPlay(string animName)
    {
        playerAnim.SetBool("Run", false);
        playerAnim.SetBool("Idle", false);
        playerAnim.SetBool("Die", false);
        playerAnim.SetBool("Somersault", false);
        playerAnim.SetBool(animName, true);
    }

    void PlayerOriginalCollider()
    {
        myFoxBody.GetComponent<BoxCollider>().size = new Vector3(1f, 1.69f, 2.79f);
        myFoxBody.GetComponent<BoxCollider>().center = new Vector3(0, 0.87f, -0.41f);
    }

    public void Jump()
    {
        if (!gameManager.levelFinished)
        {
            playerAnim.SetTrigger("Jump");
            myFoxBody.GetComponent<Player>().jumpSound.Play();
            myFoxBody.transform.DOMoveY(myFoxBody.transform.localPosition.y + jumpHeight, 0.5f).SetEase(Ease.OutFlash);
            myFoxBody.transform.DOMoveY(myFoxBody.transform.localPosition.y, 0.75f).SetDelay(0.5f).SetEase(Ease.InFlash);
        }
    }

    public void GoLeft()
    {
        if (!gameManager.levelFinished)
        {
            if (onLeft == false && mid == true)
            {
                onLeft = true;
                mid = false;
                transform.DOMoveX(leftBorder, transSpeed);
                playerAnim.SetTrigger("RunLeft");
                //transform.position = new Vector3(-2, height, transform.position.z);
            }
            else if (mid == false && onRight == true)
            {
                onRight = false;
                mid = true;
                transform.DOMoveX(0, transSpeed);
                playerAnim.SetTrigger("RunLeft");
                //transform.position = new Vector3(0, height, transform.position.z);
            }
        }
    }

    public void GoRight()
    {
        if (!gameManager.levelFinished)
        {
            if (onRight == false && mid == true)
            {
                onRight = true;
                mid = false;
                transform.DOMoveX(rightBorder, transSpeed);
                playerAnim.SetTrigger("RunRight");
                //transform.position = new Vector3(2, height, transform.position.z);
            }
            else if (onLeft == true && mid == false)
            {
                onLeft = false;
                mid = true;
                transform.DOMoveX(0, transSpeed);
                playerAnim.SetTrigger("RunRight");
                //transform.position = new Vector3(0, height, transform.position.z);
            }
        }
    }

    public void GoDown()
    {
        if (myFoxBody.GetComponent<Player>().onGround == false)
        {
            myFoxBody.transform.DOKill();
            myFoxBody.transform.DOMoveY(0.6f, 0.2f).SetEase(Ease.InFlash);
        }
        else
        {
            playerAnim.SetTrigger("Somersault");
            myFoxBody.GetComponent<BoxCollider>().size = new Vector3(1f, 0.85f, 2.79f);
            myFoxBody.GetComponent<BoxCollider>().center = new Vector3(0, 0.3f, -0.41f);
            myFoxBody.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.5f).SetDelay(0.5f).OnComplete(PlayerOriginalCollider);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!gameManager.levelFinished)
        {
            if (speed >= maxSpeed)
            {
                speed = maxSpeed;
                transform.Translate(new Vector3(0, 0, 1) * Time.deltaTime * speed);
            }
            else
            {
                transform.Translate(new Vector3(0, 0, 1) * Time.deltaTime * speed);
            }

            if (Input.GetKeyDown(KeyCode.W) && myFoxBody.GetComponent<Player>().onGround)
            {
                playerAnim.SetTrigger("Jump");
                myFoxBody.GetComponent<Player>().jumpSound.Play();
                myFoxBody.transform.DOMoveY(myFoxBody.transform.localPosition.y + jumpHeight, 0.5f).SetEase(Ease.OutFlash);
                myFoxBody.transform.DOMoveY(myFoxBody.transform.localPosition.y, 0.75f).SetDelay(0.5f).SetEase(Ease.InFlash);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                if (myFoxBody.GetComponent<Player>().onGround == false)
                {
                    myFoxBody.transform.DOKill();
                    myFoxBody.transform.DOMoveY(0.6f, 0.2f).SetEase(Ease.InFlash);
                }
                else
                {
                    playerAnim.SetTrigger("Somersault");
                    myFoxBody.GetComponent<BoxCollider>().size = new Vector3(1f, 0.85f, 2.79f);
                    myFoxBody.GetComponent<BoxCollider>().center = new Vector3(0, 0.3f, -0.41f);
                    myFoxBody.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.5f).SetDelay(0.5f).OnComplete(PlayerOriginalCollider);
                }
            }

            if (Input.GetKeyDown(KeyCode.A) && onLeft == false && mid == true)
            {
                onLeft = true;
                mid = false;
                transform.DOMoveX(leftBorder, transSpeed);
                playerAnim.SetTrigger("RunLeft");
                //transform.position = new Vector3(-2, height, transform.position.z);
            }
            else if (Input.GetKeyDown(KeyCode.A) && mid == false && onRight == true)
            {
                onRight = false;
                mid = true;
                transform.DOMoveX(0, transSpeed);
                playerAnim.SetTrigger("RunLeft");
                //transform.position = new Vector3(0, height, transform.position.z);
            }

            if (Input.GetKeyDown(KeyCode.D) && onRight == false && mid == true)
            {
                onRight = true;
                mid = false;
                transform.DOMoveX(rightBorder, transSpeed);
                playerAnim.SetTrigger("RunRight");
                //transform.position = new Vector3(2, height, transform.position.z);
            }
            else if (Input.GetKeyDown(KeyCode.D) && onLeft == true && mid == false)
            {
                onLeft = false;
                mid = true;
                transform.DOMoveX(0, transSpeed);
                playerAnim.SetTrigger("RunRight");
                //transform.position = new Vector3(0, height, transform.position.z);
            }

        }   

    }

}