using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManipulator : MonoBehaviour
{
    // Modifies the damage output of passing bullets
    [SerializeField] float damageMultiplier = 1f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Bullet bullet)){
            bullet.DamageManip(damageMultiplier);
        }
    }
}
