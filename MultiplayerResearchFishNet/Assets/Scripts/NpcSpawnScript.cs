using FishNet.Object;
using GameKit.Dependencies.Utilities;
using UnityEngine;

public class NpcSpawnScript : NetworkBehaviour
{
    public GameObject npcPrefab;
    public float spawnCooldown = 5f;

    private float spawnTimer;
    private bool npcActive;
    private GameObject npc;

    private void Start()
    {
        if (!IsServerStarted) Destroy(gameObject);
        else ResetSpawner();
    }

    private void Update()
    {
        if (npc) return;
        if (npcActive)
        {
            ResetSpawner();
            return;
        }
        var dt = Time.deltaTime;
        if (spawnTimer > 0) spawnTimer -= dt;
        else SpawnNpc();
    }

    private void SpawnNpc()
    {
        npc = Instantiate(npcPrefab, transform.position, transform.rotation);
        Spawn(npc);
        npcActive = true;
    }

    private void ResetSpawner()
    {
        transform.DestroyChildren();
        npc = null;
        npcActive = false;
        spawnTimer = spawnCooldown;
    }
}