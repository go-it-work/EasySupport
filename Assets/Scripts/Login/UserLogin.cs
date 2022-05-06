using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserLogin : MonoBehaviour
{
    private static List<Users> userList = new List<Users>();
    public DataTable usersTable = new DataTable();
    public static DataTable usersSTable = new DataTable();

    // User name on screen
    [SerializeField] private TextMeshPro buttonOne;
    [SerializeField] private TextMeshPro buttonTwo;
    [SerializeField] private TextMeshPro buttonThree;
    [SerializeField] private TextMeshPro buttonFour;
    [SerializeField] private TextMeshPro buttonFive;
    [SerializeField] private TextMeshPro buttonSix;
    [SerializeField] private TextMeshPro buttonSeven;
    [SerializeField] private TextMeshPro buttonEight;
    [SerializeField] private TextMeshPro buttonNine;

    [SerializeField] private Text buttonOneText;
    [SerializeField] private Text buttonTwoText;
    [SerializeField] private Text buttonThreeText;
    [SerializeField] private Text buttonFourText;
    [SerializeField] private Text buttonFiveText;
    [SerializeField] private Text buttonSixText;
    [SerializeField] private Text buttonSevenText;
    [SerializeField] private Text buttonEightText;
    [SerializeField] private Text buttonNineText;

    [SerializeField] private GameObject upButton;
    [SerializeField] private GameObject downButton;

    [SerializeField] private GameObject userMenu;
    [SerializeField] private GameObject orderMenu;

    [SerializeField] private GameObject errorPlate;
    [SerializeField] private static TextMeshPro debug;
    [SerializeField] private TextMeshPro errorMessage;

    public static string UserCode { get; set; }

    private int counter = -1;

    void Start()
    {
        SetUpScene();
    }

    private void SetUpScene()
    {
        // Setting up scene
        counter = -1;
        userList.Clear();

        upButton.SetActive(true);
        downButton.SetActive(false);

        errorPlate.SetActive(false);

        // Setting up the plates and menus
        if (!userMenu.activeInHierarchy)
            userMenu.SetActive(true);

        // Get all the users on database
        GetAllUsers();
    }

    #region Getting data from DB

    public async void GetConnect()
    {
        await OpenConnection();
        Debug.Log("A conexão foi um sucesso e os dados foram tratados.");
    }

    private static async Task OpenConnection()
    {
        using (var conn = new SqlConnection(ConnectionString.stringBuilder.ConnectionString))
        {
            Console.WriteLine("Opening connection");
            await conn.OpenAsync();

            // Get all users
            string sql = "SELECT * FROM Usuarios";

            using (var command = new SqlCommand(sql, conn))
            {
                // Reading all rows from the steps
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    usersSTable.Load(reader);
                }

                SaveUser(usersSTable);
            }

            Debug.Log("Quantidade de usuários cadastrados: " + userList.Count);
            // connection will be closed by the 'using' block
            Console.WriteLine("Closing connection");
        }
    }

    public void GetAllUsers()
    {
        try
        {
            // Connecting to database
            using SqlConnection connection = new SqlConnection(ConnectionString.stringBuilder.ConnectionString);
            connection.Open();

            // Get all users
            string sql = "SELECT * FROM Usuarios";

            // Reading all rows from the steps
            using SqlCommand command = new SqlCommand(sql, connection);
            using SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                usersTable.Load(reader);
            }

            SaveUser(usersTable);

            NextUsers();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            errorPlate.SetActive(true);
            errorMessage.text = e.Message;
        }
    }

    private static void SaveUser(DataTable table)
    {
        // Loop to add all data to a List<Users>
        foreach (DataRow item in table.Rows)
        {
            try
            {
                var user = new Users(item["nome"].ToString(), item["cod_usuario"].ToString(),
                    item["cod_grupousuarios"].ToString());

                Debug.Log("Usuário: " + item["nome"].ToString());
         
                userList.Add(user);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }
    }

    #endregion
    
    #region Dynamic Users's list

    // Active or deactivate buttons using the list's size as parameter
    private void ChangeButtonsState()
    {
        switch (userList.Count)
        {
            case 1:
                GetParent(buttonOne).SetActive(true);
                GetParent(buttonTwo).SetActive(false);
                GetParent(buttonThree).SetActive(false);
                GetParent(buttonFour).SetActive(false);
                GetParent(buttonFive).SetActive(false);
                GetParent(buttonSix).SetActive(false);
                GetParent(buttonSeven).SetActive(false);
                GetParent(buttonEight).SetActive(false);
                GetParent(buttonNine).SetActive(false);
                break;

            case 2:
                GetParent(buttonOne).SetActive(true);
                GetParent(buttonTwo).SetActive(true);
                GetParent(buttonThree).SetActive(false);
                GetParent(buttonFour).SetActive(false);
                GetParent(buttonFive).SetActive(false);
                GetParent(buttonSix).SetActive(false);
                GetParent(buttonSeven).SetActive(false);
                GetParent(buttonEight).SetActive(false);
                GetParent(buttonNine).SetActive(false);
                break;

            case 3:
                GetParent(buttonOne).SetActive(true);
                GetParent(buttonTwo).SetActive(true);
                GetParent(buttonThree).SetActive(true);
                GetParent(buttonFour).SetActive(false);
                GetParent(buttonFive).SetActive(false);
                GetParent(buttonSix).SetActive(false);
                GetParent(buttonSeven).SetActive(false);
                GetParent(buttonEight).SetActive(false);
                GetParent(buttonNine).SetActive(false);
                break;

            case 4:
                GetParent(buttonOne).SetActive(true);
                GetParent(buttonTwo).SetActive(true);
                GetParent(buttonThree).SetActive(true);
                GetParent(buttonFour).SetActive(true);
                GetParent(buttonFive).SetActive(false);
                GetParent(buttonSix).SetActive(false);
                GetParent(buttonSeven).SetActive(false);
                GetParent(buttonEight).SetActive(false);
                GetParent(buttonNine).SetActive(false);
                break;

            case 5:
                GetParent(buttonOne).SetActive(true);
                GetParent(buttonTwo).SetActive(true);
                GetParent(buttonThree).SetActive(true);
                GetParent(buttonFour).SetActive(true);
                GetParent(buttonFive).SetActive(true);
                GetParent(buttonSix).SetActive(false);
                GetParent(buttonSeven).SetActive(false);
                GetParent(buttonEight).SetActive(false);
                GetParent(buttonNine).SetActive(false);
                break;

            case 6:
                GetParent(buttonOne).SetActive(true);
                GetParent(buttonTwo).SetActive(true);
                GetParent(buttonThree).SetActive(true);
                GetParent(buttonFour).SetActive(true);
                GetParent(buttonFive).SetActive(true);
                GetParent(buttonSix).SetActive(true);
                GetParent(buttonSeven).SetActive(false);
                GetParent(buttonEight).SetActive(false);
                GetParent(buttonNine).SetActive(false);
                break;

            case 7:
                GetParent(buttonOne).SetActive(true);
                GetParent(buttonTwo).SetActive(true);
                GetParent(buttonThree).SetActive(true);
                GetParent(buttonFour).SetActive(true);
                GetParent(buttonFive).SetActive(true);
                GetParent(buttonSix).SetActive(true);
                GetParent(buttonSeven).SetActive(true);
                GetParent(buttonEight).SetActive(false);
                GetParent(buttonNine).SetActive(false);
                break;

            case 8:
                GetParent(buttonOne).SetActive(true);
                GetParent(buttonTwo).SetActive(true);
                GetParent(buttonThree).SetActive(true);
                GetParent(buttonFour).SetActive(true);
                GetParent(buttonFive).SetActive(true);
                GetParent(buttonSix).SetActive(true);
                GetParent(buttonSeven).SetActive(true);
                GetParent(buttonEight).SetActive(true);
                GetParent(buttonNine).SetActive(false);
                break;

            case 9:
                GetParent(buttonOne).SetActive(true);
                GetParent(buttonTwo).SetActive(true);
                GetParent(buttonThree).SetActive(true);
                GetParent(buttonFour).SetActive(true);
                GetParent(buttonFive).SetActive(true);
                GetParent(buttonSix).SetActive(true);
                GetParent(buttonSeven).SetActive(true);
                GetParent(buttonEight).SetActive(true);
                GetParent(buttonNine).SetActive(true);
                break;
        }
    }

    private GameObject GetParent(TextMeshPro tmp)
    {
        var tmpParent = tmp.transform.parent.gameObject;
        var parent = tmpParent.transform.parent.gameObject;

        return parent;
    }

    private void VerifyAndAssignName(int i, TextMeshPro button, Text txt)
    {
        try
        {
            if(counter + 1 < userList.Count)
            {
                button.text = userList[counter + i].Name;
                txt.text = userList[counter + i].UserGroupCode;

                counter += i;
            }
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private void VerifyAndAssign_Down(int i, TextMeshPro button, Text txt)
    {
        try
        {
            if(counter - 1 < userList.Count)
            {
                button.text = userList[counter - i].Name;
                txt.text = userList[counter - i].UserGroupCode;

                counter -= i;
            }
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void NextUsers()
    {
        ChangeButtonsState();
        downButton.SetActive(true);

        if(counter <= userList.Count())
        {
            // Assigning new names
            VerifyAndAssignName(1, buttonOne, buttonOneText);
            VerifyAndAssignName(1, buttonTwo, buttonTwoText);
            VerifyAndAssignName(1, buttonThree, buttonThreeText);
            VerifyAndAssignName(1, buttonFour, buttonFourText);
            VerifyAndAssignName(1, buttonFive, buttonFiveText);
            VerifyAndAssignName(1, buttonSix, buttonSixText);
            VerifyAndAssignName(1, buttonSeven, buttonSevenText);
            VerifyAndAssignName(1, buttonEight, buttonEightText);
            VerifyAndAssignName(1, buttonNine, buttonNineText);
        }
        else
        {
            upButton.SetActive(false);
        }
    }

    public void PreviousUsers()
    {
        ChangeButtonsState();

        upButton.SetActive(true);
        var auxCounter = userList.Count() - counter;

        if(auxCounter >= -8)
        {
            // Assigning new names
            VerifyAndAssign_Down(1, buttonOne, buttonOneText);
            VerifyAndAssign_Down(1, buttonTwo, buttonTwoText);
            VerifyAndAssign_Down(1, buttonThree, buttonThreeText);
            VerifyAndAssign_Down(1, buttonFour, buttonFourText);
            VerifyAndAssign_Down(1, buttonFive, buttonFiveText);
            VerifyAndAssign_Down(1, buttonSix, buttonSixText);
            VerifyAndAssign_Down(1, buttonSeven, buttonSevenText);
            VerifyAndAssign_Down(1, buttonEight, buttonEightText);
            VerifyAndAssign_Down(1, buttonNine, buttonNineText);
        }
        else
        {
            downButton.SetActive(false);
        }
    }

    #endregion
}
