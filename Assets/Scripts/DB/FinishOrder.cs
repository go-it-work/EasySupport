using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishOrder : MonoBehaviour
{
    private void Start()
    {
        foreach (var op in StartOrder.operationsList)
        {
            for (int i = 0; i < StartOrder.operationsList.Count - 1; i++)
            {
                Debug.Log("Resultado da operation " + op[i] + " :" + op.Result);
            }
        }
    }
    public void PublishResults()
    {
        ChangeMaintenanceOrderSituation(GetTime());
        PublishOperations();
        SceneManager.LoadScene("SelectionMenu");
    }

    private DateTime GetTime()
    {
        DateTime now = DateTime.UtcNow;

        return now;
    }

    private void ChangeMaintenanceOrderSituation(DateTime now)
    {
        // Connecting to database
        using SqlConnection connection = new SqlConnection(ConnectionString.stringBuilder.ConnectionString);
        connection.Open();

        // Get all users
        string sql =
            "UPDATE OrdensDeManutencao SET situacao = 3, data_inicio = @inicio, data_fim = @fim WHERE cod_ordemdemanutencao = @maintenanceorder_code";

        // Reading all rows from the steps
        using SqlCommand command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@maintenanceorder_code", StartOrder.ActiveOrder.OrderCode);
        command.Parameters.AddWithValue("@inicio", StartOrder.ActiveOrder.StartDate);
        command.Parameters.AddWithValue("@fim", now);

        command.ExecuteNonQuery();
    }

    private void PublishOperations()
    {
        // Connecting to database
        using SqlConnection connection = new SqlConnection(ConnectionString.stringBuilder.ConnectionString);
        connection.Open();

        foreach (Operations op in StartOrder.operationsList)
        {
            // Publish Operations from ActiveOrder
            string sql =
                "INSERT INTO OperacoesResultados(cod_ordemDeManutencao, cod_operacaoResultado, instrucao, ocr, ocrParametro, qrcode, qrcodeParametro, medicao, medicaoParametro, resultado, status_aprovacao, status) VALUES (@OMCode, @OpCode, @Instruction, @OCR, @ocrParam, @QRCode, @qrcodeParam, @Measure, @measureParam, @Resultado, @Aprov, @Status)";

            if (op.QRCodeParameter == DBNull.Value.ToString() || op.QRCodeParameter == null)
                op.QRCodeParameter = "Parâmetro não requisitado";

            if (op.OCRParameter == DBNull.Value.ToString() || op.OCRParameter == null)
                op.OCRParameter = "Parâmetro não requisitado";

            if (op.MeasureParameter == DBNull.Value.ToString() || op.MeasureParameter == null)
                op.MeasureParameter = "Parâmetro não requisitado";

            if (op.Result == null)
                op.Result = "Etapa não realizada pelo operador";

            // Giving all parameters and publishing it
            using SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@OpCode", op.OperationCode);
            command.Parameters.AddWithValue("@OMCode", StartOrder.ActiveOrder.OrderCode);
            command.Parameters.AddWithValue("@Instruction", op.Instruction);
            command.Parameters.AddWithValue("@OCR", op.OCR);
            command.Parameters.AddWithValue("@ocrParam", op.OCRParameter);
            command.Parameters.AddWithValue("@QRCode", op.QRCode);
            command.Parameters.AddWithValue("@qrCodeParam", op.QRCodeParameter);
            command.Parameters.AddWithValue("@Measure", op.Measure);
            command.Parameters.AddWithValue("@measureParam", op.MeasureParameter);
            command.Parameters.AddWithValue("@Resultado", op.Result);
            command.Parameters.AddWithValue("@Aprov", 2);
            command.Parameters.AddWithValue("@Status", 1);

            command.ExecuteNonQuery();
        }
    }

    public void GoBackToOperations()
    {
        SceneManager.LoadScene("Operations");
    }
}
