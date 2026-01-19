using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public bool touchFlag;
    public float touchDelta;
    // ワールド座標で返す
    public Vector3 touchStartPos;
    public Vector3 touchCurrentPos;
    public Vector3 touchEndPos;
    public TouchPhase touchPhase;


    void Start()
    {
        touchFlag = false;
        touchDelta = 0;
    }


    void Update()
    {
        // エディタで実行中
        if (Application.isEditor)
        {
            if (Input.GetMouseButtonDown(0))
            {
                touchFlag = true;
                touchDelta = 0;
                touchStartPos = Input.mousePosition;
                touchPhase = TouchPhase.Began;
            }
            else if (Input.GetMouseButton(0))
            {
                touchFlag = true;
                touchDelta += Time.deltaTime;
                touchCurrentPos = Input.mousePosition;
                touchPhase = TouchPhase.Moved;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                touchFlag = false;
                touchEndPos = Input.mousePosition;
                touchPhase = TouchPhase.Ended;
            }
        }
        // 実機で実行中
        else
        {
            if (Input.touchCount > 0)
            {
                // タッチ情報の取得
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    touchFlag = true;
                    touchDelta = 0;
                    touchStartPos = touch.position;
                    touchPhase = TouchPhase.Began;
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    touchFlag = true;
                    touchDelta += Time.deltaTime;
                    touchCurrentPos = touch.position;
                    touchPhase = TouchPhase.Moved;
                }
                else if (touch.phase == TouchPhase.Stationary)
                {
                    touchFlag = true;
                    touchDelta += Time.deltaTime;
                    touchPhase = TouchPhase.Stationary;
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    touchFlag = false;
                    touchEndPos = touch.position;
                    touchPhase = TouchPhase.Ended;
                }
            }
        }
    }
}
