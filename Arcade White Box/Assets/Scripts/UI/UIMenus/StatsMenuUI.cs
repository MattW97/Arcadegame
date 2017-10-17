using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsMenuUI : MonoBehaviour {

    [SerializeField]
    private GameObject menu1, menu2, menu3, menu4;

    private PlayerManager _playerLink;

    // Use this for initialization
    void Start () {
        _playerLink = GameManager.Instance.SceneManagerLink.GetComponent<PlayerManager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DeactivateAllStatsMenus()
    {
        menu1.SetActive(false);
        menu2.SetActive(false);
        menu3.SetActive(false);
        menu4.SetActive(false);
    }

    public void Menu1()
    {
        if (menu1.activeInHierarchy)
        {
            menu1.SetActive(false);
        }
        else
        {
            menu1.SetActive(true);
        }
    }

    public void Menu2()
    {
        if (menu2.activeInHierarchy)
        {
            menu2.SetActive(false);
        }
        else
        {
            menu2.SetActive(true);
        }
    }

    public void Menu3()
    {
        if (menu3.activeInHierarchy)
        {
            menu3.SetActive(false);
        }
        else
        {
            menu3.SetActive(true);
        }
    }

    public void Menu4()
    {
        if (menu4.activeInHierarchy)
        {
            menu4.SetActive(false);
        }
        else
        {
            menu4.SetActive(true);
        }
    }

}
