using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridDirection
{
    public static readonly Vector2Int North = new(0, 1);
    public static readonly Vector2Int NorthEast = new(1, 1);
    public static readonly Vector2Int East = new(1, 0);
    public static readonly Vector2Int SouthEast = new(1, -1);
    public static readonly Vector2Int South = new(0, -1);
    public static readonly Vector2Int SouthWest = new(-1, -1);
    public static readonly Vector2Int West = new(-1, 0);
    public static readonly Vector2Int NorthWest = new(-1, 1);

    public static readonly List<Vector2Int> AllDirections = new()
    {
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest,
    };
}
