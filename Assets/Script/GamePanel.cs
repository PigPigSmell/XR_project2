using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{

    public int counter = 0;
    public Text panelText;
    // Start is called before the first frame update
    void Start()
    {
        
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
