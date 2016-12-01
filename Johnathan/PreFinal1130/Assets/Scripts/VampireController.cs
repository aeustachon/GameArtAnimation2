using UnityEngine;
using System.Collections;

public class VampireController : MonoBehaviour
{
    public GameObject theDrop;
	public GameObject projectile;
	public float attackCD = 1.0f;
	public int health = 50;

	private float attackTimer = 0;
	private bool attacking = false;
	private bool playerInAggroRange = true;
	private bool facingLeft;
    private bool dead;
	private int randomNumber;

	private Transform firePoint;
	private Transform target;
	private Transform transform;
	private Animator animator;
	private Renderer render;
    private Color hurtColor = Color.black;
    private Color normalColor = Color.white;

    //All audio stuff
    public AudioClip audioTreant;
    private AudioSource audioSourcePlayer;

    void Start () 
	{
        render = GetComponent<Renderer>();
        dead = false;
		facingLeft = true;
		animator = GetComponent<Animator> ();
		transform = GetComponent<Transform> ();
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		firePoint = transform.FindChild("firePoint");
        audioSourcePlayer = GetComponent<AudioSource>();      //Audio Stuff
    }
		
	void Update () 
	{
		if (target != null) {
			attack ();
			lookAtTarget ();

			if (attacking) {
				if (attackTimer > 0) {
					attackTimer -= Time.deltaTime;
				} else {
					attacking = false;
				}
			}
		}
	}

    void attack()
	{
		if (attacking == false) {
            audioSourcePlayer.PlayOneShot(audioTreant, 1F);       //Treant Sound
            attackTimer = attackCD;
			attacking = true;
			Instantiate (projectile, firePoint.transform.position, transform.rotation);
			animator.SetInteger ("State", 1);
		}
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

	public void Damage(int dmg)
	{
		health = health - dmg;
		StartCoroutine(Flasher());
		if (health <= 0) {
			dead = true;
			animator.SetInteger ("State", 2);
			dropHealth ();
			Destroy (gameObject, 1);
		}
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
