using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class Orders : MonoBehaviour
{
    #region Properties

    public string Type { get; set; }
    public string Description { get; set; }
    public string OrderCode { get; set; }
    public string EquipmentCode { get; set; }
    public string Operations { get; set; }
    public string UsergroupCode { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime DeadlineDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }
    public float TotalTime { get; set; }
    public int ManPowerCost { get; set; }
    public int MaterialCost { get; set; }
    public int ExtraCost { get; set; }
    public bool Approval { get; set; }
    public string UserApproval { get; set; }
    public string ApprovalDate { get; set; }
    public byte Haste { get; set; }
    public byte Situation { get; set; }

    #endregion

    #region Constructors

    public Orders()
    {

    }

    public Orders(string type, string description, string orderCode, string equipmentCode,
        DateTime orderDate, DateTime deadlineDate,
        string userApproval, byte haste, byte situation)
    {
        this.Description = description;
        this.Type = type;
        this.OrderCode = orderCode;
        this.EquipmentCode = equipmentCode;
        this.OrderDate = orderDate;
        this.DeadlineDate = deadlineDate;
        this.UserApproval = userApproval;
        this.Haste = haste;
        this.Situation = situation;
    }

    #endregion

    #region Methods

    public string[] GetOperationsArray()
    {
        string[] strArray = this.Operations.Split('/');

        return strArray;
    }

    #endregion
}
