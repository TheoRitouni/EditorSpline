using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class EditorSpline : MonoBehaviour
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

    [Space][SerializeField]
    public TypeSpline typeSpline;

    [Serializable]
    public class PointControl 
    {
        public Vector3 position = Vector3.zero;
        public Vector3 firstTangent = Vector3.zero;
        public Vector3 secondTangent = Vector3.zero;
        public bool show = false;
    }

    [Space][SerializeField]
    public List<PointControl> pointControl = new List<PointControl>();


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
        public SerializableVector3 firstTangent= new SerializableVector3();
        public SerializableVector3 secondTangent = new SerializableVector3();

        public SerializablePointControl(Vector3 pos, Vector3 firstTan, Vector3 secondTan)
        {
            position.ConvertToSerializableVector3(pos);
            firstTangent.ConvertToSerializableVector3(firstTan);
            secondTangent.ConvertToSerializableVector3(secondTan);
        }

        public PointControl GetPointControl()
        {
            PointControl newPoint = new PointControl();
            newPoint.position = position.GetVector3();
            newPoint.firstTangent = firstTangent.GetVector3();
            newPoint.secondTangent = secondTangent.GetVector3();
            return newPoint;
        }
    }

    [HideInInspector]
    public List<SerializablePointControl> savePointControl = new List<SerializablePointControl>();

    private string saveFile;
    private BinaryFormatter converter = new BinaryFormatter();

    public void ConvertPointControlOnSerializable()
    {
        if (savePointControl.Count != 0)
            savePointControl.Clear();

        foreach(PointControl point in pointControl)
        {
            SerializablePointControl newPoint = new SerializablePointControl(point.position, point.firstTangent,point.secondTangent);
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

        }
    }
    public void SavePointControl()
    {
        Debug.Log("Saving ... ");

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
    }

}
