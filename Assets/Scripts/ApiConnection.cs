using UnityEngine;
using UnityEngine.Networking; // Required for network calls
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;


public class Conecction : MonoBehaviour

{

    public GameObject carlogic;
    public bool requestingPoints = false;

    List<List<Vector3>> carPaths = new List<List<Vector3>>();

    void Start()

    {

        for (int i = 0; i < 10; i++)
        {
            carPaths.Add(new List<Vector3>());
        }

        StartCoroutine(InitializeSequence());
    }


    IEnumerator InitializeSequence()
    {

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

    IEnumerator addpoint()
    {
        using (UnityWebRequest request = UnityWebRequest.Get("http://127.0.0.1:5000/api/step"))
        {
            yield return request.SendWebRequest();
            requestingPoints = false;


            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
            }
            else
            {

                string jsonResult = request.downloadHandler.text;

                addPoint(jsonResult);

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
            Vector3 vect = new Vector3(24 - car.y, 0.1f, car.x);
            vect.x += 0.5f;
            vect.z += 0.5f;
            carPaths[i].Add(vect);
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
                Vector3 vect = new Vector3(24 - car.y, 0.1f, car.x);
                vect.x += 0.5f;
                vect.z += 0.5f;
                carPaths[i].Add(vect);
                i++;
            }
        }



        for (int i = 0; i < 10; i++)
        {


            int count = carPaths[i].Count;

            List<Vector3> rest = (count > 1)
                ? carPaths[i].GetRange(1, count - 1)
                : new List<Vector3>();


            Car car = Instantiate(carlogic).GetComponent<Car>();

            car.connection = this;

            car.Initialize(
                carPaths[i][0],
                CarManager.carPrefabs[2],
                rest
            );

            CarManager.cars.Add(car);


        }

    }

    void addPoint(string json)
    {
        InitializeCar data = JsonUtility.FromJson<InitializeCar>(json);

        int i = 0;
        foreach (CarDTO car in data.cars)
        {
            Vector3 vect = new Vector3(24 - car.y, 0.1f, car.x);
            vect.x += 0.5f;
            vect.z += 0.5f;
            CarManager.cars[i].targets.Add(vect);
            i++;
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (requestingPoints) return;

        if (CarManager.cars[0].targets.Count <= 1000000)
        {
            requestingPoints = true;
            StartCoroutine(addpoint());
        }

    }

    void PrintCarPaths()
    {
        for (int i = 0; i < carPaths.Count; i++)
        {
            Debug.Log($"##### PATH {i} (count={carPaths[i].Count}) #####");

            for (int j = 0; j < carPaths[i].Count; j++)
            {
                Debug.Log($"{i}:{j} -> {carPaths[i][j]}");
            }
        }
    }

}
