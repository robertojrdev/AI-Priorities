using UnityEngine;

public class FollowMouse : MonoBehaviour 
{ 
    private void Update()
    {
        if (Input.GetMouseButton(0))
             transform.position = GetMousePosition();
    }

    private Vector2 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
	

}
