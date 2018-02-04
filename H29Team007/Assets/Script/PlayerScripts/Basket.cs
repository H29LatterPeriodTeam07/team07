using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour
{
    public Vector3 GetCartPosition()
    {
        return transform.localPosition;
    }

    public void SetParent(Transform parent)
    {
        transform.parent = parent;
    }
    
    public void SetBasketGlobalPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void SetBasketGlobalRotation(Quaternion angle)
    {
        transform.rotation = angle;
    }

    public void SetBasketLocalPosition(Vector3 pos)
    {
        transform.localPosition = pos;
    }

    public void SetBasketLocalRotation(float angle)
    {
        transform.localRotation = Quaternion.AngleAxis(angle, Vector3.up);
    }
}
