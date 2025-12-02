using UnityEngine;
using UnityEngine.Networking; // Required for network calls
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;


public class Conecction : MonoBehaviour

{


    List<List<Vector3>> carPaths = new List<List<Vector3>>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(InitializeSequence());

    }


    IEnumerator InitializeSequence()
    {
        for (int i = 0; i < 10; i++)
        {

            carPaths.Add(new List<Vector3>());
        }

        yield return StartCoroutine(initCars());
        yield return StartCoroutine(getData());
    }


    IEnumerator getData()
    {
        using (UnityWebRequest request = UnityWebRequest.Get("http://127.0.0.1:5000/api/step/3"))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
            }
            else
            {

                string jsonResult = request.downloadHandler.text;

                processResult(jsonResult);

            }

        }
    }

    IEnumerator initCars()
    {
        using (UnityWebRequest request = UnityWebRequest.Get("http://127.0.0.1:5000/api/cars"))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
            }
            else
            {

                string jsonResult = request.downloadHandler.text;

                initCars(jsonResult);

            }

        }

    }
    void initCars(string json)
    {
        InitializeCar data = JsonUtility.FromJson<InitializeCar>(json);

        int i = 0;
        foreach (CarDTO car in data.cars)
        {
            carPaths[i].Add(new Vector3(car.x, car.y, 0));
            i++;
        }

    }

    void processResult(string json)
    {


        CarManager.cars = new List<Car>();
        Root data = JsonUtility.FromJson<Root>(json);


        foreach (var step in data.steps)
        {
            int i = 0;
            foreach (CarDTO car in step.cars)
            {
                Debug.Log("(" + car.x + "," + car.y + ")");
                carPaths[i].Add(new Vector3(car.x, car.y, 0));
                i++;
            }
            Debug.Log("###########" + carPaths[1]);
        }

        for (int i = 0; i < 10; i++)
        {
            List<Vector3> rest = carPaths[i].GetRange(1, carPaths[i].Count - 1);

            Car newCar = new Car();
            newCar.Initialize(
                new Vector3(carPaths[i][0].x, carPaths[i][0].y, 0),
                CarManager.carPrefabs[2],
                rest
            );

            CarManager.cars.Add(newCar);
        }



    }


    // Update is called once per frame
    void Update()
    {
    }

    void PrintCarPaths()
    {
        for (int i = 0; i < carPaths.Count; i++)
        {
            Debug.Log($"---- CarPath {i} ----");
            foreach (var v in carPaths[i])
            {
                Debug.Log(v);
            }
        }
    }

}
