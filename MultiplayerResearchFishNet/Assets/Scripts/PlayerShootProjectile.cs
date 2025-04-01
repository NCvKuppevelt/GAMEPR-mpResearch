using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FishNet.Object;

public class PlayerShootProjectile : MonoBehaviour
{
    private GameObject projectileSpawner;
    public  GameObject projectile;

    public           int              maxProjectiles     = 3;
    private readonly List<GameObject> _activeProjectiles = new();

    public void Start()
    {
        if (projectile is null) throw new Exception("projectile not set!");
        projectileSpawner = transform.Find("Projectile Spawner").gameObject;
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        if (_activeProjectiles.Count >= maxProjectiles) return;

        // Uncomment when method implemented
        // SpawnProjectile(
        //     projectile,
        //     projectileSpawner.transform.position + projectileSpawner.transform.forward * 1.1f,
        //     projectileSpawner.transform.rotation
        // );
    }

    [ServerRpc]
    private void SpawnProjectile(
        //TODO: Implement parameters
        )
    {
        //TODO: Implement method
    }
    
    public void RemoveProjectile(GameObject projectileToRemove)
    {
        _activeProjectiles.Remove(projectileToRemove);
    }
}