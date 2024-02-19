using UnityEngine;

public class CellData
{
    public Vector2 Pos;
    public Vector2Int Index;
    public BuildingType cellState;

    public int gCost;
    public int hCost;
    public int fCost;
    public CellData previousCell;

    public CellData(Vector2Int index, Vector2 pos)
    {
        Index = index;
        Pos = pos;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
}
