using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    Move move;
    UIManager uimanager;
    ScoreManager scoremanager;
    float uiOpeningTime = 1.5f;
    public Transform camLastPos, camLastPos2;
    GameManager gamemanager;
    public bool isShieldActive, isMagnetActive;
    public GameObject shield;
    int shieldCounter = 0;

    private void Awake()
    {
        move = GameObject.FindGameObjectWithTag("Player").GetComponent<Move>();
        uimanager = GameObject.Find("GameManager").GetComponent<UIManager>();
        scoremanager = GameObject.Find("GameManager").GetComponent<ScoreManager>();
        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            gamemanager.levelFinished = true;
            move.AnimPlay("Victory");
            move.speed = 0;
            Invoke(nameof(WinCondition), uiOpeningTime + 2);
            Camera.main.transform.DOLocalRotate(camLastPos2.localEulerAngles, 1).SetEase(Ease.Linear);
            Camera.main.transform.DOLocalMove(camLastPos2.localPosition, 1).SetEase(Ease.Linear);
            //
            Camera.main.transform.DOLocalRotate(camLastPos.localEulerAngles, 1).SetDelay(1).SetEase(Ease.Linear);
            Camera.main.transform.DOLocalMove(camLastPos.localPosition, 1).SetDelay(1).SetEase(Ease.Linear);
        }
        if (other.CompareTag("Collectible"))
        {
            Destroy(other.gameObject);
            if (other.GetComponent<Collectible>().cType == Collectible.CollectibleType.Star)
                scoremanager.UpdateStarScore();
            else if (other.GetComponent<Collectible>().cType == Collectible.CollectibleType.DoubleXP)
            {
                StartCoroutine(scoremanager.DoubleXP());
            }
            else if (other.GetComponent<Collectible>().cType == Collectible.CollectibleType.Coin)
            {
                //StartCoroutine(scoremanager.DoubleXP());
            }
            else if (other.GetComponent<Collectible>().cType == Collectible.CollectibleType.Magnet)
            {
                isMagnetActive = true;
                //StartCoroutine(scoremanager.DoubleXP());
            }
            else if (other.GetComponent<Collectible>().cType == Collectible.CollectibleType.Shield)
            {
                isShieldActive = true;
                shieldCounter++;
                uimanager.shieldCountText.text = shieldCounter.ToString();
                uimanager.shieldIcon.color = Color.white;
                uimanager.shieldCountText.color = Color.white;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && !isShieldActive)
        {
            gamemanager.levelFinished = true;
            move.AnimPlay("Die");
            move.speed = 0;
            Invoke(nameof(FailCondition), uiOpeningTime);
            try
            {
                collision.gameObject.transform.Find("Wall Crack").gameObject.SetActive(true);
            }
            catch 
            {
                print("No child object");
            }
            
        }
        else if (collision.gameObject.CompareTag("Obstacle") && isShieldActive)
        {
            //Destroy(collision.gameObject);
            shieldCounter--;
            uimanager.shieldCountText.text = shieldCounter.ToString();
            if(shieldCounter == 0)
            {
                uimanager.shieldIcon.DOColor(new Color(1, 1, 1, 0.3f), 0.25f);
                uimanager.shieldCountText.DOColor(new Color(1, 1, 1, 0.3f), 0.25f);
                isShieldActive = false;
            }
            collision.gameObject.GetComponent<MeshRenderer>().enabled = false;
            collision.gameObject.GetComponent<BoxCollider>().enabled = false;
            collision.transform.Find("PlasmaExplosionEffect").gameObject.SetActive(true);
        }
    }

    void FailCondition()
    {
        uimanager.FailPanel();
    }

    void WinCondition()
    {
        uimanager.WinPanel();
    }
}
