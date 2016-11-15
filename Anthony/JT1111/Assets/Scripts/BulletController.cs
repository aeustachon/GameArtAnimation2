using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour
{
    public Vector2 speed;
	public int damagePerBullet;
    private Rigidbody2D rb;
    
	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = speed;	
	}
	
	void Update ()
    {
        rb.velocity = speed;
	}

    void OnTriggerEnter2D(Collider2D otherObject)
    {
		if ((otherObject.gameObject.tag == "PassiveEnemy" || otherObject.gameObject.tag == "AggressiveEnemy" || otherObject.gameObject.tag == "Enemy"))
        {
			otherObject.SendMessageUpwards ("Damage", damagePerBullet);
			Destroy (gameObject);
        }
    }
}
