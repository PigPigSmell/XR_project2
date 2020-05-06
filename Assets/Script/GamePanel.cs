using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    // Initialize
    static public int counter;

    public Text panelText;
    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
    }

    public void AddCounter()
    {
        counter += 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
          {
            AddCounter();
          }
              panelText.text = "找到球共 " + counter.ToString() + " 次";
    }
}
