using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[ExecuteInEditMode]
public class EditorSpline : MonoBehaviour
{
    private void Start()
    {
#if UNITY_EDITOR
        UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
#endif
    }
    private void Update()
    {
        if (isActivate)
        {
            if (coroutine)
                StartCoroutine(SwitchSplineWithMovement());
        }

        if (lineRealTime)
            LineRendererGenerate();
    }

    #region DataSpline

    [SerializeField]
    public enum TypeSpline
    {
        Default,
        Bezier,
        Hermite,
        BSpline,
        CatmullRom
    };

    [Space][SerializeField]
    public TypeSpline typeSpline;

    [Serializable]
    public class PointControl 
    {
        public Vector3 position = Vector3.zero;
        public Vector3 first = Vector3.zero;
        public Vector3 second = Vector3.zero;
        public bool show = false;
    }

    [Space][SerializeField]
    public List<PointControl> pointControl = new List<PointControl>();
    #endregion

    #region MovementAndLineRenderer

    // Movement Data
    public bool isActivate = false;
    public GameObject objectFollowSpline;
    public float speed = 1f;

    // Tools Data
    private Vector3 startpos = Vector3.zero, first = Vector3.zero,
                endPoint = Vector3.zero, second = Vector3.zero;


    private int splineNumber = 0;
    private bool coroutine = true;
    private float timeMovement = 0.0f;
   
    private IEnumerator SwitchSplineWithMovement()
    {

        coroutine = false;

        GetBSplineAndCatmullRomPointControl(splineNumber);
        GetBezierAndHermitePointControl(splineNumber);

        Vector3 posPoint = Vector3.zero;

        while (timeMovement <= 1)
        {
            timeMovement += speed * Time.deltaTime;

            posPoint = SwitchSpline(startpos, first, endPoint, second,timeMovement);

            objectFollowSpline.transform.position = posPoint;
            yield return new WaitForEndOfFrame();
        }

        timeMovement = 0f;
        splineNumber++;

        if ((typeSpline == EditorSpline.TypeSpline.BSpline || typeSpline == EditorSpline.TypeSpline.CatmullRom))
        {

            if (splineNumber >= pointControl.Count-3)
                splineNumber = 0;
        }
        else
        {
            if (splineNumber >= pointControl.Count-1)
                splineNumber = 0;
        }

        coroutine = true;
    }

    #endregion

    #region SplineCalculate
    private Vector3 SwitchSpline(Vector3 pt1, Vector3 pt2, Vector3 pt3, Vector3 pt4, float step)
    {
        Vector3 posPoint = Vector3.zero;

        switch (typeSpline)
        {
            case EditorSpline.TypeSpline.Bezier:
                {
                    posPoint = Bezier.GetPoint(pt1, pt2, pt3, pt4, step);
                    break;
                }
            case EditorSpline.TypeSpline.Hermite:
                {
                    posPoint = Hermite.GetPoint(pt1, pt2, pt3, pt4, step);
                    break;
                }
            case EditorSpline.TypeSpline.BSpline:
                {
                    posPoint = BSpline.GetPoint(pt1, pt2, pt3, pt4, step);
                    break;
                }
            case EditorSpline.TypeSpline.CatmullRom:
                {
                    posPoint = CatmullRom.GetPoint(pt1, pt2, pt3, pt4, step);
                    break;
                }
            default: break;
        }

        return posPoint;
    }

    private void GetBSplineAndCatmullRomPointControl(int splineNumber)
    {
        // Manage Point of BSpline and CatmullRom here
        if ((typeSpline == EditorSpline.TypeSpline.BSpline || typeSpline == EditorSpline.TypeSpline.CatmullRom)
            && splineNumber < pointControl.Count - 3)
        {
            startpos = pointControl[splineNumber].position;
            first = pointControl[splineNumber + 1].position;
            endPoint = pointControl[splineNumber + 2].position;
            second = pointControl[splineNumber + 3].position;
        }
    }

    private void GetBezierAndHermitePointControl(int splineNumber)
    {
        // Manage Control Point of Bezier and Hermite Spline 
        if ((typeSpline == EditorSpline.TypeSpline.Bezier || typeSpline == EditorSpline.TypeSpline.Hermite)
            && splineNumber < pointControl.Count - 1)
        {
            startpos = pointControl[splineNumber].position;
            first = pointControl[splineNumber].second;
            endPoint = pointControl[splineNumber + 1].position;
            second = pointControl[splineNumber + 1].first;
        }
    }
    #endregion

