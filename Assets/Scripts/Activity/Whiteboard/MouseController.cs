using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public static Vector3 GetMousePosition()
    {
        Vector3 vector = GetMousePositionWithZ(Input.mousePosition, Camera.main);
        vector.z = 0f;
        return vector;
    }

    public static Vector3 GetMousePositionWithZ()
    {
        return GetMousePositionWithZ(Input.mousePosition, Camera.main);
    }

    public static Vector3 GetMousePositionWithZ(Camera worldCamera)
    {
        return GetMousePositionWithZ(Input.mousePosition, worldCamera);
    }

    public static Vector3 GetMousePositionWithZ(Vector3 screenPosition, Camera currentCamera)
    {
        Vector3 worldPos = currentCamera.ScreenToWorldPoint(screenPosition);
        return worldPos;
    }

    public static Vector3 GetDirToMouse(Vector3 fromPosition)
    {
        Vector3 mouseWorldPosition = GetMousePosition();
        return (mouseWorldPosition - fromPosition).normalized;
    }
}
