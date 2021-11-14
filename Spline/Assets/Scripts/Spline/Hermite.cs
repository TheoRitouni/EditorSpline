using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hermite 
{
    static public Vector3 GetPoint(Vector3 posFirstPt, Vector3 firstTan, Vector3 posSecondPt, Vector3 secondTan, float ratio)
    {
        Vector3 posPoint = Vector3.zero;

         posPoint = (2.0f * ratio * ratio * ratio - 3.0f * ratio * ratio + 1.0f) * posFirstPt
                + (ratio * ratio * ratio - 2.0f * ratio * ratio + ratio) * firstTan
                + (-2.0f * ratio * ratio * ratio + 3.0f * ratio * ratio) * posSecondPt
                + (ratio * ratio * ratio - ratio * ratio) * secondTan;

        return posPoint;
    }
}
