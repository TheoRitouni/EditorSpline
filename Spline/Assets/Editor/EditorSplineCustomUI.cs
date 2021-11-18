using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EditorSpline))]
public class EditorSplineCustomUI : Editor
{
    private bool showposition = false;
    private EditorSpline func;
    private Vector3 startpos = Vector3.zero, first = Vector3.zero,
                endPoint = Vector3.zero, second = Vector3.zero;
    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();
        func = (EditorSpline)target;

        EditorGUILayout.LabelField("============ Select Spline");
        EditorGUILayout.Space();

        func.typeSpline = (EditorSpline.TypeSpline)EditorGUILayout.EnumPopup(func.typeSpline);
        EditorUtility.SetDirty(func);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("============ Point Control ");
        EditorGUILayout.Space();

        UIPointControl();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("============ Object Movement ");
        EditorGUILayout.Space();

        // WIP 

        func.isActivate = EditorGUILayout.Toggle(func.isActivate);
        func.objectFollowSpline = (GameObject)EditorGUILayout.ObjectField("Object Movement", func.objectFollowSpline, typeof(GameObject), true);
        func.speed = EditorGUILayout.FloatField(func.speed);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("============ Manager Data ");
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            func.SavePointControl();
        }
        if (GUILayout.Button("Load"))
        {
            func.LoadPointControl();
            EditorUtility.SetDirty(func);
        }
        EditorGUILayout.EndHorizontal();
    }

    private void UIPointControl()
    {
        if (func.pointControl.Count <= 3 && func.typeSpline == EditorSpline.TypeSpline.BSpline)
            EditorGUILayout.LabelField("! BSpline need 4 Control Point !");
        if (func.pointControl.Count <= 3 && func.typeSpline == EditorSpline.TypeSpline.CatmullRom)
            EditorGUILayout.LabelField("! CatmullRom need 4 Control Point !");

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Point Control number : ");
        EditorGUILayout.IntField(func.pointControl.Count);
        EditorGUILayout.EndHorizontal();

        showposition = EditorGUILayout.BeginFoldoutHeaderGroup(showposition, "List of Point Control");

        if (showposition)
        {
            EditorGUI.indentLevel = 1;
            for (int i = 0; i < func.pointControl.Count; i++)
            {
                func.pointControl[i].show = EditorGUILayout.Foldout(func.pointControl[i].show, "Point " + (i + 1));

                if (func.pointControl[i].show)
                {
                    if (func.typeSpline == EditorSpline.TypeSpline.BSpline || func.typeSpline == EditorSpline.TypeSpline.CatmullRom)
                    {
                        EditorGUILayout.Vector3Field("Position", func.pointControl[i].position);
                    }
                    else
                    {
                        EditorGUILayout.Vector3Field("position", func.pointControl[i].position);

                        if (i != 0)
                            EditorGUILayout.Vector3Field("First", func.pointControl[i].first);
                        if (i != func.pointControl.Count - 1)
                            EditorGUILayout.Vector3Field("Second", func.pointControl[i].second);
                    }
                    if (GUILayout.Button("Delete"))
                    {
                        func.pointControl.RemoveAt(i);
                    }
                }
                EditorGUILayout.Space();


            }
            EditorGUI.indentLevel = 0;
        }
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.Space();

        if (GUILayout.Button("Add Control Point"))
        {

            EditorSpline.PointControl point = new EditorSpline.PointControl();

            if (func.pointControl.Count <= 0)
                func.pointControl.Add(point);
            else
            {
                point.position = func.pointControl[func.pointControl.Count - 1].position;
                point.first = func.pointControl[func.pointControl.Count - 1].first;
                point.second = func.pointControl[func.pointControl.Count - 1].second;
                func.pointControl.Add(point);

            }

        }
        EditorGUILayout.Space();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndFoldoutHeaderGroup();
    }
    private void OnEnable()
    {
        func = (EditorSpline)target;
        Tools.hidden = true;
    }

    private void OnSceneGUI()
    {
        func = (EditorSpline)target;

        for (int i = 0; i < func.pointControl.Count; i++)
        {
            Handles.color = Color.white;

            CalculateBSplineAndCatmullRom(i);
            CalculateBezierAndHermite(i); 
            SwitchSpline(i);
        }

    }

    private void CalculateBSplineAndCatmullRom(int i)
    {
        // Manage Point of BSpline and CatmullRom here
        if ((func.typeSpline == EditorSpline.TypeSpline.BSpline || func.typeSpline == EditorSpline.TypeSpline.CatmullRom)
            && i < func.pointControl.Count - 3)
        {
            Vector3 pos = Handles.PositionHandle(func.pointControl[i].position, Quaternion.identity);

            if (i == func.pointControl.Count - 4)
            {
                Vector3 pos1 = Handles.PositionHandle(func.pointControl[i + 1].position, Quaternion.identity);
                Vector3 pos2 = Handles.PositionHandle(func.pointControl[i + 2].position, Quaternion.identity);
                Vector3 pos3 = Handles.PositionHandle(func.pointControl[i + 3].position, Quaternion.identity);
                func.pointControl[i + 1].position = pos1;
                func.pointControl[i + 2].position = pos2;
                func.pointControl[i + 3].position = pos3;

            }

            startpos = func.pointControl[i].position;
            first = func.pointControl[i + 1].position;
            endPoint = func.pointControl[i + 2].position;
            second = func.pointControl[i + 3].position;

            func.pointControl[i].position = pos;

            Handles.color = Color.blue;
        }
    }
    private void CalculateBezierAndHermite(int i)
    {
        // Manage Control Point of Bezier and Hermite Spline 
        if ((func.typeSpline == EditorSpline.TypeSpline.Bezier || func.typeSpline == EditorSpline.TypeSpline.Hermite)
            && i < func.pointControl.Count - 1)
        {
            startpos = Handles.PositionHandle(func.pointControl[i].position, Quaternion.identity);
            first = Handles.FreeMoveHandle(func.pointControl[i].second, Quaternion.identity, 0.1f, new Vector3(0.5f, 0.5f, 0.5f), Handles.SphereHandleCap);

            endPoint = Handles.PositionHandle(func.pointControl[i + 1].position, Quaternion.identity);
            second = Handles.FreeMoveHandle(func.pointControl[i + 1].first, Quaternion.identity, 0.1f, new Vector3(0.5f, 0.5f, 0.5f), Handles.SphereHandleCap);

            func.pointControl[i].position = startpos;
            func.pointControl[i].second = first;

            func.pointControl[i + 1].position = endPoint;
            func.pointControl[i + 1].first = second;

            Handles.color = Color.blue;

            Handles.DrawLine(startpos, first);
            Handles.DrawLine(endPoint, second);
        }
    }

    private void SwitchSpline(int i)
    {
        Vector3 posPoint = Vector3.zero;
        Vector3 previousPoint = Vector3.zero;

        Handles.color = Color.red;
        if (i < func.pointControl.Count - 1)
        {
            for (int t = 0; t <= 100; t++)
            {
                switch (func.typeSpline)
                {
                    case EditorSpline.TypeSpline.Bezier:
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
                    Handles.DrawLine(previousPoint, posPoint);

                previousPoint = posPoint;
            }
        }
    }

  
}
