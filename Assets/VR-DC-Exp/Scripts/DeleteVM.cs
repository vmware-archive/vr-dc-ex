using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteVM : MonoBehaviour {

	void OnTriggerEnter(Collider VM){
		if (VM.gameObject.tag == "VM") {
            //Debug.Log("Destroying " + VM.gameObject.name);
            VM.gameObject.GetComponent<Renderer>().enabled = false;
            // GameObject.Destroy(VM.gameObject);
        }
	}
}
