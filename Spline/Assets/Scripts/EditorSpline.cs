using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class EditorSpline : MonoBehaviour
{
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

    #region Movement
    public bool isActivate = false;
    public GameObject objectFollowSpline;
    public float speed = 1f;


    private Vector3 startpos = Vector3.zero, first = Vector3.zero,
                endPoint = Vector3.zero, second = Vector3.zero;


    private int splineNumber = 0;
    private bool coroutine = true;
    private float timeMovement = 0.0f;

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
    }
    private IEnumerator SwitchSplineWithMovement()
    {

        coroutine = false;

        GetBSplineAndCatmullRomPointControl(splineNumber);
        GetBezierAndHermitePointControl(splineNumber);

        Vector3 posPoint = Vector3.zero;

        while (timeMovement <= 1)
        {
            timeMovement += speed * Time.deltaTime;

            switch (typeSpline)
            {
                case EditorSpline.TypeSpline.Bezier:
                    {
                        posPoint = Bezier.GetPoint(startpos, first, endPoint, second, timeMovement);
                        break;
                    }
                case EditorSpline.TypeSpline.Hermite:
                    {
                        posPoint = Hermite.GetPoint(startpos, first, endPoint, second, timeMovement);
                        break;
                    }
                case EditorSpline.TypeSpline.BSpline:
                    {
                        posPoint = BSpline.GetPoint(startpos, first, endPoint, second, timeMovement);
                        break;
                    }
                case EditorSpline.TypeSpline.CatmullRom:
                    {
                        posPoint = CatmullRom.GetPoint(startpos, first, endPoint, second, timeMovement);
                        break;
                    }
                default: break;
            }

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
