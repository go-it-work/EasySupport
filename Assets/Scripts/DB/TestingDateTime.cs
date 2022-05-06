using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using UnityEngine;

public class TestingDateTime : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetTimeFromDatabase();
    }

    private void GetTimeFromDatabase()
    {
        // Connecting to database
        using SqlConnection connection = new SqlConnection(ConnectionString.stringBuilder.ConnectionString);
        connection.Open();

        // Get all operations
        string sql = "SELECT * FROM TestingDateTime";

        // Adding all parameters
        using SqlCommand command = new SqlCommand(sql, connection);

        // Execute reader
        using (SqlDataReader reader = command.ExecuteReader())
        {
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Debug.Log("Date: " + Convert.ToDateTime(reader[0]).ToString("dd/MM/yyyy HH:mm") + " & Hour: " + Convert.ToDateTime(reader[0]).ToString("HH:mm:ss"));
                }
            }
        }
    }
}
