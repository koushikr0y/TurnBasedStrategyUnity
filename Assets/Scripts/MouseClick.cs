//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class MouseClick : MonoBehaviour
{
    private static MouseClick instance;
    [SerializeField] private LayerMask layerMask;

    private void Awake()
    {
        instance = this;
    }


    public static Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, instance.layerMask);
        return raycastHit.point;
    }
}
