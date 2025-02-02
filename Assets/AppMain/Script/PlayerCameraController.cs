﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    // 回転操作用トランスフォーム.
    [SerializeField] Transform rotationRoot = null;
    // 高さ操作用トランスフォーム.
    [SerializeField] Transform heightRoot = null;
    // プレイヤーカメラ.
    [SerializeField] GameObject mainCamera = null; //CameraをGameObjectに変更

    // カメラが写す中心のプレイヤーから高さ.
    [SerializeField] float lookHeight = 5.0f;
    // カメラ回転スピード.
    [SerializeField] float rotationSpeed = 0.5f;
    // カメラ高さ変化スピード.
    [SerializeField] float heightSpeed = 0.001f;
    // カメラ移動制限MinMax.
    [SerializeField] Vector2 heightLimit_MinMax = new Vector2(-1f, 3f);

    // タッチスタート位置.
    Vector2 cameraStartTouch = Vector2.zero; //スマホ用
    Vector2 cameraStartJoyStick = Vector2.zero; //Oculus用
    // 現在のタッチ位置.
    Vector2 cameraTouchInput = Vector2.zero; //スマホ用
    Vector2 cameraJoyStickInput = Vector2.zero; //Oculus用


    void Start()
    {

    }

    void Update()
    {

    }

    public void UpdateCameraLook(Transform player)
    {
        // カメラをキャラの少し上に固定.
        var cameraMarker = player.position;
        cameraMarker.y += lookHeight;
        var _camLook = (cameraMarker - mainCamera.transform.position).normalized;
        mainCamera.transform.forward = _camLook;
        //mainCamera.transform.forward = player.transform.forward;
    }

    public void FixedUpdateCameraPosition(Transform player)
    {
        this.transform.position = player.position;
        //this.transform.rotation = player.rotation;
    }

    

    // Oculus用
    public void UpdateRightJoyStick()
    {
        //cameraStartJoyStick = Vector2.zero;
        
        // 現在の位置を随時保管.
        Vector2 position = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
        // 開始位置からの移動ベクトルを算出.
        cameraJoyStickInput = position - cameraStartJoyStick;

        float HorizontalcameraJoyStickInput = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x;

        // カメラ回転. //カメラの回転だけなぜかうまくいかない
        //var yRot = new Vector3(0, cameraJoyStickInput.x * rotationSpeed, 0);
        var yRot = new Vector3(0, HorizontalcameraJoyStickInput * rotationSpeed, 0);
        var rResult = rotationRoot.rotation.eulerAngles + yRot;
        var qua = Quaternion.Euler(rResult);
        rotationRoot.rotation = qua;

        /*
        // カメラ高低.
        var yHeight = new Vector3(0, cameraJoyStickInput.y * heightSpeed, 0);
        var hResult = heightRoot.transform.localPosition + yHeight;
        if (hResult.y > heightLimit_MinMax.y) hResult.y = heightLimit_MinMax.y;
        else if (hResult.y <= heightLimit_MinMax.x) hResult.y = heightLimit_MinMax.x;
        heightRoot.localPosition = hResult;
        */
        
    }


    //-------------------------------------------------
    //スマホの右側のタッチによるカメラの回転移動
    //-------------------------------------------------
    public void UpdateRightTouch(Touch touch)
    {
        
        // タッチ開始.
        if (touch.phase == TouchPhase.Began)
        {
            Debug.Log("右タッチ開始");
            // 開始位置を保管.
            cameraStartTouch = touch.position;
        }
        // タッチ中.
        else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
        {
            Debug.Log("右タッチ中");
            // 現在の位置を随時保管.
            Vector2 position = touch.position;
            // 開始位置からの移動ベクトルを算出.
            cameraTouchInput = position - cameraStartTouch;
            // カメラ回転.
            var yRot = new Vector3(0, cameraTouchInput.x * rotationSpeed, 0);
            var rResult = rotationRoot.rotation.eulerAngles + yRot;
            var qua = Quaternion.Euler(rResult);
            rotationRoot.rotation = qua;

            /*
            // カメラ高低.
            var yHeight = new Vector3(0, -cameraTouchInput.y * heightSpeed, 0);
            var hResult = heightRoot.transform.localPosition + yHeight;
            if (hResult.y > heightLimit_MinMax.y) hResult.y = heightLimit_MinMax.y;
            else if (hResult.y <= heightLimit_MinMax.x) hResult.y = heightLimit_MinMax.x;
            heightRoot.localPosition = hResult;
            */
        }
        // タッチ終了.
        else if (touch.phase == TouchPhase.Ended)
        {
            Debug.Log("右タッチ終了");
            //cameraTouchInput = Vector2.zero;
            cameraTouchInput = touch.position;
        }
    }
}