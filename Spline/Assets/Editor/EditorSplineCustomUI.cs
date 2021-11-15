using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EditorSpline))]
public class EditorSplineCustomUI : Editor
{
    bool showposition = false;

    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();
        EditorSpline func = (EditorSpline)target;

        EditorGUILayout.LabelField("============ Select Spline ============");
        EditorGUILayout.Space();

        func.typeSpline = (EditorSpline.TypeSpline)EditorGUILayout.EnumPopup(func.typeSpline);
        EditorUtility.SetDirty(func);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("============ Point Control ============");
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Point Control number");
        EditorGUILayout.IntField(func.pointControl.Count);

        showposition = EditorGUILayout.BeginFoldoutHeaderGroup(showposition,"List of Point Control");
        
        if(showposition)
        {
            EditorGUI.indentLevel = 1;
            for (int i = 0; i < func.pointControl.Count; i++)
            {
                func.pointControl[i].show = EditorGUILayout.Foldout(func.pointControl[i].show, "Point " + (i + 1) );

                if (func.pointControl[i].show)
                {
                    EditorGUILayout.Vector3Field("position", func.pointControl[i].position);

                    if (i != 0)
                        EditorGUILayout.Vector3Field("First Tangent", func.pointControl[i].firstTangent);
                    if (i != func.pointControl.Count - 1)
                        EditorGUILayout.Vector3Field("Second Tangent", func.pointControl[i].secondTangent);
                }
                EditorGUILayout.Space();


            }
            EditorGUI.indentLevel = 0;
        }

        EditorGUILayout.EndFoldoutHeaderGroup();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("============ Manager Data ============");
        EditorGUILayout.Space();

        if (GUILayout.Button("Save"))
        {
            func.SavePointControl();
        }
        if (GUILayout.Button("Load"))
        {
            func.LoadPointControl();
            EditorUtility.SetDirty(func);
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
            Vector3 startpos, first, endPoint, second;

            startpos = Handles.PositionHandle(func.pointControl[i].position, Quaternion.identity);
            first = Handles.FreeMoveHandle(func.pointControl[i].secondTangent, Quaternion.identity, 0.1f, new Vector3(0.5f, 0.5f, 0.5f), Handles.SphereHandleCap);

            endPoint = Handles.PositionHandle(func.pointControl[i + 1].position, Quaternion.identity);
            second = Handles.FreeMoveHandle(func.pointControl[i + 1].firstTangent, Quaternion.identity, 0.1f, new Vector3(0.5f, 0.5f, 0.5f), Handles.SphereHandleCap);

            func.pointControl[i].position = startpos;
            func.pointControl[i].secondTangent = first;

            func.pointControl[i + 1].position = endPoint;
            func.pointControl[i + 1].firstTangent = second;

            Handles.color = Color.blue;

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
                        posPoint = BSpline.GetPoint( first, startpos, endPoint, second, t / 100f);
                        break;
                    }
                    case EditorSpline.TypeSpline.CatmullRom:
                    {
                        posPoint = CatmullRom.GetPoint( first, startpos, endPoint, second, t / 100f);
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
