using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetTime : MonoBehaviour
{
    public Text currentTime;

    private float startTime;
    private int hours, minute;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // Set time
        float spendTime = (Time.time - startTime) / 60;
        minute = (int)(spendTime) + 30 + SceneControl.spendMinute;
        hours = (int)(minute / 60) + 15;
        minute %= 60;
        
        currentTime.text = hours.ToString() + " : " + minute.ToString();
    }
}
