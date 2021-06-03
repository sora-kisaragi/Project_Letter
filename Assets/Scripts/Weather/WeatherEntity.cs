/// <summary>
///     https://openweathermap.org/current#current_JSON
///     https://qiita.com/lycoris102/items/a6ddc468575b624b2630#entityを作成する
///</summary>
///参考サイト
using UnityEngine;

[System.Serializable]
public class WeatherEntity
{
    public Weather[] weather;
    public Main main;
    public Wind wind;

    public Sys sys;

    public string name;

}

[System.Serializable]
public class Weather
{
    public string main; // Rain, Snow, Clouds ... etc
    
}
[System.Serializable]
public class Main
{
    public float temp;
    public float temp_max;

    public float temp_min;
}

[System.Serializable]
public class Wind
{
    public float deg;
    public float speed;
}

[System.Serializable]
public class Sys
{
    public string country;
}


