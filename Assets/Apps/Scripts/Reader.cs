using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Reader : MonoBehaviour {


	public int FPS = 30;


	//CanvasのUIに紐付けてください。///////////
	public RawImage qrCodeImage;//生成したQRコード
	public Text resultText;//読み取り結果

	private WebCamTexture webcamTexture;
	private  PQRCodeManager qrManager;

	Vector2 screenSize;

	void SetScreenSize(){
		screenSize = new Vector2 (Screen.width,Screen.height);
	}

	// Use this for initialization
	void Start () {


		//カメラ準備
		WebCamDevice[] devices = WebCamTexture.devices;
		webcamTexture = new WebCamTexture(devices[0].name,(int)screenSize.y,(int)screenSize.x, FPS );


		GetComponent<MeshRenderer> ().material.mainTexture = webcamTexture;
		webcamTexture.Play();



		//QRコード管理クラス
		this.qrManager = new PQRCodeManager ();

	}

	// Update is called once per frame
	void Update () {
		//カメラから読み取り
		resultText.text = this.qrManager.read (webcamTexture);
	}


	/// <summary>
	/// QRコード生成
	/// </summary>
	public void createQRCode(){
		Debug.Log ("createQRCode");
		string inputText = "http://test.ppen.info";


		Texture2D texture = new Texture2D (256, 256);
		qrCodeImage.texture = texture;
		this.qrManager.create (inputText,(Texture2D)qrCodeImage.texture);
	}
}
