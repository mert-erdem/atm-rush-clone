using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackManager : Singleton<StackManager>
{
    [SerializeField] private Transform stackRoot;
    private List<StackItem> items = new List<StackItem>();
    public int ItemCount => items.Count;

    public void AddItem(StackItem item)
    {
        items.Add(item);
        item.transform.position = stackRoot.position + Vector3.forward * items.Count * 1.2f;
        item.transform.SetParent(stackRoot);
    }

    public void DestroyItem(StackItem collisionItem)
    {
        int collisionIndex = items.IndexOf(collisionItem);
        
        if (collisionIndex == ItemCount - 1)// last item collided with an obstacle
        {          
            items.RemoveAt(ItemCount - 1);
            Destroy(collisionItem.gameObject);
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
