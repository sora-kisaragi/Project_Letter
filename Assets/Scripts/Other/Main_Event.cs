using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoodEnough.TextToSpeech;
using FantomLib;
using System.Net.Http;
using System.Net.Http.Headers;
using Hanachiru.News;
using UnityEngine.SceneManagement;

public class Main_Event : MonoBehaviour
{
    // Start is called before the first frame update
    public TextToSpeechController textToSpeechControl;
    [SerializeField] Check_TimeState T_State;
    [SerializeField] GameObject Char;
    private bool isMove = false;
    private bool CamBack = false;
    [SerializeField] GameObject AI_manager;

    [SerializeField] GameObject UI_NewWord_Panel;

    [SerializeField] Button UI_Button_Input_NewWord;

    [SerializeField] Button UI_Button_Close_NewWord;

    [SerializeField] Text UI_NewWord_Text;
    [SerializeField] Text UI_ReturnWord_Text;

    private Load_TextData textDatas;
    [SerializeField] GameObject UI_SettingPanel;
    [SerializeField] GameObject UI_AI_Text;
    [SerializeField] GameObject UI_InputText;

    [SerializeField] GameObject UI_InputArea;
    [SerializeField] GameObject UI_AI_Text_Area;
    [SerializeField] Button UI_OpenInput_Button;
    [SerializeField] Button UI_SendButton;

    [SerializeField] Button UI_SetingButton;

    [SerializeField] Button UI_Setting_Close_Button;

    [SerializeField] GameObject UI_News_Text;
    [SerializeField] GameObject UI_Clock_Text;
    [SerializeField] GameObject UI_Date_Text;

    [SerializeField] GameObject UI_Set_Name;
    [SerializeField] Save_Load saveload_data;

    private Text AI_text;
    private Text Input_Text;

    [SerializeField] Button Button_News_Open;
    private Text News_Text;
    private Text Date_Text;
    private Text Clock_Text;
    private Text Temperature_Text;
    private String[] text_list;

    private string news_link;

    [SerializeField] GameObject UI_Voice_SettingPanel;

    void Start()
    {
        AI_text = UI_AI_Text.GetComponent<Text>();
        Input_Text = UI_InputText.GetComponent<Text>();
        News_Text = UI_News_Text.GetComponent<Text>();
        Clock_Text = UI_Clock_Text.GetComponent<Text>();
        Date_Text = UI_Date_Text.GetComponent<Text>();
        UI_SendButton.onClick.AddListener(OnClickButton);
        UI_SetingButton.onClick.AddListener(OnSettingsButton);
        UI_Setting_Close_Button.onClick.AddListener(Close_Setting_Panel);
        UI_Button_Input_NewWord.onClick.AddListener(InputNewWord);
        UI_Button_Close_NewWord.onClick.AddListener(CloseNewWord);
        UI_OpenInput_Button.onClick.AddListener(Open_InputArea);
        Button_News_Open.onClick.AddListener(Open_News);
        textDatas = AI_manager.GetComponent<Load_TextData>();

        var SpeechParm = new SpeechUtteranceParameters();


#if UNITY_ANDROID
        if (System.IO.File.Exists(Application.persistentDataPath + "/PlayerDate_Android.csv"))
        {
            Debug.Log(Application.persistentDataPath + "/PlayerDate_Android.csv");
            Debug.Log("?????????");
        }
        else
        {
            Debug.Log("????????????Android");
            UI_Set_Name.SetActive(true);
            saveload_data.Save_Data();
        }
        // ??????????????????????????????
        LocalPushNotification.AllClear();
        // ????????????????????????
        //LocalPushNotification.RegisterChannel("channelId", "?????????????????????????????????)", "??????");
        // ???????????????????????????
        //LocalPushNotification.AddSchedule("??????????????????????????????", "??????", 1, 10, "channelId");
        
#elif UNITY_IOS
        if (System.IO.File.Exists(Application.persistentDataPath + "/PlayerDate_ios.csv"))
        {
            Debug.Log(Application.persistentDataPath + "/PlayerDate_ios.csv");
            Debug.Log("?????????");
        }
        else
        {
            Debug.Log("????????? IOS");
            UI_Set_Name.SetActive(true);
            saveload_data.Save_Data();
        }
        // ??????????????????????????????
        LocalPushNotification.AllClear();
        // ???????????????????????????
        //LocalPushNotification.AddSchedule("??????????????????????????????", "??????", 1, 10, "channelId");

#elif UNITY_STANDALONE
        if (System.IO.File.Exists(Application.persistentDataPath + "/PlayerDate_Standalone.csv"))
        {
            Debug.Log(Application.persistentDataPath + "/PlayerDate_Standalone.csv");
            Debug.Log("?????????");
        }
        else
        {
            Debug.Log("????????? Standalone");
            UI_Set_Name.SetActive(true);
            saveload_data.Save_Data();
        }

#elif UNITY_EDITOR
        if (System.IO.File.Exists(Application.persistentDataPath + "/PlayerDate_Editor.csv"))
        {
            Debug.Log(Application.persistentDataPath + "/PlayerDate_Editor.csv");
            Debug.Log("?????????");
        }
        else
        {
            Debug.Log("????????? EDITOR");
            UI_Set_Name.SetActive(true);
            saveload_data.Save_Data();
        }
#endif

        getNews();




    }

