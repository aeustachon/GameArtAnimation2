using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour {
	public string sceneToLoad;
	void loadScene()
	{
		SceneManager.LoadScene(sceneToLoad);
	}
}
