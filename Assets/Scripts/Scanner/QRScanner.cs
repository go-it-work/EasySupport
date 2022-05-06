using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.Windows.WebCam;

public class QRScanner : MonoBehaviour
{

    public static QRScanner Instance;

    WebCamTexture _webcamTexture;
    string _qrCode = string.Empty;
    [SerializeField] TextMeshProUGUI _tmp;
    bool _isActive = false;
    [SerializeField] private TextMeshPro Scan;
    [SerializeField] private TextMeshPro Header;
    private RawImage rawImage;
    public static string resultado1 = "Leitura não realizada";
    public bool IsActive { get => _isActive; set => _isActive = value; }

    void Start()
    {
        Instance = this;
        rawImage = GameObject.Find("RawImage").GetComponent<RawImage>();
        var renderer = rawImage.GetComponent<RawImage>();
        _webcamTexture = new WebCamTexture(512, 512);
        renderer.material.mainTexture = _webcamTexture;
        
    }

    public void StopWebcam()
    {
        _webcamTexture.Stop();
    }

    public void ScanScreen()
    {
        Inactive();
        Active();
    }
    public void Active()
    {
        Header.text = "Escaneando códigos ...";
        _webcamTexture.Play();
        StartCoroutine("GetQRCodeIE");
        _isActive = true;
    }

    public void Inactive()
    {
        StopCoroutine("GetQRCodeIE");
        _isActive = false;
        _qrCode = string.Empty;

        Header.text = "Diga 'OK' para começar a leitura dos códigos";
        Scan.text = string.Empty;

        if (_tmp) _tmp.text = string.Empty;
    }

    public void ActiveOrInactive()
    {
        _isActive = !_isActive;
        if (_isActive)
            Active();
        else
            Inactive();
    }
    IEnumerator GetQRCodeIE()
    {
        IBarcodeReader barCodeReader = new BarcodeReader();
        var snap = new Texture2D(_webcamTexture.width, _webcamTexture.height, TextureFormat.ARGB32, false);
        while (string.IsNullOrEmpty(_qrCode))
        {
            try
            {
                if (Snap(snap, barCodeReader))
                    break;
            }
            catch (Exception ex) { Debug.LogWarning(ex.Message); }
            yield return null;
        }
    }

    bool Snap(Texture2D snap, IBarcodeReader barCodeReader)
    {
        snap.SetPixels32(_webcamTexture.GetPixels32());
        var Result = barCodeReader.Decode(snap.GetRawTextureData(), _webcamTexture.width, _webcamTexture.height, RGBLuminanceSource.BitmapFormat.ARGB32);
        if (Result != null)
        {
            _qrCode = Result.Text;
            if (!string.IsNullOrEmpty(_qrCode))
            {
                Debug.Log("Qr Decode: " + _qrCode);
                resultado1 = _qrCode;

                StartOrder.operationsList[StartOrder.counter].Result = _qrCode;

                if (StartOrder.operationsList[StartOrder.counter].QRCodeParameter == _qrCode)
                {
                    StartOrder.operationsList[StartOrder.counter].Result = _qrCode;
                    StartOrder.operationsList[StartOrder.counter].Status = "Success";
                }
                else
                {
                    StartOrder.operationsList[StartOrder.counter].Result = _qrCode;
                    StartOrder.operationsList[StartOrder.counter].Status = "Failure";
                }

                Scan.text = _qrCode;
                Header.text = "Código escaneado:";

                if (_tmp) _tmp.text = _qrCode; else Debug.LogError("Setar TextMeshproGui");
                return true;
            }
        }
        return false;
    }
}

