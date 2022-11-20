using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Vector3 lerpCounts;
    public Transform target;

    private void Start()
    {
       

        transform.position = target.position;
        StartCoroutine("TargetRoutine");
    }


    IEnumerator TargetRoutine()
    {
        Vector3 targetPos = transform.position;
        
        while (true)
        {
            targetPos.x = Mathf.Lerp(targetPos.x, target.position.x, lerpCounts.x);
            targetPos.y = Mathf.Lerp(targetPos.y, target.position.y, lerpCounts.y);
            targetPos.z = Mathf.Lerp(targetPos.z, target.position.z, lerpCounts.z);
            transform.position = targetPos;
            yield return null;
        }
    }
}