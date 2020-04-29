using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class change : MonoBehaviour
{
    private int mode;
    public Button A;
    public Button B;
    public Button game;
    public Text story;

    public Canvas user_canvas;
    //public Canvas canvas;
    

    // Start is called before the first frame update
    void Start()
    {
        mode = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (mode == 0)
        {
            A.GetComponentInChildren<Text>().text = "Go to lab";
            B.GetComponentInChildren<Text>().text = "Go to department office";
            story.text = "Oh! The door of teacher office is locked. \nWhere should I go?";
        }
        else if (mode == 1)
        {
            A.GetComponentInChildren<Text>().text = "Go to restaurant";
            B.GetComponentInChildren<Text>().text = "Go to department office";
            story.text = "Too lucky! Teacher is here!";
        }
        else if (mode == 2)
        {
            A.GetComponentInChildren<Text>().text = "Go home.";
            B.GetComponentInChildren<Text>().text = "Go to library";
            story.text = "Oh no.. the door is locked again...";
        }
        else
        {
            story.text = "End";
            A.gameObject.SetActive(false);
            B.gameObject.SetActive(false);
            mode++;
        }
    }

    public void pressButonA()
    {
        mode++;
        gameview();
    }
    public void pressButonB()
    {
        mode++;
        gameview();
    }
    public void backTo360()
    {
        A.gameObject.SetActive(true);
        B.gameObject.SetActive(true);
        user_canvas.gameObject.SetActive(true);
        game.gameObject.SetActive(false);
    }

    private void gameview()
    {
        A.gameObject.SetActive(false);
        B.gameObject.SetActive(false);
        user_canvas.gameObject.SetActive(false);
        game.gameObject.SetActive(true);
    }
}
