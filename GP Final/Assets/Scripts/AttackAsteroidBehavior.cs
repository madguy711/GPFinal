using UnityEngine;

public class AttackAsteroidBehavior : MonoBehaviour
{
    public float speed = 15f;
    public float lifetime = 15f;
    public float rotationSpeed = 5f;
    public GameObject player;
    private Rigidbody rb;
    void Start()
    {
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifetime);
        transform.LookAt(player.transform);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = transform.forward*speed;
        
    }
    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
