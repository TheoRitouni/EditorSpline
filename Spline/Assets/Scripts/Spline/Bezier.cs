using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Bezier : MonoBehaviour
{
    private List<Transform> controlPoint;
    private int nbrOfPoint = 0;

    private Vector3 posPoint;
    Bezier(List<Transform> newControlPoint,int newNbrOfPoints)
    {
        nbrOfPoint = newNbrOfPoints;
        controlPoint = newControlPoint;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        for(int i = 0; i < nbrOfPoint; i++)
        {

        }
    }
}
