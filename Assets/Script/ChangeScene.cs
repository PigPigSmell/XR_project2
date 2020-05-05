using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public Text displayText;
    public Text discription;
    public Text countDown;
    public Button readButton;
    public Button roleButton;
    public Button loadButton;
    public Button startButton;
    public Image startScene;
    public Image storyScene;

    void Start()
    {
        displayText.text = "";
        discription.text = "";
        countDown.text = "";
    }

    public void BackTo360Video()
    {
        int idx = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(0);
        //Application.LoadLevel(1);
    }

    public void ChangeToSmallGame()
    {
        SceneManager.LoadScene(1);
        //Application.LoadLevel(0);
    }

    public void RoleDisplay()
    {
        displayText.text = "你的角色是：";
        Destroy(roleButton.gameObject);
    }

    public void Loading()
    {
        displayText.text = "Loading...";             
        Destroy(loadButton.gameObject);
    }

    public void DisplayText()
    {
        displayText.text = "胖虎這學期已經不能再退選，\n" +
                           "即將被二一，\n" +
                           "他發現大雄竟然只要退選就不會被二一，\n" +
                           "心生不滿，決定阻止大雄，\n" +
                           "而大雄要越過胖虎的重重阻攔，\n" +
                           "完成退選。";
        Destroy(readButton.gameObject);
        Destroy(startScene.gameObject);
    }

    public void StartGame()
    {
        displayText.text = "";
        Destroy(startButton.gameObject);
        Destroy(storyScene.gameObject);
    }
}
