using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class Character : MonoBehaviour
{

    public float rotateSpeed = 0.5f;

    public static Animator anim;
    public bool useThirdPeroncamp = true;

    public GameObject thirdPersonCamera;
    public GameObject topViewCamera;

    private Transform character;

    private delegate float RotateValue();

    private GlobalScript globalScript;

    public Material oldMatSelected;

    public AudioClip runSound;
    private AudioSource loopSound = null;


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

            this.SwitchCameraListenner();

            this.Move();

            this.JumpListenner();
            this.InteractiveListenner();
            this.OpenMenuListenner();

            this.CheckHit();
            this.Harvest();

            this.Attack();
            this.HideMouse();

        }

        this.ClearMenuListenner();
    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0)) 
        anim.Play("SwordAttack");
    }

    private void InteractiveListenner()
    {
        if (Input.GetKeyDown("e")) {
            //anim.Play("Doing");
            this.oldMatSelected = globalScript.WaterDirt();
            globalScript.UpgradeCrew();

        }
        if (Input.GetKeyUp("e")) anim.Play("Standing");
    }

    public void SwitchCameraListenner() {
        if (Input.GetKeyUp("v")) {
            this.useThirdPeroncamp = !this.useThirdPeroncamp;
            this.topViewCamera.SetActive(!this.useThirdPeroncamp);
            this.thirdPersonCamera.SetActive(this.useThirdPeroncamp);
        }
    }

    private void Move() {
        if (useThirdPeroncamp) this.MoveListennerWithMouse();
        else this.MoveLisntennerKeyboardOnly();
    }

    private void MoveListennerWithMouse()
    {

        if (Input.GetKeyDown("w")) {

            anim.Play("Walk");

            if (loopSound == null)
            {
                loopSound = this.gameObject.AddComponent<AudioSource>();
                loopSound.clip = runSound;
                loopSound.spatialBlend = 1f;
                loopSound.volume = 100f;
                loopSound.loop = true;
            }
            loopSound.Play();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)) anim.Play("Run");

        if (Input.GetKeyDown("s")) anim.Play("TurnBack");

        if (Input.GetKeyUp("w") || Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp("s")) {
            loopSound.Stop();
            anim.Play("StopRun");
        };

        this.gameObject.transform.localEulerAngles = new Vector3(0,Input.mousePosition.x*this.rotateSpeed,0);
    }

    private void MoveLisntennerKeyboardOnly()
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

            if (loopSound == null)
            {
                loopSound = this.gameObject.AddComponent<AudioSource>();
                loopSound.clip = runSound;
                loopSound.spatialBlend = 1f;
                loopSound.volume = 100f;
                loopSound.loop = true;
            }
            loopSound.Play();
        }

        //stop animation
        if (Input.GetKeyUp("w") || Input.GetKeyUp("a") || Input.GetKeyUp("s") || Input.GetKeyUp("d")) {
            anim.Play("StopRun");
            loopSound.Stop();
        }

    }

    private void JumpListenner()
    {
        if(Input.GetKey("space")) anim.Play("Rolling");
    }

    public void HideMouse()
    {
        if (Input.GetMouseButtonDown(1)) Cursor.visible = !Cursor.visible;
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

        globalScript.buyCrewText.enabled
                    = (globalScript.hitTrader.Count() > 0 && globalScript.hitTrader.Last().tag == "trader");

        globalScript.harvestText.enabled =
            (globalScript.hitPlant.Count() > 0 && globalScript.hitPlant.Last().tag == "plant_p_3");

    }

    private void ClearMenuListenner()
    {
        if (Input.GetKeyDown("escape")) globalScript.ClearAllMenu();
    }

    private void OnTriggerEnter(Collider col) {


/*        if (col.GetComponent<Renderer>() != null) {
            this.oldMatSelected = col.GetComponent<Renderer>().material;
            col.gameObject.GetComponent<Renderer>().material = globalScript.selectedMat;
        }*/
       
        if(col.gameObject.tag == "dirt") globalScript.characterHitOn.Add(col.gameObject);
        if (col.gameObject.tag == "plant_p_3") globalScript.hitPlant.Add(col.gameObject);
        if (col.gameObject.tag == "trader") globalScript.hitTrader.Add(col.gameObject);
    }

    private void OnTriggerExit(Collider col)
    {
     /*   if (col.GetComponent<Renderer>() != null) 
            col.gameObject.GetComponent<Renderer>().material = this.oldMatSelected;*/

        if (col.gameObject.tag == "dirt") globalScript.characterHitOn.Remove(col.gameObject);
        if (col.gameObject.tag == "plant_p_3") globalScript.hitPlant.Remove(col.gameObject);
        if (col.gameObject.tag == "trader") globalScript.hitTrader.Remove(col.gameObject);

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
                globalScript.harvestText.enabled = false;

        }    
    }
}
