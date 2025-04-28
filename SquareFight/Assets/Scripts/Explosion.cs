using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
//using EZCameraShake;

public class Explosion : MonoBehaviour
{
    public bool destroyOnExplosion = false;
    public ExplosionStats stats;

    public GameObject particles;
    public float effLifetime;
    public GameObject rangeIndicator;

    public float indiSizeMalnip = 1f;

    public bool playSound;

    [Header("Explode on start")]
    public bool explodeOnStart;
    public float explodeDelay;

    [Header("Shake")]
    public bool doShake = true;

    public float mag;
    public float roughness;
    public float fadeIn;
    public float fadeOut;
    //Inspector//
    [HideInInspector]
    public float sliderVal;

    void Start()
    {
        if(rangeIndicator != null)
        {
            rangeIndicator.transform.localScale = Vector3.one * stats.radius * 2f * indiSizeMalnip;
        }
        if (explodeOnStart)
        {
            Invoke(nameof(Explode), explodeDelay);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Explode()
    {
        Collider2D[] impactedObj = Physics2D.OverlapCircleAll(transform.position, stats.radius);
        foreach (Collider2D obj in impactedObj)
        {
            // Apply force
            if (obj.TryGetComponent(out Rigidbody2D rb))
            {
                float force = stats.force * 3 / (4 * Mathf.PI * stats.radius * stats.radius * stats.radius);
                Vector2 dir = obj.transform.position - transform.position;
                rb.AddForce(force * dir, ForceMode2D.Impulse);
            }
            // Apply damage
            if (obj.TryGetComponent(out Health health))
            {
                float distanceRatio = (transform.position - obj.transform.position).magnitude / stats.radius;
                float damage = Mathf.Lerp(stats.minDamage, stats.maxDamage, 1 - distanceRatio);
                health.TakeDamage(damage);
            }
        }
        if(particles != null)
        {
            GameObject vfx = Instantiate(particles, transform.position, transform.rotation);
            Destroy(vfx, effLifetime);
        }

        if (destroyOnExplosion)
        {
            Destroy(gameObject);
        }
        
        /* Effects
        // Camera shake
        if (doShake)
        {
            CameraShaker.Instance.ShakeOnce(mag, roughness, fadeIn, fadeOut);
        }
        */
        if (playSound)
        {
            AudioManager.instance.PlaySound("Explosion");
        }
    }
}
