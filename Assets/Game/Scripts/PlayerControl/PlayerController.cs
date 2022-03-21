using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private new Rigidbody rigidbody;
    [SerializeField] private Transform playerVisual;
    [Header("Specs")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float speedHorizontal = 5f;
    // input
    private Vector2 mouseRootPos;
    private float inputHorizontal;
    // states
    private State stateCurrent, stateRun, stateIdle;

    void Start()
    {
        stateRun = new State(Move, () => { }, () => { });
        stateIdle = new State(() => { }, () => { }, () => { });
        SetState(stateRun);
    }

    void Update()
    {
        GetInput();
        stateCurrent.onUpdate();
    }

    private void SetState(State newState)
    {
        if (stateCurrent != null)
            stateCurrent.onStateExit();

        stateCurrent = newState;
        stateCurrent.onStateEnter();
    }

    private void Move()
    {
        // forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        // horizontal
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

    private IEnumerator PushBack()
    {
        SetState(stateIdle);
        rigidbody.isKinematic = false;
        rigidbody.AddForce(-transform.forward * 10, ForceMode.Impulse);

        yield return new WaitForSeconds(0.5f);

        rigidbody.isKinematic = true;
        SetState(stateRun);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Obstacle"))
        {
            StartCoroutine(PushBack());
        }
    }
}
