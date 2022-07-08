using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    public CarManager carManager;
    public EntryManager entryManager;

}

[System.Serializable]
public struct CarPosInfo
{
    public Transform carStartPos;
    public Car carPrefab;
}