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
        Camera cam = Camera.main;

        // raycast from the camera through the crosshair to find what the player is aiming at
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
        {
            targetPoint = hit.point;
        }
        else
        {
            // nothing in range — aim at a distant point along the camera's forward direction
            targetPoint = cam.transform.position + cam.transform.forward * 1000f;
        }

        // aim the bullet from the fire point toward that target
        Vector3 aimDirection = (targetPoint - firePoint.position).normalized;
        Quaternion bulletRotation = Quaternion.LookRotation(aimDirection);
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