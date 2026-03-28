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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (winText)
            {
                winText.SetActive(true);
            }
        }
    }
}
