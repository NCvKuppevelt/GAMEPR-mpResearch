using System;
using UnityEngine;
using FishNet.Object;

public class PlayerShootProjectile : NetworkBehaviour
{
    private GameObject projectileSpawner;
    public GameObject projectile;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!IsOwner) GetComponent<PlayerShootProjectile>().enabled = false;
    }

    public void Start()
    {
        if (projectile is null) throw new Exception("projectile not set!");
        projectileSpawner = transform.Find("Projectile Spawner").gameObject;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnProjectile(
                projectile,
                projectileSpawner.transform.position,
                projectileSpawner.transform.rotation
            );
        }
    }

    [ServerRpc]
    private void SpawnProjectile(GameObject projectileObj, Vector3 position, Quaternion rotation)
    {
        ServerManager.Spawn(Instantiate(
            projectileObj,
            position,
            rotation
        ));
    }
}