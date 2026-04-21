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
    public float detectionRange = 10f;

    [Header("Attack Settings")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 2f;
    
    [Header("Die Settings")]
    public GameObject destroyPrefab;

    float fireCooldown = 0;
    Transform attackTarget;
    Quaternion initialTurretRotation;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    
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
        FindPlayer();
    }

    void Attack()
    {
        // go back to navigate
        if (attackTarget == null || Vector3.Distance(transform.position, attackTarget.position) > detectionRange)
        {
            attackTarget = null;
            currentState = EnemyState.Navigate;
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

    void Die()
    {
        if (destroyPrefab)
        {
            Instantiate(destroyPrefab, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }

    void FindPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange);
        Transform nearestTower = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                float distanceToEnemy = Vector3.Distance(transform.position, collider.transform.position);

                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestTower = collider.transform;
                }
            }
        }

        if (nearestTower)
        {
            attackTarget = nearestTower;
            Debug.Log("Player detected: " + attackTarget.name);
            currentState = EnemyState.Attack;
            return;
        }
    }

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

        // Ignore collisions with every collider on the enemy (root + all children)
        /*Collider bulletCollider = bullet.GetComponent<Collider>();
        Collider[] enemyColliders = GetComponentsInChildren<Collider>();
        foreach (Collider enemyCol in enemyColliders)
        {
            Physics.IgnoreCollision(bulletCollider, enemyCol);
        }*/
    }

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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Bullet"))
        {
            
        }
    }
}
