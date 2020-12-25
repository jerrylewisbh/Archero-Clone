using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class PerspectiveShift : MonoBehaviour
{
    [SerializeField]
    private Vector2 lensShift = Vector2.zero;

    [SerializeField]
    private Vector2 positionShift = Vector2.zero;


    private Camera camera;
    private Matrix4x4 targetMatrix;
    private Vector2 baseAspectScale;


    private void Awake()
    {
        camera = GetComponent<Camera>();
    }
    
    public void OnEnable ()
    {
        targetMatrix = camera.projectionMatrix;

        baseAspectScale.x = targetMatrix.m00;
        baseAspectScale.y = targetMatrix.m11;
    }

    public void OnDisable ()
    {
        camera.ResetProjectionMatrix();
    }
    
    private void OnPreCull()
    {
        
        
        camera.ResetProjectionMatrix();
        targetMatrix = camera.projectionMatrix;

        baseAspectScale.x = targetMatrix.m00;
        baseAspectScale.y = targetMatrix.m11;
        
        if (camera.orthographic)
        {
            targetMatrix.m02 = lensShift.x / camera.orthographicSize / camera.aspect;
            targetMatrix.m12 = lensShift.y / camera.orthographicSize;
        }
        else
        {
            targetMatrix.m02 = lensShift.x * 2f;
            targetMatrix.m12 = lensShift.y * 2f;
        }
        

        targetMatrix.m03 = positionShift.x * 2f;
        targetMatrix.m13 = positionShift.y * 2f;
        
        camera.projectionMatrix = targetMatrix;
    }
}