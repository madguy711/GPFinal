using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class SpaceshipBehavior : MonoBehaviour
{
    public GameObject winText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<LevelManager>().BeatLevel();
        }
    }
}
