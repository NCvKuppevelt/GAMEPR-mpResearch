using System.Collections;
using GameKit.Dependencies.Utilities;
using UnityEngine;

public class NPCSpawnScript : MonoBehaviour
{
    public GameObject npcPrefab;
    public float cooldown = 5f;

    private float cooldownTimer;
    private bool npcActive;

    private void Start()
    {
        Reset();
    }

    private void Update()
    {
        if (npcActive) return;
        var dt = Time.deltaTime;
        if (cooldownTimer > 0) cooldownTimer -= dt;
        else SpawnNpc();
    }

    private void SpawnNpc()
    {
        var npc = Instantiate(npcPrefab, transform);
        npcActive = true;
    }

    public void Reset()
    {
        transform.DestroyChildren();
        npcActive = false;
        cooldownTimer = cooldown;
    }
}