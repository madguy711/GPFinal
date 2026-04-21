using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Slider sensitivitySlider;
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    void Update()
    {
        if (sensitivitySlider)
        {
            PlayerPrefs.SetFloat("SensitivitySetting",sensitivitySlider.value);
        }
        Debug.Log(PlayerPrefs.GetFloat("SensitivitySetting", -1f));
    }
    public void PlayLevel1()
    {
        SceneManager.LoadScene("Level1");
    }
    public void PlayLevel2()
    {
        SceneManager.LoadScene("Level2");
    }
    public void Quit()
    {
        Application.Quit();
    }
    
}
