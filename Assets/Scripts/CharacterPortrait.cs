using UnityEngine;
using System.Collections.Generic;

public class CharacterPortrait : MonoBehaviour {

    public List<Sprite> happyPortraits;
    public List<Sprite> neutralPortraits;
    public List<Sprite> sadPortraits;
    public List<string> enterSounds;
    public List<string> sadSounds;

    GameManager gm;
    SpriteRenderer sr;
    SoundHandler sh;
    int currentSatisfaction = 0;
    int currentIndex = 0;
    int lastUsedIndex = -1;
    bool started = false;

    // Use this for initialization
    void Start () {
        started = true;
        gm = FindObjectOfType<GameManager>();
        sr = GetComponent<SpriteRenderer>();
        sh = FindObjectOfType<SoundHandler>();
	}

    void Update() {
        int satisfaction = gm.getSatisfaction();
        if (satisfaction != currentSatisfaction) {
            currentSatisfaction = satisfaction;
            setSprite();
            sh.PlaySound(sadSounds[currentIndex]);
        }
    }
	
	
    public void setRandomPortrait() {
        currentSatisfaction = 0;
        currentIndex = Random.Range(0, sadPortraits.Count);
        if (currentIndex == lastUsedIndex) {
            currentIndex = Random.Range(0, sadPortraits.Count);
        }
        lastUsedIndex = currentIndex;
        setSprite();
        sh.PlaySound(enterSounds[currentIndex]);
    }

    void setSprite() {
        if (!started) {
            Start();
        }
        if (currentSatisfaction == 0) {
            sr.sprite = happyPortraits[currentIndex];
        } else if (currentSatisfaction == 1) {
            sr.sprite = neutralPortraits[currentIndex];
        } else {
            sr.sprite = sadPortraits[currentIndex];
        }
    }
}
