using UnityEngine;
using System.Collections.Generic;

//Handles Cthulhu. Gives directions to BodyParts.
public class CthulhuScript : MonoBehaviour {

    List<BodyPart> tentacles;
    InputManager i;

	void Start () {
        tentacles = new List<BodyPart>();
        i = FindObjectOfType<InputManager>();
        for (int x = 0; x < i.items.Count; x++) {
            tentacles.Add(i.items[x]);
        }
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
