using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackManager : Singleton<StackManager>
{
    [SerializeField] private Transform stackRoot;
    [SerializeField] private List<Transform> spreadPoints;

    private List<StackItem> items = new List<StackItem>();
    public int ItemCount => items.Count;

    public void AddItem(StackItem item)
    {
        items.Add(item);
        item.transform.position = stackRoot.position + Vector3.forward * items.Count * 1.2f;
        item.transform.SetParent(stackRoot);
        // DoTween
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
            int j = 0;

            for (int i = collisionIndex; i < ItemCount; i++)
            {
                // DoTween
                items[i].transform.position = spreadPoints[j].position;
                items[i].transform.SetParent(null);
                items[i].IsCollected = false;
                j++;
            }

            items.RemoveRange(collisionIndex, ItemCount - collisionIndex);
        }
    }

    public void ImproveItem()
    {
        // improve the stack's last item
    }

    public void DepositItem()
    {

    }
}
