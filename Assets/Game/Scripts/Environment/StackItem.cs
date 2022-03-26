using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class StackItem : MonoBehaviour
{
    [SerializeField] private new BoxCollider collider;
    [SerializeField] private List<MeshRenderer> visuals;
    [System.NonSerialized] public bool isCollected = false, inStack = false, enteredAtmLine = false;
    private Transform followTarget;
    private float deltaPosZ;

    private int level = 0;
    public int Level => level + 1;


    private void Update()
    {
        if (followTarget == null || !inStack) return;

        if(!enteredAtmLine)
        {
            Follow();
        }
        else
        {
            GoToLastAtm();
        }
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

        inStack = true;
        isCollected = true;
    }

    public void Throw(Vector3 position)
    {
        StartCoroutine(ThrowRoutine(position));
    }
    private IEnumerator ThrowRoutine(Vector3 position)
    {
        inStack = false;
        transform.DOMove(position, 0.25f).SetEase(Ease.OutBounce);
        collider.enabled = false;

        yield return new WaitForSeconds(0.2501f);

        isCollected = false;
        collider.enabled = true;
    }

    private void Improve()
    {
        if(level != 2)
        {
            visuals[level].enabled = false;
            level++;
            visuals[level].enabled = true;
        }
        
    }

    private void GoToLastAtm()
    {        
        transform.Translate(-transform.right * 5f * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!isCollected && !inStack && !enteredAtmLine &&
            ((other.transform.parent != null && other.transform.parent.CompareTag("Player")) || other.CompareTag("Collectable")))
        {           
            StackManager.Instance.AddItem(this);
        }

        if(inStack && isCollected)
        {
            if (other.CompareTag("Obstacle"))
            {
                StackManager.Instance.DestroyItem(this);
            }

            else if (other.CompareTag("Gate"))
            {
                Improve();
                StackManager.Instance.UpdateStackValue();
            }

            else if (other.CompareTag("Atm"))
            {
                if(enteredAtmLine)
                {
                    Destroy(gameObject);
                    return;
                }

                StackManager.Instance.DepositItem(this);
            }

            else if (other.CompareTag("AtmLine"))
            {
                enteredAtmLine = true;             
            }
        }

        
    }
}
