using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    public static UnityAction ActionGameStart, ActionGameEnd, ActionMiniGame;
}
