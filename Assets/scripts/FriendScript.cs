using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendScript : MonoBehaviour
{

    public Transform target;

    public float speed = 0.005f;
    public float distant = 2.0f;
    public Animator anim;

    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveToTarget();
    }

    public void MoveToTarget()
    {

        if(Vector3.Distance(this.transform.position, target.position) > distant)
        {
            this.gameObject.transform.LookAt(target);
            //this.gameObject.transform.Translate(0, 0, speed);
            anim.Play("Walk");

        }
        else
        {
            anim.Play("Standing");
        }
       

    }
}
