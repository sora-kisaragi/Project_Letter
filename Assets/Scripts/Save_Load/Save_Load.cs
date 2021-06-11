using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class Save_Load : MonoBehaviour
{

    [SerializeField] Button Button_Save;
    [SerializeField] Button Button_Load;

    [SerializeField] Button Button_Close;


    [SerializeField] GameObject input_Name;

    [SerializeField] GameObject temp_name;

    
    private string fileName = "/PlayerDate.csv";

    private string[] Player_Name = { "sora" };
    private string[] Start_Date = { "2021/05/16 00:00:00" };
    private string[] Save_Date = { "2021/05/16 00:00:00" };
    private float[] Communication_Time = { 0f };

    private float timeCounter = 0;
    public int Current_Player_ID = 0;

    private StreamReader csvFile;
    private StreamWriter streamWriter;

    public List<string[]> Player_Datas = new List<string[]>();// 格納用の二次元配列List

    // Start is called before the first frame update
    void Start()
    {
                
        #if UNITY_EDITOR
        fileName = "/PlayerDate_Editor.csv";
        #elif UNITY_IOS
        fileName = "/PlayerDate_ios.csv";
        #elif UNITY_ANDROID
        fileName = "/PlayerDate_android.csv";
        #elif UNITY_STANDALONE
        fileName = "/PlayerDate_Standallone.csv";
        #endif
        Button_Load.onClick.AddListener(() => Load_Data());
        Button_Close.onClick.AddListener(() => Close_Self());
        Button_Save.onClick.AddListener(() => Save_Data(input_Name.GetComponent<Text>().text));

    }

    // Update is called once per frame
    void Update()
    {
        timeCounter += Time.deltaTime;
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {

            Debug.Log("applicationWillResignActive or onPause");
        }
        else
        {
            Debug.Log("applicationDidBecomeActive or onResume");

        }
    }




    public void Load_Data()
    {

        try
        {
            using (csvFile = new StreamReader(Application.persistentDataPath + fileName))
            {
                // , で分割しつつ一行ずつ読み込み
                // リストに追加していく
                while (csvFile.Peek() != -1) // reader.Peekが-1になるまで
                {
                    string Temp_Datas = csvFile.ReadLine();// 一行ずつ読み込み
                    Player_Datas.Add(Temp_Datas.Split(',')); // , 区切りでリストに追加
                }

            }

            //Console.Write(text);

        }
        catch (Exception e)
        {
            Init_Data();
            Load_Data();
        }
        finally
        {
            if (csvFile != null)
                csvFile.Close();
        }
        SetPlayerData();
        temp_name.GetComponent<Text>().text = this.Player_Name[this.Current_Player_ID];
    }



    public void Save_Data(string name = "")
    {
        if (name != "")
        {
            this.Player_Name[this.Current_Player_ID] = name;
        }
        this.Communication_Time[this.Current_Player_ID] += timeCounter;
        this.timeCounter = 0f;


        DateTime now_Date = DateTime.Now;
        this.Save_Date[this.Current_Player_ID] = now_Date.ToString("yyyy/MM/dd HH:mm:ss");

        string new_words = Player_Name[this.Current_Player_ID] + ",";
        new_words += Start_Date[this.Current_Player_ID] + ",";
        new_words += Save_Date[this.Current_Player_ID] + ",";
        new_words += Communication_Time[this.Current_Player_ID].ToString() + ",";
        Debug.Log(new_words);

        try
        {
            using (streamWriter = new StreamWriter(Application.persistentDataPath + fileName, false))
            {
                streamWriter.WriteLine(new_words);
                Debug.Log("読み込めたぞ");
            }
        }
        catch  (Exception e)
        {
            Debug.Log("Error");
        }
        finally
        {
        streamWriter.Close();
        }


    }

    public void SetPlayerData()
    {
        
        this.Player_Name[this.Current_Player_ID] = this.Player_Datas[this.Current_Player_ID][0];
        this.Start_Date[this.Current_Player_ID] = this.Player_Datas[this.Current_Player_ID][1];
        this.Save_Date[this.Current_Player_ID] = this.Player_Datas[this.Current_Player_ID][2];
        this.Communication_Time[this.Current_Player_ID] = float.Parse(this.Player_Datas[this.Current_Player_ID][3]);
        Debug.Log(Player_Name[0]);
        Debug.Log(Player_Datas);
        Player_Datas.Clear();
        Debug.Log(Player_Datas);
    }

    private void Init_Data()
    {

        string new_words = "Sora" + ",";
        new_words += "2021/05/16 00:00:00" + ",";
        new_words += "2021/05/16 00:00:00" + ",";
        new_words += "0" + ",";

        StreamWriter streamWriter = new StreamWriter(Application.persistentDataPath + fileName, false);
        streamWriter.WriteLine(new_words);
        streamWriter.Flush();
    }

    private void Close_Self(){
        this.gameObject.SetActive(false);
    }

}
