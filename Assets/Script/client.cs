using UnityEngine;
using System.Collections;
//引入庫
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using UnityEngine.UI;
using UnityEngine.Video;


public class client : MonoBehaviour
{
    public GameObject StartScene;
    public GameObject SceneController;
    public GameObject GameController;
    public GameObject ButtonAB;
    public GameObject StartButton;
    public Image img;
    public AudioSource BGM;
    public GameObject Buttonnext;
    //
    AudioSource audio_source;
    public AudioClip welcome;
    public AudioClip b_sound;

    public Text GameResult_temp;
    private bool getGameResult = false;
    private string role;
    public Text story;

    public Text resultA;
    public Text resultB;
    public Text StartText;
    public Text time;

    public VideoPlayer video;

    string go = "false:";

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

    private bool played = false;
    private int step = 0;
    private string press;
    private bool skip;

    private float startTime;
    private float endTime;

    SceneControl sc;

    //初始化
    void InitSocket()
    {
        //定義伺服器的IP和埠，埠與伺服器對應
        ip = IPAddress.Parse("140.113.193.198"); //可以是區域網或網際網路ip，此處是本機
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
        sc = GameObject.Find("SceneManager").GetComponent<SceneControl>();
        
        audio_source = GetComponent<AudioSource>();
        StartScene.SetActive(true);
        SceneController.SetActive(false);
        GameController.SetActive(false);
        ButtonAB.SetActive(false);
        Buttonnext.SetActive(false);
        InitSocket();
        step = -5;
        video.Stop();
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
        //Debug.Log(step);
        if (step == -5)
        {
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
                StartText.text = "Your role is ... " + str[1];
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

            StartText.text = "loading...";

            StartButton.SetActive(false);
            
            if (recvStr == "scene start")
            {
                step++;
                audio_source.PlayOneShot(b_sound);
                BGM.Pause();

                img.transform.GetChild(3).gameObject.SetActive(false);
                StartScene.SetActive(false);
                startTime = Time.time;
            }
        }else if(step == 0)
        {
            video.Play();
            video.Pause();
            SceneController.SetActive(true);
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
                if(skip == true)
                {
                    step = 0;
                    sc.PressA();
                    startTime = Time.time;
                }
                else
                {
                    step++;
                }
            }
        }
        else if (step == 3)
        {
            GameController.SetActive(true);

            // Sent game result
            if (getGameResult == true)
            {
                SocketSend(GameResult_temp.text);
                GameController.SetActive(false);
                GameResult_temp.text = "";
                getGameResult = false;
                step++;
            }
        }
        else if (step == 4)
        {
            // Recieve winner information
            string[] str = recvStr.Split(':');
            if (str[0] == "winner")
            {
                if (str[1] == role)
                {
                    ButtonAB.transform.GetChild(0).GetComponent<Button>().interactable = true;
                    ButtonAB.transform.GetChild(1).GetComponent<Button>().interactable = true;
                }
                else if (str[1] != role)
                {
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
                time.text = ((int)(remainTime)).ToString() + " s";
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

    //程式退出則關閉連線
    void OnApplicationQuit()
    {
        SocketQuit();
    }
}