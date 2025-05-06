using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float horizontalMovement { get; private set; }
    [SerializeField] Team team;
    public event Action onJumpPressed;
    public event Action onJumpReleased;
    public bool isShooting { get; private set; }
    public event Action onDiscardWeapon;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        horizontalMovement = 0f;
        if(team == Team.Blue)
        {
            // Standard movement
            if (Input.GetKey(KeyCode.A))
            {
                horizontalMovement = -1f;
            }
            if (Input.GetKey(KeyCode.D))
            {
                horizontalMovement = 1f;
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                onJumpPressed?.Invoke();
            }
            if(Input.GetKeyUp(KeyCode.W)){
                onJumpReleased?.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isShooting = true;
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                isShooting = false;
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                onDiscardWeapon?.Invoke();
            }
        }

        if (team == Team.Red)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                horizontalMovement = -1f;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                horizontalMovement = 1f;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                onJumpPressed?.Invoke();
            }
            if(Input.GetKeyUp(KeyCode.UpArrow)){
                onJumpReleased?.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.Slash))
            {
                isShooting = true;
            }
            if (Input.GetKeyUp(KeyCode.Slash))
            {
                isShooting = false;
            }
            if (Input.GetKeyDown(KeyCode.Period))
            {
                onDiscardWeapon?.Invoke();
            }
        }
    }
}
