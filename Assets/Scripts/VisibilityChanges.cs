using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityChanges : MonoBehaviour
{
    [SerializeField] private GameObject handMenu;
    [SerializeField] private GameObject userMenu;

    public void SetMenuVisible()
    {
        userMenu.transform.position = new Vector3(handMenu.transform.position.x, handMenu.transform.position.y + 0.2f, handMenu.transform.position.z);

        if (userMenu.activeInHierarchy)
        {
            userMenu.SetActive(false);
        }
        else
        {
            userMenu.SetActive(true);
        }
        
    }

    public void DisableGameObjectParent()
    {
        gameObject.transform.parent.transform.gameObject.SetActive(false);
    }

    public void CloseApp()
    {
        Application.Quit();
    }
}
