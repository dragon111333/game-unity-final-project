using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Character : MonoBehaviour
{

    public static Animator anim;
    private Transform character;
    private delegate float RotateValue();

    // Main method

    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
        this.character = this.gameObject.transform;
    }

    void Update()
    {
        this.MoveLisntenner();
        this.JumpListenner();
        this.InteractiveListenner();
        //this.OnMouseDrag();
    }

    private void InteractiveListenner()
    {
        if (Input.GetKeyDown("e")) anim.Play("Doing");
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
            anim.Play("Standing");
    }

    private void JumpListenner()
    {
        if(Input.GetKey("space")) anim.Play("Jump");
    }


    float rotSpeed = 20;
    void OnMouseDrag()
    {
        float rotX = Input.GetAxis("Mouse X") * rotSpeed * Mathf.Deg2Rad;
        float rotY = Input.GetAxis("Mouse Y") * rotSpeed * Mathf.Deg2Rad;

        character.Rotate(Vector3.right, rotY);
    }
}
