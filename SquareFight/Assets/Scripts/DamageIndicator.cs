using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageIndicator : MonoBehaviour
{
    public TextMeshPro text;
    public float startingForce;

    public float lifetime = 1.5f;

    void Start()
    {
        if (TryGetComponent(out Rigidbody2D rb))
        {
            rb.AddForce(Random.insideUnitCircle * startingForce, ForceMode2D.Impulse);
        }
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateText(string t)
    {
        text.SetText(t);
    }
}
