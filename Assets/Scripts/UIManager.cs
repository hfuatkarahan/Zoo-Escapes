using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject startPanel, inGamePanel, endPanel, winPanel, failPanel;
    public Image shieldIcon, starIcon, magnetIcon;
    public TextMeshProUGUI starCountText, scoreText, winScoreText, shieldCountText, coinCountText;
    GameObject playerParent;
    ScoreManager scoreManager;
    GameManager gameManager;

    private void Awake()
    {
        playerParent = GameObject.FindGameObjectWithTag("PlayerParent");
        scoreManager = GetComponent<ScoreManager>();
        gameManager = GetComponent<GameManager>();
    }

    void OpenPanel(GameObject panelObject, GameObject secondPanel)
    {
        startPanel.SetActive(false);
        inGamePanel.SetActive(false);
        endPanel.SetActive(false);
        winPanel.SetActive(false);
        failPanel.SetActive(false);
        panelObject.SetActive(true);
        if (secondPanel != null)
        {
            secondPanel.SetActive(true);
        }
        
    }
    void Start()
    {
        OpenPanel(startPanel, null);
    }

    public void FailPanel()
    {
        OpenPanel(failPanel, endPanel);
    }
    public void WinPanel()
    {
        OpenPanel(winPanel, endPanel);
    }
    #region Button Functions
    public void TapToStart()
    {
        startPanel.SetActive(false);
        inGamePanel.SetActive(true);
        playerParent.GetComponent<Move>().speed = 10;
        playerParent.GetComponent<Move>().AnimPlay("Run");
        StartCoroutine(scoreManager.ScoreUpdate());
        gameManager.levelFinished = false;
    }

    public void NextLevel()
    {
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        

        if(currentLevel < 200)
        {
            PlayerPrefs.SetInt("CurrentLevel", currentLevel + 1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if (currentLevel >= 200)
        {
            PlayerPrefs.SetInt("CurrentLevel", 0);
            SceneManager.LoadScene(0);
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #endregion
}
