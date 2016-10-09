using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

    TextMesh mesh;
    int scoreToAdd = 0;
    int displayScore = 0;
    int increment = 0;
    const int NUM_INCREMENTS = 10;

	// Use this for initialization
	void Start () {
        mesh = GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (scoreToAdd != 0) {
            displayScore += increment;
            scoreToAdd -= increment;
        }
        mesh.text = "" + displayScore;
	}

    public void addScore(int score) {
        //if we arent done adding the last score, add it all now
        if (scoreToAdd != 0) {
            displayScore += scoreToAdd;
        }
        //then set scoreToAdd to whatever score we are adding
        scoreToAdd = score;
        increment = scoreToAdd / NUM_INCREMENTS;
    }
}
