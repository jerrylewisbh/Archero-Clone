using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HeroAimController : AimController
{
    //TODO remove it later
    private Transform testTarget;

    protected  override void Awake()
    {
        base.Awake();
        //TODO: remove it later
        testTarget = GameObject.Find("Target").transform;
    }
    
    //finds a target;
    protected override bool Aim()
    {
        var targetFound = false;
        minDistance = -1;
        
        //TODO: loop through nearby enemies and try to assign them to current target;
        targetFound = AssignTarget(testTarget);
        
        return targetFound;
    }
}