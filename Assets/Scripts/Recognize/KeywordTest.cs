using System;
using System.Text;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityBoyomichanClient;

public class KeywordTest : MonoBehaviour {

    private KeywordController keyCon;
    private string[][] keywords;
    private BoyomichanClient boyomi;


    // Use this for initialization
    void Start () {
        keywords = new string[2][];
        keywords[0] = new string[] {"シンシアリ","sincerely"};//ひらがなでもカタカナでもいい
        keywords[1] = new string[] {"スタート","Start","起動","きどう"};

        keyCon = new KeywordController(keywords,true);//keywordControllerのインスタンスを作成
        keyCon.SetKeywords();//KeywordRecognizerにkeywordsを設定する
        keyCon.StartRecognizing(0);//シーン中で音声認識を始めたいときに呼び出す
        keyCon.StartRecognizing(1);

        boyomi = new BoyomichanClient("127.0.0.1", 50001);

        
    }

    // Update is called once per frame
    void Update () {
        if (keyCon.hasRecognized[0])//設定したKeywords[0]の単語らが認識されたらtrueになる
        {
            Debug.Log("keyword[0] was recognized");
            keyCon.hasRecognized[0] = false;
        }
        if (keyCon.hasRecognized[1])
        {
            Debug.Log("keyword[1] was recognized");
            keyCon.hasRecognized[1] = false;
        }
    }



}
