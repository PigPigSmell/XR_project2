using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class GameBall : MonoBehaviour
{


    public AudioSource C;
    public void RandomlyTeleport()
    {
        gameObject.transform.position = new Vector3(
            GetRandomCoordinate(), Random.Range(0.5f, 2), GetRandomCoordinate());
    }

    private float GetRandomCoordinate()
    {
        var coordinate = Random.Range(-7, 7);
        while(coordinate > -1.5 && coordinate < 1.5)
        {
            coordinate = Random.Range(-7, 7);
        }
        return coordinate;
    }

    private void Update()
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
            C.Play();
            RandomlyTeleport();
        }
    }
}
