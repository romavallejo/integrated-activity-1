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
    public bool takingCorner, preparingForCorner, outOfParking;
    public int rotationProgress;
    public float rotationProgressStep;
    public Vector3 pivotPosition;

    //to indicate teh car is moving forward
    public bool movingForward, waiting;
    public int waitingTimer;

    public void Initialize(Vector3 initPos, GameObject prefab, List<Vector3> initTargets)
    {
        waitingTimer = 0;
        rotationProgress = 0;
        movingForward = false;
        waiting = false;
        takingCorner = false;
        preparingForCorner = false;
        outOfParking = false;

        tra = MathFunctions.TranslateM(initPos);
        carInstance = Instantiate(prefab);

        Vector3 desiredDir = MathFunctions.Normalize(initTargets[0] - initPos);
        Vector3 defaultDir = new Vector3(1, 0, 0);
        float angle = MathFunctions.AngleBetween(defaultDir, desiredDir);
        float sign = MathFunctions.CrossProduct(defaultDir, desiredDir).y >= 0 ? 1f : -1f;
        rot = MathFunctions.RotateY(angle * sign);

        mem = tra * rot;
        //mem = tra;

        if (rotationNeeded(initPos, initTargets[0], initTargets[1]))
            outOfParking = true;

        MeshFilter mf = carInstance.GetComponent<MeshFilter>();
        originals = new List<Vector3>(mf.mesh.vertices);

        List<Vector3> transformed = MathFunctions.ApplyTransform(mem, originals);
        mf.mesh.vertices = transformed.ToArray();
        mf.mesh.RecalculateNormals();
        targets = initTargets;
        //targets.Insert(0, initPos);
    }


    public void addTarget(Vector3 target)
    {
        targets.Add(target);
    }

    private bool rotationNeeded(Vector3 A, Vector3 B, Vector3 C)
    {
        Vector3 dirIn = MathFunctions.Normalize(B - A);
        Vector3 dirOut = MathFunctions.Normalize(C - B);
        return MathFunctions.DotProduct(dirIn, dirOut) < 0.99f;
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
    float Snap(float value, float step)
    {
        return Mathf.Round(value / step) * step;
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
        Matrix4x4 forward = MathFunctions.TranslateM(new Vector3(speed, 0, 0));
        mem = mem * forward;
    }

    public void Update()
    {
        currPos = new Vector3(mem[0, 3], mem[1, 3], mem[2, 3]);
        if (targets.Count == 0)
            return;

        if (preparingForCorner)
        {
            rotationProgress++;
            moveForward(0.01f);
            if (rotationProgress == 50)
            {
                outOfParking = false;
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
                currPos = new Vector3(mem[0, 3], mem[1, 3], mem[2, 3]);
                //Debug.Log("position after finishing corne4r: "+currPos);
                takingCorner = false;
                rotationProgress = 0;
                Vector3 temp = targets[0];
                targets.RemoveAt(0);
                currTarget = targets[0]; // dont understand why i needed this but is needed


                Vector3 forward = new Vector3(mem[0, 0], mem[1, 0], mem[2, 0]);

                bool movingInX = Mathf.Abs(forward.x) > Mathf.Abs(forward.z);
                if (movingInX)
                {
                    mem[0, 3] = Mathf.Round(mem[0, 3]);
                    mem[2, 3] = Snap(mem[2, 3], 0.5f);
                }
                else
                {
                    mem[2, 3] = Mathf.Round(mem[2, 3]);
                    mem[0, 3] = Snap(mem[0, 3], 0.5f);
                }

                currPos = new Vector3(mem[0, 3], mem[1, 3], mem[2, 3]);

                if (targets.Count > 1)
                {
                    //Debug.Log(targets[0]+";"+targets[1]+"-> CurrPos:"+currPos);
                    //Debug.Log(rotationNeeded(currPos, targets[0], targets[1]));
                    //Debug.Break();
                    if (rotationNeeded(currPos, targets[0], targets[1]))
                    {
                        Vector3 A = temp;
                        Vector3 B = targets[0];
                        Vector3 C = targets[1];

                        Vector3 dirIn = MathFunctions.Normalize(B - A);
                        Vector3 dirOut = MathFunctions.Normalize(C - B);

                        float cross = dirIn.x * dirOut.z - dirIn.z * dirOut.x;
                        rotationProgressStep = (cross > 0) ? -0.9f : 0.9f;

                        float pivotX = (A.x + C.x) * 0.5f;
                        float pivotZ = (A.z + C.z) * 0.5f;

                        pivotPosition = new Vector3(pivotX, B.y, pivotZ);

                        takingCorner = true;
                    }
                }
            }
        }

        else if (movingForward)
        {
            moveForward(0.01f);
            if (Vector3.Distance(currPos, currTarget) <= 0.02f)
            {
                //Debug.Log("Reached: "+currTarget);
                movingForward = false;
                targets.RemoveAt(0);


                if (targets.Count > 1)
                {
                    //Debug.Log("Finished Forward"+currPos+";"+targets[0]+";"+targets[1]);
                    //Debug.Log(rotationNeeded(currPos, targets[0], targets[1]));
                    if (rotationNeeded(currPos, targets[0], targets[1]))
                    {
                        Vector3 A = currPos;
                        Vector3 B = targets[0];
                        Vector3 C = targets[1];

                        Vector3 dirIn = MathFunctions.Normalize(B - A);
                        Vector3 dirOut = MathFunctions.Normalize(C - B);

                        float cross = dirIn.x * dirOut.z - dirIn.z * dirOut.x;
                        rotationProgressStep = (cross > 0) ? -0.9f : 0.9f;

                        float pivotX = (A.x + C.x) * 0.5f;
                        float pivotZ = (A.z + C.z) * 0.5f;

                        pivotPosition = new Vector3(pivotX, B.y, pivotZ);

                        preparingForCorner = true;
                    }
                }
            }
        }

        else if (waiting)
        {
            //Debug.Log("waiting");
            waitingTimer++;
            if (waitingTimer == 100)
            {
                waiting = false;
                waitingTimer = 0;
                targets.RemoveAt(0);



                currPos = new Vector3(mem[0, 3], mem[1, 3], mem[2, 3]);
                //making sure it is at exactly the center
                mem[0, 3] = Snap(mem[0, 3], 0.5f);
                mem[2, 3] = Snap(mem[2, 3], 0.5f);


                currPos = new Vector3(mem[0, 3], mem[1, 3], mem[2, 3]);


                // changing direction if needed
                if (targets.Count > 0)
                {
                    if (targets[0] != currPos)
                    {
                        Vector3 desiredDir = MathFunctions.Normalize(targets[0] - currPos);
                        Vector3 forward = new Vector3(mem[0, 0], mem[1, 0], mem[2, 0]);
                        float angle = MathFunctions.AngleBetween(forward, desiredDir);
                        float sign = MathFunctions.CrossProduct(forward, desiredDir).y >= 0 ? 1f : -1f;
                        Matrix4x4 rotFix = MathFunctions.RotateY(angle * sign);
                        mem = rotFix * mem;
                        // Update forward vector after fix
                        forward = new Vector3(mem[0, 0], mem[1, 0], mem[2, 0]);
                    }

                }

                if (targets.Count > 1)
                {
                    //Debug.Log("Finished Forward"+currPos+";"+targets[0]+";"+targets[1]);
                    //Debug.Log(rotationNeeded(currPos, targets[0], targets[1]));
                    if (rotationNeeded(currPos, targets[0], targets[1]))
                    {
                        Vector3 A = currPos;
                        Vector3 B = targets[0];
                        Vector3 C = targets[1];

                        Vector3 dirIn = MathFunctions.Normalize(B - A);
                        Vector3 dirOut = MathFunctions.Normalize(C - B);

                        float cross = dirIn.x * dirOut.z - dirIn.z * dirOut.x;
                        rotationProgressStep = (cross > 0) ? -0.9f : 0.9f;

                        float pivotX = (A.x + C.x) * 0.5f;
                        float pivotZ = (A.z + C.z) * 0.5f;

                        pivotPosition = new Vector3(pivotX, B.y, pivotZ);

                        preparingForCorner = true;
                    }
                }

            }
        }

        else if (targets.Count > 0)
        {
            currTarget = targets[0];
            //Debug.Log("next target: " + currTarget);

            if (outOfParking)
            {
                Vector3 A = currPos;
                Vector3 B = targets[0];
                Vector3 C = targets[1];

                Vector3 dirIn = MathFunctions.Normalize(B - A);
                Vector3 dirOut = MathFunctions.Normalize(C - B);

                float cross = dirIn.x * dirOut.z - dirIn.z * dirOut.x;
                rotationProgressStep = (cross > 0) ? -0.9f : 0.9f;

                float pivotX = (A.x + C.x) * 0.5f;
                float pivotZ = (A.z + C.z) * 0.5f;
                pivotPosition = new Vector3(pivotX, B.y, pivotZ);

                preparingForCorner = true;
            }

            if (Vector3.Distance(currPos, currTarget) <= 0.02f)
                waiting = true;


            else
            {
                movingForward = true;//Debug.Log("moving forward")/;
            }


        }

        List<Vector3> transformed = MathFunctions.ApplyTransform(mem, originals);
        carInstance.GetComponent<MeshFilter>().mesh.vertices = transformed.ToArray();
        carInstance.GetComponent<MeshFilter>().mesh.RecalculateNormals();
    }
}
