using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebCam : MonoBehaviour {

	//public int FPS = 30;

	[SerializeField]
	Camera targetCamera;

	WebCamTexture webcamTexture;

	WebCamDevice[] devices;

	int selectedCameraId = 0;

	int cameraNum = 0;

	Vector2 screenSize;

	Vector3 originalSize;

	[SerializeField]
	Text resultText;//読み取り結果

	private  PQRCodeManager qrManager;

	bool init = false;

	float time = 0;

	void Start(){
		Application.targetFrameRate = 30;

		SetScreenSize ();

		//デバイスを取得
		devices = WebCamTexture.devices;

		if (devices.Length > 0) {
			cameraNum = devices.Length - 1;
			selectedCameraId = cameraNum;
			webcamTexture = new WebCamTexture(devices[selectedCameraId].name,(int)screenSize.y,(int)screenSize.x);

			webcamTexture.Play();

			FitQuadSize ();

			GetComponent<MeshRenderer> ().material.mainTexture = webcamTexture;

			//QRコード管理クラス
			this.qrManager = new PQRCodeManager ();

			init = true;

		} else {
			Debug.Log ("カメラなし");
			// 確認のみのダイアログ
			/**
			 * 
			 * "カメラにアクセスできません",
				"設定>プライバシー>カメラ>(このアプリ名)で、許可をお願いします",
			*/
			return;
		}
	}

	void SetScreenSize(){
		screenSize = new Vector2 (Screen.width,Screen.height);
	}


	//サイズをぴったりにする
	void FitQuadSize(){
		float h = targetCamera.orthographicSize * 2;
		float w = h * targetCamera.aspect;
		transform.localScale = new Vector3 (h, w,1);
		originalSize = transform.localScale;

		var euler = transform.localRotation.eulerAngles;
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			transform.localRotation = Quaternion.Euler (euler.x, euler.y, euler.z - 90);
		} else if (Application.platform == RuntimePlatform.Android) {
			transform.localRotation = Quaternion.Euler (euler.x, euler.y, euler.z + 90);
		}
	}

	//カメラ変更
	void ChangeCamera(){
		Debug.Log ("Change");
		webcamTexture.Stop ();
		selectedCameraId++;
		if (selectedCameraId > cameraNum) {
			selectedCameraId = 0;
		}
		webcamTexture.deviceName = devices[selectedCameraId].name;
		if (selectedCameraId != cameraNum) {
			transform.localScale = new Vector3 (originalSize.x,originalSize.y*-1,originalSize.z);
		} else {
			transform.localScale = new Vector3 (originalSize.x,originalSize.y,originalSize.z);
		}
		webcamTexture.Play ();
	}

	// Update is called once per frame
	void Update () {
		//カメラから読み取り
		if (init) {
			time += Time.deltaTime;
			if (time > 1) {
				resultText.text = this.qrManager.read (webcamTexture);
				time = 0;
			}
		}
	}
}
