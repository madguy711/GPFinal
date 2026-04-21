using UnityEngine;

public class GunBehavior : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public ParticleSystem muzzleFlash;
    public float timeBetweenShots = 1f;

    private float fireCooldown = 0f;
    
    void Update()
    {
        fireCooldown -= Time.deltaTime;
        
        if (Input.GetButton("Fire1") && fireCooldown <= 0f)
        {
            Shoot();
            fireCooldown = timeBetweenShots;
        }
    }

    void Shoot()
    {
        // Use the camera's forward direction for aiming
        Camera cam = Camera.main;
        Quaternion bulletRotation = Quaternion.LookRotation(cam.transform.forward);
        var bullet = Instantiate(projectilePrefab, firePoint.position, bulletRotation);

        if (muzzleFlash)
        {
            muzzleFlash.Play();
        }
        
    }
}