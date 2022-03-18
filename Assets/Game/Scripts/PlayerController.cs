using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform playerVisual;
    [Header("Specs")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float speedHorizontal = 5f;
    // input
    private Vector2 mouseRootPos;
    private float inputHorizontal;

    void Start()
    {
        
    }

    void Update()
    {
        GetInput();
        Move();
    }

    private void Move()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        var playerBasePos = playerVisual.localPosition;
        playerBasePos += Vector3.right * inputHorizontal * speedHorizontal;
        playerBasePos.x = Mathf.Clamp(playerBasePos.x, -3f, 3f);
        playerVisual.localPosition = playerBasePos;
    }

    private void GetInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            mouseRootPos = Input.mousePosition;
        }
        else if(Input.GetMouseButton(0))
        {
            var dragVec = (Vector2) Input.mousePosition - mouseRootPos;
            dragVec.Normalize();
            inputHorizontal = dragVec.x;

            mouseRootPos = Input.mousePosition;
        }
        else
        {
            inputHorizontal = 0;
        }
    }
}
