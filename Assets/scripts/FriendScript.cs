using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendScript : MonoBehaviour
{

    public Transform target;

    public int health = 2;

    public float speed = 0.005f;
    public float distant = 2.0f;
    public Animator anim;

    public Transform allEnemy;

    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (allEnemy.childCount > 0)
            AttactEnemy();
        else
            MoveToTarget();

        Die();
    }


    private void Die()
    {
        if (this.health <= 0) Destroy(this.gameObject);
    }

private void AttactEnemy()
    {
        this.transform.LookAt(allEnemy.GetChild(0).transform);
        anim.Play("Run");
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
    private void OnCollisionEnter(Collision other)
    {
        print("Friend hit ->" + other.gameObject.tag);

        if (other.gameObject.tag == "enemy")
        {
           other.gameObject.GetComponent<Enemy>().health -= 1;
        }
    }
}
