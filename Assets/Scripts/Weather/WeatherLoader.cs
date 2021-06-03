using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class WeatherLoader : MonoBehaviour
{
    public string BaseUrl = "http://api.openweathermap.org/data/2.5/weather";
    
    //[SerializeField] string[] ApiKey; /// インスペクタで入力するタイプの場合
    private string[] ApiKey = {
            "3147af9d645977c4fbc600f847a6cf0a", /// Unity-Test
            "e906faea57f9c62a32fa79bd9300f331", /// Sincerely
            "53cf49a66978c3e9204c8b87d7fefcdc"  /// PostScript
        }; 
    public int TimeOutSeconds = 10;
    public int count = 0;

    public IEnumerator Load(float latitude, float longitude, UnityAction<WeatherEntity> callback)
    {
        var url = string.Format("{0}?units=metric&lat={1}&lon={2}&appid={3}", BaseUrl, latitude.ToString(), longitude.ToString(), ApiKey[0]);
        var request = UnityWebRequest.Get(url);
        var progress = request.Send();

        int waitSeconds = 0;
        count = ApiKey.Length;
        Debug.Log(ApiKey[0]);
        while (!progress.isDone)
        {
            yield return new WaitForSeconds(1.0f);
            waitSeconds++;
            if (waitSeconds >= TimeOutSeconds)
            {
                Debug.Log("timeout:" + url);
                yield break;
            }
        }

        if (request.isNetworkError)
        {
            Debug.Log("error:" + url);
        }
        else
        {
            string jsonText = request.downloadHandler.text;
            callback(JsonUtility.FromJson<WeatherEntity>(jsonText));
            yield break;
        }
    }
}