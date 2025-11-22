using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CItyCreation : MonoBehaviour
{
    public Material graassMaterial;
    List<Vector3> cityPositions = new List<Vector3>();
    /*
    > || <: horizontal street
    ^ || v: vertical street
    x: could turn all ways
    T: untiont to street up
    L: corner
    
    0: 0
    1: 90
    2: 180
    3: 270
    */
    List<String> cityInformation = new List<String>()
    {
        "L1",">",">","T2","T2",">",">","T2",">","T2",">",">",">",">",">","S","T2","T2",">","T2",">","T2",">","T2","L2",
        "^","E","E","^","v","E","E","^","E","P","E","E","E","E","E","E","S","S","E","P","E","P","E","v","v",
        "T1","P","E","^","v","E","E","^","E","E","E","E","E","E","E","E","^","^","E","E","E","E","E","v","v",
        "^","E","E","^","v","E","E","^","E","E","E","E","L1","T2","<","<","X","X","S","<","<","<","<","T3","v",
        "^","E","E","^","v","E","E","^","E","E","E","P","T3","v","E","E","S","S","E","E","E","E","E","v","v",
        "T1","P","E","^","v","E","E","^","E","E","E","E","v","v","E","E","^","^","E","E","E","E","E","v","v",
        "^","E","E","^","v","E","E","^","E","E","E","E","v","T1","P","E","^","T1","P","E","E","E","E","v","v",
        "T1","<","<","T3","v","E","E","T1","T2",">","T2",">","T3","v","E","E","^","^","E","E","E","E","E","v","v",
        "^","E","E","^","v","E","E","^","^","E","P","E","v","v","E","E","^","T1","<","<","<","<","<","T3","v",
        "^","E","E","^","T1","P","E","^","^","E","E","E","v","T1","P","E","^","L0","<","T2","<","<","<","T3","v",
        "T1",">",">","X","S",">",">","T0","T1","E","E","E","v","T1","T3","E","^","E","E","P","E","E","E","v","v",
        "T1","T2","T2","S",">","T3","T3",">","T1","E","P","E","S","S","^","E","^","E","E","E","E","E","E","S","S",
        "^","^","^","R","R","S","S",">",">",">",">","S",">",">",">",">",">",">",">",">",">",">",">","T3","v",
        "^","^","^","R","R","v","v","S","<","<","<","<","<","<","<","<","<","<","T2","<","<","<","<","T3","v",
        "T1","T0","T0","T2","T2","X","X","S","<","<","<","<","L2","E","E","E","E","E","P","E","E","E","E","v","v",
        "^","E","E","S","S","v","v","E","E","E","E","E","^","E","E","E","P","E","E","E","E","E","E","v","v",
        "^","E","E","^","^","v","v","E","E","E","E","P","X","<","<","<","T0","<","<","T2","<","<","<","T3","v",
        "^","E","E","^","^","v","v","E","E","E","E","E","^","E","E","E","P","E","E","P","E","E","E","v","v",
        "^","E","E","^","^","v","T1","P","E","E","E","E","^","E","E","E","E","E","E","E","E","E","E","v","v",
        "^","E","E","^","^","v","v","E","E","E","E","E","^","E","E","E","E","E","E","E","E","E","E","v","v",
        "^","E","E","^","^","v","v","E","E","E","E","E","^","E","E","E","E","E","E","E","P","E","E","v","v",
        "^","E","E","^","T1","T0","T0",">",">",">",">",">","T0",">",">",">",">",">",">",">","T0",">",">","T3","v",
        "^","E","E","^","T1",">",">",">",">",">",">",">",">",">",">",">",">",">",">",">",">",">",">","T3","v",
        "^","E","E","^","^","E","E","E","E","E","E","E","E","E","E","E","E","E","E","E","E","E","E","v","v",
        "L0","<","<","T0","T0","<","<","<","<","<","<","<","<","<","<","<","<","<","<","<","<","<","<","T0","L3",
    };
    public GameObject straightStreet;
    public GameObject L;
    public GameObject T;
    public GameObject X;
    public GameObject parking;

    void createCityBase()
    {
        List<Vector3> geometry = new List<Vector3>()
        {
            new Vector3(-0.5f, -0.5f, 0.5f),
            new Vector3(0.5f, -0.5f, 0.5f),
            new Vector3(-0.5f, 0.5f, 0.5f),
            new Vector3(0.5f, 0.5f, 0.5f),
            new Vector3(0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, 0.5f, -0.5f),
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(-0.5f, 0.5f, -0.5f)
        };

        List<int> topology = new List<int>()
        {
            0,1,2, 2,1,3,
            1,4,3, 3,4,5,
            4,6,5, 5,6,7,
            6,0,7, 7,0,2,
            2,3,7, 7,3,5,
            6,4,0, 0,4,1
        };
    
        Mesh m;

        GameObject go;
        Matrix4x4 mem;
        Matrix4x4 scale = MathFunctions.ScaleM(new Vector3(27.5f, 0, 27.5f));

        m = new Mesh();
        m.vertices = geometry.ToArray();
        m.triangles = topology.ToArray();

        go = new GameObject("grass");
        MeshRenderer mr = go.AddComponent<MeshRenderer>();
        mr.material = graassMaterial;
        go.AddComponent<MeshFilter>().mesh = m;

        mem = MathFunctions.TranslateM(new Vector3(12.5f, 0.0f, 12.5f));
        Matrix4x4 temp = mem * scale;
        List<Vector3> geo = MathFunctions.ApplyTransform(temp, geometry);
        
        go.GetComponent<MeshFilter>().mesh.vertices = geo.ToArray();
        go.GetComponent<MeshFilter>().mesh.RecalculateNormals();
    }

    void createCity()
    {
        for (float i = 0.5f; i < 25; i++)
        {
            for (float j = 0.5f; j < 25; j++)
            {
                cityPositions.Add(new Vector3(i,0,j));
            }
        }

        Transform child;
        for (int i = 0; i < cityPositions.Count; i++)
        {
            if (cityInformation[i] == "<" || cityInformation[i] == ">")
            {
                GameObject horistonalStreet = Instantiate(straightStreet, cityPositions[i], Quaternion.identity);
                child = horistonalStreet.transform.Find("default");
                child.transform.localScale = new Vector3(0.5f,0.5f,0.5f);

            }
            else if (cityInformation[i] == "^" || cityInformation[i] == "v")
            {
                GameObject verticalStreet = Instantiate(straightStreet,cityPositions[i], Quaternion.identity);
                child = verticalStreet.transform.Find("default");
                child.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
                child.transform.localRotation = Quaternion.Euler(0, 90, 0);
            }
            else if (cityInformation[i].Contains("L"))
            {
                GameObject verticalStreet = Instantiate(L,cityPositions[i], Quaternion.identity);
                child = verticalStreet.transform.Find("default");
                child.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
                switch (int.Parse(cityInformation[i][1].ToString()))
                {
                    case 1:
                        child.transform.localRotation = Quaternion.Euler(0, 90, 0);
                        break;
                    case 2:
                        child.transform.localRotation = Quaternion.Euler(0, 180, 0);
                        break;
                    case 3:
                        child.transform.localRotation = Quaternion.Euler(0, 270, 0);
                        break;
                    default:
                        break;
                }
            }
            else if (cityInformation[i].Contains("T"))
            {
                GameObject verticalStreet = Instantiate(T,cityPositions[i], Quaternion.identity);
                child = verticalStreet.transform.Find("default");
                child.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
                switch (int.Parse(cityInformation[i][1].ToString()))
                {
                    case 1:
                        child.transform.localRotation = Quaternion.Euler(0, 90, 0);
                        break;
                    case 2:
                        child.transform.localRotation = Quaternion.Euler(0, 180, 0);
                        break;
                    case 3:
                        child.transform.localRotation = Quaternion.Euler(0, 270, 0);
                        break;
                    default:
                        break;
                }
            }
            else if (cityInformation[i].Contains("X"))
            {
                GameObject verticalStreet = Instantiate(X,cityPositions[i], Quaternion.identity);
                child = verticalStreet.transform.Find("default");
                child.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
            }
            else if (cityInformation[i] == "P")
            {
                GameObject park = Instantiate(parking,cityPositions[i], Quaternion.identity);
                park.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
            }
        }
    }

    void Start()
    {
        createCityBase();
        createCity();
    }

    void Update()
    {
        
    }
}
