using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int _placementPosition;
    public int PlacementPosition { get => _placementPosition; set => _placementPosition = value; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        DontDestroyOnLoad(Instance);

        _placementPosition = -1;
    }
}
