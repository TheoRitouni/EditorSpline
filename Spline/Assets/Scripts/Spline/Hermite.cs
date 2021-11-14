using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hermite 
{
    static public Vector3 GetPoint(Vector3 posFirstPt, Vector3 firstTan, Vector3 posSecondPt, Vector3 secondTan, float ratio)
    {
        Vector3 posPoint = Vector3.zero;

        //tan1 -= point1;
        //tan2 -= point2;

         posPoint = (2.0f * ratio * ratio * ratio - 3.0f * ratio * ratio + 1.0f) * posFirstPt
                + (ratio * ratio * ratio - 2.0f * ratio * ratio + ratio) * firstTan
                + (-2.0f * ratio * ratio * ratio + 3.0f * ratio * ratio) * posSecondPt
                + (ratio * ratio * ratio - ratio * ratio) * secondTan;

        //posPoint = (2 * ratio * 3 - 3 * Mathf.Pow(ratio,2)+ 1) * point1 + 
        //           (-2 * ratio * 3 + 3* Mathf.Pow(ratio, 2))* point2 + 
        //           (ratio * 3 - 2 * Mathf.Pow(ratio, 2) + ratio) * tan1 + 
        //           (ratio *3 - Mathf.Pow(ratio, 2)) * tan2;

        return posPoint;
    }
}
