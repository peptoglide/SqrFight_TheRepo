using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Vector2[] positions;
    [SerializeField] float moveTime = 1f;
    int _current = 0;
    bool _usable = true;
    bool _is_moving = false;
    int _total_pos;

    float _move_progress = 0f;
    Vector2[] global_pos;

    // Whether platforms launch objects off
    [Header("Launching")]
    [SerializeField] bool launchObjects;
    [Tooltip("Fraction of momentum carried to object")]
    [SerializeField] float momentumCarry;

    Vector2 _velocity;
    Vector2 _lastPos;
    float transferDelay = 0.15f; // Delay between each transfer to prevent momentum duplication
    float transferTimer = 0f;

    void Start()
    {
        _total_pos = positions.Length;
        Vector2 myPos_2D = new Vector2(transform.position.x, transform.position.y);
        global_pos = new Vector2[_total_pos];
        for (int i = 0; i < _total_pos; i++) global_pos[i] = positions[i] + myPos_2D;

        _lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transferTimer += Time.deltaTime;
        _velocity = ((Vector2)transform.position - _lastPos) / Time.deltaTime;
        _lastPos = transform.position;
        if (moveTime == 0f) return; // To the future bitches who wish an explosion on my computer
        if(!_is_moving) return;
        _move_progress += Time.deltaTime;
        float blend = GetBlendFactor();

        Vector2 _start = global_pos[_current];
        Vector2 _end = global_pos[(_current + 1) % _total_pos];
        transform.position = Vector2.Lerp(_start, _end, blend);
        if(_move_progress >= moveTime){
            // Reset
            _usable = true;
            _is_moving = false;
            _move_progress = 0f;
            _current = (_current + 1) % _total_pos;
        }
    }

    void TryToActivate(){
        if(!_usable) return;
        _usable = false;
        _is_moving = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Rigidbody2D rb)){
            // Set parent
            collision.transform.parent = transform;
            rb.freezeRotation = true; // Rotation sucks with scale
        }
        if(collision.TryGetComponent(out InputManager input_manager)){
            collision.transform.parent = transform;
            input_manager.onInteract += TryToActivate;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Rigidbody2D rb) && collision.transform.parent == transform)
        {
            // Set parent
            collision.transform.parent = null;
            rb.freezeRotation = false; // Rotation sucks with scale
            if (launchObjects && transferTimer >= transferDelay)
            {
                rb.AddForce(_velocity * momentumCarry, ForceMode2D.Impulse);
                transferTimer = 0f;
            }
        }
        if(collision.TryGetComponent(out InputManager input_manager)){
            collision.transform.parent = null;
            input_manager.onInteract -= TryToActivate;
            Rigidbody2D player_rb = collision.GetComponent<Rigidbody2D>();
            player_rb.freezeRotation = true;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        _total_pos = positions.Length;
        global_pos = new Vector2[_total_pos];
        Vector2 myPos_2D = new Vector2(transform.position.x, transform.position.y);
        for (int i = 0; i < _total_pos; i++) global_pos[i] = positions[i] + myPos_2D;

        for (int i = 0; i < _total_pos; i++)
        {
            if (i != _total_pos - 1)
            {
                Color old = Gizmos.color;
                Gizmos.color = Color.white;
                Gizmos.DrawLine(global_pos[i], global_pos[i + 1]);
                Gizmos.color = old;
            }
            Gizmos.DrawCube(global_pos[i], 0.25f * Vector3.one);
            float rgb = (float)(i + 2f) / (float)_total_pos;
            Color new_col = new Color(rgb, rgb, rgb, 1);
            Gizmos.color = new_col;
        }
    }

    float GetBlendFactor(){
        // Get a function
        float progress = Mathf.Min(1f, _move_progress / moveTime);
        return Mathf.Sin(progress * Mathf.PI / 2f);
    }
}
