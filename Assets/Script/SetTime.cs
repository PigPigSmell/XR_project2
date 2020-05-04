using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetTime : MonoBehaviour
{
    public Text currentTime;

    private float startTime;
    private int minute;

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
        
        currentTime.text = minute.ToString();

        if(minute <= 0)
        {
            c.setStep(7);
        }
    }
}
