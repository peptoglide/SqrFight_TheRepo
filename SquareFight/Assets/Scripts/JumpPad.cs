using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float force;
    public ParticleSystem particles;
    [SerializeField] bool resetXVelocity = true;
    [SerializeField] bool resetYVelocity = true;

    public bool playSound = true;
    public bool onlyPlayWhenPlayer = true;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Rigidbody2D rb))
        {
            if (resetXVelocity)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            if(resetYVelocity){
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
            
            rb.AddForce(force * transform.up, ForceMode2D.Impulse);
            if(particles != null)
            {
                particles.Play();
            }
            if (playSound)
            {
                if (onlyPlayWhenPlayer)
                {
                    if (collision.CompareTag("Player") || collision.CompareTag("Player 2"))
                        AudioManager.instance.PlaySound("JumpPad");
                }
                else
                    AudioManager.instance.PlaySound("JumpPad");
            }
        }
    }

}
