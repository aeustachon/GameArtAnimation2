using UnityEngine;
using System.Collections;

public class HealthUI : MonoBehaviour {
	
    public float texWidth;
    public float texHeight;
	public Texture tex;

	private GameObject player1;

	void Start () {
        texWidth = tex.width/2;
        texHeight = tex.height/2;
    }

    void OnGUI()
    {
		player1 = GameObject.FindGameObjectWithTag ("Player");
		Player player = player1.GetComponent<Player> ();
		if (player.playerStats.health > 0) {
			Rect posRect = new Rect (0, 0, texWidth / 8 * player.playerStats.health, texHeight);
			Rect texRect = new Rect (0, 0, 1.0f / 8f * player.playerStats.health, 1.0f);
			GUI.DrawTextureWithTexCoords (posRect, tex, texRect);
		}

    }
}
