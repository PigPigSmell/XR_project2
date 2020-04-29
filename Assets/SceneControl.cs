using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

using LitJson;

public class SceneControl : MonoBehaviour
{
    public Button A;
    public Button B;
    public Text story;
    public VideoPlayer video;
    private int v;

    private int mode;
    //private StringBuilder story_data = new StringBuilder(); // Store the data read from Json
    private string story_line = "Assets/story/story_line.json";
    private string path = "Assets/story/";

    private JsonData data; // About the story line

    client c;

    // Start is called before the first frame update
    void Start()
    {
        // Store Story Line
        StringBuilder story_data = new StringBuilder(); // Store the data read from Json
        using (StreamReader sr = new StreamReader(story_line))
        {
            story_data.Append(sr.ReadToEnd());// read all data
        }
        data = JsonMapper.ToObject(story_data.ToString());

        // Initialize
        mode = 0;
        ShowInformation();

        c = GameObject.Find("Manager").GetComponent<client>();
        v = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //ShowInformation();
        string str = c.getGo();
        string[] goNext = str.Split(':');
        if(goNext[0] == "true")
        {
            if(goNext[1] == "A")
            {
                PressA();
            }
            else if (goNext[1] == "B")
            {
                PressB();
            }
            c.setGo("false:");
        }
    }

    public void PressA()
    {
        mode = int.Parse((string)(data[mode]["option"][0]));
        ShowInformation();
    }
    public void PressB()
    {
        mode = int.Parse((string)(data[mode]["option"][1]));
        ShowInformation();
    }

    private void ShowInformation()
    {
        // Store Story
        string filePath = path + "story" + mode.ToString() + ".json";
        StringBuilder story_data = new StringBuilder(); // Store the data read from Json
        using (StreamReader sr = new StreamReader(filePath))
        {
            story_data.Append(sr.ReadToEnd());// read all data
        }
        JsonData story_i = JsonMapper.ToObject(story_data.ToString());
        
        // story description
        story.text = (string)(story_i["description"]);

        // option description
        if((string)(data[mode]["option"][0]) == "-1")
        {
            A.gameObject.SetActive(false);
            B.gameObject.SetActive(false);
        }
        else
        {
            A.GetComponentInChildren<Text>().text = (string)(story_i["option"][0]["name"]) + " : " + (string)(story_i["option"][0]["description"]);
            if(story_i["option"].Count > 1)
            {
                B.GetComponentInChildren<Text>().text = (string)(story_i["option"][1]["name"]) + " : " + (string)(story_i["option"][1]["description"]);
            }
            else
            {
                //B.gameObject.SetActive(false);
                B.GetComponentInChildren<Text>().text = "NULL";
            }
        }

        // video setting
        video.url = "Media/360/" + data[mode]["video"];
        //video.url = "Media/360/" + v.ToString() + ".MP4";
        //v++;
        //v %= 2;

        // audio setting

    }

    private void EndGame()
    {
        story.text = "End";
        A.gameObject.SetActive(false);
        B.gameObject.SetActive(false);
    }
}
