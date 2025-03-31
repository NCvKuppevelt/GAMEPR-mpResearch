using UnityEngine;

public class NpcScript : MonoBehaviour
{
    public float shootCooldown = 1f;
    private float shootTimer;
    public float rotateSpeed = 10f;
    public GameObject projectile;

    private GameObject projectileSpawner;

    private void Start()
    {
        shootTimer = shootCooldown;
        projectileSpawner = transform.Find("Projectile Spawner").gameObject;
    }

    private void FixedUpdate()
    {
        var dt = Time.deltaTime;
        transform.Rotate(Vector3.up, rotateSpeed * dt);
        if (shootTimer > 0) shootTimer -= dt;
        else Shoot();
    }

    private void Shoot()
    {
        Instantiate(
            projectile,
            projectileSpawner.transform.position,
            projectileSpawner.transform.rotation
        );
        shootTimer = shootCooldown;
    }

    public void Die()
    {
        transform.parent.GetComponent<NPCSpawnScript>().Reset();
    }
}