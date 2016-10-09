using UnityEngine;
using System.Collections.Generic;

public class CharacterPortrait : MonoBehaviour {

    public List<Sprite> happyPortraits;
    public List<Sprite> neutralPortraits;
    public List<Sprite> sadPortraits;

    GameManager gm;
    SpriteRenderer sr;
    int currentSatisfaction = 0;
    int currentIndex = 0;
    int lastUsedIndex = -1;

    // Use this for initialization
    void Start () {
        gm = FindObjectOfType<GameManager>();
        sr = FindObjectOfType<SpriteRenderer>();
	}

    void Update() {
        int satisfaction = gm.getSatisfaction();
        if (satisfaction != currentSatisfaction) {
            currentSatisfaction = satisfaction;
            setSprite();
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
    }

    void setSprite() {
        if (currentSatisfaction == 0) {
            sr.sprite = happyPortraits[currentIndex];
        } else if (currentSatisfaction == 1) {
            sr.sprite = neutralPortraits[currentIndex];
        } else {
            sr.sprite = sadPortraits[currentIndex];
        }
    }
}
