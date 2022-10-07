using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform character;
    public  float zoomSpeed = -0.5f;
    public float distant;
    public bool topView = true;


    private void Start()
    {
        character = GameObject.Find("/MainCharacter").transform;
        distant = this.gameObject.transform.position.y;
    }
    void Update()
    {
        this.MoveToCharacter();
        this.Zoom();
    }


    private void MoveToCharacter()
    {
        if (topView) TopView();
    }

    private void TopView()
    {
        this.gameObject.transform.position = new Vector3(
                                            character.position.x
                                            , distant
                                            , character.position.z - 1.9f
                                        );
    }

    private void Zoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            this.gameObject.transform.LookAt(character);
            if (Input.GetAxis("Mouse ScrollWheel") > 0f) distant++;
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f) distant--;
        }
            
    }
}
