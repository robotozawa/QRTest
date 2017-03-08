using ZXing;
using ZXing.QrCode;
using UnityEngine;

public class PQRCodeManager  {


	/// <summary>
	/// QRコード読み取り)(返り文字列が-1の場合は、読み込めていない)
	/// </summary>
	/// <param name="cameraTexture">Camera texture.</param>
	public string read(WebCamTexture cameraTexture){
		BarcodeReader reader = new BarcodeReader ();
		Color32[] color = cameraTexture.GetPixels32 ();
		int width = cameraTexture.width;
		int height = cameraTexture.height;
		Result result = reader.Decode (color, width, height);
		if (result != null){
			return result.Text;
		}
		return "-1";//エラー時
	}


	/// <summary>
	/// QRコード作成
	/// </summary>
	/// <param name="inputString">QRコード生成元の文字列</param>
	/// <param name="textture">QRの画像がここに入る</param>
	public void create(string inputString,Texture2D textture){
		var qrCodeColors = Write (inputString, textture.width, textture.height);
		textture.SetPixels32 (qrCodeColors);
		textture.Apply ();
	}


	private Color32[] Write(string content,int width,int height){
		var writer = new BarcodeWriter {
			Format = BarcodeFormat.QR_CODE,
			Options = new QrCodeEncodingOptions {
				Height = height,
				Width = width
			}
		};

		return writer.Write (content);
	}
}
