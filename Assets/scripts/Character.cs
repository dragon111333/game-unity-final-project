using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class Character : MonoBehaviour
{

    public static Animator anim;
    private Transform character;
    private delegate float RotateValue();

    private GlobalScript globalScript;

    public Material oldMatSelected;

    
    // Main method
    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
        this.character = this.gameObject.transform;

        globalScript = GameObject.FindObjectOfType<GlobalScript>();
    }

    void Update()
    {
        if (globalScript.enableCharacterMove) {
            this.MoveLisntenner();
            this.JumpListenner();
            this.InteractiveListenner();
            this.OpenMenuListenner();

            this.CheckHit();

        }

        this.ClearMenuListenner();
    }

    private void InteractiveListenner()
    {
        if (Input.GetKeyDown("e")) {
            anim.Play("Doing");

            this.oldMatSelected = globalScript.WaterDirt();
        }
        if (Input.GetKeyUp("e")) anim.Play("Standing");
    }

    private void MoveLisntenner()
    {

        RotateValue rv = delegate ()
        {
            if (Input.GetKeyDown("w")) return 0;
            else if (Input.GetKeyDown("a")) return 270;
            else if (Input.GetKeyDown("s")) return 180;
            else if (Input.GetKeyDown("d")) return 90;
            return 0;
        };


        if (Input.GetKeyDown("w") || Input.GetKeyDown("a") || Input.GetKeyDown("s") || Input.GetKeyDown("d"))
        {

            character.eulerAngles = new Vector3(character.transform.eulerAngles.x, rv(), character.eulerAngles.z);
            anim.Play("Run");
        }

        //stop animation
        if (Input.GetKeyUp("w") || Input.GetKeyUp("a") || Input.GetKeyUp("s") || Input.GetKeyUp("d"))
            anim.Play("StopRun");
    }

    private void JumpListenner()
    {
        if(Input.GetKey("space")) anim.Play("Jump");
    }

    private void OpenMenuListenner()
    {

        if (Input.GetKeyDown("r") && globalScript.characterHitOn.Count() > 0)
        {
                switch (globalScript.characterHitOn.Last().tag)
                {
                    case "dirt":
                        globalScript.SwitchPlantMenu(true);
                        break;                  
                    default: 
                        //-----
                        break;
                }
        }

    }

    private void CheckHit()
    {
        if (globalScript.characterHitOn.Count > 0)
        {
            //----- HIT
            GameObject hitAt = globalScript.characterHitOn.Last();
          //  print("Hit on ->"+ hitAt.name);

            if (hitAt.tag == "dirt") globalScript.plantText.enabled = true;
        }
        else globalScript.ClearAllLabel();

    }

    private void ClearMenuListenner()
    {
        if (Input.GetKeyDown("escape")) globalScript.ClearAllMenu();
    }

    private void OnTriggerEnter(Collider col) {

        this.oldMatSelected = col.GetComponent<Renderer>().material;

        col.gameObject.GetComponent<Renderer>().material = globalScript.selectedMat;
        globalScript.characterHitOn.Add(col.gameObject);

    }

    private void OnTriggerExit(Collider col)
    {
        col.gameObject.GetComponent<Renderer>().material = this.oldMatSelected;
        globalScript.characterHitOn.Remove(col.gameObject);
    }
}
