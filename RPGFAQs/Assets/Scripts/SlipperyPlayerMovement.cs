using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipperyPlayerMovement : MonoBehaviour {

    // How many units per second the player will move
    public float MaxSpeed = 5f;
    // Determines how quickly the player will reach max speed 
    // 2f = half second
    // 1f = one second
    // 0.5f = two seconds
    public float Acceleration = 2f;
    // Determines the amount of time it takes for the player to stop
    public float StoppingTime = 20f;
    // Defines the minimum amount of joystick movement that will cause the player to move
    public float Deadzone = 0.1f;
    
    private Rigidbody2D _rb;
    private Vector2 _velocity = Vector2.zero;
    private float _xDamp = 0f;
    private float _yDamp = 0f;

	// Use this for initialization
	void Start () {
        _rb = GetComponent<Rigidbody2D>();	
	}
	
	// Update is called once per frame
	void Update () {
        Move();
    }

    void Move()
    {
        float hSpeed = Input.GetAxis(InputConstants.MOV_H_AXIS);
        float vSpeed = Input.GetAxis(InputConstants.MOV_V_AXIS);
        var input = new Vector2(hSpeed, vSpeed);
        if (input.magnitude < Deadzone)
        {
            input = Vector2.zero;
        }
        else
        {
            input = input.normalized * ((input.magnitude - Deadzone) / (1 - Deadzone));
        }
        input.x *= Time.deltaTime * MaxSpeed * Acceleration;
        if (Mathf.Sign(input.x) != Mathf.Sign(_velocity.x))
        {
            input.x *= 2;
        }
        input.y *= Time.deltaTime * MaxSpeed * Acceleration;
        if (Mathf.Sign(input.y) != Mathf.Sign(_velocity.y))
        {
            input.y *= 2;
        }
        _velocity += input;
        _velocity.x = Mathf.Clamp(_velocity.x, -MaxSpeed, MaxSpeed);
        _velocity.y = Mathf.Clamp(_velocity.y, -MaxSpeed, MaxSpeed);
        if (input.x == 0f && _velocity.x != 0f)
        {
            _velocity.x = Mathf.SmoothDamp(_velocity.x, 0f, ref _xDamp, Time.deltaTime * StoppingTime);
        }
        if (input.y == 0f && _velocity.y != 0f)
        {
            _velocity.y = Mathf.SmoothDamp(_velocity.y, 0f, ref _yDamp, Time.deltaTime * StoppingTime);
        }
        _rb.velocity = _velocity;
    }
}
