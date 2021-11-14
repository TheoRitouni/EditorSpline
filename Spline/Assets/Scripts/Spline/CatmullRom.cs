using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatmullRom 
{
    static public Vector3 GetPoint(Vector3 pt1, Vector3 pt2, Vector3 pt3 , Vector3 pt4, float ratio)
    {
        Vector3 posPoint = Vector3.zero;

        Matrix4x4 m = new Matrix4x4();
        Matrix4x4 pos = new Matrix4x4();

        Vector4 vecRatio = new Vector4(Mathf.Pow(ratio, 3), ratio * ratio, ratio, 1.0f);


        pos.SetColumn(0, new Vector4(pt1.x, pt2.x, pt3.x, pt4.x));
        pos.SetColumn(1, new Vector4(pt1.y, pt2.y, pt3.y, pt4.y));
        pos.SetColumn(2, new Vector4(pt1.z ,pt2.z, pt3.z, pt4.z));
        pos.SetColumn(3, new Vector4(1.0f, 1.0f, 1.0f, 1.0f));


        m.SetColumn(0, new Vector4(-1.0f,  2.0f, -1.0f, 0.0f));
        m.SetColumn(1, new Vector4( 3.0f, -5.0f,  0.0f, 2.0f));
        m.SetColumn(2, new Vector4(-3.0f,  4.0f,  1.0f, 0.0f));
        m.SetColumn(3, new Vector4( 1.0f, -1.0f,  0.0f, 0.0f));



        posPoint = (pos * m) * vecRatio;

        return posPoint;
    }
}
