using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using vapitypes;

public class ShowInfo : MonoBehaviour, HUD.IHudVisible {
    public string name;
    public string type;
    public List<HUD.Resource> resources;
    public List<HUD.Statistic> statistics;
    public Dictionary<string, string> tags;
    public HUD.ColorFamily colorFamily;

    private UnityEngine.UI.Text[] VMDetails;
    private HUD HUD;
    private UI UI;

    // Use this for initialization

    void Start () {
        VMDetails = this.GetComponentsInChildren<UnityEngine.UI.Text>();
		VMDetails[1].enabled = false;

        HUD = GameObject.Find("HUD").GetComponent<HUD> ();
        UI = GameObject.Find("GUI").GetComponent<UI> ();

    }
	
	// Update is called once per frameo = 
	void Update () {
		
	}

    public void OnTriggerEnter()
    {
        VMDetails = this.GetComponentsInChildren<UnityEngine.UI.Text>();
		VMDetails[0].enabled = false;
		VMDetails[1].enabled = true;

        UI.transform.position = transform.position;
        UI.transform.SetParent(transform);
        HUD.updateHud(this);
    }


    public void OnTriggerExit()
    {
        VMDetails = this.GetComponentsInChildren<UnityEngine.UI.Text>();
        VMDetails[0].enabled = true;
		VMDetails[1].enabled = false;

        HUD.updateHud(null);

    }

    // IHudVisible implementation
    public string Name { get { return name; } }
    public string Type { get { return type; } }
    public HUD.ColorFamily ColorFamily { get { return colorFamily; } }
    public List<HUD.Statistic> Statistics { get { return statistics; } }
    public List<HUD.Resource> Resources { get { return resources; } }
    public Dictionary<string, string> Tags { get { return tags; } }
}

