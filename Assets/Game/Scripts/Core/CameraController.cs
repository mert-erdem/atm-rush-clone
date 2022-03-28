using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cmMenu, cmInGame, cmMiniGame;

    private void OnEnable()
    {
        GameManager.ActionGameStart += SetInGameCamera;
        GameManager.ActionMiniGame += SetMiniGameCamera;
    }

    private void SetInGameCamera()
    {
        cmMenu.enabled = false;
        cmInGame.enabled = true;
    }

    private void SetMiniGameCamera()
    {
        cmInGame.enabled = false;
        cmMiniGame.enabled = true;
    }

    private void OnDisable()
    {
        GameManager.ActionGameStart -= SetInGameCamera;
        GameManager.ActionMiniGame -= SetMiniGameCamera;
    }
}
