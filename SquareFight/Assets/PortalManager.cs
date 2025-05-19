using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    [SerializeField] GameObject portal;
    [SerializeField] Bounds spawnZone;
    [SerializeField] float inactiveTimeAfterUse = 1f;
    [SerializeField] float lifetime = 6f;
    [SerializeField] float portalSpace = 12.5f; // Min distance between two portals
    GameManager _manager;
    Portal portal1, portal2;
    // Start is called before the first frame update
    void Start()
    {
        _manager = GameManager.instance;
        // Immediately find two positions
        Vector3 spawnPos1 = GetValidPosition(false, transform.position);
        Vector3 spawnPos2 = GetValidPosition(true, spawnPos1);
        if (!_manager.ValidPosition(spawnPos1) || !_manager.ValidPosition(spawnPos2))
        {
            Destroy(gameObject);
            return;
        }

        portal1 = Instantiate(portal, spawnPos1, transform.rotation).GetComponent<Portal>();
        portal1.portalManager = this;

        
        portal2 = Instantiate(portal, spawnPos2, transform.rotation).GetComponent<Portal>();
        portal2.portalManager = this;

        portal1.otherPosition = portal2.transform;
        portal2.otherPosition = portal1.transform;


        Destroy(gameObject, lifetime);
        Destroy(portal1.gameObject, lifetime);
        Destroy(portal2.gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {

    }

    Vector3 GetValidPosition(bool hasOther, Vector3 otherPortal)
    {
        // Prevent softlocks
        int iterations = 0;
        Vector3 spawnPos = transform.position;
        do
        {
            spawnPos = transform.position + new Vector3(Random.Range(-spawnZone.extents.x, spawnZone.extents.x),
                    Random.Range(-spawnZone.extents.y, spawnZone.extents.y), 0f);
            iterations++;
        } while (iterations <= 100 && !_manager.ValidPosition(spawnPos) && (!hasOther || Vector3.Distance(spawnPos, otherPortal) <= portalSpace));
        
        return spawnPos;
    }

    /// <summary>
    /// Set both portals to be inactive
    /// </summary>
    public void SetInactiveBoth()
    {
        portal1.SetInactive();
        portal2.SetInactive();
        portal1.Invoke(nameof(portal1.SetActive), inactiveTimeAfterUse);
        portal2.Invoke(nameof(portal2.SetActive), inactiveTimeAfterUse);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 drawPos = transform.position + spawnZone.center;
        Gizmos.DrawWireCube(drawPos, spawnZone.size);
    }
}
