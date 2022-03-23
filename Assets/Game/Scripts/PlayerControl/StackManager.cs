using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StackManager : Singleton<StackManager>
{
    [SerializeField] private Transform stackRoot;
    [SerializeField] private List<Transform> spreadPoints;

    private List<StackItem> items = new List<StackItem>();
    public int ItemCount => items.Count;
    [SerializeField] private float itemDeltaPosZ = 1.25f;

    // animation
    private bool animationPerforming = false;


    public void AddItem(StackItem item)
    {
        if(items.Count > 0)
        {
            item.SetTarget(items[ItemCount - 1].transform, itemDeltaPosZ);
        }
        else// first item to add
        {
            item.SetTarget(stackRoot, itemDeltaPosZ);
        }

        items.Add(item);
        
        if(!animationPerforming)
        {
            animationPerforming = true;
            StartCoroutine(PerformCollectAnim());
        }       
    }

    public void DestroyItem(StackItem collisionItem)
    {
        int collisionIndex = items.IndexOf(collisionItem);

        if (collisionIndex == ItemCount - 1)// last item collided with an obstacle
        {
            items.RemoveAt(ItemCount - 1);
            Destroy(collisionItem.gameObject);
        }
        else// horizontal collision
        {
            for (int i = collisionIndex; i < ItemCount; i++)
            {
                items[i].Throw(spreadPoints[i].position);
            }

            items.RemoveRange(collisionIndex, ItemCount - collisionIndex);
        }

    }

    public void DepositItem()
    {

    }

    private IEnumerator PerformCollectAnim()
    {
        for (int i = ItemCount - 1; i >= 0; i--)
        {
            items[i].transform.DOPunchScale(Vector3.one, 0.2f, 2, 1f);

            yield return new WaitForSeconds(0.05f);
        }

        animationPerforming = false;
    }
}
