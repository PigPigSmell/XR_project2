using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using System.IO;

public class GameToJson : MonoBehaviour
{
    //新增一個物件類型為playerState的變數 loadData
    static public Variables Data = new Variables();

    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - startTime > 10)
        {
            WriteData();
            SceneManager.LoadScene(0);
        }
    }

    public void WriteData()
    {
        Variables var = new Variables();

        var.gameResult = GamePanel.counter;


        string saveString = JsonUtility.ToJson(var);

        //write to Josn
        StreamWriter file = new StreamWriter(System.IO.Path.Combine(Application.streamingAssetsPath, "GameResult.json"));
        file.Write(saveString);
        file.Close();
    }

    public void LoadData(string filename)
    {
        //讀取json檔案並轉存成文字格式
        StreamReader file = new StreamReader(System.IO.Path.Combine(Application.streamingAssetsPath, filename));
        string loadJson = file.ReadToEnd();
        file.Close();

        //使用JsonUtillty的FromJson方法將存文字轉成Json
        Data = JsonUtility.FromJson<Variables>(loadJson);
    }

    [System.Serializable]
    public class Variables
    {
        // GamePanel.cs
        public int gameResult;
    }

}
