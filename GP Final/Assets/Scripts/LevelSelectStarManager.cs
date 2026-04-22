using UnityEngine;
using UnityEngine.UI;

public class LevelSelectStarManager : MonoBehaviour
{
    public int levelNum = 1;
    public GameObject[] stars;
    public float[] starThresholds = { 0f, 20f, 30f, 50f, 70f };
    public GameObject text;
    void Start()
    {
        float timeLeft=-1f;
        if(levelNum==1){
            timeLeft = PlayerPrefs.GetFloat("TimeLeft1", -1f);
        }
        else if (levelNum==2)
        {
            timeLeft = PlayerPrefs.GetFloat("TimeLeft2", -1f);
        }
        if (levelNum > 1)
        {
            if (PlayerPrefs.GetFloat("TimeLeft1", -1f) < 50f)
            {
                gameObject.SetActive(false);
                text.SetActive(true);
            }
        }
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

    
}
