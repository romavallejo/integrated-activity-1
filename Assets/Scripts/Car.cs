using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Car : MonoBehaviour
{
    //to save the geometry
    public List<Vector3> originals, initialPosition;
    //for transformations
    public Matrix4x4 tra, rot, mem;
    //the actual instace of the car created out of the carpreb vertices and being transformed
    public GameObject carInstance;

    //to see current positio nto reach
    public List<Vector3> targets;
    public Vector3 currTarget;

    //to help with the smooth rotation and curve int he corern
    public bool takingCorner;
    public int rotationProgress;
    public int rotationDirection;
    public Vector3 pivotPosition;

    //to indicate teh car is moving forward
    public bool movingForward;
    public bool waiting;
    public int waitingTimer;

    public void Initialize(Vector3 initPos, GameObject prefab, List<Vector3> initTargets)
    { 
        waitingTimer = 0;
        rotationProgress = 0;

        tra = MathFunctions.TranslateM(initPos);

        carInstance = Instantiate(prefab);

        MeshFilter mf = carInstance.GetComponent<MeshFilter>();
        originals = new List<Vector3>(mf.mesh.vertices);

        mem = tra;

        List<Vector3> transformed = MathFunctions.ApplyTransform(mem, originals);

        mf.mesh.vertices = transformed.ToArray();
        mf.mesh.RecalculateNormals();

        targets = initTargets;
    }

    public void addTarget(Vector3 target)
    {
        targets.Add(target);
    }

    private bool rotationNedded(List<Vector3> points)
    {
        Vector3 dir1 = MathFunctions.Normalize(points[1] - points[0]);
        Vector3 dir2 = MathFunctions.Normalize(points[2] - points[1]);
        return MathFunctions.DotProduct(dir1, dir2) < 0.99f;
    }
    public void takeCorner()
    {
        Matrix4x4 toPivot = MathFunctions.TranslateM(-pivotPosition);
        Matrix4x4 rot = MathFunctions.RotateY(rotationDirection);
        Matrix4x4 back = MathFunctions.TranslateM(pivotPosition);
        mem = back * rot * toPivot * mem;
    }

    public void moveForward()
    {
        Matrix4x4 forward = MathFunctions.TranslateM(new Vector3(0.01f,0,0));
        mem = forward * mem;
    }

    public void Update()
    {
        Vector3 currPos = new Vector3(mem[0,3], mem[1,3], mem[2,3]);
        Debug.Log("Current Position: " + currPos);

        if (takingCorner)
        {
            Debug.Log("takingCorner");
            takeCorner();
            rotationProgress++;
            if (rotationProgress == 90)
            {
                takingCorner = false;
                targets.RemoveAt(0);
                targets.RemoveAt(0);
            }
        }   

        else if (movingForward)
        {
            Debug.Log("moving forward");
            Debug.Log("curr target: "+currTarget);
            Debug.Log(Vector3.Distance(currPos, currTarget) <= 0.02f);
            if (Vector3.Distance(currPos, currTarget) <= 0.02f)
            {
                movingForward = false;
                targets.RemoveAt(0);
            }
            moveForward();
        }
        
        else if (waiting)
        {
            Debug.Log("waiting");
            waitingTimer++;
            if (waitingTimer == 100)
            {
                waiting = false;
                waitingTimer = 0;
                targets.RemoveAt(0);
            }
        }
        
        else if (targets.Count > 0)
        {
            currTarget = targets[0];

            if (targets.Count > 2)
            {
                
            }

            if (Vector3.Distance(currPos, currTarget) <= 0.02f)
                waiting = true;
            
            else movingForward = true;
            
        }

        List<Vector3> transformed = MathFunctions.ApplyTransform(mem, originals);
        carInstance.GetComponent<MeshFilter>().mesh.vertices = transformed.ToArray();
        carInstance.GetComponent<MeshFilter>().mesh.RecalculateNormals();
    }
}
