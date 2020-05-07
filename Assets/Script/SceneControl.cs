using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using LitJson;

public class SceneControl : MonoBehaviour
{
    public Button A;
    public Button B;
    public TextMeshProUGUI story;
    public VideoPlayer video;
    public VideoClip[] videoclips = new VideoClip[28];

    // Check List
    public TextMeshProUGUI teacher;
    public TextMeshProUGUI department;
    public TextMeshProUGUI office;

    public GameObject environment;


    //private StringBuilder story_data = new StringBuilder(); // Store the data read from Json
    private string story_line = "Assets/story/story_line.json";
    private string path = "Assets/story/";

    private JsonData data; // About the story line

    // Initialize
    static public int mode;
    static public int miss;
    static public int spendMinute;
    // Check List
    static public string check_teacher;
    static public string check_department;
    static public string check_office;

    static public int GameIdx;
    
    client c;

    JsonData story_i;

    // Start is called before the first frame update
    void Start()
    {
        c = GameObject.Find("Manager").GetComponent<client>();

        // Store Story Line
        StringBuilder story_data = new StringBuilder(); // Store the data read from Json
        using (StreamReader sr = new StreamReader(story_line))
        {
            story_data.Append(sr.ReadToEnd());// read all data
        }
        data = JsonMapper.ToObject(story_data.ToString());
        

        // Initialize
        mode = ToJson.Data.mode;
        spendMinute = ToJson.Data.spendMinute;
        miss = ToJson.Data.miss;

        check_teacher = ToJson.Data.check_teacher;
        check_department = ToJson.Data.check_department;
        check_office = ToJson.Data.check_office;

        ShowInformation();
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
        story_i = JsonMapper.ToObject(story_data.ToString());
        

        // story description
        story.text = (string)(story_i["description"]);
        
        int i = int.Parse((string)data[mode]["video"]);
        video.clip = videoclips[i];
        
        // audio setting

        // option description
        miss = 0;
        if (int.Parse((string)(data[mode]["option"][0])) == 0) // win
        {
            if((string)(string)(data[mode]["option"][0]) == "+0")
            {
                miss = 2; // win
            }else if((string)(string)(data[mode]["option"][0]) == "-0")
            {
                miss = 3; // lose
            }
            else
            {
                miss = 4; //special
            }
            // end
            A.gameObject.SetActive(false);
            B.gameObject.SetActive(false);
        }
        else
        {
            char signA = data[mode]["option"][0].ToString()[0];
            char signB = data[mode]["option"][1].ToString()[0];

            if(signA =='+' || signA == '-')
            {
                ShowButton(0, signA);
                ShowButton(1, signB);
            }
            else
            {
                miss = 1;
            }
        }

        teacher.text = check_teacher;
        department.text = check_department;
        office.text = check_office;

        GameIdx = (int)(data[mode]["game_env"]);
    }

    private void ShowButton(int i, char sign)
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
            A.GetComponentInChildren<TextMeshProUGUI>().text = str + (string)(story_i["option"][0]["name"]) + " : " + (string)(story_i["option"][0]["description"]);
        }
        else if (i == 1)
        {
            B.GetComponentInChildren<TextMeshProUGUI>().text = str + (string)(story_i["option"][1]["name"]) + " : " + (string)(story_i["option"][1]["description"]);
        }
    }

    public void CheckList()
    {
        int listLen = data[mode]["check_list"].Count;
        Debug.Log("listLen " + listLen);

        for(int i=0; i < listLen; i++)
        {
            if((int)(data[mode]["check_list"][i]) == 0)
            {
                check_teacher = "O 老師簽名";
            }
            else if((int)(data[mode]["check_list"][i]) == 1)
            {
                check_department = "O 系辦蓋章";
            }
            else if ((int)(data[mode]["check_list"][i]) == 2)
            {
                check_office = "O 教務處蓋章";
            }
        }
    }
    
    public void Rotation()
    {
        float x = 180 - float.Parse(data[mode]["rotation"][0].ToString());
        float y = float.Parse(data[mode]["rotation"][1].ToString()) - 180;
        float z = float.Parse(data[mode]["rotation"][2].ToString()) - 180;
        
        // environment setting
        environment.transform.rotation = Quaternion.Euler(x, y, z);
    }
    
    private void EndGame()
    {
        story.text = "End";
        A.gameObject.SetActive(false);
        B.gameObject.SetActive(false);
    }
}
