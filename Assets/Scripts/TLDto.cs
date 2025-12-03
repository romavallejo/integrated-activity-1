using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TrafficLightDTO
{
    public int id;
    public string state;
    public float x;
    public float y;
}

[System.Serializable]
public class TrafficLightResponse
{
    public int step;
    public List<TrafficLightDTO> traffic_lights;
}
