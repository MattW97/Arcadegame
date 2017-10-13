using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffMenuUI : MonoBehaviour {

    [SerializeField]
    private GameObject workerMenu, chefsMenu, techniciansMenu, janitorMenu;

    public void WorkerMenu()
    {
        if (workerMenu.activeInHierarchy)
        {
            workerMenu.SetActive(false);
        }
        else
        {
            workerMenu.SetActive(true);
        }
    }

    public void ChefsMenu()
    {
        if (chefsMenu.activeInHierarchy)
        {
            chefsMenu.SetActive(false);
        }
        else
        {
            chefsMenu.SetActive(true);
        }
    }

    public void TechniciansMenu()
    {
        if (techniciansMenu.activeInHierarchy)
        {
            techniciansMenu.SetActive(false);
        }
        else
        {
            techniciansMenu.SetActive(true);
        }
    }

    public void JanitorMenu()
    {
        if (janitorMenu.activeInHierarchy)
        {
            janitorMenu.SetActive(false);
        }
        else
        {
            janitorMenu.SetActive(true);
        }
    }

    public void DeactivateAllStaffSubMenus()
    {
        workerMenu.SetActive(false);
        chefsMenu.SetActive(false);
        techniciansMenu.SetActive(false);
        janitorMenu.SetActive(false);
    }
  
}
