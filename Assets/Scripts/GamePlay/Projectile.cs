using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour, IResettable
{
    [SerializeField]
    protected float speed = 10;
    protected new Rigidbody rigidbody;

    protected void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        Reset();
    }

    public void Reset()
    {
        gameObject.SetActive(false);
    }
    
    public virtual void Shoot()
    {
        rigidbody.velocity = transform.forward * speed;
    }

}
