using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier 
{
    static public Vector3 GetPoint(Vector3 posFirstPt, Vector3 firstTan , Vector3 posSecondPt , Vector3 secondTan, float ratio)
    {
        Vector3 posPoint;

        posPoint = Mathf.Pow(1 - ratio, 3) * posFirstPt +
            3 * Mathf.Pow(1 - ratio, 2) * ratio * firstTan +
            3 * (1 - ratio) * Mathf.Pow(ratio, 2) * secondTan +
            Mathf.Pow(ratio, 3) * posSecondPt;

        return posPoint;
    }

}
