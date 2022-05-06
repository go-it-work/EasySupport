using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GetAllOrders : MonoBehaviour
{
    public DataTable orderTable = new DataTable();
    public List<Orders> ordersList = new List<Orders>();

    [SerializeField] private TextMeshPro instruction;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject previousButton;
    [SerializeField] private GameObject failurePlate;
    [SerializeField] private TextMeshPro failureText;
    [SerializeField] private GameObject instructionPlate;
    [SerializeField] private GameObject infoPlate;

    [SerializeField] private GameObject rowOne;
    [SerializeField] private GameObject rowTwo;
    [SerializeField] private GameObject rowThree;
    [SerializeField] private GameObject rowFour;
    [SerializeField] private GameObject rowFive;

    [SerializeField] private TextMeshPro headerOne;
    [SerializeField] private TextMeshPro headerTwo;
    [SerializeField] private TextMeshPro headerThree;
    [SerializeField] private TextMeshPro headerFour;
    [SerializeField] private TextMeshPro headerFive;

    [SerializeField] private Text omOne;
    [SerializeField] private Text omTwo;
    [SerializeField] private Text omThree;
    [SerializeField] private Text omFour;
    [SerializeField] private Text omFive;

    private int counter = 0;
    private int orderCounter = -1;
    public static string OMCode = null;

    #region DataBase methods

    void Start()
    {
        orderCounter = -1;

        instructionPlate.SetActive(false);
        infoPlate.SetActive(false);

        if(failurePlate.activeInHierarchy)
            failurePlate.SetActive(true);
    }

    private List<string> GetAllOrdersForUserGroup()
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
                "SELECT * FROM OrdensDeManutencaoGrupoUsuarios WHERE cod_grupoUsuario = @usergroup_code;";

            // Reading all rows from the steps
            using SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@usergroup_code", GetLoginParameters.UserGoupCode);

            using SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                table.Load(reader);
            }

            foreach (DataRow str in table.Rows)
            {
                strArray.Add(str["cod_ordemDeManutencao"].ToString());
            }
        }
        catch(SqlException e)
        {
            Debug.Log(e.Message);

            instructionPlate.SetActive(false);
            failurePlate.SetActive(true);
        }

        return strArray;
    }

    public void GetOrdersByUser()
    {
        DataTable localDataTable = new DataTable();
        var localList = GetAllOrdersForUserGroup();

        Debug.Log("Tamanho da Lista: " + localList.Count);

        try
        {
            foreach (var str in localList)
            {
                // Connecting to database
                using SqlConnection connection = new SqlConnection(ConnectionString.stringBuilder.ConnectionString);
                connection.Open();

                // Get all users
                string sql =
                    "SELECT * FROM OrdensDeManutencao WHERE cod_ordemDeManutencao = @cod_ordemDeManutencao AND situacao = 0 AND status = 1 ORDER BY cod_ordemDeManutencao ASC;";

                // Reading all rows from the steps
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@cod_ordemDeManutencao", str);

                using SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    localDataTable.Load(reader);
                }

                SaveOrder(localDataTable);
                localDataTable.Clear();
            }

            NextOrders();
        }
        catch(SqlException e)
        {
            Debug.Log(e.Message);

            instructionPlate.SetActive(false);
            failurePlate.SetActive(true);
        }
    }

    private void SaveOrder(DataTable table)
    {
        // Loop to add all data to a List<Orders>
        foreach (DataRow item in table.Rows)
        {
            try
            {
                var order = new Orders(item["tipo"].ToString(), item["descricao"].ToString(), item["cod_ordemdemanutencao"].ToString(),
                    item["cod_equipamento"].ToString(),
                     Convert.ToDateTime(item["data_ordem"]),
                    Convert.ToDateTime(item["data_prazo"]),
                    item["aprovacao_situacao"].ToString(), Convert.ToByte(item["urgencia"]), Convert.ToByte(item["situacao"]));

                ordersList.Add(order);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }
    }

    private string ChangeByteToString(List<Orders> order, int i)
    {
        string haste = "";

        if (order[i].Haste == 0)
        {
            haste = "Sim";
        }
        else if (order[i].Haste == 1)
        {
            haste = "Não";
        }

        return haste;
    }

    #endregion

    #region Front-end methods

    public void GetBackToAllOrders()
    {
        instructionPlate.SetActive(true);
        infoPlate.SetActive(false);
    }

    public void GetOrderDataOne()
    {
        DataTable localDataTable = new DataTable();

        instructionPlate.SetActive(false);
        infoPlate.SetActive(true);

        try
        {
            // Connecting to database
            using SqlConnection connection = new SqlConnection(ConnectionString.stringBuilder.ConnectionString);
            connection.Open();

            // Get all users
            string sql =
                "SELECT * FROM OrdensDeManutencao WHERE cod_ordemDeManutencao = @cod_ordemDeManutencao;";

            // Reading all rows from the steps
            using SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@cod_ordemDeManutencao", omOne.text);

            using SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                localDataTable.Load(reader);
            }

            // Loop to add all data to a List<Orders>
            foreach (DataRow item in localDataTable.Rows)
            {
                try
                {
                    var order = new Orders(item["tipo"].ToString(), item["descricao"].ToString(), item["cod_ordemdemanutencao"].ToString(),
                        item["cod_equipamento"].ToString(),
                        Convert.ToDateTime(item["data_ordem"]),
                        Convert.ToDateTime(item["data_prazo"]),
                        item["aprovacao_situacao"].ToString(), Convert.ToByte(item["urgencia"]), Convert.ToByte(item["situacao"]));

                    instruction.text = "Equipamento " + order.EquipmentCode + "\nPrazo: " +
                                       order.OrderDate.ToString("dd/MM/yyyy HH:mm") + "\nTipo: " + order.Type;
                }
                catch (Exception ex)
                {
                    Debug.Log(ex);
                }
            }

            localDataTable.Clear();
        }
        catch(SqlException e)
        {
            Debug.Log(e.Message);

            instructionPlate.SetActive(false);
            failurePlate.SetActive(true);
        }
    }

    public void GetOrderDataTwo()
    {
        DataTable localDataTable = new DataTable();

        instructionPlate.SetActive(false);
        infoPlate.SetActive(true);

        try
        {
            // Connecting to database
            using SqlConnection connection = new SqlConnection(ConnectionString.stringBuilder.ConnectionString);
            connection.Open();

            // Get all users
            string sql =
                "SELECT * FROM OrdensDeManutencao WHERE cod_ordemDeManutencao = @cod_ordemDeManutencao;";

            // Reading all rows from the steps
            using SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@cod_ordemDeManutencao", omTwo.text);

            using SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                localDataTable.Load(reader);
            }

            // Loop to add all data to a List<Orders>
            foreach (DataRow item in localDataTable.Rows)
            {
                try
                {
                    var order = new Orders(item["tipo"].ToString(), item["descricao"].ToString(), item["cod_ordemdemanutencao"].ToString(),
                        item["cod_equipamento"].ToString(),
                        Convert.ToDateTime(item["data_ordem"]),
                        Convert.ToDateTime(item["data_prazo"]),
                        item["aprovacao_situacao"].ToString(), Convert.ToByte(item["urgencia"]), Convert.ToByte(item["situacao"]));

                    instruction.text = "Equipamento " + order.EquipmentCode + "\nPrazo: " +
                                       order.OrderDate.ToString("dd/MM/yyyy HH:mm") + "\nTipo: " + order.Type;
                }
                catch (Exception ex)
                {
                    Debug.Log(ex);
                }
            }

            localDataTable.Clear();
        }
        catch(SqlException e)
        {
            Debug.Log(e.Message);

            instructionPlate.SetActive(false);
            failurePlate.SetActive(true);
        }
    }

    public void GetOrderDataThree()
    {
        DataTable localDataTable = new DataTable();

        instructionPlate.SetActive(false);
        infoPlate.SetActive(true);

        try
        {
            // Connecting to database
            using SqlConnection connection = new SqlConnection(ConnectionString.stringBuilder.ConnectionString);
            connection.Open();

            // Get all users
            string sql =
                "SELECT * FROM OrdensDeManutencao WHERE cod_ordemDeManutencao = @cod_ordemDeManutencao;";

            // Reading all rows from the steps
            using SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@cod_ordemDeManutencao", omThree.text);

            using SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                localDataTable.Load(reader);
            }

            // Loop to add all data to a List<Orders>
            foreach (DataRow item in localDataTable.Rows)
            {
                try
                {
                    var order = new Orders(item["tipo"].ToString(), item["descricao"].ToString(), item["cod_ordemdemanutencao"].ToString(),
                        item["cod_equipamento"].ToString(),
                        Convert.ToDateTime(item["data_ordem"]),
                        Convert.ToDateTime(item["data_prazo"]),
                        item["aprovacao_situacao"].ToString(), Convert.ToByte(item["urgencia"]), Convert.ToByte(item["situacao"]));

                    instruction.text = "Equipamento " + order.EquipmentCode + "\nPrazo: " +
                                       order.OrderDate.ToString("dd/MM/yyyy HH:mm") + "\nTipo: " + order.Type;
                }
                catch (Exception ex)
                {
                    Debug.Log(ex);
                }
            }

            localDataTable.Clear();
        }
        catch(SqlException e)
        {
            Debug.Log(e.Message);

            instructionPlate.SetActive(false);
            failurePlate.SetActive(true);
        }
    }

    public void GetOrderDataFour()
    {
        DataTable localDataTable = new DataTable();

        instructionPlate.SetActive(false);
        infoPlate.SetActive(true);

        try
        {
            // Connecting to database
            using SqlConnection connection = new SqlConnection(ConnectionString.stringBuilder.ConnectionString);
            connection.Open();

            // Get all users
            string sql =
                "SELECT * FROM OrdensDeManutencao WHERE cod_ordemDeManutencao = @cod_ordemDeManutencao;";

            // Reading all rows from the steps
            using SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@cod_ordemDeManutencao", omFour.text);

            using SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                localDataTable.Load(reader);
            }

            // Loop to add all data to a List<Orders>
            foreach (DataRow item in localDataTable.Rows)
            {
                try
                {
                    var order = new Orders(item["tipo"].ToString(), item["descricao"].ToString(), item["cod_ordemdemanutencao"].ToString(),
                        item["cod_equipamento"].ToString(),
                        Convert.ToDateTime(item["data_ordem"]),
                        Convert.ToDateTime(item["data_prazo"]),
                        item["aprovacao_situacao"].ToString(), Convert.ToByte(item["urgencia"]), Convert.ToByte(item["situacao"]));

                    instruction.text = "Equipamento " + order.EquipmentCode + "\nPrazo: " +
                                       order.OrderDate.ToString("dd/MM/yyyy HH:mm") + "\nTipo: " + order.Type;
                }
                catch (Exception ex)
                {
                    Debug.Log(ex);
                }
            }

            localDataTable.Clear();
        }
        catch(SqlException e)
        {
            Debug.Log(e.Message);

            instructionPlate.SetActive(false);
            failurePlate.SetActive(true);
        }
    }

    public void GetOrderDataFive()
    {
        DataTable localDataTable = new DataTable();

        instructionPlate.SetActive(false);
        infoPlate.SetActive(true);

        try
        {
            // Connecting to database
            using SqlConnection connection = new SqlConnection(ConnectionString.stringBuilder.ConnectionString);
            connection.Open();

            // Get all users
            string sql =
                "SELECT * FROM OrdensDeManutencao WHERE cod_ordemDeManutencao = @cod_ordemDeManutencao;";

            // Reading all rows from the steps
            using SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@cod_ordemDeManutencao", omFive.text);

            using SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                localDataTable.Load(reader);
            }

            // Loop to add all data to a List<Orders>
            foreach (DataRow item in localDataTable.Rows)
            {
                try
                {
                    var order = new Orders(item["tipo"].ToString(), item["descricao"].ToString(), item["cod_ordemdemanutencao"].ToString(),
                        item["cod_equipamento"].ToString(),
                        Convert.ToDateTime(item["data_ordem"]),
                        Convert.ToDateTime(item["data_prazo"]),
                        item["aprovacao_situacao"].ToString(), Convert.ToByte(item["urgencia"]), Convert.ToByte(item["situacao"]));

                    instruction.text = "Equipamento " + order.EquipmentCode + "\nPrazo: " +
                                       order.OrderDate.ToString("dd/MM/yyyy HH:mm") + "\nTipo: " + order.Type;
                }
                catch (Exception ex)
                {
                    Debug.Log(ex);
                }
            }

            localDataTable.Clear();
        }
        catch(SqlException e)
        {
            Debug.Log(e.Message);

            instructionPlate.SetActive(false);
            failurePlate.SetActive(true);
        }
    }

    private void SetFirstOrderOnPanel()
    {
        try
        {
            for(int i = 0; i < ordersList.Count; i++)
            {
                Debug.Log("Código da Ordem " + i + " : " + ordersList[i].OrderCode);
            }

            instruction.text = "Equipamento " + ordersList[0].EquipmentCode + "\nPrazo: " +
                               ordersList[0].OrderDate +
                               "\nUrgência: " + ChangeByteToString(ordersList, 0);
        }
        catch(Exception e)
        {
            Debug.Log(e);
            
            instructionPlate.SetActive(false);
            failurePlate.SetActive(true);
            failurePlate.GetComponentInChildren<TextMeshPro>().text =
                "Não existem Ordens de Manutenção disponíveis.\nTente dar um 'refresh' através do menu de pulso";
        }
    }

    public void NextOrder()
    {
        if (ordersList[counter + 1].OrderCode != null)
        {
            previousButton.SetActive(true);
            counter += 1;
            instruction.text = "Equipamento " + ordersList[counter].EquipmentCode + "\nPrazo: " +
                               ordersList[counter].OrderDate + "\nUrgência: " +
                               ChangeByteToString(ordersList, counter);
        }
        else
        {
            nextButton.SetActive(false);
        }
    }

    public void PreviousOrder()
    {
        if (ordersList[counter - 1].OrderCode != null)
        {
            nextButton.SetActive(true);
            counter -= 1;
            instruction.text = "Equipamento " + ordersList[counter].EquipmentCode + "\nPrazo: " +
                               ordersList[counter].OrderDate + "\nUrgência: " +
                               ChangeByteToString(ordersList, counter);
        }
        else
        {
            previousButton.SetActive(false);
        }
    }

    private void SetRowsVisibility()
    {
        if (ordersList.Count <= 5)
        {
            switch (ordersList.Count)
            {
                case 0:
                    failurePlate.SetActive(true);
                    failureText.text = "Não existem Ordens de Manutenção pendentes";
                    break;

                case 1:
                    rowOne.SetActive(true);
                    rowTwo.SetActive(false);
                    rowThree.SetActive(false);
                    rowFour.SetActive(false);
                    rowFive.SetActive(false);
                    break;

                case 2:
                    rowOne.SetActive(true);
                    rowTwo.SetActive(true);
                    rowThree.SetActive(false);
                    rowFour.SetActive(false);
                    rowFive.SetActive(false);
                    break;

                case 3:
                    rowOne.SetActive(true);
                    rowTwo.SetActive(true);
                    rowThree.SetActive(true);
                    rowFour.SetActive(false);
                    rowFive.SetActive(false);
                    break;

                case 4:
                    rowOne.SetActive(true);
                    rowTwo.SetActive(true);
                    rowThree.SetActive(true);
                    rowFour.SetActive(true);
                    rowFive.SetActive(false);
                    break;

                case 5:
                    rowOne.SetActive(true);
                    rowTwo.SetActive(true);
                    rowThree.SetActive(true);
                    rowFour.SetActive(true);
                    rowFive.SetActive(true);
                    break;
            }
        }
        else
        {
            rowOne.SetActive(true);
            rowTwo.SetActive(true);
            rowThree.SetActive(true);
            rowFour.SetActive(true);
            rowFive.SetActive(true);
        }
    }

    public void NextOrders()
    {
        if (ordersList.Count != 0)
            instructionPlate.SetActive(true);
        else
        {
            failurePlate.SetActive(true);
            failureText.text = "Não existem ordens de manutenção dispóníveis para o usuário realizar. Tente dar um refresh atráves do menu de pulso";
        }

        SetRowsVisibility();

        if (counter <= ordersList.Count)
        {
            AssignNextValuesToRow(headerOne, omOne);
            AssignNextValuesToRow(headerTwo, omTwo);
            AssignNextValuesToRow(headerThree, omThree);
            AssignNextValuesToRow(headerFour, omFour);
            AssignNextValuesToRow(headerFive, omFive);
        }
    }

    public void PreviousOrders()
    {
        SetRowsVisibility();
        var auxCounter = ordersList.Count - orderCounter;

        if(auxCounter >= -4)
        {
            AssignPreviousValuesToRow(headerOne, omOne);
            AssignPreviousValuesToRow(headerTwo, omTwo);
            AssignPreviousValuesToRow(headerThree, omThree);
            AssignPreviousValuesToRow(headerFour, omFour);
            AssignPreviousValuesToRow(headerFive, omFive);
        }
    }

    private void AssignNextValuesToRow(TextMeshPro button, Text txt)
    {
        try
        {
            if(orderCounter + 1 < ordersList.Count)
            {
                button.text = ordersList[orderCounter + 1].Description;
                txt.text = ordersList[orderCounter + 1].OrderCode;

                orderCounter += 1;
            }
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private void AssignPreviousValuesToRow(TextMeshPro button, Text txt)
    {
        try
        {
            if(orderCounter - 1 < ordersList.Count)
            {
                button.text = ordersList[orderCounter - 1].Description;
                txt.text = ordersList[orderCounter - 1].OrderCode;

                orderCounter -= 1;
            }
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    #endregion

    #region Back-end methods

    public void SelectOrder()
    {
        if (CheckMaintenanceOrder())
        {
            string[] s = GetOperations(ordersList[counter]);

            StartOrder.ActiveOrder = new Orders
            {
                Type = ordersList[counter].Type,
                OrderCode = ordersList[counter].OrderCode,
                StartDate = ordersList[counter].StartDate,
                FinishDate = ordersList[counter].FinishDate,
                TotalTime = ordersList[counter].TotalTime,
                Approval = ordersList[counter].Approval,
                ApprovalDate = ordersList[counter].ApprovalDate
            };

            StartOrder.ActiveOrder = ordersList[counter];
            OMCode = ordersList[counter].OrderCode;
            ChangeMaintenanceOrderSituation();

            SceneManager.LoadScene("Operations");
        }
        else
        {
            GetOrdersByUser();
            SetFirstOrderOnPanel();

            failurePlate.SetActive(true);
        }
    }

    private bool CheckMaintenanceOrder()
    {
        var aux = true;

        // Connecting to database
        using SqlConnection connection = new SqlConnection(ConnectionString.stringBuilder.ConnectionString);
        connection.Open();

        // Get all users
        string sql = "SELECT Situacao FROM OrdensDeManutencao WHERE cod_ordemdemanutencao = @maintenanceorder_code;";

        // Reading all rows from the steps
        using SqlCommand command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@maintenanceorder_code", ordersList[counter].OrderCode);

        using SqlDataReader reader = command.ExecuteReader();
        if (reader.Read())
        {
            var situation = Convert.ToInt32(reader[0]);

            switch (situation)
            {
                case 0:
                    aux = true;
                    break;

                case 1:
                    aux = false;
                    break;

                case 2:
                    aux = false;
                    break;

                case 3:
                    aux = false;
                    break;
            }
        }

        return aux;
    }

    private void ChangeMaintenanceOrderSituation()
    {
        try
        {
            // Connecting to database
            using SqlConnection connection = new SqlConnection(ConnectionString.stringBuilder.ConnectionString);
            connection.Open();
            
            Debug.Log("Código da ordem: " + StartOrder.ActiveOrder.OrderCode);
            // Get all users
            string sql = "UPDATE OrdensDeManutencao SET situacao = 1 WHERE cod_ordemdemanutencao = @maintenanceorder_code";

            // Reading all rows from the steps
            using SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@maintenanceorder_code", ordersList[counter].OrderCode);
            Debug.Log("Ordem de Manutenção já foi alterada.");
            command.ExecuteNonQuery();
        }
        catch(Exception e)
        {
            Debug.Log("Deu erro na troca de situação da ordem: " + e.ToString());
        }
    }

    private string[] GetOperations(Orders order)
    {
        string[] opArray = order.OrderCode.Split('/');

        return opArray;
    }

    #endregion
}
