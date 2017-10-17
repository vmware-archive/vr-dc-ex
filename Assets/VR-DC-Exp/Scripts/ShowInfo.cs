using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using vapitypes;

public class ShowInfo : MonoBehaviour {

    UnityEngine.UI.Text[] VMDetails;

    // Use this for initialization

    void Start () {
        VMDetails = this.GetComponentsInChildren<UnityEngine.UI.Text>();
        VMDetails[1].enabled = false;
    }
	
	// Update is called once per frameo = 
	void Update () {
		
	}

    public void OnTriggerEnter()
    {
        VMDetails = this.GetComponentsInChildren<UnityEngine.UI.Text>();
        VMDetails[1].enabled = true;
    }


    public void OnTriggerExit()
    {
        VMDetails = this.GetComponentsInChildren<UnityEngine.UI.Text>();
        VMDetails[1].enabled = false;
    }
}

