using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSpline
{
    static public Vector3 GetPoint(Vector3 pt1, Vector3 pt2, Vector3 pt3, Vector3 pt4, float ratio)
    { 
        Vector3 posPoint = Vector3.zero;

        Matrix4x4 m = new Matrix4x4(
            new Vector4(-1 / 6f,  3 / 6f, -3 / 6f,  1 / 6f),
            new Vector4( 3 / 6f,      -1,  3 / 6f,       0),
            new Vector4(-3 / 6f,       0,  3 / 6f,       0),
            new Vector4( 1 / 6f,  4 / 6f,  1 / 6f,       0));

        Matrix4x4 pos = new Matrix4x4(
            new Vector4(pt1.x, pt1.y, pt1.z, 0),
            new Vector4(pt2.x, pt2.y, pt2.z, 0),
            new Vector4(pt3.x, pt3.y, pt3.z, 0),
            new Vector4(pt4.x, pt4.y, pt4.z, 0));

        Vector4 vecRatio = new Vector4(ratio * ratio * ratio, ratio * ratio, ratio, 1.0f);

        posPoint = (pos * m) * vecRatio;

        return posPoint;
    }
}