    // Update is called once per frame
    void Update()
    {

        MoveChar(isMove, CamBack);
        Set_Text_Time();

        if (Input.touchCount > 2)
        {
            Touch touch = Input.GetTouch(3);
            if (touch.phase == TouchPhase.Began)
            {

            }
            else if (touch.phase == TouchPhase.Moved)
            {
                // ???????????????
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                // if (SceneManager.GetActiveScene().name == "Main_ios")
                // {
                //     SceneManager.LoadScene("Main_Android");
                // }
                // else if (SceneManager.GetActiveScene().name == "Main_Android")
                // {
                //     SceneManager.LoadScene("Main_ios");
                // }
            }
        }

    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            Debug.Log("BackGroud??????");
        }
        else
        {
            getNews();
        }
    }


    public void OnClickButton()
    {
        Debug.Log("?????????????????????");

        return_talk();
    }

    public void OnSettingsButton()
    {

        UI_SettingPanel.SetActive(true);

    }

    public void InputNewWord()
    {

        textDatas.Save_NewWords(UI_NewWord_Text.text, UI_ReturnWord_Text.text);
        UI_NewWord_Panel.SetActive(false);
    }

    public void CloseNewWord()
    {
        UI_NewWord_Panel.SetActive(false);
    }
    public void Close_Setting_Panel()
    {
        UI_SettingPanel.SetActive(false);
    }

    public void Open_InputArea()
    {
        UI_InputArea.SetActive(!UI_InputArea.activeInHierarchy);
    }

    private void Set_Text_Time()
    {
        var dt = new DateTime();
        dt = DateTime.Now;
        Clock_Text.text = dt.ToString("HH:mm");
        Date_Text.text = dt.ToString("MM???d???");
    }

    public void return_talk()
    {

        bool isNewWord = false;
        Debug.Log("ReturnTalk,???????????????");
        foreach (var word_data in textDatas.Word_Datas)
        {
            Debug.Log("?????????????????????");
            isNewWord = false;
            if (Return_NowTime())
            {
                break;
            }
            if (input_greeting(word_data))
            {
                Debug.Log("????????????????????????");
                break;
            }
            if (Input_Text.text.Contains("????????????")){
                UI_Voice_SettingPanel.SetActive(true);
                break;
            }
            if (Input_Text.text.Contains(word_data[1]))
            {
                AI_text.text = word_data[4];
                AI_Speak(word_data[4]);
                isNewWord = false;
                Debug.Log(word_data[4]);
                break;
            }
            else if (Input_Text.text.Contains("?????????"))
            {
                /// ????????????
                SetMove(true, false);

            }
            else if (Input_Text.text.Contains("?????????"))
            {
                /// ????????????
                SetMove(true, true);
            }
            else
            {
                Debug.Log("????????????");
                isNewWord = true;
            }
        }
        if (isNewWord == true)
        {
            UI_NewWord_Panel.SetActive(true);
            UI_NewWord_Text.text = Input_Text.text;
            Debug.Log("?????????????????????");
            AI_text.text = "???????????????????????????????????????????????????????????????????????????????????????";
            AI_Speak(AI_text.text);
        }



    }

    ///<summary>
    /// ????????????????????????????????????????????????foreach???????????????????????? bool????????????
    ///</summary>
    private bool Return_NowTime()
    {

        if (Input_Text.text.Contains("??????"))
        {
            var dt = new DateTime();
            dt = DateTime.Now;
            AI_text.text = dt.ToString("HH???mm?????????");

            AI_Speak(AI_text.text);
            PublishMessage(AI_text.text);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Category = greeting ???????????????????????????????????????
    /// 1. ?????????????????????????????????
    /// 2. ?????? ??? ??????????????????????????????
    /// </summary>
    private bool input_greeting(string[] words)
    {
        Debug.Log("????????????????????????");
        if (Input_Text.text.Contains(words[1]))
        {
            if (words[3] == "greeting")
            {
                string temp_greeting_word = "";
                switch (T_State.getTimeState_to_int())
                {
                    case 0:
                        temp_greeting_word = "???????????????????????????????????????????????????";
                        break;
                    case 1:
                        temp_greeting_word = "???????????????, ????????????????????????";
                        break;
                    case 2:
                        temp_greeting_word = "???????????????, ??????????????????????????????????????????";
                        break;
                    case 3:
                        temp_greeting_word = "????????????????????????????????????????????????.";
                        break;
                    case 4:
                        temp_greeting_word = "?????????????????????,???????????????????????????";
                        break;
                }

                AI_text.text = temp_greeting_word;
                AI_Speak(temp_greeting_word);

                return true;
            }
        }
        return false;
    }

    private bool is_WordsCheck_Array(string[] Words_array)
    {
        return Input_Text.text.Contains(Words_array[1]);
    }



    private void NewWord_Input(bool isInput)
    {
        if (isInput == true)
        {
            //??????????????????????????????UI????????????
        }
        else
        {
            //??????????????????????????????
        }
    }


    //??????????????????
    private static async void PublishMessage(string message)
    {
        // ????????????????????????????????????
        /// iPhone
        ///
        /// Android

        var ACCESS_TOKEN = "U4QtSFkwi9iKruvcqgbvqelDxxBcTRJuDNbpe4wOOgF";
#if UNITY_EDITOR
        ACCESS_TOKEN = "U4QtSFkwi9iKruvcqgbvqelDxxBcTRJuDNbpe4wOOgF";
#elif UNITY_STANDALONE
        ACCESS_TOKEN = "U4QtSFkwi9iKruvcqgbvqelDxxBcTRJuDNbpe4wOOgF";
#elif UNITY_ANDROID
        ACCESS_TOKEN = "iQu5aHuLKgfdH8cnexqtjdCKFATvubqjeiuOO8QgUyM";
#elif UNITY_IOS
        ACCESS_TOKEN = "PHyjayJIPYIb2lhuPHbTq1lqX1i4kgQoh3v10U5V7dn";
#endif

        using (var client = new HttpClient())
        {
            // ???????????????????????????
            var content = new FormUrlEncodedContent(new Dictionary<string, string> { { "message", message } });

            // ????????????????????????????????????????????????
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ACCESS_TOKEN);

            // ??????
            var result = await client.PostAsync("https://notify-api.line.me/api/notify", content);
        }
    }

    /// NmyOgXdZgmWGP2iZMJGdYtkMltGHWNz5ZSnllwo1LRb
    private void MoveChar(bool isMove, bool camBack)
    {
        float distance = 0.5f;
        if (isMove)
        {
            if (camBack && (Char.transform.position.x >= 0))
            {
                Vector3 direction = new Vector3(1f, 0f, 10f);
                float speed = -1f;
                float step = speed * Time.deltaTime;
                Char.transform.position = Vector3.MoveTowards(Char.transform.position, direction, step);
            }
            else if (!camBack && (Char.transform.position.x <= distance))
            {
                Vector3 direction = new Vector3(1f, 0f, 10f);
                float speed = 1f;
                float step = speed * Time.deltaTime;
                Char.transform.position = Vector3.MoveTowards(Char.transform.position, direction, step);
            }

        }
    }

    private void SetMove(bool move, bool back)
    {
        isMove = move;
        CamBack = back;
    }


    private void AI_Speak(string speak_word)
    {
        Debug.Log("Speak ???????????????");
#if UNITY_EDITOR

#elif UNITY_ANDROID
    
        textToSpeechControl.StartSpeech(speak_word);
                
#elif UNITY_IOS
        TTS.Speak(speak_word);
    
#endif
        Debug.Log("?????????");
        AI_Speak_OpenFrame();

    }

    private void AI_Speak_OpenFrame()
    {
        Debug.Log("AI??????????????????");
        //??????
        UI_AI_Text_Area.SetActive(true);

        Debug.Log("??????????????????????????????");
        // x????????? AI_Talk????????????????????????
        Invoke(nameof(Set_AI_Talk_Frame_Active), 30f);
        Debug.Log("??????????????????????????????");

    }

    private void Set_AI_Talk_Frame_Active()
    {
        Debug.Log("??????????????????????????????");
        UI_AI_Text_Area.SetActive(false);
    }

    private void getNews()
    {
        string[] searchWord = { "AI", "Game", "????????????", "????????????" };
        int Number = UnityEngine.Random.Range(0, 4);
        List<string> originalValues = new List<string> { searchWord[Number] };
        IReadOnlyCollection<string> keywords = originalValues;
        StartCoroutine(NewsReader.ReadNews(news =>
        {
            Debug.Log("title: " + news.Title);
            Debug.Log("description: " + news.Description);
            Debug.Log("link: " + news.Link);
            News_Text.text = news.Title;
            news_link = news.Link;
        }, keywords));
        Invoke(nameof(getNews), 60f);
    }

    private void Open_News()
    {
        Application.OpenURL(news_link);
    }


}
