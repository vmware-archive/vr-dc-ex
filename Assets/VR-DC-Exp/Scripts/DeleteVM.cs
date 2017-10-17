using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteVM : MonoBehaviour {

	void OnTriggerEnter(Collider VM){
		if (VM.gameObject.tag == "VM") {
			//other.gameObject.GetComponent<Renderer>().enabled = false;
			Destroy(VM, 1);
		}
	}
}
