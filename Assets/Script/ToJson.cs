using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public class ToJson : MonoBehaviour
{
    public GameObject StartScene;
    public GameObject SceneController;
    public GameObject GameController;
    public GameObject ButtonAB;
    public GameObject StartButton;
    public GameObject Buttonnext;

    //新增一個物件類型為playerState的變數 loadData
    static public Variables Data = new Variables();
    static public GameResult GameData = new GameResult();
    

    // Start is called before the first frame update
    void Start()
    {
        //InitializeAll();
        LoadData();
    }

    // Update is called once per frame
    void Update()
    {
        //
    }

    public void WriteData()
    {
        Variables var = new Variables();

        // client.cs
        var.step = client.step;
        var.played = client.played;
        var.skip = client.skip;
        var.startTime = client.startTime;
        var.go = client.go;
        var.role = client.role;
        var.getGameResult = client.getGameResult;

        var.StartScene = StartScene.activeSelf;
        var.SceneController = SceneController.activeSelf;
        var.GameController = GameController.activeSelf;
        var.ButtonAB = ButtonAB.activeSelf;
        var.Buttonnext = Buttonnext.activeSelf;

        // SceneControl.cs
        var.mode = SceneControl.mode;
        var.miss = SceneControl.miss;
        var.spendMinute = SceneControl.spendMinute;

        var.check_teacher = SceneControl.check_teacher;
        var.check_department = SceneControl.check_department;
        var.check_office = SceneControl.check_office;

        string saveString = JsonUtility.ToJson(var);

        //write to Josn
        StreamWriter file = new StreamWriter(System.IO.Path.Combine(Application.streamingAssetsPath, "Variables.json"));
        file.Write(saveString);
        file.Close();
    }

    public void WriteGameIdx()
    {
        GameResult var = new GameResult();

        var.gameIdx = SceneControl.GameIdx;
        var.gameResult = 0;

        string saveString = JsonUtility.ToJson(var);

        //write to Josn
        StreamWriter file = new StreamWriter(System.IO.Path.Combine(Application.streamingAssetsPath, "GameResult.json"));
        file.Write(saveString);
        file.Close();
    }

    public void LoadData()
    {
        //讀取json檔案並轉存成文字格式
        StreamReader file = new StreamReader(System.IO.Path.Combine(Application.streamingAssetsPath, "Variables.json"));
        string loadJson = file.ReadToEnd();
        file.Close();

        //使用JsonUtillty的FromJson方法將存文字轉成Json
        Data = JsonUtility.FromJson<Variables>(loadJson);
    }

    public void LoadGameResult()
    {
        //讀取json檔案並轉存成文字格式
        StreamReader file = new StreamReader(System.IO.Path.Combine(Application.streamingAssetsPath, "GameResult.json"));
        string loadJson = file.ReadToEnd();
        file.Close();

        //使用JsonUtillty的FromJson方法將存文字轉成Json
        GameData = JsonUtility.FromJson<GameResult>(loadJson);
    }

    public void InitializeAll()
    {
        //client c = GameObject.Find("Manager").GetComponent<client>();
        //SceneControl sc = GameObject.Find("SceneManager").GetComponent<SceneControl>();

        Variables var = new Variables();

        // client.cs
        var.step = -5;
        var.played = false;
        var.skip = 0;
        var.startTime = 0.0f;
        var.go = "false:";
        var.role = "";
        var.getGameResult = false;

        var.StartScene = true;
        var.SceneController = false;
        var.GameController = false;
        var.ButtonAB = false;
        var.Buttonnext = false;

        // SceneControl.cs
        var.mode = 0;
        var.miss = 0;
        var.spendMinute = 0;

        var.check_teacher = "- 老師簽名";
        var.check_department = "- 系辦蓋章";
        var.check_office = "- 教務處蓋章";

        string saveString = JsonUtility.ToJson(var);

        //write to Josn
        StreamWriter file = new StreamWriter(System.IO.Path.Combine(Application.streamingAssetsPath, "Variables.json"));
        file.Write(saveString);
        file.Close();

        //LoadData();

        //c.InitializeAll();
        //sc.InitializeAll();
    }

    [System.Serializable]
    public class Variables
    {
        // client.cs
        public int step;
        public bool played;
        public int skip;
        public float startTime;
        public string go;
        public string role;
        public bool getGameResult;

        public bool StartScene;
        public bool SceneController;
        public bool GameController;
        public bool ButtonAB;
        public bool Buttonnext;

        // SceneControl
        public int mode;
        public int miss;
        public int spendMinute;

        public string check_teacher;
        public string check_department;
        public string check_office;
    }

    [System.Serializable]
    public class GameResult
    {
        public int gameResult;
        public int gameIdx;
    }

}
