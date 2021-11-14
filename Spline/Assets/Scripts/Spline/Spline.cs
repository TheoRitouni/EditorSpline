using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spline : MonoBehaviour
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
    [SerializeField]
    protected List<GameObject> controlPoint;
    [SerializeField]
    protected int nbrOfPoint = 30;


}
