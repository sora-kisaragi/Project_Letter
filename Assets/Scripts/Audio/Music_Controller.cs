using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Music_Controller : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource source;
    public Button Button_Pause;

    public Slider Music_Time;

    public GameObject Music_Name_Object;

    private Text Music_Name;

    [SerializeField] Image Music_icon;
    [SerializeField] Sprite[] Music_icon_Textures;

    bool isPlay_Music = true;
    void Start()
    {
        source = this.gameObject.GetComponent<AudioSource>();
        Button_Pause.onClick.AddListener(pause_Audio);
        Music_Name = Music_Name_Object.GetComponent<Text>();
        source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        Music_Time.value = source.time / source.clip.length;
        Music_Name.text = source.name;
    }

    private void pause_Audio()
    {
        if (isPlay_Music == true)
        {
            source.Pause();
            isPlay_Music = !isPlay_Music;
            Music_icon.sprite = Music_icon_Textures[1];

        }
        else
        {
            source.UnPause();
            isPlay_Music = !isPlay_Music;
            Music_icon.sprite = Music_icon_Textures[0];
        }


    }
}
