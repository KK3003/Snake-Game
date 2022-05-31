using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject pausePanel, gameOverPanel, gameOverPanel1, gameOverPanel2;
    public Text scoreT;
    public SnakeCtrl snakectrl;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PausePanel()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Play()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 0.5f;
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
        scoreT.text = snakectrl.score.ToString();
       
    }

   /* public void GameOver1()
    {
        gameOverPanel1.SetActive(true);
        scoreT.text = snakectrl.score.ToString();
        Time.timeScale = 0;
    }
    public void GameOver2()
    {
        gameOverPanel2.SetActive(true);
        scoreT.text = snakectrl.score.ToString();
        Time.timeScale = 0;
    }
   */
}
