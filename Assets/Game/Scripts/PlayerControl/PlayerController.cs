using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private new Rigidbody rigidbody;
    [SerializeField] private Transform playerVisual;
    [SerializeField] private Transform miniGamePos;
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
        GameManager.ActionGameStart += StartToMove;
        GameManager.ActionMiniGame += PerformMiniGame;

        stateRun = new State(Move, () => { }, () => { });
        stateIdle = new State(() => { }, () => { }, () => { });
        SetState(stateIdle);
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

    private void StartToMove()
    {
        SetState(stateRun);
    }

    private void Stop()
    {
        SetState(stateIdle);
    }

    private void PerformMiniGame()
    {
        StartCoroutine(PerformMiniGameRoutine());
    }
    private IEnumerator PerformMiniGameRoutine()
    {
        yield return new WaitForSeconds(1.5f);

        // set root transform's position
        var newPlayerPos = transform.position;
        newPlayerPos.z = miniGamePos.position.z;
        transform.position = newPlayerPos;
        // set visual's position
        playerVisual.position = transform.position;

        var currentPoolObjectPos = miniGamePos.position;

        for (int i = 0; i < StackManager.Instance.CurrentStackValue; i++)
        {
            var poolObject = MiniGameItemPool.Instance.GetObject();
            poolObject.gameObject.SetActive(true);
            // set player's position
            newPlayerPos.y += MiniGameItemPool.Instance.GetObjectHeight();
            transform.position = newPlayerPos;
            // set pool object's position
            var newPoolObjectPos = currentPoolObjectPos;
            newPoolObjectPos.y += MiniGameItemPool.Instance.GetObjectHeight();
            poolObject.position = newPoolObjectPos;
            currentPoolObjectPos = newPoolObjectPos;

            yield return new WaitForSeconds(0.025f);
        }

        // end level action
        GameManager.ActionGameEnd?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Obstacle"))
        {
            StartCoroutine(PushBack());
        }

        if(other.CompareTag("FinishLine"))
        {
            Stop();
            GameManager.ActionMiniGame?.Invoke();
        }

        if(other.CompareTag("Multiplier"))
        {
            var newMultiplierPos = other.transform.position;
            newMultiplierPos.z -= 3;
            other.transform.position = newMultiplierPos;

            MoneyManager.Instance.CurrentMultiplier = int.Parse(other.name);
        }
    }

    private void OnDestroy()
    {
        GameManager.ActionGameStart -= StartToMove;
        GameManager.ActionMiniGame -= PerformMiniGame;
    }
}
