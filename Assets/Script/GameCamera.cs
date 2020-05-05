using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 40.0f;
    private float pitch = 50.0f;

    private Vector3 angles;

    // Start is called before the first frame update
    void Start()
    {
        Input.gyro.enabled = true;
        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        angles = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        angles.y += speedH * Input.GetAxis("Mouse X");            
        angles.x -= speedV * Input.GetAxis("Mouse Y");
        //  yaw += speedH * Input.acceleration.x;
        //  pitch -= speedV * Input.acceleration.y;

        //  pitch -= speedV * 0.1f;
        //  yaw += speedH * 0.1f;
        transform.eulerAngles = angles;

        //   Vector3 previousEulerAngles = transform.eulerAngles;
        //    Vector3 gyroInput = -Input.gyro.rotationRateUnbiased;

        //   Vector3 targetEulerAngles = previousEulerAngles + gyroInput * Time.deltaTime * Mathf.Rad2Deg;
        //  targetEulerAngles.x = 0.0f; // Only this line has been added
        //  targetEulerAngles.z = 0.0f;

        //  transform.eulerAngles = targetEulerAngles;
        // }
    }
}

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// /// <summary>
// /// 陀螺儀實現全景圖效果
// /// </summary>
// public class GameCamera : MonoBehaviour
// {
//     private Quaternion gyroscopeQua;    //陀螺儀四元數
//     private Quaternion quatMult = new Quaternion(0, 0, 1, 0);   // 沿著 Z 周旋轉的四元數 因子
//     private float speedH = 0.2f;    //差值
//     protected void Start()
//     {
//         //啟用陀螺儀
//         Input.gyro.enabled = true;
//         //因安卓裝置的陀螺儀四元數水平值為[0,0,0,0]水平向下，所以將相機初始位置修改與其對齊
//         transform.eulerAngles = new Vector3(90, 90, 0);
//     }
//     protected void Update()
//     {
//         GyroModifyCamera();
//     }
//     /// <summary>
//     /// 檢測陀螺儀運動的函式
//     /// </summary>
//     protected void GyroModifyCamera()
//     {
//         gyroscopeQua = Input.gyro.attitude * quatMult;      //為球面運算做準備
//         transform.localRotation = Quaternion.Slerp(
//             transform.localRotation, gyroscopeQua, speedH);
//     }
// }