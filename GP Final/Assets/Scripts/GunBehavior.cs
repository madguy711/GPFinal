using UnityEngine;

public class GunBehavior : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public Animator gunAnimator;
    public ParticleSystem muzzleFlash;
    public AudioSource gunSFX;
    public AudioSource spinSFX;
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

        if (gunAnimator)
        {
            gunAnimator.SetTrigger("Shoot");
            spinSFX.PlayDelayed(0.25f);
        }

        if (gunSFX)
        {
            gunSFX.Play();
        }
        
    }
}