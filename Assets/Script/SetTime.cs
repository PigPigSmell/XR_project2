﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetTime : MonoBehaviour
{
    public TextMeshProUGUI currentTime;

    private float startTime;
    static public int minute;

    client c;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        c = GameObject.Find("Manager").GetComponent<client>();
    }

    // Update is called once per frame
    void Update()
    {
        // Set time
        float spendTime = (Time.time - startTime) / 60;
        minute = 90 - ((int)(spendTime) + SceneControl.spendMinute);
        
        currentTime.text = "剩下時間: " + minute.ToString() + " min";

        if(minute <= 0)
        {
            c.setStep(7);
        }
    }
}
