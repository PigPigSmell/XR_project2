using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

using System.Collections;
//引入庫
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using TMPro;

public class client : MonoBehaviour
{
    [Header ("GameObject")]
    public GameObject StartScene;
    public GameObject SceneController;
    public GameObject GameController;
    public GameObject ButtonAB;
    public GameObject StartButton;
    public GameObject Buttonnext;
    public GameObject Checklist;
    public GameObject StoryBackGround;

    [Header("Image")]
    public Image img;
    public Image Endimg;

    [Header("Text")]
    public Text GameResult_temp;

    [Header("AudioSource")]
    public AudioSource BGM;
    public AudioSource Good;
    public AudioSource Bad;
    AudioSource audio_source;

    [Header("VideoPlayer")]
    public VideoPlayer video;

    [Header("AudioClip")]
    public AudioClip welcome;
    public AudioClip b_sound;


    [Header("TextMeshProUGUI")]
    public TextMeshProUGUI story;
    public TextMeshProUGUI resultA;
    public TextMeshProUGUI resultB;
    public TextMeshProUGUI StartText;
    public TextMeshProUGUI RoleText;
    public TextMeshProUGUI time;
    
    // socket
    string editString = "start"; //編輯框文字
    Socket serverSocket; //伺服器端socket
    IPAddress ip; //主機ip
    IPEndPoint ipEnd;
    string recvStr = ""; //接收的字串
    string sendStr; //傳送的字串
    byte[] recvData = new byte[1024]; //接收的資料，必須為位元組
    byte[] sendData = new byte[1024]; //傳送的資料，必須為位元組
    int recvLen; //接收的資料長度
    Thread connectThread; //連線執行緒

    private bool connect = false;
    private bool bgmflag = true;

    // Initialize
    static public int step;
    static public bool played;
    static public int skip;
    static public float startTime;
    static public string go;
    static public bool getGameResult;
    static public string role;

    SceneControl sc;
    ToJson toJ;

    //初始化
    void InitSocket()
    {
        //定義伺服器的IP和埠，埠與伺服器對應
        ip = IPAddress.Parse("140.113.193.198"); //可以是區域網或網際網路ip
        ipEnd = new IPEndPoint(ip, 2222);
        
        //ip = IPAddress.Parse("127.0.0.1"); //可以是區域網或網際網路ip，此處是本機
        //ipEnd = new IPEndPoint(ip, 5566);


        //開啟一個執行緒連線，必須的，否則主執行緒卡死
        connectThread = new Thread(new ThreadStart(SocketReceive));
        connectThread.Start();
    }

    void SocketConnet()
    {
        if (serverSocket != null)
            serverSocket.Close();
        //定義套接字型別,必須在子執行緒中定義
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        print("ready to connect");

        //連線
        serverSocket.Connect(ipEnd);
        connect = true;

        //輸出初次連線收到的字串
        recvLen = serverSocket.Receive(recvData);
        recvStr = Encoding.ASCII.GetString(recvData, 0, recvLen);
        print(recvStr);
    }

    void SocketSend(string sendStr)
    {
        //清空傳送快取
        sendData = new byte[1024];
        //資料型別轉換
        sendData = Encoding.ASCII.GetBytes(sendStr);
        //傳送
        serverSocket.Send(sendData, sendData.Length, SocketFlags.None);
    }

    void SocketReceive()
    {
        SocketConnet();
        //不斷接收伺服器發來的資料
        while (true)
        {
            recvData = new byte[1024];
            recvLen = serverSocket.Receive(recvData);
            if (recvLen == 0)
            {
                SocketConnet();
                continue;
            }
            recvStr = Encoding.ASCII.GetString(recvData, 0, recvLen);
            
            print(recvStr);
        }
    }

    void SocketQuit()
    {
        //關閉執行緒
        if (connectThread != null)
        {
            connectThread.Interrupt();
            connectThread.Abort();
        }
        //最後關閉伺服器
        if (serverSocket != null)
            serverSocket.Close();
        print("diconnect");
    }

