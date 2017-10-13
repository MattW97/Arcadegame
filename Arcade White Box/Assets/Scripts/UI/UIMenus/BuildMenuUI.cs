using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMenuUI : MonoBehaviour {

    [SerializeField]
    private GameObject buildMenu, editMenu, staffMenu, statsMenu, optionsMenu;

    [SerializeField]
    private GameObject joystickMenu, friesMenu, stoolMenu, weirdThingAtTheBottomMenu, advisorMenu;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void BuildMenu()
    {
        if (buildMenu.activeInHierarchy)
        {
            buildMenu.SetActive(false);
        }
        else
        {
            buildMenu.SetActive(true);
        }
    }

    public void EditMenu()
    {
        if (editMenu.activeInHierarchy)
        {
            editMenu.SetActive(false);
        }
        else
        {
            editMenu.SetActive(true);
        }
    }

    public void StaffMenu()
    {
        if (staffMenu.activeInHierarchy)
        {
            staffMenu.SetActive(false);
        }
        else
        {
            staffMenu.SetActive(true);
        }
    }

    public void StatsMenu()
    {
        if (statsMenu.activeInHierarchy)
        {
            statsMenu.SetActive(false);
        }
        else
        {
            statsMenu.SetActive(true);
        }
    }

    public void OptionsMenu()
    {
        if (optionsMenu.activeInHierarchy)
        {
            optionsMenu.SetActive(false);
        }
        else
        {
            optionsMenu.SetActive(true);
        }
    }

    public void JoyStickMenu()
    {
        if (joystickMenu.activeInHierarchy)
        {
            joystickMenu.SetActive(false);
        }
        else
        {
            joystickMenu.SetActive(true);
        }
    }

    public void FriesMenu()
    {
        if (friesMenu.activeInHierarchy)
        {
            friesMenu.SetActive(false);
        }
        else
        {
            friesMenu.SetActive(true);
        }
    }

    public void StoolMenu()
    {
        if (stoolMenu.activeInHierarchy)
        {
            stoolMenu.SetActive(false);
        }
        else
        {
            stoolMenu.SetActive(true);
        }
    }

    public void WeirdThingMenu()
    {
        if (weirdThingAtTheBottomMenu.activeInHierarchy)
        {
            weirdThingAtTheBottomMenu.SetActive(false);
        }
        else
        {
            weirdThingAtTheBottomMenu.SetActive(true);
        }
    }

    public void AdvisorMenu()
    {
        if (advisorMenu.activeInHierarchy)
        {
            advisorMenu.SetActive(false);
        }
        else
        {
            advisorMenu.SetActive(true);
        }
    }

    public void DeactivateAllBuildSubMenus()
    {
        joystickMenu.SetActive(false);
        friesMenu.SetActive(false);
        stoolMenu.SetActive(false);
        weirdThingAtTheBottomMenu.SetActive(false);

    }

    public void DeactivateAllMenus()
    {
        buildMenu.SetActive(false);
        editMenu.SetActive(false);
        staffMenu.SetActive(false);
        statsMenu.SetActive(false);
        optionsMenu.SetActive(false);
        DeactivateAllBuildSubMenus();
        this.GetComponent<StaffMenuUI>().DeactivateAllStaffSubMenus();
        
    }
}
