using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text bestScoreText;
    public Text ScoreText;
    public GameObject GameOverText;
    int currentBestScore;
    string currentBestPlayer;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;


    // Start is called before the first frame update
    void Start()
    {
        currentBestScore = BestScore.Instance.bestScore;
        currentBestPlayer = BestScore.Instance.bestPlayer;
        bestScoreText.text = $"Best Score : {currentBestPlayer} : {currentBestScore}";
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                BestScore.Instance.LoadScore();
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
        if (m_Points == 96)
        {
            ScoreText.text = $"Congratulations! You beat the game! : {m_Points}";
            GameOver();
        }
    }

    public void GameOver()
    {
        if (m_Points > currentBestScore)
        {
            BestScore.Instance.bestScore = m_Points;
            BestScore.Instance.bestPlayer = BestScore.Instance.playerName;
            BestScore.Instance.SaveScore();
        }
        m_GameOver = true;
        
        GameOverText.SetActive(true);
    }
}
