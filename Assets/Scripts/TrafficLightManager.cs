using UnityEngine;

public class TrafficLightManager : MonoBehaviour
{
    public int id = -1; 
    public string currentState = "RED";

    public GameObject redLight;
    public GameObject yellowLight;
    public GameObject greenLight;

    public void SetState(string state)
    {
        currentState = state;

        redLight.SetActive(state == "RED");
        yellowLight.SetActive(state == "ORANGE");
        greenLight.SetActive(state == "GREEN");
    }

}
