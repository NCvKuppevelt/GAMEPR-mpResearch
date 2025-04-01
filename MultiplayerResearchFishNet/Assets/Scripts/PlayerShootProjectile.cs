using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FishNet.Object;

public class PlayerShootProjectile : NetworkBehaviour
{
    private GameObject projectileSpawner;
    public  GameObject projectile;

    public           int              maxProjectiles     = 3;
    private readonly List<GameObject> _activeProjectiles = new();

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
        if (!Input.GetMouseButtonDown(0)) return;
        if (_activeProjectiles.Count >= maxProjectiles) return;

        SpawnProjectile(
            projectile,
            projectileSpawner.transform.position + projectileSpawner.transform.forward * 1.1f,
            projectileSpawner.transform.rotation
        );
    }

    [ServerRpc]
    private void SpawnProjectile(GameObject projectileObj, Vector3 position, Quaternion rotation)
    {
        var p = Instantiate(projectileObj, position, rotation);
        ServerManager.Spawn(p);
        _activeProjectiles.Add(p);
    }
    
    public void RemoveProjectile(GameObject projectileToRemove)
    {
        _activeProjectiles.Remove(projectileToRemove);
    }
}