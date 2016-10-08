using UnityEngine;
using System.Collections.Generic;

//Handles Cthulhu. Gives directions to BodyParts.
public class CthulhuScript : MonoBehaviour {

    List<BodyPart> tentacles;

	void Start () {
	
	}
	
	void Update () {
	
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
