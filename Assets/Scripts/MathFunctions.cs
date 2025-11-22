using System;
using System.Collections.Generic;
using UnityEngine;

public static class MathFunctions
{
    public static float Magnitude(Vector3 v)
    {
        return Mathf.Sqrt(v.x * v.x + v.y * v.y + v.z * v.z);
    }

    public static Vector3 Normalize(Vector3 v)
    {
        float mag = Magnitude(v);
        return new Vector3(v.x / mag, v.y / mag, v.z / mag);
    }

    public static float DotProduct(Vector3 u, Vector3 v)
    {
        return u.x * v.x + u.y * v.y + u.z * v.z;
    }

    public static Vector3 CrossProduct(Vector3 u, Vector3 v)
    {
        return new Vector3(u.y * v.z - u.z * v.y, u.z * v.x - u.x * v.z, u.x * v.y - u.y * v.x);
    }

    public static float AngleBetween(Vector3 u, Vector3 v)
    {
        Vector3 nu = Normalize(u);
        Vector3 nv = Normalize(v);
        float dot = DotProduct(nu, nv);
        float a = Mathf.Acos(dot);
        return Mathf.Rad2Deg * a; //degrees
    }

    public static Matrix4x4 TranslateM(Vector3 t)
    {
        Matrix4x4 m = Matrix4x4.identity;
        m[0, 3] = t.x; //top right
        m[1, 3] = t.y; //second right
        m[2, 3] = t.z; //third right
        return m;
    }

    public static Matrix4x4 ScaleM(Vector3 s)
    {
        Matrix4x4 m = Matrix4x4.identity;
        m[0, 0] = s.x;
        m[1, 1] = s.y;
        m[2, 2] = s.z;
        return m;
    }

    public static Matrix4x4 RotateX(float degrees)
    {
        float r = Mathf.Deg2Rad * degrees;
        Matrix4x4 m = Matrix4x4.identity;
        m[1, 1] = Mathf.Cos(r);
        m[2, 1] = Mathf.Sin(r);
        m[1, 2] = -Mathf.Sin(r);
        m[2, 2] = Mathf.Cos(r);
        return m;
    }

    public static Matrix4x4 RotateY(float degrees)
    {
        float r = Mathf.Deg2Rad * degrees;
        Matrix4x4 m = Matrix4x4.identity;
        m[0, 0] = Mathf.Cos(r);
        m[2, 0] = -Mathf.Sin(r);
        m[0, 2] = Mathf.Sin(r);
        m[2, 2] = Mathf.Cos(r);
        return m;
    }

    public static Matrix4x4 RotateZ(float degrees)
    {
        float r = Mathf.Deg2Rad * degrees;
        Matrix4x4 m = Matrix4x4.identity;
        m[0, 0] = Mathf.Cos(r);
        m[1, 0] = Mathf.Sin(r);
        m[0, 1] = -Mathf.Sin(r);
        m[1, 1] = Mathf.Cos(r);
        return m;
    }

    public static List<Vector3> ApplyTransform(Matrix4x4 matrix, List<Vector3> geometry)
    {
        List<Vector3> result = new List<Vector3>(geometry.Count);

        for (int i = 0; i < geometry.Count; i++)
        {
            result.Add(matrix.MultiplyPoint3x4(geometry[i]));
        }

        return result;
    }

    //public static Vector3 Reflect(Vector3 v, Vector3 n)
    //public static Vector3 Illuminate(Vector3 CAM, Vector3 LIGHT,
    //Vector3 PoI, Vector3 n, Vector3 ka, Vector3 kd, Vector3 ks,
    //Vector3 La, Vector3 Ld, Vector3 Ls, float ALPHA)

    public static Vector3 Reflect(Vector3 l, Vector3 n)
    {
        Vector3 nn = Normalize(n);
        float dot = DotProduct(l, nn);
        return l - 2f * dot * nn;
    }

    public static Vector3 Illuminate(Vector3 CAM, Vector3 LIGHT, Vector3 PoI, Vector3 n,
    Vector3 ka, Vector3 kd, Vector3 ks,
    Vector3 La, Vector3 Ld, Vector3 Ls, float ALPHA)
    {
        Vector3 nn = Normalize(n);
        Vector3 l = Normalize(LIGHT - PoI);
        Vector3 v = Normalize(CAM - PoI);
        Vector3 r = Reflect(-l, nn);
        // Difusa
        float diff = Mathf.Max(0f, DotProduct(nn, l));
        // Especular
        float spec = Mathf.Max(0f, DotProduct(r, v));
        spec = Mathf.Pow(spec, ALPHA);
        float R = ka.x * La.x + kd.x * Ld.x *    diff + ks.x * Ls.x * spec;
        float G = ka.y * La.y + kd.y * Ld.y * diff + ks.y * Ls.y * spec;
        float B = ka.z * La.z + kd.z * Ld.z * diff + ks.z * Ls.z * spec;
        return new Vector3(R, G, B);
    }

    //inclination around -X axis, azimuth around Y axis
    public static Vector3 SphericalToCartesian(float iDeg, float aDeg, Vector3 SC, float SR)
    {
        float iRadians = iDeg * Mathf.Deg2Rad;
        float aRadians = aDeg * Mathf.Deg2Rad;
        float PoIx = SC.x + SR * Mathf.Sin(iRadians) * Mathf.Sin(aRadians);
        float PoIy = SC.y + SR * Mathf.Cos(iRadians);
        float PoIz = SC.z + SR * Mathf.Sin(iRadians) * Mathf.Cos(aRadians);
        return new Vector3(PoIx,PoIy,PoIz);
    }

}