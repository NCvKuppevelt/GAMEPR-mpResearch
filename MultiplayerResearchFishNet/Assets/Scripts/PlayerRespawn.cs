using FishNet.Object;
using FishNet.Managing;
using UnityEngine;

public class PlayerRespawn : NetworkBehaviour
{
    public float fallHeightThreshold = -10f;
    private SpawnManager spawnManager;

    private void Start()
    {
        if (SpawnManager.Instance != null)
        {
            spawnManager = SpawnManager.Instance;
        }
    }

    private void Update()
    {
        if (IsOwner && transform.position.y < fallHeightThreshold)
        {
            RespawnPlayer();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (IsOwner && other.gameObject.CompareTag("Projectile"))
        {
            RespawnPlayer();
        }
    }

    public void RespawnPlayer()
    {
        if (IsOwner)
        {
            CmdRespawnPlayer();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void CmdRespawnPlayer()
    {
        // Despawn the current player object on the server
        if (IsOwner)
        {
            NetworkObject networkObject = GetComponent<NetworkObject>();
            ServerManager.Despawn(networkObject.gameObject);
        }

        // Get a random spawn point and instantiate a new player
        Transform selectedSpawnPoint = spawnManager.GetRandomSpawnPoint();
        if (selectedSpawnPoint != null)
        {
            GameObject newPlayer = Instantiate(gameObject, selectedSpawnPoint.position, Quaternion.identity);

            // Spawn the new player on the server and assign ownership
            NetworkObject newNetworkObject = newPlayer.GetComponent<NetworkObject>();
            ServerManager.Spawn(newNetworkObject.gameObject, Owner);

            // Ensure that only one player instance exists by despawning the old player
            Destroy(gameObject); // this will remove the old player object
        }
    }
}
