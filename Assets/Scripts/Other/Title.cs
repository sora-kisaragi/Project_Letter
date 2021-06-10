using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public Text test;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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


}