    #region LineRenderer
    // Line Rendere Data 
    public LineRenderer lineRenderer;
    public bool lineRealTime = false;
    public void CreateALineRender()
    {
        lineRenderer = gameObject.AddComponent(typeof(LineRenderer)) as LineRenderer;
    }

    public void RemoveLineRenderer()
    {
        if (lineRenderer)
            DestroyImmediate(lineRenderer);
    }
    public void LineRendererGenerate()
    {
        List<Vector3> listPoint = new List<Vector3>();
        for (int i = 0; i < pointControl.Count; i++)
        {

            GetBSplineAndCatmullRomPointControl(i);
            GetBezierAndHermitePointControl(i);

            for (int t = 0; t <= 100; t++)
            {

                Vector3 posPoint = Vector3.zero;

                posPoint = SwitchSpline(startpos, first, endPoint, second, t / 100f);

                listPoint.Add(posPoint);
            }
        }
        lineRenderer.positionCount = listPoint.Count;
        lineRenderer.SetPositions(listPoint.ToArray());
    }

    #endregion

    #region DataManagement
    // Manage Data, function Load and Save of Spline
    [Serializable]
    public class SerializableVector3
    {
        public float x;
        public float y;
        public float z;

        public void ConvertToSerializableVector3(Vector3 vec)
        {
            x = vec.x;
            y = vec.y;
            z = vec.z;
        }

        public Vector3 GetVector3()
        {
            return new Vector3(x, y, z);
        }

    }

    [Serializable]
    public class SerializablePointControl
    {
        public SerializableVector3 position = new SerializableVector3();
        public SerializableVector3 first= new SerializableVector3();
        public SerializableVector3 second = new SerializableVector3();

        public SerializablePointControl(Vector3 pos, Vector3 firstTan, Vector3 secondTan)
        {
            position.ConvertToSerializableVector3(pos);
            first.ConvertToSerializableVector3(firstTan);
            second.ConvertToSerializableVector3(secondTan);
        }

        public PointControl GetPointControl()
        {
            PointControl newPoint = new PointControl();
            newPoint.position = position.GetVector3();
            newPoint.first = first.GetVector3();
            newPoint.second = second.GetVector3();
            return newPoint;
        }
    }

    [HideInInspector]
    public List<SerializablePointControl> savePointControl;

    private string saveFile;
    private BinaryFormatter converter = new BinaryFormatter();

    public void ConvertPointControlOnSerializable()
    {
        if (savePointControl.Count != 0)
            savePointControl.Clear();

        foreach(PointControl point in pointControl)
        {
            SerializablePointControl newPoint = new SerializablePointControl(point.position, point.first,point.second);
            savePointControl.Add(newPoint);
        }
    }

    public void GetPointControlFromSerializable()
    {
        if (savePointControl.Count == 0)
            return;

        pointControl.Clear();

        for(int i = 0; i < savePointControl.Count; i++)
        {
            pointControl.Add( savePointControl[i].GetPointControl());
        }
    }

    public void LoadPointControl()
    {
        Debug.Log("Loading ...");

        if (File.Exists(saveFile))
        {
            FileStream inputStream = new FileStream(saveFile, FileMode.Open);

            savePointControl = converter.Deserialize(inputStream) as List<SerializablePointControl>;
            GetPointControlFromSerializable();

            inputStream.Close();
            Debug.Log("Loading Done !");

        }
    }
    public void SavePointControl()
    {
        Debug.Log("Saving ... ");

        savePointControl = new List<SerializablePointControl>();

        ConvertPointControlOnSerializable();
        if (File.Exists(saveFile))
        {
            FileStream inputStream = new FileStream(saveFile, FileMode.Open);

            converter.Serialize(inputStream, savePointControl);

            inputStream.Close();

        }
        else
        {

            saveFile = Application.persistentDataPath + "/gamedata.data";

            FileStream outputStream = new FileStream(saveFile, FileMode.Create);

            converter.Serialize(outputStream, savePointControl);

            outputStream.Close();

        }

        Debug.Log("Saving Done !");

    }
    #endregion
}
