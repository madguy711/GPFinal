using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState {Navigate, Attack, Die}
    [Header("General Settings")]
    public EnemyState currentState = EnemyState.Navigate;

    [Header("Navigation Settings")]
    // public Transform turret;
    public float rotationSpeed = 30f;
    public float detectionRange = 10f;      // The detection range for the player
    public float bobHeight = 0.3f;          // how high/low it bobs
    public float bobSpeed = 2f;             // how fast it bobs
    public float scanSpeed = 45f;           // degrees per second for scanning
    public float scanAngle = 60f;           // max angle to turn left or right

    [Header("Attack Settings")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 2f;
    
    [Header("Die Settings")]
    public GameObject destroyPrefab;

    float fireCooldown = 0;
    Transform attackTarget;
    Quaternion initialTurretRotation;

    // Navigation state tracking
    float centerHeight;                // The "center" height to bob around
    float bobTimer;             // Drives the sine wave for bobbing
    float lookTimer;            // Drives the looking rotation
    float baseYaw;           // The yaw the drone scans around
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        centerHeight = transform.position.y;
        baseYaw = transform.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Navigate:
                Navigate();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Die:
                Die();
                break;
        }
    }

    void Navigate()
    {
        // bob up and down
        bobTimer += Time.deltaTime * bobSpeed;
        Vector3 pos = transform.position;
        pos.y = centerHeight + Mathf.Sin(bobTimer) * bobHeight;
        transform.position = pos;

        // look left and right
        lookTimer += Time.deltaTime;
        float yawOffset = Mathf.Sin(lookTimer * Mathf.Deg2Rad * scanSpeed) * scanAngle;
        transform.rotation = Quaternion.Euler(0f, baseYaw + yawOffset, 0f);

        FindPlayer();
    }

    void Attack()
    {
        // go back to navigate
        if (attackTarget == null || Vector3.Distance(transform.position, attackTarget.position) > detectionRange)
        {
            attackTarget = null;
            currentState = EnemyState.Navigate;
            // Make the bobbing and looking resume normally from wherever it is now
            centerHeight = transform.position.y;
            baseYaw = transform.eulerAngles.y;
            bobTimer = 0f;
            lookTimer = 0f;
            return;
        }

        // attack
        Vector3 direction = attackTarget.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        firePoint.rotation = Quaternion.Slerp(firePoint.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        // cooldown; can we shoot?
        if (fireCooldown <= 0 && HasLineOfSight(attackTarget))
        {
            Shoot();
            fireCooldown = 1f / fireRate;
        }
        fireCooldown -= Time.deltaTime;
    }

    // explode when dying 
    void Die()
    {
        if (destroyPrefab)
        {
            Instantiate(destroyPrefab, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }

    // fi
    void FindPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange);
        Transform nearestPlayer = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                float distanceToEnemy = Vector3.Distance(transform.position, collider.transform.position);

                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestPlayer = collider.transform;
                }
            }
        }

        if (nearestPlayer)
        {
            attackTarget = nearestPlayer;
            Debug.Log("Player detected: " + attackTarget.name);
            currentState = EnemyState.Attack;
            return;
        }
    }

    // shoot at the player's current location, bullets do not track the player
    void Shoot()
    {
        Vector3 aimDirection = attackTarget.position - firePoint.position;
        Quaternion bulletRotation = Quaternion.LookRotation(aimDirection);
        var bullet = Instantiate(projectilePrefab, firePoint.position, bulletRotation);
        BulletBehavior bulletBehavior = bullet.GetComponent<BulletBehavior>();

        if (bulletBehavior)
        {
            bulletBehavior.SetTarget(attackTarget);
        }

    }

    // the game won't really have a case where you're behind cover but nice to have
    // Checks to see if the drone has a line of sight on the player
    bool HasLineOfSight(Transform target)
    {
        RaycastHit hit;
        Vector3 direction = (target.position - firePoint.position).normalized;

        if (Physics.Raycast(firePoint.position, direction, out hit, detectionRange))
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }
        
        return false;
    }

    // just to show the range of the drone
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    // Dies when shot by a bullet 
    void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Bullet"))
        {
            Die();
        }
    }
}
