/// 参考URL
/// https://qiita.com/hirano/items/dde92f4ed76fb377746e
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RealWorld_Location : MonoBehaviour
{

    public static RealWorld_Location Instance { set; get; }

    public float latitude = 35.67896f;
    public float longitude = 139.7405f;
    public float altitude;




    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        StartCoroutine(StartLocationService());
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GetLocation(){
         StartCoroutine(StartLocationService());
    }

    

    private IEnumerator StartLocationService()
    {
        // 位置情報取得の許可が有効でない場合、終了。
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("GPS not enabled");
            yield break;
        }

        // ロケーションサービス(GPSの取得)を開始する
        Input.location.Start();

        // 初期化終了までの猶予時間
        int Wait_Time = 20;
        // 初期化終了 or Wait_Timeが過ぎるまで待つ
        while ((Input.location.status == LocationServiceStatus.Initializing) && (Wait_Time > 0))
        {
            // 1秒待つ
            yield return new WaitForSeconds(1);
            Wait_Time--;
        }

        if (Wait_Time < 1)
        {
            Debug.Log("GPS is Time out");
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location");
            yield break;
        }
        else
        {
            // アクセスの許可と位置情報の取得に成功
            Debug.Log("Location: " + Input.location.lastData.latitude + " "
                               + Input.location.lastData.longitude + " "
                               + Input.location.lastData.altitude + " "
                               + Input.location.lastData.horizontalAccuracy + " "
                               + Input.location.lastData.timestamp);
        }

        while (true)
        {
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;
            altitude = Input.location.lastData.altitude;
            Debug.Log("実行されましyた");
            yield return new WaitForSeconds(30);
        }



    }


}
