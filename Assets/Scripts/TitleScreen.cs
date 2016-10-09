using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour {

    SoundHandler sh;
    ScoreAndDifficulty sd;
	// Use this for initialization
	void Start () {
        sh = FindObjectOfType<SoundHandler>();
        sd = FindObjectOfType<ScoreAndDifficulty>();
        if (SceneManager.GetActiveScene().name == "Title")
        {
            sh.PlaySound("level_loop_1");
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (SceneManager.GetActiveScene().name == "IntroCutscene")
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("Title");
            }
        }
        if (SceneManager.GetActiveScene().name == "Title")
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                sh.GetComponent<AudioSource>().Stop();
                sd.setDifficulty(1);
                sd.setUnlimitedMode(false);
                SceneManager.LoadScene("Cutscene2");
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                sh.GetComponent<AudioSource>().Stop();
                sd.setDifficulty(1);
                sd.setUnlimitedMode(true);
                SceneManager.LoadScene("MikeTestScene");
            }
        }
        if (SceneManager.GetActiveScene().name == "Cutscene2")
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("Level");
            }
        }
        if (SceneManager.GetActiveScene().name == "Cutscene3" || SceneManager.GetActiveScene().name == "YouLose")
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("Title");
            }
        }
    }
}
