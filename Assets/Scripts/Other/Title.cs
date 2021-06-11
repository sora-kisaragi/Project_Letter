using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class Title : MonoBehaviour
{
    public Text test;
    private KeywordController keyCon;
    private string[][] keywords;

    private bool isStartFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        keywords = new string[2][];
        keywords[0] = new string[] { "シンシアリ", "sincerely" };//ひらがなでもカタカナでもいい
        keywords[1] = new string[] { "スタート", "Start", "起動", "きどう" };

        keyCon = new KeywordController(keywords, true);//keywordControllerのインスタンスを作成
        keyCon.SetKeywords();//KeywordRecognizerにkeywordsを設定する
        keyCon.StartRecognizing(0);//シーン中で音声認識を始めたいときに呼び出す
        keyCon.StartRecognizing(1);

    }

    // Update is called once per frame
    void Update()
    {

        CheckVoice();

        if (Input.GetMouseButton(0))
        {
            test.text = "Go to Next Scene";
            SceneManager.LoadScene("Main_ios");
            Debug.Log("クリックされたよ");
        }
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
#if UNITY_IOS
                    SceneManager.LoadScene("Main_ios");
#elif UNITY_ANDROID
                    SceneManager.LoadScene("Main_Android");
#elif UNITY_EDITOR

#endif
            }
        }

    }

    void CheckVoice()
    {
        if (keyCon.hasRecognized[0])//設定したKeywords[0]の単語らが認識されたらtrueになる
        {
            Debug.Log("keyword[0] was recognized");
            keyCon.hasRecognized[0] = false;
            isStartFlag = true;
        }
        if (keyCon.hasRecognized[1])
        {
            Debug.Log("keyword[1] was recognized");
            keyCon.hasRecognized[1] = false;
            if (isStartFlag)
            {
                test.text = "Go to Next Scene";
                SceneManager.LoadScene("Main_ios");
            }
        }
    }


}

