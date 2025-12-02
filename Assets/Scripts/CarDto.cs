using System.Collections.Generic;
[System.Serializable]
public class Root
{
    public List<Step> steps;
}

[System.Serializable]
public class Step
{
    public List<CarDTO> cars;
    public int step;
    public float timestamp;
}

[System.Serializable]
public class CarDTO
{
    public int destination_parking;
    public bool has_arrived;
    public int id;
    public int start_parking;
    public string state;
    public int x;
    public int y;
}
