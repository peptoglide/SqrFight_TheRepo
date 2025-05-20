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
    Rigidbody2D rb;
    Collider2D coll;
    // Whether the bullet can interact with objects with Health
    bool is_bullet_active = true;
    Transform _tracked_object;
    float _time_elapsed = 0f;

    public void Start()
    {
        is_bullet_active = true;
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
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        _time_elapsed += Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(!stats.tracking) return;
        _time_elapsed += Time.deltaTime;
        // Tracking
        if(_tracked_object == null) return;
        if(_time_elapsed < stats.trackDelay) return;
        Debug.Log("Should be tracking");
        // Angle track
        float angle = AngleTo(_tracked_object);
        float blend_factor = Mathf.Pow(0.5f, Time.deltaTime * stats.trackTurnSpeed);
        Quaternion desired = Quaternion.Euler(0f, 0f, angle);
        Quaternion new_rotation = Quaternion.Lerp(desired, transform.rotation, blend_factor);
        transform.rotation = new_rotation;

        // Speed track
        Vector2 desired_vel = transform.right * stats.trackSpeed;
        rb.velocity = Vector2.Lerp(desired_vel, rb.velocity, blend_factor);
        //rb.velocity = transform.right * stats.trackSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!is_bullet_active) return;
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

    /// <summary>
    /// Multiply the damage output by given argument
    /// </summary>
    /// <param name="multiplier"></param>
    public void DamageManip(float multiplier){
        damage *= multiplier;
    }

    /// <summary>
    /// For the first how many seconds is the bullet inactive against objects with health
    /// </summary>
    /// <param name="time">The duration given in seconds</param>
    public void SetSleepTime(float time){
        is_bullet_active = false;
        Invoke(nameof(Unsleep), time);
    }

    void Unsleep(){
        is_bullet_active = true;
    }

    /// <summary>
    /// Set tracked target
    /// </summary>
    /// <param name="tracked">Desired target to be tracked</param>
    public void Track(Transform tracked){
        _tracked_object = tracked;
    }

    private float AngleTo(Transform target)
    {
        if (target == null) return -1f;
        Vector2 dir = target.position - transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return angle;
    }
}
