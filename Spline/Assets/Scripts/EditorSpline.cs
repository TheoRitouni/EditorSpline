using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorSpline : MonoBehaviour
{
    [SerializeField]
    public enum TypeSpline
    {
        Default,
        Bezier,
        Hermite,
        BSpline,
        CatmullRom
    };

    [SerializeField]
    public TypeSpline typeSpline;

    [Serializable]
    public class BezierPointControl 
    {
        public Vector3 position = Vector3.zero;
        public Vector3 firstTangent = Vector3.zero;
        public Vector3 secondTangent = Vector3.zero;
    }

    [SerializeField]
    public List<BezierPointControl> pointControl = new List<BezierPointControl>();

}