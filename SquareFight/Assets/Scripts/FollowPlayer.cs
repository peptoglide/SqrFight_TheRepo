using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform[] players;
    public float chaseSpeed;

    public Vector3 offset;

    Vector3 currentVel;
    Vector3 centerPos;

    public Vector2 minMaxZoom;
    public float maxDistance = 30f;

    Camera cam;
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (players.Length == 0) return;
        // Bounding
        if (players.Length == 1)
        {
            centerPos = players[0].position;
        }

        var bounds = new Bounds();
        if (players[0] != null)
        { bounds = new Bounds(players[0].position, Vector3.zero); }
        else
        {
            if (players[1] == null) return;
            bounds = new Bounds(players[1].position, Vector3.zero);
        }
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] != null)
                bounds.Encapsulate(players[i].position);
        }

        centerPos = bounds.center;
        float zoom = Mathf.Lerp(minMaxZoom.x, minMaxZoom.y, bounds.size.magnitude / maxDistance);
        
        Vector3 targetPos = centerPos + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref currentVel, chaseSpeed);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoom, Time.deltaTime);
    }
}
