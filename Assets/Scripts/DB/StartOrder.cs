using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.MixedReality.Toolkit;
using MySql.Data.MySqlClient;
using Nest;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

public class StartOrder : MonoBehaviour
{
    public static Orders ActiveOrder;
    [SerializeField] private GameObject slider;

    public DataTable operationTable = new DataTable();
    public static List<Operations> operationsList = new List<Operations>();

    public static readonly Operations ActiveOperation = new Operations();
    public static List<string[]> epiList = new List<string[]>();

    private static List<GameObject> gameObjectsList = new List<GameObject>();

    private List<string> operationsArray = new List<string>();

    [SerializeField] private TextMeshPro instruction;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject nextButtonPlate;
    [SerializeField] private GameObject previousButton;
    [SerializeField] private TextMeshPro stepCounter;
    [SerializeField] private TextMeshPro operation;
    [SerializeField] private Material successGreen;
    [SerializeField] private Material originalMaterial;

    // Functions
    [SerializeField] private GameObject qrcode;
    [SerializeField] private GameObject measuring;
    [SerializeField] private GameObject ocr;

    public static int counter = 0;
    private int pdfName = 0;
    private float fraction = 1f;

    // Image tools
    [SerializeField] private Image image;
    private static long Bytes;
    public static byte[] bytesArray;
    public static byte[] imageBytes;
    private static byte[] imageVarbinary;

    // Start is called before the first frame update
    void Start()
    {
        // Setting back to Zero
        slider.transform.localScale = new Vector3(0, 1, 1);
        counter = 0;

        // Clearing data
        operationsArray.Clear();
        operationsList.Clear();
        epiList.Clear();
        DisableAll();

        // Getting data from database
        operationsArray = GetOperationsFromOrdemDeManutencao();
        GetAllOperations();
        SetStartTime();
        SetUpScene();

        // Deactivating all GameObjects
        imageCanvas.SetActive(false);
        pdfCanvas.SetActive(false);
    }

    #region DataBase methods

    private List<string> GetOperationsFromOrdemDeManutencao()
    {
        List<string> strArray = new List<string>();
        strArray.Clear();

        try
        {
            // Connecting to database
            using SqlConnection connection = new SqlConnection(ConnectionString.stringBuilder.ConnectionString);
            connection.Open();

            // Get all users
            string sql =
                "SELECT cod_operacao FROM OrdensDeManutencaoOperacoes WHERE cod_ordemDeManutencao = @omCode;";

            // Reading all rows from the steps
            using SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@omCode", ActiveOrder.OrderCode);

            using SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                DataTable table = new DataTable();
                table.Load(reader);

                foreach (DataRow str in table.Rows)
                {
                    strArray.Add(str["cod_operacao"].ToString());
                    Debug.Log("Operation registered: " + str["cod_operacao"].ToString());
                }
            }
        }
        catch(SqlException e)
        {
            Debug.Log(e.Message);
        }

