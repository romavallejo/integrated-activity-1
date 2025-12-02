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

            Vector3 vect = new Vector3(car.x, car.y, 0);
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
                if (i >= carPaths.Count)
                {
                    Debug.LogError($"Index {i} out of range for carPaths");
                    break;
                }

                carPaths[i].Add(new Vector3(car.x, car.y, 0));
                i++;
            }
        }



        for (int i = 0; i < 10; i++)
        {


            int count = carPaths[i].Count;

            // Si solo tiene 1 punto, rest será una lista vacía
            List<Vector3> rest = (count > 1)
                ? carPaths[i].GetRange(1, count - 1)
                : new List<Vector3>();

            Debug.Log("resto: " + string.Join(", ", rest));

            Debug.Log("Prefab array: " + CarManager.carPrefabs.Count);

            if (CarManager.carPrefabs[2] == null)
                Debug.LogError("carPrefabs[2] is NULL!");

            GameObject test = CarManager.carPrefabs[2];
            Debug.Log("Prefab name: " + test.name);

            GameObject carObj = Instantiate(CarManager.carPrefabs[2]);

            if (carObj == null)
                Debug.LogError("Instantiate returned NULL!");

            Car newCar = carObj.GetComponent<Car>();

            if (newCar == null)
                Debug.LogError("Car component missing on prefab!");

            newCar.Initialize(
                carPaths[i][0],
                CarManager.carPrefabs[2],
                rest
            );

            CarManager.cars.Add(newCar);
        }

    }

    void addPoint(string json)
    {
        InitializeCar data = JsonUtility.FromJson<InitializeCar>(json);

        int i = 0;
        foreach (CarDTO car in data.cars)
        {

            Vector3 vect = new Vector3(car.x, car.y, 0);
            carPaths[i].Add(vect);
            i++;
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
            Debug.Log($"##### PATH {i} (count={carPaths[i].Count}) #####");

            for (int j = 0; j < carPaths[i].Count; j++)
            {
                Debug.Log($"{i}:{j} -> {carPaths[i][j]}");
            }
        }
    }


}
