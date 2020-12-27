using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterController : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private int poolSize = 10;

    private Pool<Projectile> pool;

    private void Start()
    {
        pool = new Pool<Projectile>(new PrefabFactory<Projectile>(projectilePrefab), poolSize);
    }

    public virtual void Shoot()
    {
        Projectile newProj = pool.Allocate();
        Transform projTransform = newProj.transform;
        projTransform.position = transform.position;
        projTransform.rotation = transform.rotation;
        newProj.gameObject.SetActive(true);
        newProj.GetComponent<Projectile>().Shoot();
    }
}