        return strArray;
    }

    private void GetAllOperations()
    {
        DataTable dt = new DataTable();

        // Connecting to database
        using SqlConnection connection = new SqlConnection(ConnectionString.stringBuilder.ConnectionString);
        connection.Open();

        foreach (var opCode in operationsArray)
        {
            // Get all operations
            string sql = "SELECT * FROM Operacoes WHERE cod_operacao = @opCode";

            // Adding all parameters
            using SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@opCode", opCode);

            // Execute reader
            using SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                dt.Load(reader);
            }

            Random rdr = new Random();

            foreach (DataRow item in dt.Rows)
            {
                Debug.Log("Estou no loop de salvar os dados da operação " + item["cod_operacao"]);

                var op = new Operations()
                {
                    OperationCode = item["cod_operacao"].ToString(),
                    Descricao = item["descricao"].ToString(),
                    Instruction = item["instrucao"].ToString(),
                    Image = item["imagem"].ToString(),
                    PDF = item["pdf"].ToString(),
                    PDFName = rdr.Next(),
                    FBX = item["fbx"].ToString(),
                    Video = item["video"].ToString(),
                    OCR = Convert.ToBoolean(item["ocr"]),
                    OCRParameter = item["ocrParametro"].ToString(),
                    QRCode = Convert.ToBoolean(item["qrcode"]),
                    QRCodeParameter = item["qrcodeParametro"].ToString(),
                    Measure = Convert.ToBoolean(item["medicao"]),
                    MeasureParameter = item["medicaoParametro"].ToString(),
                };

                Debug.Log("Booleans | OCR: " + op.OCR + " & QRCode: " + op.QRCode + " & Measure: " + op.Measure);
                operationsList.Add(op);
            }

            dt.Clear();
        }

        connection.Close();
    }

    private void GetPDF()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString.stringBuilder.ConnectionString))
            {
                connection.Open();

                // SQL Command through C#
                String sql = "SELECT DATALENGTH(pdf), pdf FROM Operacoes WHERE cod_operacao = @operationCode";

                // Reading all rows from the first line
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@operation_code", operationsList[counter].OperationCode);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(1))
                                {
                                    var longVar = reader.GetInt64(0);
                                    longVar = 10;

                                    // Trying to convert the VARBINARY to a array of bytes
                                    try
                                    {
                                        int ndx = reader.GetOrdinal("pdf");

                                        if (!reader.IsDBNull(ndx))
                                        {
                                            long size = reader.GetBytes(ndx, 0, null, 0, 0); //get the length of data
                                            bytesArray = new byte[size];

                                            int bufferSize = 1024;
                                            long bytesRead = 0;
                                            int curPos = 0;
                                            int i = 0;

                                            while (bytesRead < size)
                                            {
                                                i++;
                                                bytesRead += reader.GetBytes(ndx, curPos, bytesArray, curPos,
                                                    bufferSize);
                                                curPos += bufferSize;
                                            }

                                            try
                                            {
                                                string path = "Assets/StreamingAssets/" + operationsList[counter].PDFName + ".pdf";
                                                System.IO.File.WriteAllBytes(path, bytesArray);
                                            }
                                            catch (Exception e)
                                            {
                                                Debug.Log(e.ToString());
                                            }

                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        Debug.Log(e.ToString());
                                    }
                                    finally
                                    {
                                        Paroxe.PdfRenderer.PDFViewer.pdfName = operationsList[counter].PDFName + ".pdf";

                                        Debug.Log("O PDF foi salvo com sucesso");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    private void GetImage()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString.stringBuilder.ConnectionString))
            {
                connection.Open();

                // SQL Command through C#
                String sql = "SELECT DATALENGTH(image), image FROM Operacoes WHERE cod_operacao = @operation_code";

                // Reading all rows from the first line
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@operation_code", ActiveOperation.OperationCode);

                    // Reading data
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int ndx = reader.GetOrdinal("image");

                                image.gameObject.SetActive(true);

                                if (!reader.IsDBNull(0))
                                {
                                    var longVar = reader.GetInt64(0);
                                    longVar = 10;
                                    Bytes = reader.GetBytes(1, 0, imageBytes, 0, (int)Bytes);

                                    long size = reader.GetBytes(ndx, 0, null, 0, 0);  //get the length of data
                                    bytesArray = new byte[size];

                                    int bufferSize = 1024;
                                    long bytesRead = 0;
                                    int curPos = 0;

                                    // Parsing VARBINARY to byte[]
                                    while (bytesRead < size)
                                    {
                                        bytesRead += reader.GetBytes(ndx, curPos, bytesArray, curPos, bufferSize);
                                        curPos += bufferSize;
                                    }

                                    // Creating and changing the texture of the image
                                    Texture2D tex = new Texture2D(16, 9, TextureFormat.RGBA32, false);
                                    tex.LoadImage(bytesArray);
                                    tex.Apply();
                                    image.material.mainTexture = tex;

                                    image.gameObject.SetActive(false);
                                    image.gameObject.SetActive(true);
                                    imageCanvas.SetActive(true);
                                }
                                else
                                {
                                    imageCanvas.SetActive(false);
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (SqlException e)
        {
            Debug.Log(e.Message);
        }
    }

    [SerializeField] private GameObject imageCanvas;
    [SerializeField] private GameObject pdfCanvas;
    [SerializeField] private Canvas epis;

    public void OpenImage()
    {
        GetImage();

        imageCanvas.SetActive(true);
        pdfCanvas.SetActive(false);
    }

    public void OpenPDF()
    {
        GetPDF();

        imageCanvas.SetActive(false);
        pdfCanvas.SetActive(true);
    }

    public void OpenVideo()
    {
        imageCanvas.SetActive(false);
        pdfCanvas.SetActive(false);
        Application.OpenURL(StartOrder.ActiveOperation[StartOrder.counter].Video);

        // The second option it's to download the video to MyVideos folder and access it after
        /*
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), "test.mp4");
        var videoPlayer = GetComponent<VideoPlayer>();
 
        Uri u = new Uri(new Uri("file://"), path);
        videoPlayer.url = u.AbsoluteUri;
        */
    }

    public void OpenEpis()
    {
        imageCanvas.SetActive(false);
        pdfCanvas.SetActive(false);
    }

    #endregion

    #region Front-end

    private void DisableAll()
    {
        qrcode.SetActive(false);
        ocr.SetActive(false);
        measuring.SetActive(false);
        imageCanvas.SetActive(false);
        pdfCanvas.SetActive(false);
    }

    private void SetUpScene()
    {
        fraction = 0 + fraction/operationsList.Count;

        // Getting all functions the scene need
        if (operationsList[0].QRCode)
        {
            qrcode.SetActive(true);
        }
        else
        {
            qrcode.SetActive(false);
        }

        if (operationsList[0].Measure)
        {
            measuring.SetActive(true);
        }
        else
        {
            measuring.SetActive(false);
        }

        if (operationsList[0].OCR)
        {
            ocr.SetActive(true);
        }
        else
        {
            ocr.SetActive(false);
        }

        if(!previousButton.activeInHierarchy)
            previousButton.SetActive(true);

        instruction.text = operationsList[0].Instruction;
        stepCounter.text = "Passo #" + (counter + 1);
        slider.transform.localScale = new Vector3(slider.transform.localScale.x + (float)fraction,
            slider.transform.localScale.y, slider.transform.localScale.z);
    }

    public void MoveToNextOperation()
    {
        counter++;

        if (counter + 1> operationsList.Count)
        {
            Debug.Log("Tamanho da variável auxiliar: " + counter);

            SceneManager.LoadScene("Results");
        }
        else if (counter + 1 == operationsList.Count)
        {
            Debug.Log("Tamanho da variável auxiliar: " + counter);

            nextButtonPlate.GetComponent<Renderer>().material = successGreen;
            instruction.text = operationsList[counter].Instruction;
            stepCounter.text = "Passo #" + (counter + 1);
            slider.transform.localScale = new Vector3(slider.transform.localScale.x + (float)fraction,
                slider.transform.localScale.y, slider.transform.localScale.z);

            if (!previousButton.activeInHierarchy)
            {
                previousButton.SetActive(true);
            }

            // Getting all functions the scene need
            if (operationsList[counter].QRCode)
            {
                qrcode.SetActive(true);
            }
            else
            {
                qrcode.SetActive(false);
            }

            if (operationsList[counter].Measure)
            {
                measuring.SetActive(true);
            }
            else
            {
                measuring.SetActive(false);
            }

            if (operationsList[counter].OCR)
            {
                ocr.SetActive(true);
            }
            else
            {
                ocr.SetActive(false);
            }
        }
        else
        {
            Debug.Log("Tamanho da variável auxiliar: " + counter);

            if (!previousButton.activeInHierarchy)
            {
                previousButton.SetActive(true);
            }

            // Getting all functions the scene need
            if (operationsList[counter].QRCode)
            {
                qrcode.SetActive(true);
            }
            else
            {
                qrcode.SetActive(false);
            }

            if (operationsList[counter].Measure)
            {
                measuring.SetActive(true);
            }
            else
            {
                measuring.SetActive(false);
            }

            if (operationsList[counter].OCR)
            {
                ocr.SetActive(true);
            }
            else
            {
                ocr.SetActive(false);
            }

            instruction.text = operationsList[counter].Instruction;
            stepCounter.text = "Passo #" + (counter + 1);
            slider.transform.localScale = new Vector3(slider.transform.localScale.x + (float)fraction,
                slider.transform.localScale.y, slider.transform.localScale.z);
        }
    }

    public void MoveToPreviousOperation()
    {
        counter = counter - 1;

        if (counter > operationsList.Count)
        {
            previousButton.SetActive(false);
        }
        else if (counter == operationsList.Count)
        {
            nextButtonPlate.GetComponent<Renderer>().material = successGreen;
            instruction.text = operationsList[counter].Instruction;
            stepCounter.text = "Passo #" + (counter + 1);
            slider.transform.localScale = new Vector3(slider.transform.localScale.x - (float)fraction,
                slider.transform.localScale.y, slider.transform.localScale.z);

            // Getting all functions the scene need
            if (operationsList[counter].QRCode)
            {
                qrcode.SetActive(true);
            }
            else
            {
                qrcode.SetActive(false);
            }

            if (operationsList[counter].Measure)
            {
                measuring.SetActive(true);
            }
            else
            {
                measuring.SetActive(false);
            }

            if (operationsList[counter].OCR)
            {
                ocr.SetActive(true);
            }
            else
            {
                ocr.SetActive(false);
            }
        }
        else
        {
            if (!previousButton.activeInHierarchy)
            {
                previousButton.SetActive(true);
            }

            // Getting all functions the scene need
            if (operationsList[counter].QRCode)
            {
                qrcode.SetActive(true);
            }
            else
            {
                qrcode.SetActive(false);
            }

            if (operationsList[counter].Measure)
            {
                measuring.SetActive(true);
            }
            else
            {
                measuring.SetActive(false);
            }

            if (operationsList[counter].OCR)
            {
                ocr.SetActive(true);
            }
            else
            {
                ocr.SetActive(false);
            }

            var s = ActiveOperation.PPE.Split('/');
            instruction.text = ActiveOperation[counter].Instruction;
            stepCounter.text = "Passo #" + (counter + 1);
            Debug.Log("X Scale: " + slider.transform.localScale.x);
            slider.transform.localScale = new Vector3(slider.transform.localScale.x - (float)fraction,
                slider.transform.localScale.y, slider.transform.localScale.z);
            Debug.Log("Fraction: " + fraction + "X Scale: " + slider.transform.localScale.x);
        }
    }

    // Instantiate EPIs
    public float x_Start, y_Start;
    public int ColumnLength;
    public float x_Space, y_Space;

    private List<string> GetAllCodesFromEpis()
    {
        DataTable table = new DataTable();
        List<string> strArray = new List<string>();
        strArray.Clear();

        try
        {
            // Connecting to database
            using SqlConnection connection = new SqlConnection(ConnectionString.stringBuilder.ConnectionString);
            connection.Open();

            // Get all users
            string sql =
                "SELECT cod_epi FROM OperacoesEpis WHERE cod_operacao = @opCode;";

            // Reading all rows from the steps
            using SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@opCode", operationsList[counter].OperationCode);

            using SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                Debug.Log("Estou salvando os códigos");

                table.Load(reader);
            }

            foreach (DataRow str in table.Rows)
            {
                Debug.Log("Código do EPI: " + str["cod_epi"].ToString());
                strArray.Add(str["cod_epi"].ToString());
            }
        }
        catch(SqlException e)
        {
            Debug.Log(e.Message);
        }

        return strArray;
    }

    private List<string> GetAllNamesFromEpis()
    {
        DataTable table = new DataTable();
        List<string> strArray = new List<string>();
        strArray.Clear();

        var localList = GetAllCodesFromEpis();

        strArray.Clear();

        try
        {
            // Connecting to database
            using SqlConnection connection = new SqlConnection(ConnectionString.stringBuilder.ConnectionString);
            connection.Open();

            foreach (var code in localList)
            {
                Debug.Log("Estou salvando os nomes");

                // Get all users
                string sql =
                    "SELECT descricao FROM Epis WHERE cod_epi = @epiCode;";

                // Reading all rows from the steps
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@epiCode", code);

                using SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    table.Load(reader);
                }

                foreach (DataRow str in table.Rows)
                {
                    Debug.Log("Nome do EPI: " + str["descricao"].ToString());
                    strArray.Add(str["descricao"].ToString());
                }

                table.Clear();
            }
        }
        catch (SqlException e)
        {
            Debug.Log(e.Message);
        }

        return strArray;
    }

    private void DestroyAllEpis()
    {
        try
        {
            foreach (var objects in epis.GetComponentsInParent<GameObject>())
            {
                Destroy(objects);
            }
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }

        try
        {
            foreach (var objects in epis.GetComponentsInParent<GameObject>())
            {
                Destroy(objects);
            }
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }

    public void InstantiateEpis()
    {
        imageCanvas.SetActive(false);
        pdfCanvas.SetActive(false);

        var localList = GetAllNamesFromEpis();

        for (int i = 0; i < localList.Count; i++)
        {
            var gameObject = Resources.Load(localList[i]) as GameObject;

            Vector3 position = new Vector3(x_Start + (x_Space * (i % ColumnLength)), y_Start + (-y_Space * (i / ColumnLength)), 0.5f);
            Instantiate(Resources.Load(localList[i]), position, gameObject.transform.rotation);
        }
    }

    #endregion

    #region Back-end

    private void SetStartTime()
    {
        DateTime now = DateTime.UtcNow;

        ActiveOrder.StartDate = now;
    }

    public void SetSuccess()
    {
        operationsList[counter].Result = "Success";
        Debug.Log("Index da lista " + (counter) + " tem o valor de " + operationsList[counter].Status);

        MoveToNextOperation();
    } 

    public void SetFailure()
    {
        operationsList[counter].Result = "Failure";
        Debug.Log("Index da lista " + (counter) + " tem o valor de " + operationsList[counter].Status);

        MoveToNextOperation();
    }

    #endregion
}
