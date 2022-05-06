using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GetLoginParameters : MonoBehaviour
{
    [SerializeField] private GameObject userMenu;
    [SerializeField] private GameObject listMenu;
    [SerializeField] private GameObject infoPlate;

    public static string GlobalUsername;

    public static string Username { get; set; }
    public static string UserGoupCode { get; set; }

    public void SetUsername()
    {
        Username = this.gameObject.GetComponent<TextMeshPro>().text;
        GlobalUsername = this.gameObject.GetComponent<TextMeshPro>().text;
        UserGoupCode = this.gameObject.transform.parent.GetComponent<Text>().text;

        userMenu.SetActive(false);
        listMenu.SetActive(true);
        infoPlate.SetActive(false);

        listMenu.transform.position = new Vector3(0, 0.0928f, 0.4331f);
    }
}
