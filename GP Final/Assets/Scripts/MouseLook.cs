using UnityEngine;

public class MouseLook : MonoBehaviour
{
    Transform playerBody;
    float pitch;
    float pitchMin = -90f;
    float pitchMax = 90f;
    float mouseSensitivity;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mouseSensitivity = PlayerPrefs.GetFloat("SensitivitySetting",0.5f)*400+100;
        playerBody = transform.parent.transform;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float moveY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // yaw at the player
        if (playerBody)
        {
            playerBody.Rotate(Vector3.up * moveX);
        }

        // pitch at the camera
        pitch -= moveY;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);
        transform.localRotation = Quaternion.Euler(pitch, 0, 0);

        // Debug.Log("moveX: " + moveX);
    }
}
