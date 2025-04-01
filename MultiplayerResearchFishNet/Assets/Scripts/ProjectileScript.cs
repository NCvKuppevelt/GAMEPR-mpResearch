using System.Runtime.CompilerServices;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public  float                 speed          = 10f;
    public  float                 expirationTime = 3f; //seconds
    private float                 timeAlive;
    private PlayerShootProjectile shooter;

    private void Start()
    {
        timeAlive = 0;
        shooter = FindObjectOfType<PlayerShootProjectile>();
    }

    private void FixedUpdate()
    {
        var dt = Time.deltaTime;
        transform.Translate(Vector3.forward * (speed * dt));
        timeAlive += dt;

        if (timeAlive < expirationTime) return;
        DestroyProjectile();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) other.GetComponentInParent<PlayerRespawn>().RespawnPlayer();
        else if (other.CompareTag("NPC")) other.GetComponentInParent<NpcScript>().Die();

        DestroyProjectile();
    }

    private void DestroyProjectile()
    {
        if (shooter)
        {
            shooter.RemoveProjectile(gameObject);
        }

        Destroy(gameObject);
    }
}