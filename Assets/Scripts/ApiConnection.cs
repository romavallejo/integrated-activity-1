using UnityEngine;
using UnityEngine.Networking; // Required for network calls
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;


public class Conecction : MonoBehaviour

{


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()

    {
        Debug.Log("corre");
        StartCoroutine(getData());

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
                Debug.Log("Received: " + jsonResult);

                processResult(jsonResult);

            }

        }
    }

    void processResult(string json)
    {
        Root data = JsonUtility.FromJson<Root>(json);
        foreach (var step in data.steps)
        {
            foreach (CarDTO car in step.cars)
            {
                Debug.Log(car.x + car.y);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
