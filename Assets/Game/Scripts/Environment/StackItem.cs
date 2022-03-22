using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class StackItem : MonoBehaviour
{
    private bool isCollected = false, inStack = false;
    private Transform followTarget;
    private float deltaPosZ;


    private void Update()
    {
        if (followTarget == null || !inStack) return;

        Follow();
    }

    private void Follow()
    {
        var targetPos = followTarget.position + Vector3.forward * deltaPosZ;
        var targetPosX = Mathf.Lerp(transform.position.x, targetPos.x, 0.1f);
        targetPos.x = targetPosX;
        transform.position = targetPos;
    }

    public void SetTarget(Transform followTarget,float deltaPosZ)
    {
        this.followTarget = followTarget;
        this.deltaPosZ = deltaPosZ;
    }

    public void Throw(Vector3 position)
    {
        StartCoroutine(ThrowRoutine(position));
    }

    private IEnumerator ThrowRoutine(Vector3 position)
    {
        inStack = false;
        transform.DOMove(position, 0.5f).SetEase(Ease.OutBounce);

        yield return new WaitForSeconds(0.5001f);

        isCollected = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!isCollected && ((other.transform.parent != null && other.transform.parent.CompareTag("Player")) || other.CompareTag("Collectable")))
        {
            inStack = true;
            isCollected = true;
            StackManager.Instance.AddItem(this);
        }

        if(other.CompareTag("Obstacle"))
        {
            StackManager.Instance.DestroyItem(this);
        }
    }
}
