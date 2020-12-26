﻿using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Hero : Creature
{
    [SerializeField]
    private Joystick joystick;

    private new Rigidbody rigidbody;
    private float direction;
    private float lastShootTime;


    protected override void Awake()
    {
        base.Awake();
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.isKinematic = false;
    }


    private void Update()
    {
        CheckState();
        if (currentState == CreatureState.Idle)
        {
            aimController.HandleTarget();
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void CheckState()
    {
        switch (currentState)
        {
            case CreatureState.Idle when joystick.Direction != Vector2.zero:
                currentState = CreatureState.Moving;
                break;
            case CreatureState.Moving when joystick.Direction == Vector2.zero:
                currentState = CreatureState.Idle;
                break;
            case CreatureState.Dead:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }


    protected override void Move()
    {
        base.Move();
        var dir = new Vector3(joystick.Direction.x, 0, joystick.Direction.y);
        transform.LookAt(transform.position + dir);
#if UNITY_EDITOR
        Debug.DrawRay(transform.position, dir * 3, Color.blue);
#endif
        rigidbody.velocity = dir * Speed;
        if (joystick.Direction == Vector2.zero)
        {
            rigidbody.angularVelocity = dir;
        }
    }
}