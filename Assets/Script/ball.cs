using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ball : MonoBehaviour
{

    public VideoPlayer[] vs;
    public AudioSource[] audio;

    private int mode;
    private bool open;

    
    // Start is called before the first frame update
    void Start()
    {
        StopAll();
        mode = 0;
        open = true;

    }

    // Update is called once per frame
    void Update()
    {

        for (int i=0; i < vs.Length; i++)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                StopAll();
                vs[i].Play();
                break;
            }
        }
        if(mode > 2)
        {
            mode = 0;
        }
    }

    private void StopAll()
    {
        for(int i=0; i<vs.Length; i++)
        {
            if (vs[i].isPlaying)
            {
                vs[i].Stop();
                audio[i].Pause();
            }
        }
    }

    public void pressButonA()
    {
        StopAll();
        open = false;
    }
    public void pressButonB()
    {
        StopAll();
        open = false;
    }
    public void backTo360()
    {
        mode++;
        vs[mode].Play();
        audio[mode].Play();
        open = true;
    }
}
