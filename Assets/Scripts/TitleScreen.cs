using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour {

    SoundHandler sh;
	// Use this for initialization
	void Start () {
        sh = FindObjectOfType<SoundHandler>();
        sh.PlaySound("level_loop_1");
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A)) {
            SceneManager.LoadScene("TestLevel");
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            SceneManager.LoadScene("MikeTestLevel");
        }
    }
}
