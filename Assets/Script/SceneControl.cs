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
    public VideoClip[] videoclips = new VideoClip[30];

    static public int miss;
    static public int spendMinute;

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
        spendMinute = 0;
        miss = 0;
        ShowInformation();

        c = GameObject.Find("Manager").GetComponent<client>();
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
        spendMinute += int.Parse((string)(data[mode]["path_cost"][0]));
        mode = Mathf.Abs(int.Parse((string)(data[mode]["option"][0])));
        ShowInformation();
    }
    public void PressB()
    {
        spendMinute += int.Parse((string)(data[mode]["path_cost"][1]));
        mode = Mathf.Abs(int.Parse((string)(data[mode]["option"][1])));
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

        // video setting
        //video.url = "file://E:/tmp/" + data[mode]["video"];
        
        //video.url = "Media/360/0.MP4";
        int i = int.Parse((string)data[mode]["video"]);
        video.clip = videoclips[i];
        
        // audio setting

        // option description
        miss = 0;
        if (int.Parse((string)(data[mode]["option"][0])) == 0) // win
        {
            miss = 2; // end
            A.gameObject.SetActive(false);
            B.gameObject.SetActive(false);
        }
        else
        {
            char signA = data[mode]["option"][0].ToString()[0];
            char signB = data[mode]["option"][1].ToString()[0];

            if(signA =='+' || signA == '-')
            {
                ShowButton(story_i, 0, signA);
                ShowButton(story_i, 1, signB);
            }
            else
            {
                miss = 1;
                //PressA();
            }
        }
    }

    private void ShowButton(JsonData story_i, int i, char sign)
    {
        string str = "";
        if (sign == '+')
        {
            // positive option
            str = "(70%) ";
        }
        else if (sign == '-')
        {
            // negative option
            str = "(30%) ";
        }

        if(i == 0)
        {
            A.GetComponentInChildren<Text>().text = str + (string)(story_i["option"][0]["name"]) + " : " + (string)(story_i["option"][0]["description"]);
        }
        else if (i == 1)
        {
            B.GetComponentInChildren<Text>().text = str + (string)(story_i["option"][1]["name"]) + " : " + (string)(story_i["option"][1]["description"]);
        }
    }

    private void EndGame()
    {
        story.text = "End";
        A.gameObject.SetActive(false);
        B.gameObject.SetActive(false);
    }
}
