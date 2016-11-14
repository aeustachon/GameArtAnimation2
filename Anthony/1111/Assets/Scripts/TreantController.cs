using UnityEngine;
using System.Collections;

public class TreantController : MonoBehaviour
{
	public Collider2D attackTrigger;
    public GameObject theDrop;
	public float attackCD = 0.3f;

	private Transform target;
	private bool attacking = false;
	private float attackTimer = 0;
	private Transform transform;
	private Animator animator;
	private bool playerInAggroRange;
	private bool facingLeft;
    private bool dead;
    private int randomNumber;
	private Renderer render;
    private Color hurtColor = Color.black;
    private Color normalColor = Color.white;

    void Start () 
	{
        render = GetComponent<Renderer>();
        dead = false;
		facingLeft = false;
		playerInAggroRange = false;
		attackTrigger.enabled = false;
		animator = GetComponent<Animator> ();
		transform = GetComponent<Transform> ();
		target = GameObject.FindGameObjectWithTag ("Player").transform;
	}
		
	void Update () 
	{
		if (playerInAggroRange && !dead && attacking) {
			attack();
		} else if (!playerInAggroRange && !attacking) {
			idle ();
		}

		if (attacking) 
		{
			if (attackTimer > 0) 
			{
				attackTimer -= Time.deltaTime;
			}
			else 
			{
				attacking = false;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D otherObject)
	{
		if (otherObject.gameObject.tag == "Player")
		{
			playerInAggroRange = true;
			attacking = true;
			lookAtTarget ();
		}
	}

	void OnTriggerExit2D(Collider2D otherObject)
	{
		playerInAggroRange = false;
	}

    void attack()
	{
		attackTimer = attackCD;
        attackTrigger.enabled = true;
        animator.SetBool("Attacking", attacking);
	}

	void idle()
	{
		attackTrigger.enabled = false;
		animator.SetBool ("Attacking", attacking);
	}

	void setFacingDirection(Vector2 v2)
	{
		transform.localScale = v2;
	}

	void lookAtTarget()
	{
		if (target.transform.position.x > transform.position.x)
		{
			if (facingLeft)
			{
				transform.localScale = new Vector2 (-transform.localScale.x, transform.localScale.y);
				facingLeft = false;
			}
		}
		else if (target.transform.position.x < transform.position.x)
		{
			if (!facingLeft)
			{
				transform.localScale = new Vector2 (-transform.localScale.x, transform.localScale.y);
				facingLeft = true;
			}
		}
	}

	public void Damage()
	{
		dead = true;
		idle();
        StartCoroutine(Flasher());
		dropHealth();
		Destroy(gameObject, 1);
	}

    public void dropHealth()
    {
        randomNumber = Random.Range(1, 101);
        if (randomNumber < 35)
            Instantiate(theDrop, transform.position, transform.rotation);
    }

    IEnumerator Flasher()
    {
        for (int i = 0; i < 5; i++)
        {
            render.material.color = hurtColor;
            yield return new WaitForSeconds(.1f);
            render.material.color = normalColor;
            yield return new WaitForSeconds(.05f);
        }
    }
}
