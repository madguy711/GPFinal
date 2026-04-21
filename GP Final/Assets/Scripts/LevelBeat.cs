using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelBeat : MonoBehaviour
{
    public GameObject[] stars;
    public float[] starThresholds = { 0f, 20f, 30f, 50f, 70f };

    void Start()
    {
        // enable the cursor in the menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        float timeLeft = PlayerPrefs.GetFloat("TimeLeft", 0f);
        Debug.Log("Time left: " + timeLeft);

        // award stars out of 5 based on time left 
        for (int i = 0; i < stars.Length; i++)
        {
            Debug.Log("Star " + i + " threshold: " + starThresholds[i] + " | showing: " + (timeLeft >= starThresholds[i]));
            if (timeLeft >= starThresholds[i])
            {
                stars[i].SetActive(true);
            }
            else
            {
                stars[i].SetActive(false);
            }
        }
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("LastLevel", 0));
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("LastLevel", 0) + 1);
    }
}