using UnityEngine;

public class LookBehavior : MonoBehaviour 
{
    private Vector2 lookDirection;
    public ForwardDirection forwardDirection;

    public enum ForwardDirection
    {
        Right, Left, Up, Down
    }

    private void Update()
    {
        Look();
    }

    private void Look()
    {
        Vector3 forward = GetFOrwardDirection();
        float angle = Vector3.SignedAngle(forward, lookDirection, Vector3.forward);
        transform.eulerAngles = new Vector3(0,0,angle);
    }

    private Vector2 GetFOrwardDirection()
    {
        Vector2 dir = Vector2.zero;

        switch (forwardDirection)
        {
            case ForwardDirection.Right:
                dir = Vector2.right;
                break;
            case ForwardDirection.Left:
                dir = Vector2.left;
                break;
            case ForwardDirection.Up:
                dir = Vector2.up;
                break;
            case ForwardDirection.Down:
                dir = Vector2.down;
                break;
        }

        return dir;
    }

    public void SetLookDirection(Vector2 direction)
    {
        lookDirection = direction;
    }
}
