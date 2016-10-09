using UnityEngine;
using System.Collections.Generic;

//Handles Cthulhu. Gives directions to BodyParts.
public class CthulhuScript : MonoBehaviour {

    public Animator Knife, Paddle;
    public GameObject cloud;
    enum CState { neutral, swiping, clapping, waiting, shipping };
    public float SWIPE_TIME = .25f; //time you spend swiping your knife
    public float CLAP_TIME = .6f; //time you spend before the cloud appears
    public float WAIT_TIME = .5f; //time you spend observing your sushi
    public float SHIP_TIME = .25f; //time you spend before you can make more sushi
    float timer = 0;
    string[] victorys = { "Cthulu_8arms", "Cthulu_Enjoy", "Cthulu_hopeyoulikeit", "Cthulu_just4u", "Cthulu_just4u_2", "Cthulu_madewithlove" };


    List<BodyPart> tentacles;
    InputManager i;
    GameManager gm;
    SoundHandler sh;
    bool multipleTentacleMovement = true;
    CState currentState = CState.neutral;

    void Start () {
        tentacles = new List<BodyPart>();
        gm = FindObjectOfType<GameManager>();
        sh = FindObjectOfType<SoundHandler>();
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
                    Knife.SetBool("Hit", false);
                    currentState = CState.neutral;
                    break;
                case CState.clapping:
                    Knife.SetBool("Hit", false);
                    Paddle.SetBool("Space", false);
                    currentState = CState.waiting;
                    timer = WAIT_TIME;
                    //TODO - create cloud and make sushi
                    cloud.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                    gm.getCurrentPlate().CreateSushi();
                    break;
                case CState.waiting:
                    if (cloud.GetComponent<SpriteRenderer>().color.a > 0)
                    {
                        currentState = CState.waiting;
                        cloud.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, cloud.GetComponent<SpriteRenderer>().color.a - .025f);
                    }
                    else
                    {
                        currentState = CState.shipping;
                        timer = SHIP_TIME;
                        gm.finishOrder();
                    }
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
            Knife.SetBool("Hit", true);
            Paddle.SetBool("Space", true);
            timer = CLAP_TIME;
            gm.scorePlate();
            sh.PlaySound("finished_plate");
            sh.PlaySound(victorys[Random.Range(0, 5)]);
        }
    }

    public void grabFood(int whichTentacle) {
        if (currentState != CState.neutral && currentState != CState.swiping) {
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
        Knife.SetBool("Hit", true);
        currentState = CState.swiping;
        timer = SWIPE_TIME;
    }
}
