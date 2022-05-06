using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using MySql.Data;

public class ConnectionString : MonoBehaviour
{
    public static MySqlConnectionStringBuilder stringBuilder = new MySqlConnectionStringBuilder
    {
        Server = "goit2.database.windows.net",
        Database = "EasySupport",
        UserID = "goit-db-server",
        Password = "@servidorBD2022!",
    };
}
