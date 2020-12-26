using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Creature : MonoBehaviour
{
    public delegate void CreatureEvent();

    public event CreatureEvent CreatureDied;
    public event CreatureEvent CreatureMoved;

    [SerializeField]
    protected float speed;

    [SerializeField]
    protected float maxHp;

    [SerializeField]
    protected float attackSpeed;

    [SerializeField]
    protected float damage;

    protected float currentHp;

    public float Speed => speed;

    public float MaxHp => maxHp;
    public float Hp => currentHp;

    public float AttackSpeed => attackSpeed;

    public float Damage => damage;


    protected virtual void Awake()
    {
        currentHp = maxHp;
    }

    protected virtual void Death(Creature killer)
    {
        CreatureDied?.Invoke();
    }

    protected virtual void Move()
    {
        CreatureMoved?.Invoke();
    }


    public void TakeDamage(DamageData damageReport)
    {
        currentHp -= damageReport.damage;
        if (currentHp <= 0)
        {
            Death(damageReport.attacker);
        }
    }
}