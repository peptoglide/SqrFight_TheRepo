using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GunStats stats;
    public GameObject particles;
    public float effLifetime;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GunDestroy()
    {
        Destroy(gameObject);
        if(particles != null)
        {
            GameObject eff = Instantiate(particles, transform.position, transform.rotation);
            Destroy(eff, effLifetime);
        }
    }
}
