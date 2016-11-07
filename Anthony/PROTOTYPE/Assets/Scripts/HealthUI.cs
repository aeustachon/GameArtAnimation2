using UnityEngine;
using System.Collections;

public class HealthUI : MonoBehaviour {
    public Texture tex;
    private GameObject player1;
    public float texWidth;
    public float texHeight;

	void Start () {
        texWidth = tex.width;
        texHeight = tex.height;
    }

    void OnGUI()
    {
        player1 = GameObject.Find("Player");
        Player player = player1.GetComponent<Player>();
        if (player.playerStats.health > 0)
        {
            Rect posRect = new Rect(0, 0, texWidth / 5 * player.playerStats.health, texHeight);
            Rect texRect = new Rect(0, 0, 1.0f / 5 * player.playerStats.health, 1.0f);
            GUI.DrawTextureWithTexCoords(posRect, tex, texRect);
        }
        
    }


}
