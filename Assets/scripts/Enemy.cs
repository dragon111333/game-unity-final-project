using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int health = 20;
    public Transform target;
    public Text targetLife;
    public Animator anim;

    public SoundControl sc;

    private bool Running = true;

    private void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
    }
    void Update()
    {
        GoToTarget();
        Die();
    }

    private void Die()
    {
        if (this.health <= 0) 
        {
            sc.PlayMonsterDie();
            Destroy(this.gameObject);
        }
    }

    private void GoToTarget()
    {
        if (Running)
        {
            this.gameObject.transform.LookAt(target);
            anim.Play("Go");
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        print("Enemy hit ->" + collision.gameObject.tag);

        if (collision.gameObject.tag == "friend") {
            {
              collision.gameObject.GetComponent<FriendScript>().health -= 1;
            }
        }
        if (collision.gameObject.tag == "Player") {
            targetLife.text = (int.Parse(targetLife.text.ToString()) - 2).ToString();
        }
      
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.tag);
        if (other.gameObject.tag == "wepon")  this.health -= 2;
    }

}

