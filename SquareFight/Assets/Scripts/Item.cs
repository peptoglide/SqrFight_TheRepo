using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        Heal,
        Damage,
        Speed
    }

    public ItemType type;
    public GameObject vfx;
    public bool playSound = true;

    [HideInInspector]
    public float healAmount;

    [HideInInspector]
    public float damageAmount;

    [HideInInspector]
    public float speedIncrease;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player") && !collision.collider.CompareTag("Player 2")) return;

        if(collision.collider.TryGetComponent(out Health health) && type == ItemType.Heal)
        {
            health.TakeDamage(-healAmount, false);
            ItemDestroy();
        }
        if(type == ItemType.Damage)
        {
            if(collision.collider.TryGetComponent(out Movement m))
            {
                m.gun.damageMultiplier += damageAmount;
                ItemDestroy();
            }
        }
        if(type == ItemType.Speed)
        {
            if (collision.collider.TryGetComponent(out Movement m))
            {
                m.stats.moveSpeed *= speedIncrease;
                ItemDestroy();
            }
        }
    }

    void ItemDestroy()
    {
        if(vfx != null)
        {
            GameObject eff = Instantiate(vfx, transform.position, Quaternion.identity);
            Destroy(eff, 3f);
        }
        if (playSound)
        {
            AudioManager.instance.PlaySound("PowerUp");
        }
        Destroy(gameObject);
    }
}
