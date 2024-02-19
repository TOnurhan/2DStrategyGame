using UnityEngine;
using UnityEngine.EventSystems;

public abstract class PlayerController : MonoBehaviour
{
    protected Vector2 MousePos;
    protected bool IsActive;
    [SerializeField] protected GridController GridController;

    public virtual void Dispose()
    {
        Deactivate();
    }

    public Vector2 GetMousePos()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }

    public virtual void Activate()
    {
        IsActive = true;
    }

    public virtual void Deactivate()
    {
        IsActive = false;
    }
}
