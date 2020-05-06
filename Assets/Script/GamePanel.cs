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
        Vector3 clickPos;
        bool clicked;

#if UNITY_ANDROID || UNITY_IOS
         clicked = Input.touchCount > 0;
         if(clicked)
             clickPos = Input.GetTouch(0).position;
#else
        clicked = Input.GetMouseButtonDown(0);
        clickPos = Input.mousePosition;
#endif
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(clickPos);
        if (Physics.Raycast(ray, out hit) && clicked)
        {
            AddCounter();
        }
        panelText.text = "找到球共 " + counter.ToString() + " 次";
    }
}
