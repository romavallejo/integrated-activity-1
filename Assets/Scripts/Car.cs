using System.Collections.Generic;
using NUnit.Framework;
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
    public Vector3 currPos;

    //to help with the smooth rotation and curve int he corern
    public bool takingCorner, preparingForCorner;
    public int rotationProgress;
    public float rotationProgressStep;
    public Vector3 pivotPosition;

    //to indicate teh car is moving forward
    public bool movingForward,  waiting;
    public int waitingTimer;

    public void Initialize(Vector3 initPos, GameObject prefab, List<Vector3> initTargets)
    { 
        waitingTimer = 0;
        rotationProgress = 0;
        movingForward = false;
        waiting = false;
        takingCorner = false;
        preparingForCorner = false;

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

    public void calcPivot()
    {
        Vector3 A = targets[0];
        Vector3 B = targets[1];
        Vector3 C = targets[2];

        float pivotX = (A.x + C.x) * 0.5f;
        float pivotZ = (A.z + C.z) * 0.5f;

        pivotPosition = new Vector3(pivotX, B.y, pivotZ);
    }

    public void takeCorner()
    {
    Matrix4x4 notP = MathFunctions.TranslateM(-pivotPosition);
    Matrix4x4 rotM = MathFunctions.RotateY(rotationProgressStep); 
    Matrix4x4 yesP = MathFunctions.TranslateM(pivotPosition);
    mem = yesP * rotM * notP * mem;
    }

    public void moveForward(float speed)
    {
        Matrix4x4 forward = MathFunctions.TranslateM(new Vector3(speed,0,0));
        mem = mem * forward;
    }

    public void Update()
    {
        currPos = new Vector3(mem[0,3], mem[1,3], mem[2,3]);
        //Debug.Log("Current Position: " + currPos);

        if (preparingForCorner)
        {
            rotationProgress++;
            moveForward(0.01f);
            if (rotationProgress == 150)
            {
                rotationProgress = 0;
                preparingForCorner = false;
                takingCorner = true;
            }
        }

        else if (takingCorner)
        {
            takeCorner();
            rotationProgress++;
            if (rotationProgress == 100)
            {
                takingCorner = false;
                rotationProgress = 0;
                targets.RemoveAt(0);
                targets.RemoveAt(0);
                currTarget = targets[0]; // dont understand why i needed this but is needed
            }
        }   
        
        else if (movingForward)
        {
            if (Vector3.Distance(currPos, currTarget) <= 0.02f)
            {
                movingForward = false;
                targets.RemoveAt(0);
            }
            moveForward(0.01f);
        }
        
        else if (waiting)
        {
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
            Debug.Log("next target: "+currTarget);

            if (targets.Count > 2)
                if (rotationNedded(targets))
                {
                    Vector3 currentForward = new Vector3(mem[0, 0], mem[1, 0], mem[2, 0]);
                    Vector3 toTarget = MathFunctions.Normalize(targets[1] - currPos);

                    Vector3 A = targets[0];
                    Vector3 B = targets[1];
                    Vector3 C = targets[2];

                    Vector3 dirIn  = MathFunctions.Normalize(B - A);
                    Vector3 dirOut = MathFunctions.Normalize(C - B);

                    float cross = dirIn.x * dirOut.z - dirIn.z * dirOut.x;
                    rotationProgressStep = (cross > 0) ? -0.9f : 0.9f;
                    calcPivot();                    
                    preparingForCorner = true;
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
