using UnityEngine;
using TMPro;

public class SetUsername : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro username;

    public void Start()
    {
        username.text = "Olá, " + GetLoginParameters.UserGoupCode;
    }
}
