using FishNet.Object;
using FishNet.Managing;
using UnityEngine;

public class PlayerRespawn : NetworkBehaviour
{
    public float fallHeightThreshold = -10f;
    private SpawnManager spawnManager;

    private void Start()
    {
        if (SpawnManager.Instance == null)
        {
        }
        else
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
        if (IsOwner)
        {
            NetworkObject networkObject = GetComponent<NetworkObject>();
            ServerManager.Despawn(networkObject.gameObject);
        }

        Transform selectedSpawnPoint = spawnManager.GetRandomSpawnPoint();
        if (selectedSpawnPoint != null)
        {
            GameObject newPlayer = Instantiate(gameObject, selectedSpawnPoint.position, Quaternion.identity);

            NetworkObject newNetworkObject = newPlayer.GetComponent<NetworkObject>();
            ServerManager.Spawn(newNetworkObject.gameObject, Owner);
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
    }
}
