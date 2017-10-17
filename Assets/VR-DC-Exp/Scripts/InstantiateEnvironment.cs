// Copyright ©2017 VMware, Inc. All Rights Reserved.
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InstantiateEnvironment : MonoBehaviour {

	public GameObject SourceVMObject;
	public GameObject SourcePedistal;
	public GameObject Trashcan;
	public GameObject ConnectionMsg;

    // Use this for initialization
    void Start()
	{
		RESTConnection n = new RESTConnection();
		n.Start();

		GameObject CloneVM;
		GameObject ClonedPedistal;
		UnityEngine.UI.Text[] VMDetails;
        float XPosition;
		float YPosition;
		float ZPosition;
		float OriginalYPosition;
		float HostXPosition;
		float HostYPosition;
		float HostZPosition;
		float HostOriginalYPosition;
		float HostOriginalZPosition;

		HostXPosition = -2f;
		HostYPosition = 0f;
		HostZPosition = 5f;

		HostOriginalYPosition = HostYPosition;
		HostOriginalZPosition = HostZPosition;

		XPosition = HostXPosition + -0.25f;
		YPosition = 5.5415f;
		ZPosition = HostZPosition + -0.30f;

		OriginalYPosition = YPosition;

		RESTConnection.HostList vmhosts = n.ListHosts();

		// Iterate through all ESXi hosts
		for (int i = 0; i < vmhosts.value.Count; i++)
		{
			// Name of ESXi host
			string vmhostName = vmhosts.value[i].name;
			// Id of ESXi host
			string vmhostId = vmhosts.value[i].host;
			XPosition = HostXPosition + -0.25f;
			ZPosition = HostZPosition + -0.30f;

			// Clone the SourceObject for our New Pedistal
			ClonedPedistal = Instantiate(SourcePedistal);

			ClonedPedistal.transform.position = new Vector3(HostXPosition, HostOriginalYPosition, HostOriginalZPosition);
			ClonedPedistal.name = vmhostName;

			// Change the name of the Pedestal Text
			ClonedPedistal.GetComponentInChildren<Text>().text = vmhostName;

			// Retrieve VMs from ESXi host
			VmList vms = n.ListVmsByHostId(vmhostId);
			for (int j = 0; j < vms.value.Count; j++) {
				vapitypes.VmDetails details = n.GetVmDetails(vms.value[j].vm);
				string vmDetails = n.GetVmDetailsStr(details);
				string vmName = vms.value[j].name;

				// Clone the SourceObject for our New VM
				CloneVM = Instantiate(SourceVMObject);
				CloneVM.transform.SetParent(ClonedPedistal.transform);
				CloneVM.transform.position = new Vector3(XPosition, OriginalYPosition, ZPosition);

				CloneVM.name = vmName;

				// Fill in the Details on the Summary canvas
				VMDetails = CloneVM.GetComponentsInChildren<UnityEngine.UI.Text>();
				VMDetails[0].text = vmName;
				VMDetails[1].text = vmDetails;
           

                //CloneVM.GetComponents();

                // Set the location of the cloned VM
                XPosition += 0.20f;

				if ((j + 1) % 4 == 0)
				{
					XPosition = HostXPosition + -0.25f;
					ZPosition += 0.15f;
				}
				//Debug.Log("VM: " + vmDetails);
			}
			HostXPosition = HostXPosition + 1.5f;
		}

	}

	// Update is called once per frame
	void Update () {
		Trashcan.transform.position = new Vector3(((this.transform.position.x) + 1), (this.transform.position.y), ((this.transform.position.z)));
	}
		
}