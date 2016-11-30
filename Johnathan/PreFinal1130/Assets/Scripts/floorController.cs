using UnityEngine;
using System.Collections;

public class floorController : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D otherObject)
	{
		if (otherObject.gameObject.tag == "Bullet") {
			Destroy (otherObject.gameObject);
		}
	}
}
