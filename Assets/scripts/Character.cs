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

            this.Harvest();

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
        globalScript.plantText.enabled 
                = (globalScript.characterHitOn.Count() > 0 && globalScript.characterHitOn.Last().tag == "dirt");
        globalScript.harvestText.enabled
                = (globalScript.hitPlant.Count() > 0 && globalScript.hitPlant.Last().tag == "plant_p_3") ;

    }

    private void ClearMenuListenner()
    {
        if (Input.GetKeyDown("escape")) globalScript.ClearAllMenu();
    }

    private void OnTriggerEnter(Collider col) {


        if (col.GetComponent<Renderer>() != null) {
            this.oldMatSelected = col.GetComponent<Renderer>().material;
            col.gameObject.GetComponent<Renderer>().material = globalScript.selectedMat;
        }
       
        if(col.gameObject.tag == "dirt") globalScript.characterHitOn.Add(col.gameObject);
        if (col.gameObject.tag == "plant_p_3") globalScript.hitPlant.Add(col.gameObject); 
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.GetComponent<Renderer>() != null) 
            col.gameObject.GetComponent<Renderer>().material = this.oldMatSelected;

        if (col.gameObject.tag == "dirt") globalScript.characterHitOn.Remove(col.gameObject);
        if (col.gameObject.tag == "plant_p_3") globalScript.hitPlant.Remove(col.gameObject);

    }

    private void Harvest()
    {
        if (Input.GetKeyDown("f")
           && globalScript.hitPlant.Count() > 0 
           && globalScript.hitPlant.Last().tag == "plant_p_3"
          ) {

                int index = globalScript.hitPlant.Count() - 1;

                string[] pos = globalScript.hitPlant.Last().name.Split("_");

                int x = int.Parse(pos[0]);
                int y = int.Parse(pos[1]);

                float price = globalScript.hitPlant.Last().GetComponent<PlantDetail>().price;
                globalScript.coin.text = (float.Parse(globalScript.coin.text) + price).ToString();


                globalScript.plantInfo[x][y]["plantName"] = "";
                globalScript.plantInfo[x][y]["plant"] = null;
                globalScript.plantInfo[x][y]["time"] = 0;
                globalScript.plantInfo[x][y]["phase"] = 0;


                Destroy(globalScript.hitPlant.Last().gameObject);
                globalScript.hitPlant.RemoveAt(index);

            
        }    
    }
}
