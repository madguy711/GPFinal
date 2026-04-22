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
    public GameObject pauseMenu;
    bool isGamePaused = false;

    void Start()
    {
        timerRunning = true;
        timerText.color = Color.white;
        BGM.Play();
    }
    
    void Update()
    {
       LevelTimer();
       if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isGamePaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
        
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
        if(SceneManager.GetActiveScene().name == "Level1"){
            PlayerPrefs.SetFloat("TimeLeft1", levelTime);
        }else if (SceneManager.GetActiveScene().name == "Level2")
        {
            PlayerPrefs.SetFloat("TimeLeft2", levelTime);
        }
        PlayerPrefs.SetInt("LastLevel", SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.Save();
        if (SceneManager.GetActiveScene().name != "Level2"){
            SceneManager.LoadScene("LevelComplete");
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
    public void MainMenu()
    {
        ResumeGame();
        SceneManager.LoadScene("MainMenu");
    }

    public void PauseGame()
    {
        isGamePaused = true;
        timerRunning = false;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseMenu.SetActive(true);
    }
    public void ResumeGame()
    {
        isGamePaused = false;
        timerRunning = true;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenu.SetActive(false);
    }


}