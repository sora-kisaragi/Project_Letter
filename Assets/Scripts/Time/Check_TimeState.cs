using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Check_TimeState : MonoBehaviour
{



    private TimeSpan ts_morning = new TimeSpan(5, 0, 0);
    private TimeSpan ts_afternoon = new TimeSpan(10, 0, 0);
    private TimeSpan ts_start_evening = new TimeSpan(16, 0, 0);
    private TimeSpan ts_end_evening = new TimeSpan(18,0,0);
    private TimeSpan ts_goodnight = new TimeSpan(24, 0, 0);
    private TimeSpan ts_DeepNight = new TimeSpan(0, 0, 0);

    public enum e_TimeState
    {
        morning,
        afternoon,
        evening,
        goodnight,
        deepnight,
    }
    private e_TimeState TimeState = 0;




    // Start is called before the first frame update
    void Start()
    {
        Set_TimeState();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Set_TimeState()
    {
        // 現在の日時を取得します
        DateTime dt_now = System.DateTime.Now;
        // 時刻を取得します
        TimeSpan ts_now = dt_now.TimeOfDay;

        if ((ts_now > ts_morning) && (ts_now <= ts_afternoon))
        {
            TimeState = e_TimeState.morning;
        }
        else if ((ts_now > ts_afternoon) && (ts_now <= ts_start_evening))
        {
            TimeState = e_TimeState.afternoon;
        }
        else if ((ts_now > ts_start_evening) && (ts_now <= ts_end_evening))
        {
            TimeState = e_TimeState.goodnight;
        }
        else if((ts_now > ts_end_evening) && (ts_now <= ts_goodnight))
        {
            TimeState = e_TimeState.goodnight;
        }
        else if ((ts_now > ts_DeepNight) && (ts_now <= ts_morning))
        {   
            TimeState = e_TimeState.deepnight;
        }
    }

    public int getTimeState_to_int()
    {
        return (int)this.TimeState;
    }


}
