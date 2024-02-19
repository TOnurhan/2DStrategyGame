using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private float _cellRadius;

    public GridData GridData { get; private set; }

    public float GetCellRadius() => _cellRadius;

    public void Initialize()
    {
        GridData = new GridData(_gridSize, _cellRadius, transform.position);
        foreach (var item in GridData.CellArray)
        {
            var startPosX = new Vector2(item.Pos.x - _cellRadius / 2, item.Pos.y - _cellRadius / 2);
            var endPosX = new Vector2(item.Pos.x + _cellRadius / 2, item.Pos.y - _cellRadius / 2);

            var startPosY = new Vector2(item.Pos.x - _cellRadius / 2, item.Pos.y - _cellRadius / 2);
            var endPosY = new Vector2(item.Pos.x - _cellRadius / 2, item.Pos.y + _cellRadius / 2);
            Debug.DrawLine(startPosY, endPosY,Color.black,1000);
            Debug.DrawLine(startPosX, endPosX,Color.black,1000);
        }
    }

    public bool IsMouseInGrid(Vector2 mousePos)
    {
        var IsXValid = mousePos.x >= transform.position.x && mousePos.x < _gridSize.x * _cellRadius + transform.position.x;
        var IsYValid = mousePos.y >= transform.position.y && mousePos.y < _gridSize.y * _cellRadius + transform.position.y;

        var IsPosInGrid = IsXValid && IsYValid;

        return IsPosInGrid;
    }
}
