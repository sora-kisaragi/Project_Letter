using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeatherRenderer : MonoBehaviour
{
    public float latitude;
    public float longitude;
    public WeatherLoader Loader;

    public RealWorld_Location RW_location;
    public Text WeatherText;
    public Text TempCurrentText;


    public uint ThresholdSeconds = 60 * 60; // 1時間更新しない

    private DateTime lastUpdatedDateTime;

    void Start()
    {

        // コルーチンの起動
        StartCoroutine(DelayCoroutine(25, () =>
        {
            Debug.Log("25秒たちました");
            Get_WeatherData();
            // 時間を管理するシステムを改修する必要あり
            Invoke("Get_WeatherData", 60 * 60 * 3);
        }));

    }

    public void Get_WeatherData()
    {
        StartCoroutine(Loader.Load(RW_location.latitude, RW_location.longitude, weatherEntity =>
        {
            Render(weatherEntity);
        }));
    }

    void Render(WeatherEntity weatherEntity)
    {
        WeatherText.text = string.Format("{0}", weatherEntity.weather[0].main);
        TempCurrentText.text = string.Format("{0}℃", weatherEntity.main.temp.ToString("f1"));
        Debug.Log(weatherEntity.main.temp);
        Debug.Log(weatherEntity.sys.country);
        Debug.Log(weatherEntity.name);
    }

    // 華氏 [℉] から　摂氏 [℃] に変換
    private float Convert_to_Celsius(float fahrenheit)
    {
        float celsius = (fahrenheit - 32) / 1.8f;
        return celsius;
    }

    // 一定時間後に処理を呼び出すコルーチン
    private IEnumerator DelayCoroutine(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);
        action?.Invoke();
    }

}