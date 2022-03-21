using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackItem : MonoBehaviour
{
    public bool IsCollected = false;

    private void OnTriggerEnter(Collider other)
    {
        if(!IsCollected && ((other.transform.parent != null && other.transform.parent.CompareTag("Player")) || other.CompareTag("Collectable")))
        {
            IsCollected = true;
            StackManager.Instance.AddItem(this);
        }

        if(other.CompareTag("Obstacle"))
        {
            StackManager.Instance.DestroyItem(this);
        }
    }
}
