using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnObj
    {
        public GameObject obj;
        public float probability;
        public bool addForce = false;
        public float additionalForce = 0f;
    }

    public SpawnObj[] spawns;
    public Bounds spawnZone;
    float prob;

    [Tooltip("Spawn rate is divided by this")]
    public float probDenominator = 1f;

    void Start()
    {
        print(spawnZone.extents.x);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        prob = Random.Range(0f, 100f) * probDenominator / GameManager.instance.rateIncrease;
        // Checking
        for (int i = 0; i < spawns.Length; i++)
        {
            if(prob < spawns[i].probability)
            {
                Vector3 spawnPos = spawnZone.center + new Vector3(Random.Range(-spawnZone.extents.x, spawnZone.extents.x),
                    Random.Range(-spawnZone.extents.y, spawnZone.extents.y), 0f);

                GameObject obj = Instantiate(spawns[i].obj, spawnPos, Quaternion.identity);
                if (spawns[i].addForce && obj.TryGetComponent(out Rigidbody2D rb))
                {
                    rb.AddForce(spawns[i].additionalForce * Random.insideUnitCircle, ForceMode2D.Impulse);
                }
                break;
            }
        }
    }

    public void Spawn()
    {

    }

    private void OnDrawGizmosSelected()
    {
        Vector3 drawPos = transform.position + spawnZone.center;
        Gizmos.DrawWireCube(drawPos, spawnZone.size);
    }
}
