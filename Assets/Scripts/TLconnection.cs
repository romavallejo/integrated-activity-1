using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class TrafficLightConnection : MonoBehaviour
{
    public string trafficLightsApiUrl = "http://127.0.0.1:5000/api/traffic_light_step";
    public float updateInterval = 0.5f;

    void Start()
    {
        StartCoroutine(UpdateTrafficLightsLoop());
    }

    IEnumerator UpdateTrafficLightsLoop()
    {
        while (true)
        {
            yield return StartCoroutine(GetTrafficLightData());
            yield return new WaitForSeconds(updateInterval);
        }
    }

    IEnumerator GetTrafficLightData()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(trafficLightsApiUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("[TrafficLightConnection] Error querying traffic lights " + request.error);
            }
            else
            {
                UpdateTrafficLights(request.downloadHandler.text);
            }
        }
    }

    void UpdateTrafficLights(string json)
    {
        TrafficLightResponse data = JsonUtility.FromJson<TrafficLightResponse>(json);

        if (data == null || data.traffic_lights == null)
        {
            Debug.LogError("[TrafficLightConnection] Error deserializing JSON.");
            return;
        }

        TrafficLightManager[] allTrafficLights =
            FindObjectsByType<TrafficLightManager>(FindObjectsSortMode.None);

        if (allTrafficLights.Length == 0)
        {
            Debug.LogError("[TrafficLightConnection] No TrafficLightManager was found in the scene");
            return;
        }

        foreach (TrafficLightDTO tl in data.traffic_lights)
        {
            foreach (var light in allTrafficLights)
            {
                if (light.id == tl.id)
                {
                    light.SetState(tl.state);
                    break;
                }
            }
        }
    }
}
