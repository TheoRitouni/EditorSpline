using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spline : MonoBehaviour
{
    public enum TypeSpline
    {
        Default,
        Bezier,
        Hermite,
        BSpline,
        CatmullRom
    };

    [SerializeField]
    private TypeSpline typeSpline;
    [SerializeField]
    protected List<GameObject> controlPoint;
    [SerializeField]
    protected int nbrOfPoint = 30;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
