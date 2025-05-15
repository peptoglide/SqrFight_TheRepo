using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Vector2 startPos;
    [SerializeField] Vector2 endPos;
    [SerializeField] float moveTime = 1f;

    Vector2 _start;
    Vector2 _end;
    bool _at_start = true;
    bool _usable = true;
    bool _is_moving = false;

    float _move_progress = 0f;

    void Start()
    {
        _start = new Vector2(transform.position.x, transform.position.y) + startPos;
        _end = new Vector2(transform.position.x, transform.position.y) + endPos;
    }

    // Update is called once per frame
    void Update()
    {
        if(moveTime == 0f) return; // To the future bitches who wish an explosion on my computer
        if(!_is_moving) return;
        _move_progress += Time.deltaTime;
        float blend = GetBlendFactor();
        if(!_at_start) blend = 1 - blend;
        
        transform.position = Vector2.Lerp(_start, _end, blend);
        if(_move_progress >= moveTime){
            // Reset
            _usable = true;
            _is_moving = false;
            _move_progress = 0f;
            _at_start = !_at_start;
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
        if(collision.TryGetComponent(out Rigidbody2D rb) && collision.transform.parent == transform){
            // Set parent
            collision.transform.parent = null;
            rb.freezeRotation = false; // Rotation sucks with scale
        }
        if(collision.TryGetComponent(out InputManager input_manager)){
            collision.transform.parent = null;
            input_manager.onInteract -= TryToActivate;
            Rigidbody2D player_rb = collision.GetComponent<Rigidbody2D>();
            player_rb.freezeRotation = true;
        }
    }

    void OnDrawGizmosSelected()
    {
        _start = new Vector2(transform.position.x, transform.position.y) + startPos;
        _end = new Vector2(transform.position.x, transform.position.y) + endPos;
        Gizmos.DrawLine(_start, _end);
        Gizmos.DrawCube(_start, 0.25f * Vector3.one);
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(_end, 0.25f * Vector3.one);
    }

    float GetBlendFactor(){
        // Get a function
        float progress = Mathf.Min(1f, _move_progress / moveTime);
        return Mathf.Sin(progress * Mathf.PI / 2f);
    }
}
