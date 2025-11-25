using System.Collections.Generic;
using UnityEngine;

public class InstanceCars : MonoBehaviour
{
    [SerializeField]
    GameObject CarPrefab;
    List<Vector3> originals;
    Matrix4x4 tra, rot, mem;
    GameObject myCar;

    void Start()
    {
        rot = MathFunctions.RotateY(90);
        tra = MathFunctions.TranslateM(new Vector3(0,0,-0.0005f));
        myCar = Instantiate(CarPrefab);
        originals = new List<Vector3>(myCar.GetComponent<MeshFilter>().mesh.vertices);
        mem = rot;
    }

    void Update()
    {
        //ToDo: translate the car from 0,0,0 to 0,0,-20
        //use homogeneous transformations
        myCar.GetComponent<MeshFilter>().mesh.vertices = MathFunctions.ApplyTransform(mem*tra,originals).ToArray();
        mem = mem* tra;
    }
}
