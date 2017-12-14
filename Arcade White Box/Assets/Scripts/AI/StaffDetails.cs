using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaffDetails : MonoBehaviour {

    private Staff janitor;

    [SerializeField] private Text staffName, staffRole;
    [SerializeField] private Image staffIcon;

    void Awake()
    {
        janitor = GameManager.Instance.ScriptHolderLink.GetComponent<Janitor>();
    }

    public void AssignStaffMember(Staff staffMember)
    {

        staffName.text = staffMember.Name1;
        staffRole.text = staffMember.staffType.ToString();
        staffIcon.sprite = staffMember.Icon;

    }

}
