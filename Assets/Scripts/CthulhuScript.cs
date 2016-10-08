using UnityEngine;
using System.Collections.Generic;

//Handles Cthulhu. Gives directions to BodyParts.
public class CthulhuScript : MonoBehaviour {

    List<BodyPart> tentacles;
    InputManager i;
    GameManager gm;

	void Start () {
        tentacles = new List<BodyPart>();
        gm = FindObjectOfType<GameManager>();
        i = FindObjectOfType<InputManager>();
        for (int x = 0; x < i.items.Count; x++) {
            tentacles.Add(i.items[x]);
        }
	}
	
	void Update () {
	
	}

    public void finishPlate() {
        if (!isGrabbingFood()) {
            //TODO - animation for boxing up the order. Call advancePlates after this
            gm.finishOrder();            
        }
    }

    public void grabFood(int whichTentacle) {
        if (!isGrabbingFood()) {
            tentacles[whichTentacle].grabFood();
        }
    }


    public bool isGrabbingFood() {
        foreach (BodyPart b in tentacles) {
            if (b.isGrabbingFood()) {
                return true;
            }
        }
        return false;
    }
}
