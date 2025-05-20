using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[System.Serializable]
    public class SpawnObj
    {
        public GameObject obj;
        [Range(0f, 100f)]
        public float probability;
        public bool addForce = false;
        public float additionalForce = 0f;
}
    
public class Spawner : MonoBehaviour
{


    [Tooltip("List of items to spawn, should sum to 100")]
    public SpawnObj[] spawns;
    public Bounds spawnZone;
    [SerializeField] float avgSpawnsPerSec; // Just an average

    [Tooltip("Spawn rate is divided by this, or, spawn rate gets reduced by this many times")]
    public float probDecrease = 1f;

    GameManager _manager;

    void Start()
    {
        _manager = GameManager.instance;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool can_spawn = Random.Range(0f, Application.targetFrameRate) * probDecrease < avgSpawnsPerSec * GameManager.instance.rateIncrease;
        if (!can_spawn) return;

        int to_spawn = GetRandomItem();
        if (to_spawn == -1) return; // Sum isn't 100
        Spawn(to_spawn);
    }

    /// <summary>
    /// Return index of the item to be spawned. If failed, return -1
    /// </summary>
    /// <returns>Index of the item</returns>
    int GetRandomItem()
    {
        float prob = Random.Range(0f, 100f);
        for (int i = 0; i < spawns.Length; i++)
        {
            if (prob < spawns[i].probability)
            {
                return i;
            }
            prob -= spawns[i].probability;
        }
        return -1;
    }

    void Spawn(int index)
    {
        Vector3 spawnPos = GetValidPosition();
        if (spawnPos.x == -1800) return;

        GameObject obj = Instantiate(spawns[index].obj, spawnPos, Quaternion.identity);
        if (spawns[index].addForce && obj.TryGetComponent(out Rigidbody2D rb))
        {
            rb.AddForce(spawns[index].additionalForce * Random.insideUnitCircle, ForceMode2D.Impulse);
        }
    }

    Vector3 GetValidPosition()
    {
        // Prevent softlocks
        int iterations = 0;
        Vector3 spawnPos;
        do
        {
            spawnPos = new Vector3(Random.Range(-spawnZone.extents.x, spawnZone.extents.x),
                    Random.Range(-spawnZone.extents.y, spawnZone.extents.y), 0f) + transform.position;
            iterations++;
        } while (iterations <= 100 && !_manager.ValidPosition(spawnPos));
        
        if (!_manager.ValidPosition(spawnPos)) return new Vector3(-1800, 0, 0);
        return spawnPos;
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 drawPos = transform.position + spawnZone.center;
        Gizmos.DrawWireCube(drawPos, spawnZone.size);
    }
}
