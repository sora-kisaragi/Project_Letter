using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load_TextData : MonoBehaviour
{

    // csvファイル名
    private string fileName = "/ai_text_dictionary.csv";
    public List<string[]> Word_Datas = new List<string[]>();// 格納用の二次元配列List


    // Start is called before the first frame update
    void Start()
    {

        FileInfo fi = new FileInfo(Application.persistentDataPath + "/test.txt");
        StreamWriter streamWriter = fi.AppendText();
        streamWriter.WriteLine("test");
        streamWriter.Flush();

        Load_Words(false);
        Debug.Log(Word_Datas[1][1]);

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 再読み込み時 is_reload = true
    ///            new_word  = 追加する単語のcsv
    /// 初回読み込み is_reload = false
    /// </summary>
    public void Load_Words(bool is_reload, string new_word = "default")
    {
        if (is_reload == true)
        {
            Word_Datas.Add(new_word.Split(',')); // , 区切りでリストに追加
        }
        else ///初回読み込みなど
        {
            // Resourcesのcsvフォルダ内のcsvファイルをTextAssetとして取得
            var csvFile = new StreamReader(Application.persistentDataPath + fileName);

            // , で分割しつつ一行ずつ読み込み
            // リストに追加していく
            while (csvFile.Peek() != -1) // reader.Peekが-1になるまで
            {
                string line = csvFile.ReadLine(); // 一行ずつ読み込み
                Word_Datas.Add(line.Split(',')); // , 区切りでリストに追加
            }
        }

    }
    public void Save_NewWords(string hit_words, string return_words, string category = "Default Category")
    {
        //string new_words;//"0,そら,false,name,優空,"
        string new_words = Word_Datas.Count.ToString() + ",";
        new_words += hit_words + ",";
        new_words += "false" + ",";
        new_words += category + ",";
        new_words += return_words + ",";
        
        FileInfo fi = new FileInfo(Application.persistentDataPath + fileName);
        StreamWriter streamWriter = fi.AppendText();
        streamWriter.WriteLine(new_words);
        streamWriter.Flush();

        Load_Words(true, new_words);

    }
}
