using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [Header("Destruction delay")]
    public bool destroyAfterDelay;
    public float destroyDelay;
    //
    public float damagePerSecond;
    public Transform follow;
    //
    [Header("Burn duration")]
    public bool extraBurn = true;
    public float burnDuration;
    public float newFireDamage;
    public GameObject fire;

    void Start()
    {
        if (destroyAfterDelay)
        {
            Destroy(gameObject, destroyDelay);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(follow != null)
        {
            transform.position = follow.position;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Health health))
        {
            health.TakeDamage(damagePerSecond * Time.deltaTime * GameManager.instance.globalDamageMultiplier, false);
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        InflictExtraFire(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        InflictExtraFire(collision.collider);
    }

    void InflictExtraFire(Collider2D collision)
    {
        if (!extraBurn) return;
        if (collision.TryGetComponent(out Health health))
        {
            GameObject newFire = Instantiate(fire, transform.position, Quaternion.identity);
            Fire f = newFire.GetComponent<Fire>();
            if (f != null)
            {
                f.follow = collision.transform;
                f.destroyAfterDelay = true;
                f.destroyDelay = burnDuration;
                f.damagePerSecond = newFireDamage;
            }
        }
    }
}
