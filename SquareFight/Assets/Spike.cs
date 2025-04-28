using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public float damage;
    public float repelForce;
    public ParticleSystem[] effects;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Inflict damage
        if(collision.collider.TryGetComponent(out Health health))
        {
            health.TakeDamage(damage);
            // Play effects
            if(effects.Length > 0)
            {
                foreach(ParticleSystem eff in effects)
                {
                    eff.Play();
                }
            }
        }

        // Repel the object
        if(collision.collider.TryGetComponent(out Rigidbody2D rb))
        {
            Vector2 dir = collision.transform.position - transform.position; dir.Normalize();
            rb.AddForce(dir * repelForce, ForceMode2D.Impulse);
        }

        if(collision.gameObject.layer == 9)
        {
            Gun gun = collision.gameObject.GetComponent<Gun>();
            if(gun != null) gun.GunDestroy();
        }
    }
}
