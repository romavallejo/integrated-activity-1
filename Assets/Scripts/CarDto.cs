[System.Serializable]
public class CarDTO
{
    public int destination_parking;
    public bool has_arrived;
    public int id;
    public int start_parking;
    public string state;
    public float x;
    public float y;
}

[System.Serializable]
public class InitializeCar
{
    public CarDTO[] cars;
}

[System.Serializable]
public class Step
{
    public CarDTO[] cars;
    public int step;
}

[System.Serializable]
public class Root
{
    public Step[] steps;
}
