using LitJson;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class json : MonoBehaviour
{

    private string path = "Assets/story/story0.json";//輸出的檔案路徑和檔名，其實副檔名可隨意

    void OnGUI()
    {
        if (GUILayout.Button("WriteJson") == true)
        {
            outputJsonFile(writeJson(new string[] { "Name", "Age" }, new string[] { "Yang", "21" }));//編寫Json檔並輸出檔案
        }
        if (GUILayout.Button("ReadJson") == true)
        {
            readJson(loadJsonFile());//載入Json檔並讀取內部資訊
        }
        /*if (GUI.Button(new Rect((Screen.width/2)-75, Screen.height / 2, 150, 30), "ChangeToGameScene") == true)
        {
            Application.LoadLevel(0); //change scene
        }*/
    }

    private string writeJson(string[] key, string[] value)
    {
        if (key.Length != value.Length)
        {
            throw new System.Exception("key.Length != value.Length");
        }

        StringBuilder sb = new StringBuilder();//若字串會被修改，則建議使用StringBuilder，速度比String還快。(可以自行google搜尋這兩者的差異)
        JsonWriter jw = new JsonWriter(sb);//資料將會寫在sb內

        jw.WriteArrayStart();//寫入"["到sb內
        jw.WriteObjectStart();//寫入"{"到sb內
        for (int i = 0; i < key.Length; i++)
        {
            jw.WritePropertyName(key[i]);
            jw.Write(value[i]);
        }
        jw.WriteObjectEnd();//寫入"}"到sb內
        jw.WriteArrayEnd();//寫入"]"到sb內

        return sb.ToString();
    }

    private void readJson(string jsonStr)
    {
        /*JsonReader jr = new JsonReader(jsonStr.ToString());

        while (jr.Read() == true)//連續讀取資料直到讀取完畢
        {
            if (jr.Value != null)//若有資料，則開始列印
            {
                Debug.Log(jr.Value);//列印出讀取到的資料
            }
        }*/
        
        JsonData data = JsonMapper.ToObject(jsonStr);

        // Scalar elements stored in a JsonData instance can be cast to
        // their natural types
        //string artist = (string)data["album"]["artist"];
        //int year = (int)data["album"]["year"];

        // description
        /*description.text = (string) data["description"];
        Debug.Log(description);*/

        // dialog
        /*string dialog = (string)(data["dialog"]);
        Debug.Log(dialog);*/

        // option_description
        /*string opt_description = (string)(data["option_description"]);
        Debug.Log(opt_description);*/

        // option
        string option = (string)(data["option"][0]["name"]);
        Debug.Log(option);

    }

    private void outputJsonFile(string sb)
    {
        using (StreamWriter sw = new StreamWriter(path, true))//將資料寫入，若原先已有資料則寫在原先資料的後面
        {
            sw.WriteLine(sb);//將資料寫入
        }
    }

    private string loadJsonFile()
    {
        StringBuilder sbJson = new StringBuilder();//用來儲存讀入的Json內容
        using (StreamReader sr = new StreamReader(path))
        {
            sbJson.Append(sr.ReadToEnd());//一次性將資料全部讀入
        }

        return sbJson.ToString();
    }
}

