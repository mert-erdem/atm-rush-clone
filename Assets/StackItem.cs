using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackItem : MonoBehaviour
{
    private bool isCollected = false;

    private void OnTriggerEnter(Collider other)
    {
        if(!isCollected && (other.transform.parent.CompareTag("Player") || other.CompareTag("Collectable")))
        {
            isCollected = true;
            StackManager.Instance.AddItem(this);
        }

        if(other.CompareTag("Obstacle"))
        {
            StackManager.Instance.DestroyItem(this);
        }
    }
}
