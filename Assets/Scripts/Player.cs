using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    Move move;
    UIManager uimanager;
    ScoreManager scoremanager;
    float uiOpeningTime = 0.75f;
    public Transform camLastPos, camLastPos2;
    GameManager gamemanager;

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
                StartCoroutine(scoremanager.DoubleXP());
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            gamemanager.levelFinished = true;
            move.AnimPlay("Die");
            move.speed = 0;
            Invoke(nameof(FailCondition), uiOpeningTime);
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
