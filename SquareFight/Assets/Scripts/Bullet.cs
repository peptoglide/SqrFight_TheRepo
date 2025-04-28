using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool playSound = true;
    public bool destroyOnCollision = true;
    [HideInInspector]
    public GunStats stats;

    public GameObject eff;

    public float effLifetime;

    float damage;
    
    // Explosion
    public bool isExplosive;
    [HideInInspector]
    public ExplosionStats explosionStats;

    [HideInInspector]
    public float damageMultiplier = 1f;

    Explosion explosion;

    void Start()
    {
        // Queue a destruction
        Destroy(gameObject, stats.lifetime);
        damage = (stats.damage + Random.Range(-stats.damageOffset, stats.damageOffset)) * damageMultiplier * GameManager.instance.globalDamageMultiplier;
        // Explosive
        if (stats.explosive)
        {
            isExplosive = true;
            explosion = GetComponent<Explosion>();
            explosion.stats = stats.explosionStats;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            BulletDestroy();
        }
        if (collision.gameObject.TryGetComponent(out Health health))
        {
            // Deal damage if object has health
            health.TakeDamage(damage);
            BulletDestroy();
            if (playSound)
            {
                AudioManager.instance.PlaySound("Hit");
            }
        }
    }

    void BulletDestroy()
    {
        if (!destroyOnCollision) return;
        Destroy(gameObject);
        if(eff != null)
        {
            GameObject vfx = Instantiate(eff, transform.position, Quaternion.identity);
            Destroy(vfx, effLifetime);
        }
        if (isExplosive && explosion != null)
        {
            explosion.Explode();
        }
    }
}
