using System;

[Serializable]
public class DamageData
{
    public float damage;
    public Creature attacker;
    
    public DamageData(float damage, Creature attacker)
    {
        this.damage = damage;
        this.attacker = attacker;
    }
}