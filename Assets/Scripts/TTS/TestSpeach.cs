using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using FantomLib;
using UnityEngine;


public class TestSpeach : MonoBehaviour
{

    public TextToSpeechController textToSpeechControl;
    [SerializeField] Button UI_Button;
    // Start is called before the first frame update
    void Start()
    {
        UI_Button.onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnClick()
    {
        Debug.Log("しゃべるよ");
        textToSpeechControl.StartSpeech("おはようございます。いい天気ですね");

    }
}
