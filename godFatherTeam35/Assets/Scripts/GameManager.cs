using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int _placementPositionIndex;
    private Vector3 _placementPosition;
    private bool _IsPlayerHit;

    public int PlacementPositionIndex { get => _placementPositionIndex; set => _placementPositionIndex = value; }
    public Vector3 PlacementPosition { get => _placementPosition; set => _placementPosition = value; }
    public bool IsPlayerHit { get => _IsPlayerHit; set => _IsPlayerHit = value; }


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        DontDestroyOnLoad(Instance);

        //Reset placement position index & vector 3
        _placementPositionIndex = -1;
        _placementPosition = Vector3.zero;
    }
}
