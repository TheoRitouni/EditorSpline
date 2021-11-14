using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EditorSpline))]
public class EditorSplineCustomUI : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorSpline func = (EditorSpline)target;

        //if (GUILayout.Button("Add"))
        //{
        //    //GUILayout.
        //}
        //if (GUILayout.Button("ClearAll"))
        //{
        //    //GUILayout.
        //}
    }

    private void OnEnable()
    {
        EditorSpline func = (EditorSpline)target;
        Tools.hidden = true;
    }

    private void OnSceneGUI()
    {
        EditorSpline func = (EditorSpline)target;

        for (int i  = 0; i < func.pointControl.Count - 1; i++ )
        {
            
            Vector3 startpos = Handles.PositionHandle(func.pointControl[i].position, Quaternion.identity);
            Vector3 first = Handles.FreeMoveHandle(func.pointControl[i].secondTangent, Quaternion.identity,0.1f,new Vector3(0.5f,0.5f,0.5f),Handles.SphereHandleCap);


            Vector3 endPoint = Handles.PositionHandle(func.pointControl[i+1].position, Quaternion.identity);
            Vector3 second = Handles.FreeMoveHandle(func.pointControl[i+1].firstTangent, Quaternion.identity, 0.1f, new Vector3(0.5f, 0.5f, 0.5f), Handles.SphereHandleCap);

            func.pointControl[i].position = startpos;
            func.pointControl[i].secondTangent = first;

            func.pointControl[i + 1].position = endPoint;
            func.pointControl[i + 1].firstTangent = second;

            Handles.color = Color.blue;

            //Draw Line mal utilisé à regarder 
            //if (i != 0 && i != func.pointControl.Count)
            //{
            //    func.pointControl[i].firstTangent = -func.pointControl[i].secondTangent;
            //    Handles.DrawLine(startpos, func.pointControl[i].firstTangent);

            //}

            Handles.DrawLine(startpos, first);
            Handles.DrawLine(endPoint, second);

            Vector3 posPoint = Vector3.zero;
            Vector3 previousPoint = Vector3.zero;

            Handles.color = Color.red;

            for (int t = 0; t < 100; t++)
            {
                switch (func.typeSpline)
                {
                    case EditorSpline.TypeSpline.Bezier :
                    {
                        posPoint = Bezier.GetPoint(startpos, first, endPoint, second, t / 100f);
                        break;
                    }
                    case EditorSpline.TypeSpline.Hermite:
                    {
                        posPoint = Hermite.GetPoint(startpos, first, endPoint, second, t / 100f);
                        break;
                    }
                    case EditorSpline.TypeSpline.BSpline:
                    {
                        posPoint = BSpline.GetPoint(startpos, first, endPoint, second, t / 100f);
                        break;
                    }
                    case EditorSpline.TypeSpline.CatmullRom:
                    {
                        posPoint = CatmullRom.GetPoint(startpos, first, endPoint, second, t / 100f);
                        break;
                    }
                    default: break;    
                }

                if (t != 0)
                    Handles.DrawLine(previousPoint,posPoint);

                previousPoint = posPoint;
            }

        }
    }
}
