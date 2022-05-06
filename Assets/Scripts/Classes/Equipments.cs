using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipments : MonoBehaviour
{
    #region Properties

    public int ID { get; set; }
    public string EquipmentCode { get; set; }
    public string MachineGroupCode { get; set; }
    public string WelfareCOde { get; set; }
    public string LocalCode { get; set; }
    public string CostCenterCode { get; set; }
    public string DesignCode { get; set; }
    public string RevisionCode { get; set; }
    public string TagCode { get; set; }
    public string FBXCode { get; set; }
    public string TypeCode { get; set; }
    public string Description { get; set; }
    public string Situation { get; set; }
    public string Supplier { get; set; }
    public string SupplierContact { get; set; }
    public string NF { get; set; }
    public string NFSerie { get; set; }
    public DateTime PurchaseDate { get; set; }
    public int PurchasePrice { get; set; }
    public string Producer { get; set; }
    public byte Criticality { get; set; }

    #endregion
}
