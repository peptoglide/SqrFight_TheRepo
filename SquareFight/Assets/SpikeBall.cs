using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBall : MonoBehaviour
{
    [SerializeField] int numberOfSpikes;
    [SerializeField] float lifetime;
    [SerializeField] float damage;
    [SerializeField] float force;
    [SerializeField] bool explosive;
    [SerializeField] ExplosionStats explosionStats;

    [SerializeField] float explosionDelay;
    [SerializeField] GameObject spike;

    [SerializeField] GameObject particles;
    GunStats bulletStats;

    [Tooltip("Whether spike ball explodes on contact")]
    [SerializeField] bool explodeOnContact;
    [SerializeField] float shardsInactiveTime = 0.04f;


    void Start()
    {
        bulletStats = new();
        bulletStats.damage = damage;
        bulletStats.damageOffset = 0f;
        bulletStats.lifetime = lifetime;
        bulletStats.explosive = explosive;
        if(explosionStats != null) bulletStats.explosionStats = explosionStats;
        Invoke(nameof(Explode), explosionDelay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (explodeOnContact)
        {
            Explode();
        }
    }

    void Explode()
    {
        Destroy(gameObject);
        // Spawn effects if possible
        if(particles != null)
        {
            GameObject eff = Instantiate(particles, transform.position, Quaternion.identity);
            Destroy(eff, 3f);
        }
        // Spawn correct number of spikes and shoot out
        for (int i = 0; i < numberOfSpikes; i++)
        {
            GameObject shard = Instantiate(spike, transform.position, Quaternion.identity);
            if(shard.TryGetComponent(out Rigidbody2D rb))
            {
                rb.AddForce(force * Random.insideUnitCircle, ForceMode2D.Impulse);
                rb.AddTorque(Random.Range(30f, 180f));
            }
            if(shard.TryGetComponent(out Bullet bullet))
            {
                bullet.stats = bulletStats;
                bullet.SetSleepTime(shardsInactiveTime);
            }
        }
    }
}
