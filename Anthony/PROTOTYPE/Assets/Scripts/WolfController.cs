using UnityEngine;
using System.Collections;

public class WolfController : MonoBehaviour
{
    public float speed;
    private float wallLeft;
    private float wallRight;
    public float patrolWidth = 3.50f;
    Vector3 walkAmount;
    Transform myTrans;
    public Transform target;

    bool playerInAggroRange;
    bool facingRight;

    bool isDead = false;
    Animator animator;
    Rigidbody2D rb;
    BoxCollider2D bc2d;
    CircleCollider2D c2d;
    GameObject childGo;
    public GameObject theDrop;
    private int randomNumber;

	void Start ()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<BoxCollider2D>();
        c2d = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        wallLeft = transform.position.x - patrolWidth / 2;
        wallRight = transform.position.x + patrolWidth / 2;
        myTrans = this.transform;
        playerInAggroRange = false;
        facingRight = true;
    }

    void FixedUpdate()
    {
        if (playerInAggroRange) {
            animator.SetInteger("State", 1);
            MoveToPlayer();
        }

        if (!playerInAggroRange)
        {
            walkAmount.x = speed * Time.deltaTime;
            if (facingRight && transform.position.x >= wallRight)
            {
                Vector3 currRot = myTrans.eulerAngles;
                currRot.y += 180;
                myTrans.eulerAngles = currRot;
                facingRight = false;
            }

            else if (!facingRight && transform.position.x <= wallLeft)
            {
                Vector3 currRot = myTrans.eulerAngles;
                currRot.y += 180;
                myTrans.eulerAngles = currRot;
                facingRight = true;
            }

            transform.Translate(walkAmount);
            animator.SetInteger("State", 1);
        }

        HasBeenHit();
        
	}

    void OnTriggerEnter2D(Collider2D otherObject)
    {
        if (otherObject.gameObject.tag == "Player")
        {
            playerInAggroRange = true;
            MoveToPlayer();
        }

        if (otherObject.gameObject.tag == "PlayerAttack")
        {
            isDead = true;
            StopWalk();
            rb.isKinematic = true;
            dropHealth();
            Destroy(bc2d);
            Destroy(c2d);
            Destroy(gameObject, 1);
        }
    }

    void MoveToPlayer()
    {
        //rotate to look at player
        if (target.transform.position.x > transform.position.x)
        {
            if (facingRight)
            {
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
            }
            else
            {
                Vector3 currRot = myTrans.eulerAngles;
                currRot.y += 180;
                myTrans.eulerAngles = currRot;
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
                facingRight = true;

            }
        }
        else if (target.transform.position.x < transform.position.x)
        {
            if (facingRight)
            {
                Vector3 currRot = myTrans.eulerAngles;
                currRot.y += 180;
                myTrans.eulerAngles = currRot;
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
                facingRight = false;
            }
            else
            {
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));

            }
        }

    }

    void HasBeenHit()
    {
        if(isDead == true)
        {
            animator.SetBool("IsDead", isDead);
        }
    }


    public void StopWalk()
    {
        animator.SetInteger("State", 1);
        speed = 0;
    }

    public void dropHealth()
    {
        randomNumber = Random.Range(1, 101);
        if (randomNumber < 35)
            Instantiate(theDrop, transform.position, transform.rotation);
    }
}
