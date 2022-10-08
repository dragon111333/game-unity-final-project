using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform character;
    public  float zoomSpeed = -0.5f;
    public float distant;
    public bool topView = true;

    public float zoomMin = 30.0f;
    public float zoomMax = 75.0f;

    private Transform Camera;

    private void Start()
    {
        distant = this.gameObject.transform.position.y;
        Camera = this.gameObject.transform;
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
                                            , character.position.z - 5f
                                        );
    }

    private void Zoom()
    {
        float mouseScrollValue = Input.GetAxis("Mouse ScrollWheel");
        float currentAngleX = this.gameObject.transform.eulerAngles.x;

        if (mouseScrollValue != 0f)
        {
            if ((mouseScrollValue > 0f) && (currentAngleX <= zoomMax)) distant++;
             if ((mouseScrollValue < 0f) && (currentAngleX >= zoomMin) ) distant--;
            
            this.gameObject.transform.LookAt(character.Find("Renderer_Head"));
        }

    }
}
