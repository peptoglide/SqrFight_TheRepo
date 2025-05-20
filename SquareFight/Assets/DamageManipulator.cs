using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageManipulator : MonoBehaviour
{
    [SerializeField] TextMeshPro damageText; // Displays multiplier
    [SerializeField] float damageMultiplier = 1f;
    void Start()
    {
        if (damageText != null) damageText.SetText($"x{damageMultiplier}");
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
