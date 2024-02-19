using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private const int STRAIGHT_COST = 10;
    private const int DIAGONAL_COST = 14;

    private List<CellData> _openList;
    private List<CellData> _closedList;
    private GridController _gridController;

    public Pathfinding(GridController gridController)
    {
        _gridController = gridController;
    }

    public List<Vector2> FindPath(Vector2 startPos, Vector2 endPos)
    {
        var startIndex = _gridController.GridData.GetCellFromWorldPosition(startPos).Index;
        var endCell = _gridController.GridData.GetCellFromWorldPosition(endPos);

        if (endCell == null) return null;
        if (!_gridController.GridData.IsCellValid(endCell.Index.x, endCell.Index.y))
        {
            endCell = _gridController.GridData.GetRandomNeighbour(endCell);
            if(endCell == null) return null;
        }
        var path = FindPath(startIndex, endCell.Index);

        if (path == null) return null;
        var vectorPath = new List<Vector2>();
        path.ForEach(pathCell =>
        {
            vectorPath.Add(new Vector2(pathCell.Pos.x, pathCell.Pos.y));
        });

        return vectorPath;
    }

    public List<CellData> FindPath(Vector2Int startIndex, Vector2Int endIndex)
    {
        var startCell = _gridController.GridData.CellArray[startIndex.x, startIndex.y];
        var endCell = _gridController.GridData.CellArray[endIndex.x, endIndex.y];

        _openList = new List<CellData> { startCell };
        _closedList = new List<CellData>();

        foreach (var cell in _gridController.GridData.CellArray)
        {
            cell.gCost = int.MaxValue;
            cell.CalculateFCost();
            cell.previousCell = null;
        }

        startCell.gCost = 0;
        startCell.hCost = CalculateDistanceCost(startCell, endCell);
        startCell.CalculateFCost();

        while (_openList.Count > 0)
        {
            var currentCell = GetLowestFCostCell(_openList);
            if (currentCell == endCell)
            {
                return CalculatePath(endCell);
            }

            _openList.Remove(currentCell);
            _closedList.Add(currentCell);

            foreach(CellData neighbourCell in _gridController.GridData.GetNeighbours(currentCell))
            {
                if (_closedList.Contains(neighbourCell)) continue;
                if (neighbourCell.cellState != BuildingType.None)
                {
                    _closedList.Add(neighbourCell);
                    continue;
                }

                var tentativeGCost = currentCell.gCost + CalculateDistanceCost(currentCell, neighbourCell);
                if(tentativeGCost < neighbourCell.gCost)
                {
                    neighbourCell.previousCell = currentCell;
                    neighbourCell.gCost = tentativeGCost;
                    neighbourCell.hCost = CalculateDistanceCost(neighbourCell, endCell);
                    neighbourCell.CalculateFCost();

                    if (!_openList.Contains(neighbourCell))
                    {
                        _openList.Add(neighbourCell);
                    }
                }
            }
        }
        return null;
    }

    private int CalculateDistanceCost(CellData a, CellData b)
    {
        var xDistance = Mathf.Abs(a.Index.x - b.Index.x);
        var yDistance = Mathf.Abs(a.Index.y - b.Index.y);
        var remaining = Mathf.Abs(xDistance - yDistance);
        return DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + STRAIGHT_COST * remaining;
    }

    private CellData GetLowestFCostCell(List<CellData> pathCellList)
    {
        var lowestFCostCell = pathCellList[0];
        for (int i = 0; i < pathCellList.Count; i++)
        {
            if (pathCellList[i].fCost < lowestFCostCell.fCost)
            {
                lowestFCostCell = pathCellList[i];
            }
        }
        return lowestFCostCell;
    }

    private List<CellData> CalculatePath(CellData endCell)
    {
        var path = new List<CellData> { endCell };
        var currentCell = endCell;
        while(currentCell.previousCell != null)
        {
            path.Add(currentCell.previousCell);
            currentCell = currentCell.previousCell;
        }
        path.Reverse();
        return path;
    }
}