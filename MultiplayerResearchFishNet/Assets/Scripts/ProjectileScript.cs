using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float speed = 10f;
    public float expirationTime = 3f; //seconds
    private float timeAlive;
    
    private void Start()
    {
        timeAlive = 0;
    }

    private void FixedUpdate()
    {
        var dt = Time.deltaTime;
        transform.Translate(Vector3.forward * (speed * dt));
        timeAlive += dt;

        if (timeAlive < expirationTime) return;
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider _)
    {
        Destroy(gameObject);
    }
}
