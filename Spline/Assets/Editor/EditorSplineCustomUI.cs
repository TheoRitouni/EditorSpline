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

        if (GUILayout.Button("Add"))
        {
            //GUILayout.
            func.AddPartSpline();
        }
        if (GUILayout.Button("ClearAll"))
        {
            //GUILayout.
            func.ClearPartSpline();
        }
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
            Vector3 second = Handles.FreeMoveHandle(func.pointControl[i+1].firstTangent, Quaternion.identity, 0.1f, new Vector3(0.5f, 0.5f, 0.5f), Handles.SphereHandleCap); ;

            func.pointControl[i].position = startpos;
            func.pointControl[i].secondTangent = first;

            func.pointControl[i + 1].position = endPoint;
            func.pointControl[i + 1].firstTangent = second;


            Handles.DrawLine(startpos, first);
            Handles.DrawLine(endPoint, second);

            Handles.DrawBezier(startpos, endPoint, first, second, Color.red, null, 2f);
        }
    }
}
