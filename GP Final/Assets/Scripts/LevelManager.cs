using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public float levelTime = 120f;
    public TextMeshProUGUI timerText;
    private bool timerRunning;
    public AudioSource BGM;

    void Start()
    {
        timerRunning = true;
        timerText.color = Color.white;
        BGM.Play();
    }
    
    void Update()
    {
       LevelTimer();
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void LevelTimer()
    {
        if (!timerRunning)
        {
            return;
        }

        levelTime -= Time.deltaTime;

        if (levelTime <= 0f)
        {
            levelTime = 0f;
            timerRunning = false;
            RestartLevel();
        }

        timerText.text = Mathf.CeilToInt(levelTime).ToString();

        if (levelTime <= 10f)
        {
            timerText.color = Color.red;
        }
    }

    public void BeatLevel()
    {
        Debug.Log("levelTime at completion: " + levelTime);
        timerRunning = false;
        PlayerPrefs.SetFloat("TimeLeft", levelTime);
        PlayerPrefs.SetInt("LastLevel", SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.Save();
        SceneManager.LoadScene("LevelComplete");
    }
}