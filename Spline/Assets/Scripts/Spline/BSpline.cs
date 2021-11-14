using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSpline
{
    static public Vector3 GetPoint(Vector3 pt1, Vector3 pt2, Vector3 pt3, Vector3 pt4, float ratio)
    {
        Vector3 posPoint = Vector3.zero;

        posPoint = ((Mathf.Pow(1 - ratio, 3)) * pt1 +
                   (3 * Mathf.Pow(ratio, 3) - 6 * Mathf.Pow(ratio, 2) + 4) * pt2 +
                   (- 3 * Mathf.Pow(ratio, 3) + 3 * Mathf.Pow(ratio, 3) + 3 * ratio + 1) * pt3 +
                   Mathf.Pow(ratio, 3) * pt4) / 6.0f ;

        return posPoint;
    }
}
