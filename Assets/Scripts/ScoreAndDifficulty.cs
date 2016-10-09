using UnityEngine;
using System.Collections.Generic;

public class ScoreAndDifficulty : MonoBehaviour {

    int difficulty = 1;
    bool unlimitedMode = false;
    //For easy, medium, hard, and unlimited
    public List<int> highScores = new List<int>() { 0, 0, 0, 0 };

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this);
	}
	
	public int getDifficulty() { return difficulty; }
    public void setDifficulty(int d) { difficulty = d; }
    public int getHighScore(int where) { return highScores[where]; }
    public void setHighScore(int where, int score) { highScores[where] = score; }
    public bool getUnlimitedMode() { return unlimitedMode; }
    public void setUnlimitedMode(bool un) { unlimitedMode = un; }
}
