using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    Move move;
    UIManager uimanager;
    ScoreManager scoremanager;
    float uiOpeningTime = 1.5f;
    public Transform camLastPos, camLastPos2;
    GameManager gamemanager;
    public bool isShieldActive, isMagnetActive, onGround;
    public GameObject shield, frontShoe, backShoe;
    int shieldCounter = 0;
    public AudioSource coinSound, magnetSound, starSound, wallHitSound, shieldSound, jumpSound;
    public GameObject[] roads;
    int roadCounter = 0, platformPlace = 48;

    private void Awake()
    {
        move = GameObject.FindGameObjectWithTag("PlayerParent").GetComponent<Move>();
        uimanager = GameObject.Find("GameManager").GetComponent<UIManager>();
        scoremanager = GameObject.Find("GameManager").GetComponent<ScoreManager>();
        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    //private void Start()
    //{
    //    DOTween.SetTweensCapacity(500, 2000);
    //}

    IEnumerator MagnetActive()
    {
        uimanager.magnetIcon.fillAmount = 1;

        for (float i = 1; i >= 0; i -= 0.01f)
        {
            yield return new WaitForSeconds(0.05f);
            uimanager.magnetIcon.fillAmount = i;
        }
        isMagnetActive = false;
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
            if (other.GetComponent<Collectible>().cType == Collectible.CollectibleType.Star)
                scoremanager.UpdateStarScore();
            else if (other.GetComponent<Collectible>().cType == Collectible.CollectibleType.DoubleXP)
            {
                starSound.Play();
                StartCoroutine(scoremanager.DoubleXP());
            }
            else if (other.GetComponent<Collectible>().cType == Collectible.CollectibleType.Coin)
            {
                coinSound.Play();
                scoremanager.levelCoin++;
                uimanager.coinCountText.text = scoremanager.levelCoin.ToString();
                other.GetComponent<Collectible>().collected = true;
                other.transform.DOKill();
            }
            else if (other.GetComponent<Collectible>().cType == Collectible.CollectibleType.Magnet)
            {
                magnetSound.Play();
                isMagnetActive = true;
                StartCoroutine(MagnetActive());
            }
            else if (other.GetComponent<Collectible>().cType == Collectible.CollectibleType.Sneaker)
            {
                shieldSound.Play();
                frontShoe.SetActive(true);
                backShoe.SetActive(true);
                //isMagnetActive = true;
                //StartCoroutine(MagnetActive());
                move.jumpHeight = 4.5f;
            }
            else if (other.GetComponent<Collectible>().cType == Collectible.CollectibleType.Shield)
            {
                shieldSound.Play();
                isShieldActive = true;
                shieldCounter++;
                uimanager.shieldCountText.text = shieldCounter.ToString();
                uimanager.shieldIcon.color = Color.white;
                uimanager.shieldCountText.color = Color.white;
            }
            Destroy(other.gameObject);
        }
        if (other.CompareTag("NewRoad"))
        {
            int newRoadNo;
            move.speed++;
            roadCounter++;
            platformPlace += 123;
            if(roadCounter % 3 == 0)
            {
                newRoadNo = 3;
            }
            else
            {
                newRoadNo = Random.Range(0, 3);
            }
            GameObject newRoad = Instantiate(roads[newRoadNo], new Vector3(26,-7,platformPlace), Quaternion.identity);
            newRoad.SetActive(true);
            newRoad.name = "RoadPart" + (newRoadNo + 1).ToString();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && !isShieldActive)
        {
            wallHitSound.Play();
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
            collision.gameObject.GetComponent<BoxCollider>().enabled = false;
            wallHitSound.Play();
            shieldCounter--;
            uimanager.shieldCountText.text = shieldCounter.ToString();
            if(shieldCounter == 0)
            {
                uimanager.shieldIcon.DOColor(new Color(1, 1, 1, 0.3f), 0.25f);
                uimanager.shieldCountText.DOColor(new Color(1, 1, 1, 0.3f), 0.25f);
                isShieldActive = false;
            }
            
                GameObject expEffect = collision.transform.Find("PlasmaExplosionEffect").gameObject;
                expEffect.SetActive(true);
                expEffect.GetComponent<ParticleSystem>().Play();
                expEffect.transform.parent = null;
                StartCoroutine(ObjectDestroyer(expEffect, 1));
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Surface") && !gamemanager.levelFinished)
        {
            transform.DOKill();
            move.AnimPlay("Run");
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Surface"))
        {
            onGround = true;
        }
    }

    private void OnCollisionExit (Collision collision)
    {
        if (collision.gameObject.CompareTag("Surface"))
        {
            onGround = false;
        }
    }

    IEnumerator ObjectDestroyer(GameObject destroyObj, float timer)
    {
        yield return new WaitForSeconds(timer);
        Destroy(destroyObj);
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
