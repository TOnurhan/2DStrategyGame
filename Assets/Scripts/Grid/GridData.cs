using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    private Vector2Int _gridSize;
    private float _gridRadius;
    private Vector2 _origin;

    public CellData[,] CellArray;

    public GridData(Vector2Int gridSize, float gridRadius, Vector2 origin)
    {
        _gridSize = gridSize;
        _gridRadius = gridRadius;
        _origin = origin;

        GenerateGrid();
    }

    private void GenerateGrid()
    {
        CellArray = new CellData[_gridSize.x, _gridSize.y];

        for (int y = 0; y < _gridSize.y; y++)
        {
            for (int x = 0; x < _gridSize.x; x++)
            {
                var index = new Vector2Int(x, y);
                var pos = GetWorldPositionFromIndex(index);
                CellData cellData = new(index, pos);
                CellArray[x, y] = cellData;
            }
        }
    }

    private Vector2 GetWorldPositionFromIndex(Vector2Int index) => _gridRadius * (Vector2)index + _origin + Vector2.one * _gridRadius / 2;

    public CellData GetCellFromWorldPosition(Vector2 pos)
    {
        var newPos = pos - _origin;
        var x = Mathf.FloorToInt(newPos.x);
        var y = Mathf.FloorToInt(newPos.y);

        if (!IsIndexInsideGrid(x, y)) return null;

        return CellArray[x, y];
    }

    public bool IsIndexInsideGrid(int x, int y)
    {
        var isXIndexExist = x >= 0 && x < _gridSize.x;
        var isYIndexExist = y >= 0 && y < _gridSize.y;
        return isXIndexExist && isYIndexExist;
    }
    public bool IsCellValid(int x, int y)
    {
        var isXIndexExist = x >= 0 && x < _gridSize.x;
        var isYIndexExist = y >= 0 && y < _gridSize.y;
        
        if(!isXIndexExist || !isYIndexExist) return false;

        var isIndexEmpty = CellArray[x,y].cellState == BuildingType.None;

        return isIndexEmpty;
    }

    public List<CellData> GetNeighbours(CellData cell)
    {
        var neighbourCellList = new List<CellData>();

        var cellIndex = cell.Index;
        GridDirection.AllDirections.ForEach(direction =>
        {
            var newX = cellIndex.x + direction.x;
            var newY = cellIndex.y + direction.y;
            if (IsCellValid(newX, newY)) neighbourCellList.Add(CellArray[newX, newY]);
        });
        return neighbourCellList;
    }

    public CellData GetRandomNeighbour(CellData cell)
    {
        var neighbours = GetNeighbours(cell);
        while (neighbours.Count <= 0)
        {
            //Komþunun komþusunu gez.
        }
        var randomIndex = Random.Range(0, neighbours.Count);
        return neighbours.Count > 0 ? neighbours[randomIndex] : null;
    }
}
