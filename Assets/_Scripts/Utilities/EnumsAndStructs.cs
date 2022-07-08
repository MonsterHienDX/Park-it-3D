
[System.Serializable]
public struct MoveActionInfo
{
    public MoveDirection moveDirection;
    public float distance;
}

public enum CarDirection
{
    NegativeZ = 0,
    PositiveZ = 180,
    NegativeX = 90,
    PositiveX = 270,
}

public enum MoveDirection
{
    MoveAhead,
    MoveBack,
    TurnLeft,
    TurnRight
}