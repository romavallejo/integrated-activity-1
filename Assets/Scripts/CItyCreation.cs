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
    I: illumination
    C: Crossing
    
    0: 0
    1: 90
    2: 180
    3: 270

    F: filler of random tree
    B: random building
    O: fountain
    K: bench
    Y: fountain
    */
    List<String> cityInformation = new List<String>()
    {
        "L1","C>",">","T2","T2","C>",">","T2",">","T2","C>",">",">",">",">","SC>","T2","T2",">","T2",">","T2","C>","T2","L2",
        "^","F","F","C^","Cv","B3","B3","C^","F","P2","B3","B3","B3","B3","B3","F","SC^","SC^","B3","P2","F","P2","B3","Cv","Cv",
        "T1","P1","B0","^","v","B2","B0","^","B2","E","F","K","B1","B1","B1","F","^","^","B1","B1","B1","B1","B1","v","v",
        "^","B2","B0","^","vI","B2","B0","^","B2","Y","E","B0","L1","T2","<","C<","X","X","SC<","<","<","<","C<","T3","v",
        "^","B2","B0","^","v","B2","B0","^I","B2","E","E","P1","T3","vI","B3","B3","SC^","SC^","B3","B3","B3","B3","B3","v","v",
        "T1","P1","B0","^","v","B2","B0","^","B2","F","K","B0","v","v","B2","B0","^","^","B2","Y","E","F","B0","v","v",
        "^","F","F","C^","Cv","B2","B0","^","F","B1","B1","F","v","T1","P1","B0","^","T1","P1","E","E","K","B0","v","v",
        "T1","<","<","T3","v","B2","B0","T1","T2",">","T2","C>","T3","v","B2","B0","^","^","F","B1","B1","B1","B1","v","v",
        "^","F","F","^","v","B2","B0","^","^","B2","P2","B0","v","v","B2","B0","^","T1","<","<","<","<","C<","T3","vI",
        "C^I","F","K","^","T1","P0","F","^","^","B2","F","B0","v","T1","P1","B0","^","L0","<","T2","<","<","C<","T3","v",
        "T1",">",">","T3","ST13",">",">","T0","T3","B2","K","B0","v","T1","T3","K","^","F","B3","P0","B3","B3","B0","v","v",
        "T1","T2","T2","ST02","T0","T2","T2",">","T3","B2","P0","B0","SCv","SCv","^","F","^","K","B1","B1","B1","B1","B0","SCv","SCv",
        "^","^","^","Y","R","Sv","ST13",">","T0",">I",">","S>",">",">","T0",">",">",">",">",">I",">",">","SC>","T3","v",
        "^","^","^","R","R","v","v","S<","<","<","<","<","<","<","<","<","<","<","T2","<","<","<","C<","T3","v",
        "T1","T0","T0","T2","T2","X","X","S<","<","<","<","<","L2","F","B3","B3","B3","B3","P0","B3","B3","B3","F","v","v",
        "C^","F","F","SC^","SC^","Cv","Cv","F","B3","B3","B3","F","C^","F","B1","B1","P2","B1","B1","B1","B1","B1","F","v","v",
        "^","B2","B0","^","^","v","vI","B2","E","E","K","P1","X","<","<","<","T0","<","<I","T2","<","<","C<","T3","v",
        "^","B2","B0","^","^","v","v","B2","F","E","E","B0","^","F","B3","B3","P2","B3","B3","P2","B3","B3","F","v","v",
        "^","B2","B0","^","^","v","T1","P1","E","K","E","B0","^","B2","E","F","E","K","E","E","E","E","B0","v","v",
        "^I","F","K","^","^","v","v","B2","F","E","F","B0","^","B2","E","E","E","E","F","E","F","E","B0","v","v",
        "^","B2","B0","C^","C^","Cv","Cv","F","B1","B1","B1","F","C^","F","B1","B1","B1","B1","B1","B1","P0","B1","F","Cv","Cv",
        "^","B2","B0","^","T1","T0","T0","C>I",">",">",">",">","T0",">I",">",">",">",">",">",">","T0",">","C>I","T3","v",
        "^","B2","B0","^","T1",">",">","C>",">",">",">",">","C>",">",">",">",">",">",">",">",">",">",">","T3","v",
        "C^","K","F","^","^","F","K","F","E","F","K","F","E","F","E","F","E","F","K","F","E","F","K","v","v",
        "L0","C<","<","T0","T0","<I","<","C<","<","<","<","<","C<","<","<I","<","<","<","<","<","<","<","C<I","T0","L3",
    };
    public GameObject straightStreet;
    public GameObject crossing;
    public GameObject L;
    public GameObject T;
    public GameObject X;
    public GameObject parking;

    public GameObject tree1;
    public GameObject tree2;
    public GameObject tree3;

    public GameObject buildingA;
    public GameObject buildingB;
    public GameObject buildingC;
    public GameObject buildingD;
    public GameObject buildingE;
    public GameObject buildingF;
    public GameObject buildingG;
    public GameObject buildingH;

    public GameObject bench;
    public GameObject lightPost;
    public GameObject fountain;
    
    public GameObject stopLightSingle;

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

        for (int i = 0; i < cityPositions.Count; i++)
        {
            if (cityInformation[i].Contains("<") || cityInformation[i].Contains(">"))
            {
                GameObject horistonalStreet;
                if (cityInformation[i].Contains("C"))
                    horistonalStreet = Instantiate(crossing, cityPositions[i], Quaternion.identity);
                else
                    horistonalStreet = Instantiate(straightStreet, cityPositions[i], Quaternion.identity);
                horistonalStreet.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
                if (cityInformation[i].Contains("<"))
                    horistonalStreet.transform.localRotation = Quaternion.Euler(0, 180, 0);
                if (cityInformation[i].Contains("I"))
                {
                    GameObject light = Instantiate(lightPost,cityPositions[i],Quaternion.identity);
                }
                else if (cityInformation[i].Contains("S"))
                {
                    GameObject stopLight = Instantiate(stopLightSingle,cityPositions[i],Quaternion.identity);
                    if (cityInformation[i].Contains(">"))
                    {
                        stopLight.transform.localRotation = Quaternion.Euler(0,180, 0);
                    }
                }
            }
            else if (cityInformation[i].Contains("^") || cityInformation[i].Contains("v"))
            {
                GameObject verticalStreet;
                if (cityInformation[i].Contains("C"))
                    verticalStreet = Instantiate(crossing,cityPositions[i], Quaternion.identity);
                else
                    verticalStreet = Instantiate(straightStreet,cityPositions[i], Quaternion.identity);
                verticalStreet.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
                if (cityInformation[i].Contains("^"))
                    verticalStreet.transform.localRotation = Quaternion.Euler(0, -90, 0);
                else
                    verticalStreet.transform.localRotation = Quaternion.Euler(0, 90, 0);
                if (cityInformation[i].Contains("I"))
                {
                    GameObject light = Instantiate(lightPost,cityPositions[i],Quaternion.identity);
                    light.transform.localRotation = Quaternion.Euler(0, 90, 0);
                }
                else if (cityInformation[i].Contains("S"))
                {
                    if (cityInformation[i].Contains("^"))
                    {
                        GameObject stopLight = Instantiate(stopLightSingle,cityPositions[i],Quaternion.identity);
                        stopLight.transform.localRotation = Quaternion.Euler(0,90, 0);
                    } 
                    else if (cityInformation[i].Contains("v"))
                    {
                        GameObject stopLight = Instantiate(stopLightSingle,cityPositions[i],Quaternion.identity);
                        stopLight.transform.localRotation = Quaternion.Euler(0,270, 0);
                    }
                }
            }
            else if (cityInformation[i].Contains("L"))
            {
                GameObject cornerStreet = Instantiate(L,cityPositions[i], Quaternion.identity);
                cornerStreet.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
                cornerStreet.transform.localRotation = Quaternion.Euler(0,int.Parse(cityInformation[i][1].ToString())*90,0);
            }
            else if (cityInformation[i].Contains("T"))
            {
                GameObject tStreet = Instantiate(T,cityPositions[i], Quaternion.identity);
                tStreet.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
                if (cityInformation[i].Contains("S"))
                {
                    tStreet.transform.localRotation = Quaternion.Euler(0,int.Parse(cityInformation[i][2].ToString())*90, 0);
                    GameObject stopLight = Instantiate(stopLightSingle,cityPositions[i],Quaternion.identity);
                    stopLight.transform.localRotation = Quaternion.Euler(0,cityInformation[i][3]*90, 0);
                }
                else 
                    tStreet.transform.localRotation = Quaternion.Euler(0,int.Parse(cityInformation[i][1].ToString())*90, 0);
            }
            else if (cityInformation[i].Contains("X"))
            {
                GameObject xStreet = Instantiate(X,cityPositions[i], Quaternion.identity);
                xStreet.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
            }
            else if (cityInformation[i].Contains("P"))
            {
                GameObject park = Instantiate(parking,cityPositions[i], Quaternion.identity);
                park.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
                park.transform.localRotation = Quaternion.Euler(0,int.Parse(cityInformation[i][1].ToString())*90, 0);
            }
            else if (cityInformation[i] == "F")
            {
                int randomInt = UnityEngine.Random.Range(0, 3);
                GameObject[] trees = new GameObject[] {tree1,tree2,tree3};
                GameObject tree = Instantiate(trees[randomInt], cityPositions[i], Quaternion.identity);
                tree.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
            }
            else if (cityInformation[i].Contains("B"))
            {
                int randomInt = UnityEngine.Random.Range(0, 8);
                GameObject[] buldings = new GameObject[] {buildingA,buildingB,buildingC,buildingD,buildingE,buildingF,buildingG,buildingH};
                GameObject bulding = Instantiate(buldings[randomInt], cityPositions[i], Quaternion.identity);
                bulding.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
                bulding.transform.localRotation = Quaternion.Euler(0,int.Parse(cityInformation[i][1].ToString())*90, 0);
            }
            else if (cityInformation[i] == "Y")
            {
                GameObject fountainArea = Instantiate(fountain,new Vector3(cityPositions[i].x+0.5f,cityPositions[i].y,cityPositions[i].z+0.5f),Quaternion.identity);
                int randomInt = UnityEngine.Random.Range(0, 4);
                fountainArea.transform.localRotation = Quaternion.Euler(0, randomInt*90, 0);
            }
            else if (cityInformation[i] == "K")
            {
                GameObject benchArea = Instantiate(bench,cityPositions[i],Quaternion.identity);
                int randomInt = UnityEngine.Random.Range(0, 4);
                benchArea.transform.localRotation = Quaternion.Euler(0, randomInt*90, 0);
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
