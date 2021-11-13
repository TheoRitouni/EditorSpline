using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorSpline : MonoBehaviour
{

    [Serializable]
    public class BezierPointControl 
    {
        public Vector3 position = Vector3.zero;
        public Vector3 firstTangent = Vector3.zero;
        public Vector3 secondTangent = Vector3.zero;
    }

    [SerializeField]
    public List<BezierPointControl> pointControl = new List<BezierPointControl>();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPartSpline()
    {

    }

    public void ClearPartSpline()
    {

    }
}