    // Use this for initialization
    void Start()
    {
        InitSocket();
        sc = GameObject.Find("SceneManager").GetComponent<SceneControl>();
        toJ = GameObject.Find("enviroment").GetComponent<ToJson>();

        
        audio_source = GetComponent<AudioSource>();
        video.Stop();

        // Initialize variable
        StartScene.SetActive(ToJson.Data.StartScene);
        SceneController.SetActive(ToJson.Data.SceneController);
        GameController.SetActive(ToJson.Data.GameController);
        ButtonAB.SetActive(ToJson.Data.ButtonAB);
        Buttonnext.SetActive(ToJson.Data.Buttonnext);
        step = ToJson.Data.step;
        played = ToJson.Data.played;
        skip = ToJson.Data.skip;
        startTime = ToJson.Data.startTime;
        go = ToJson.Data.go;
        role = ToJson.Data.role;
    }

    void OnGUI()
    {
        editString = GUI.TextField(new Rect(10, 10, 100, 20), editString);
        if (GUI.Button(new Rect(10, 30, 60, 20), "send"))
            SocketSend(editString);
    }

    // Update is called once per frame
    void Update()
    {
        if(SetTime.minute < 0)
        {
            step = 7;
        }

        //Debug.Log(step);
        if (step == -5)
        {
            Checklist.SetActive(false);
            StartButton.GetComponentInChildren<Text>().text = "Start";
            img.transform.GetChild(0).gameObject.SetActive(true);
        }
        else if (step == -4)
        {
            img.transform.GetChild(0).gameObject.SetActive(false);
            img.transform.GetChild(1).gameObject.SetActive(true);
            StartButton.GetComponentInChildren<Text>().text = "Continue";
        }
        else if (step == -3)
        {
            SocketSend("connect");
            step++;
        }
        else if (step == -2)
        {
            string[] str = recvStr.Split(':');
            img.transform.GetChild(1).gameObject.SetActive(false);
            img.transform.GetChild(2).gameObject.SetActive(true);
            if (str[0] == "role")
            {
                audio_source.PlayOneShot(welcome);
                role = str[1];
                if(str[1] == "good") RoleText.text ="大雄";
                else RoleText.text = "胖虎";
                    StartButton.GetComponentInChildren<Text>().text = "OK";
            }
            /*else
            {
                Debug.Log("Fail to load role.");
            }*/
        }
        else if (step == -1)
        {
            if (!played)
            {
                audio_source.PlayOneShot(b_sound);
                played = true;
            }

            img.transform.GetChild(2).gameObject.SetActive(false);
            img.transform.GetChild(3).gameObject.SetActive(true);
            RoleText.text = "";
            StartText.text = "loading...";

            StartButton.SetActive(false);
            ////////////////
            //step = 7; ///test end
            ///////////////
            
            if (recvStr == "scene start")
            {
                step++;
                audio_source.PlayOneShot(b_sound);
                BGM.Pause();

                img.transform.GetChild(3).gameObject.SetActive(false);
                StartScene.SetActive(false);
                startTime = Time.time;
                
            }
        }
        else if(step == 0)
        {
            
            StoryBackGround.SetActive(true);
            SceneController.SetActive(true);
            story.gameObject.SetActive(true);
            Checklist.SetActive(true);
            video.Play(); 
            video.Pause();
            skip = SceneControl.miss;
            
            if (Time.time - startTime > 3)
            {
                step++;
            }            
        }
        else if (step == 1)
        {
            Buttonnext.SetActive(false);
            story.text = "";
            StoryBackGround.SetActive(false);
            //Checklist.SetActive(false);

            if (bgmflag) Good.Play();
            else Bad.Play();

            video.Play();

            if (video.isPlaying)
            {
                step++;
            }
        }
        else if (step == 2)
        {
            if (!video.isPlaying)
            {
                sc.CheckList();

                if (bgmflag) Good.Pause();
                else Bad.Pause();

                if (skip == 1)
                {
                    step = 0;
                    sc.PressA();
                    startTime = Time.time;
                }
                else if(skip == 2)
                {
                    step = 10;
                }
                else
                {
                    step++;
                }
            }
        }
        else if (step == 3)
        {
            if (getGameResult == false)
            {
                // GameController.SetActive(true);
                getGameResult = true;
                toJ.WriteData();
                toJ.WriteGameIdx();
                SceneManager.LoadScene(1);
            }
            // Sent game result
            else if (getGameResult == true && connect == true)
            {
                toJ.LoadGameResult();
                video.Play();

                string str = "";
                if (role == "good")
                {
                    str = "+" + (ToJson.GameData.gameResult).ToString();
                }
                else if (role == "bad")
                {
                    str = "-" + (ToJson.GameData.gameResult).ToString();
                }

                SocketSend(str);
                GameController.SetActive(false);
                GameResult_temp.text = "";

                if (recvStr == "got game result")
                {
                    step++;
                    getGameResult = false;
                }
            }
        }
        else if (step == 4)
        {
            video.Pause();
            // Recieve winner information
            string[] str = recvStr.Split(':');
            story.gameObject.SetActive(false);
            if (str[0] == "winner")
            {
                if (str[1] == role)
                {
                    bgmflag = true;
                    ButtonAB.transform.GetChild(0).GetComponent<Button>().interactable = true;
                    ButtonAB.transform.GetChild(1).GetComponent<Button>().interactable = true;
                }
                else if (str[1] != role)
                {
                    bgmflag = false;
                    ButtonAB.transform.GetChild(0).GetComponent<Button>().interactable = false;
                    ButtonAB.transform.GetChild(1).GetComponent<Button>().interactable = false;
                }
                step++;
                ButtonAB.SetActive(true);
                startTime = Time.time;
            }
            /*else
            {
                Debug.Log("Fail to load winner.");
            }*/
        }
        else if (step == 5)
        {
            string[] str = recvStr.Split('=');
            if (str[0] == "A:B")
            {
                string[] str1 = recvStr.Split('=');
                string[] num = str1[1].Split(':');
                resultA.text = num[0];
                resultB.text = num[1];
            }

            float remainTime = 10 - (Time.time - startTime);
            if(remainTime < 0)
            {
                step++;
            }
            else
            {
                time.text = "還剩 " + ((int)(remainTime)).ToString() + " 秒";
            }
            /*else
            {
                Debug.Log("Fail to load vote result.");
            }*/
        }
        else if (step == 6)
        {
            string[] str = recvStr.Split(':');
            
            if (str[0] == "go")
            {
                go = "true:" + str[1];
                ButtonAB.gameObject.SetActive(false);
                recvStr = "scene start";
                resultA.text = "";
                resultB.text = "";
                step = 0;
                startTime = Time.time;
            }
        }
        else
        {
            // end
            
            story.text = "end";
        }
        if(step == 7)
        {
            //Debug.Log(step);
            BGM.Pause();
            StartScene.SetActive(false);
            //EndScene.SetActive(true);
            ButtonAB.SetActive(false);
            Endimg.transform.GetChild(0).gameObject.SetActive(true);

            sc.CheckList();
        }
    }

    public void PressA()
    {
        SocketSend("vote:A");
        ButtonAB.transform.GetChild(0).GetComponent<Button>().interactable = false;
        ButtonAB.transform.GetChild(1).GetComponent<Button>().interactable = false;
    }
    public void PressB()
    {
        SocketSend("vote:B");
        ButtonAB.transform.GetChild(0).GetComponent<Button>().interactable = false;
        ButtonAB.transform.GetChild(1).GetComponent<Button>().interactable = false;
    }
    public void temp_gameResult()
    {
        getGameResult = true;
    }
    public void StepPlus()
    {
        step++;
    }

    public string getGo()
    {
        return go;
    }
    public void setGo(string str)
    {
        go = str;
    }
    public void setStep(int n)
    {
        step = n;
    }

    //程式退出則關閉連線
    void OnApplicationQuit()
    {
        SocketQuit();
    }
}