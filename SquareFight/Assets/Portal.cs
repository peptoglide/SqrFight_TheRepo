using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public PortalManager portalManager;
    public Transform otherPosition;

    bool _active = true;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!_active) return;
        // Teleport physical objects
        if (collider.TryGetComponent(out Rigidbody2D rb) && rb.bodyType == RigidbodyType2D.Dynamic)
        {
            collider.transform.position = otherPosition.position;
            // Set inactivity to prevent indefinite teleportation
            portalManager.SetInactiveBoth();
        }
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (!_active) return;
        // Teleport physical objects
        if (collider.gameObject.TryGetComponent(out Rigidbody2D rb) && rb.bodyType == RigidbodyType2D.Dynamic)
        {
            collider.transform.position = otherPosition.position;
            // Set inactivity to prevent indefinite teleportation
            portalManager.SetInactiveBoth();
        }
    }

    public void SetInactive()
    {
        _active = false;
    }

    public void SetActive()
    {
        _active = true;
    }
}
