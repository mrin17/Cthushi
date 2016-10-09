using UnityEngine;
using System.Collections.Generic;

//Handles Cthulhu. Gives directions to BodyParts.
public class CthulhuScript : MonoBehaviour {

    enum CState { neutral, swiping, clapping, waiting, shipping };
    public float SWIPE_TIME = .25f; //time you spend swiping your knife
    public float CLAP_TIME = .5f; //time you spend before the cloud appears
    public float WAIT_TIME = .5f; //time you spend observing your sushi
    public float SHIP_TIME = .25f; //time you spend before you can make more sushi
    float timer = 0;

    List<BodyPart> tentacles;
    InputManager i;
    GameManager gm;
    bool multipleTentacleMovement = true;
    CState currentState = CState.neutral;

    void Start () {
        tentacles = new List<BodyPart>();
        gm = FindObjectOfType<GameManager>();
        i = FindObjectOfType<InputManager>();
        for (int x = 0; x < i.items.Count; x++) {
            tentacles.Add(i.items[x]);
        }
	}
	
	void Update () {
        if (currentState == CState.neutral) {
            return;
        }

        if (timer > 0) {
            timer -= Time.deltaTime;
        } else {
            switch (currentState) {
                case CState.swiping:
                    currentState = CState.neutral;
                    break;
                case CState.clapping:
                    currentState = CState.waiting;
                    timer = WAIT_TIME;
                    //TODO - create cloud and make sushi
                    gm.getCurrentPlate().CreateSushi();
                    break;
                case CState.waiting:
                    currentState = CState.shipping;
                    timer = SHIP_TIME;
                    gm.finishOrder();
                    break;
                case CState.shipping:
                    currentState = CState.neutral;
                    break;
            }
        }
    }

    public void finishPlate() {
        if (!isBusy()) {
            currentState = CState.clapping;
            timer = CLAP_TIME;
            gm.scorePlate();
        }
    }

    public void grabFood(int whichTentacle) {
        if (isBusy()) {
            return;
        }
        if (multipleTentacleMovement) {
            if (!tentacles[whichTentacle].isGrabbingFood()) {
                tentacles[whichTentacle].grabFood();
            }
        } else {
            if (!isGrabbingFood()) {
                tentacles[whichTentacle].grabFood();
            }
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

    public bool isBusy() {
        return isGrabbingFood() || !currentState.Equals(CState.neutral);
    }

    public void beginSwiping() {
        currentState = CState.swiping;
        timer = SWIPE_TIME;
    }
}
