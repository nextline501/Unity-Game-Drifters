using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster Instance;
    public Transform lastCheckPoint;

    private void Awake()
    {
        Instance = this;
    }
}
