using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAppLogo : MonoBehaviour {
    public Texture[] myTextures = new Texture[3];

    void Start()
    {
        var randapp = Random.Range(0, 3);
        if (this.transform.parent.transform.parent.name == "ON-PREMVM01")
        {
            this.GetComponent<Renderer>().material.mainTexture = myTextures[1];
        }
        else
        {
            this.GetComponent<Renderer>().material.mainTexture = myTextures[randapp];
        }

    }


    void Update()
    {

    }
  }
