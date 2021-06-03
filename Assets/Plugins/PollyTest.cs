using System.IO;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Amazon.Polly;
using Amazon.Polly.Model;

public class PollyTest : MonoBehaviour
{
    public VoiceType voiceType;
    public string text;
    private AmazonPollyClient client;
    private AudioSource audioSource;
    private List<FieldInfo> voiceList = new List<FieldInfo>();

    private string fileName = "voice.ogg";

    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        string voiceNames = "";
        var list = typeof(VoiceId).GetFields();
        foreach (FieldInfo prop in list)
        {
            voiceNames += prop.Name + ",";
            voiceList.Add(prop);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Speak());
        }
    }

    private IEnumerator Speak()
    {
        string key = "AKIA2DS5TZM5KW4A3QVG";
        string s_key = "7+gOReWvAiZG28yHrl6ErzM+g1nhPt1j5kNX72BA";
        
        client = new AmazonPollyClient(key, s_key, Amazon.RegionEndpoint.USEast1);
        SynthesizeSpeechRequest sreq = new SynthesizeSpeechRequest();
        sreq.Text = text;
        sreq.OutputFormat = OutputFormat.Ogg_vorbis;
        sreq.VoiceId = voiceList[(int)voiceType].GetValue(null) as VoiceId;
        SynthesizeSpeechResponse sres = client.SynthesizeSpeech(sreq);

        //save voice
        using (var fileStream = File.Create(Application.persistentDataPath + "/" + fileName))
        {
            sres.AudioStream.CopyTo(fileStream);
            fileStream.Flush();
            fileStream.Close();
        }
        //play voice
        using (WWW www = new WWW("file:///" + Application.persistentDataPath + "/" + fileName))
        {
            yield return www;
            audioSource.clip = www.GetAudioClip(false, true, AudioType.OGGVORBIS);
            audioSource.Play();
        }
    }
}

public enum VoiceType
{
    Aditi, Amy, Astrid, Bianca, Brian, Camila, Carla, Carmen, Celine, Chantal, Conchita, Cristiano, Dora, Emma, Enrique, Ewa, Filiz, Geraint, Giorgio, Gwyneth, Hans, Ines, Ivy, Jacek, Jan, Joanna, Joey, Justin, Karl, Kendra, Kimberly, Lea, Liv, Lotte, Lucia, Lupe, Mads, Maja, Marlene, Mathieu, Matthew, Maxim, Mia, Miguel, Mizuki, Naja, Nicole, Penelope, Raveena, Ricardo, Ruben, Russell, Salli, Seoyeon, Takumi, Tatyana, Vicki, Vitoria, Zeina, Zhiyu
}