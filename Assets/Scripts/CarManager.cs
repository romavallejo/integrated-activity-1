using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    //PrebaCars
    public GameObject convertible;
    public GameObject pickuptruck;
    public GameObject cybertruck;
    public GameObject sedan;
    List<GameObject> carPrefabs;
    List<Car> carInstances;
    void Start()
    {
        carPrefabs = new List<GameObject>() {convertible,pickuptruck,cybertruck,sedan};
        //making the below in for loop
        int randomInt = UnityEngine.Random.Range(0, 3);
        Car currentCar = new Car();

        int randomInt = UnityEngine.Random.Range(0, 3);
        Car currentCar = new Car();
        
    }

    void Update()
    {
        
    }
}
