using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 5f;
    public float rotationSpeed = 5f;
    public float lifetime = 5f;
    public GameObject bulletHitPrefab;
    private Transform target;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifetime);
        Debug.Log("Bullet spawned with speed: " + speed);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target == null)
        {
            return;
        }
        
        // direction
        Vector3 direction = (target.position - transform.position).normalized;

        // smooth rotation
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

        // move bullet forward
        rb.linearVelocity = transform.forward * speed;
    }

    public void SetTarget(Transform currentTarget)
    {
        target = currentTarget;
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Bullet hit " + collision.transform.name);

        if (bulletHitPrefab)
        {
            var pos = collision.contacts[0].point;
            Instantiate(bulletHitPrefab, pos, Quaternion.identity);
        }
    }
}
