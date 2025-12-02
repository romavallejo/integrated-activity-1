using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    public GameObject carScriptLogic;
    //PrebaCars
    public GameObject convertible;
    public GameObject pickuptruck;
    public GameObject cybertruck;
    public GameObject sedan;
    List<GameObject> carPrefabs;
    List<Car> carInstances;
    void Start()
    {
        //choose random car
        carPrefabs = new List<GameObject>() {convertible,pickuptruck,cybertruck,sedan};
        int randomInt = UnityEngine.Random.Range(0, 3);
        Car car = Instantiate(carScriptLogic).GetComponent<Car>();
        //so the innit position needs to be the car, then the next tree targets
        /*car.Initialize(new Vector3(2.5f,0.1f,1.5f),carPrefabs[1], new List<Vector3>()
        {
            new Vector3(2.5f,0.1f,0.5f),
            new Vector3(1.5f,0.1f,0.5f),
            new Vector3(0.5f,0.1f,0.5f),
            new Vector3(0.5f,0.1f,1.5f),
            new Vector3(-0.5f,0.1f,1.5f),
            new Vector3(-0.5f,0.1f,2.5f)
        });*/
        car.Initialize(new Vector3(2.5f,0.1f,1.5f),carPrefabs[1], new List<Vector3>()
        {
            new Vector3(2.5f,0.1f,0.5f),
            new Vector3(1.5f,0.1f,0.5f),
            new Vector3(0.5f,0.1f,0.5f),
            new Vector3(0.5f,0.1f,1.5f),
            new Vector3(0.5f,0.1f,2.5f),
            new Vector3(0.5f,0.1f,3.5f),
            new Vector3(0.5f,0.1f,4.5f),
            new Vector3(1.5f,0.1f,4.5f),
            new Vector3(9.5f,0.1f,4.5f),
            new Vector3(10.5f,0.1f,4.5f),
            new Vector3(10.5f,0.1f,5.5f),
            new Vector3(10.5f,0.1f,6.5f),
            new Vector3(10.5f,0.1f,7.5f),
            new Vector3(9.5f,0.1f,7.5f),
            new Vector3(8.5f,0.1f,7.5f),
            new Vector3(7.5f,0.1f,7.5f),
            new Vector3(7.5f,0.1f,8.5f),
            new Vector3(7.5f,0.1f,9.5f),
            new Vector3(7.5f,0.1f,10.5f),
            new Vector3(8.5f,0.1f,10.5f),
        });
    }

    void Update()
    {
        
    }
}
