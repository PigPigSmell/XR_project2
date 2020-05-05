using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameInput : MonoBehaviour
{
    public static UnityEvent onTouchStart = new UnityEvent();
    public static UnityEvent onTouchEnd = new UnityEvent();


    public static UnityEvent onTouchpadLeftUp = new UnityEvent();
    public static UnityEvent onTouchpadRightUp = new UnityEvent();

    void Update()
    {
#if UNITY_EDITOR

#endif
#if UNITY_EDITOR
        keyInput();
#endif
    }

    private void keyInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (onTouchStart != null)
            {
                onTouchStart.Invoke();
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (onTouchEnd != null)
            {
                onTouchEnd.Invoke();
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (onTouchpadLeftUp != null)
            {
                onTouchpadLeftUp.Invoke();
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (onTouchpadRightUp != null)
            {
                onTouchpadRightUp.Invoke();
            }
        }
    }
}


