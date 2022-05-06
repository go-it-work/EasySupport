using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Users : MonoBehaviour
{
    #region Properties

    public string Name { get; set; }
    public string UserCode { get; set; }
    public string UserGroupCode { get; set; }

    #endregion

    #region Constructors

    public Users(string name, string userCode, string userGroupCode)
    {
        this.Name = name;
        this.UserCode = userCode;
        this.UserGroupCode = userGroupCode;
    }

    #endregion
}

