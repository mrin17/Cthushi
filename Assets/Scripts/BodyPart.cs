using UnityEngine;
using System.Collections;

public class BodyPart : MonoBehaviour {

    enum State { neutral, grabbing, placing, throwing, cooldown };

    public int index;
    GameObject foodHolding;
    GameManager gm;
    public float GRAB_TIME = .5f;
    public float DROP_ON_PLATE_TIME = .5f;
    //TODO - implement throw away time on ingredients that you throw away
    public float THROW_AWAY_TIME = .5f;
    public float COOLDOWN_TIME = .25f;
    float timer = 0;
    bool grabbingCorrectFood = false;

    State currentState = State.neutral;

	void Start () {
        gm = FindObjectOfType<GameManager>();
	}
	
	void Update () {
        if (currentState == State.neutral) {
            return;
        }

        if (timer > 0) {
            timer -= Time.deltaTime;
        } else {
            switch (currentState) {
                case State.grabbing:
                    if (grabbingCorrectFood) {
                        currentState = State.placing;
                        timer = DROP_ON_PLATE_TIME;
                    } else {
                        currentState = State.throwing;
                        //TODO - do whatever he does when he's throwing
                        timer = THROW_AWAY_TIME;
                    }
                    foodHolding = (GameObject)Instantiate(Resources.Load("food"), transform.position + new Vector3(0, -1, 0), Quaternion.identity);
                    foodHolding.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shrimp");
                    break;
                case State.placing:
                case State.throwing:
                    currentState = State.cooldown;
                    //TODO drop food prefab on plate
                    gm.dropFoodOnPlate(foodHolding);
                    foodHolding = null;
                    timer = COOLDOWN_TIME;
                    break;
                case State.cooldown:
                    currentState = State.neutral;
                    timer = 0;
                    break;
            }
        }
	}

    public void grabFood() {
        currentState = State.grabbing;
        timer = GRAB_TIME;
        //TODO - enable tentacle animation
        Ingredient food = GameManager.indexToIngredient[index];
        //TODO - add food to current Order, bool result returns whether the ingredient was correct or not
        grabbingCorrectFood = gm.addToOrder(food);
        for (int x = 0; x < gm.getCurrentOrder().getCurrentPlate().Count; x++)
        {
            if (gm.getCurrentOrder().getCurrentPlate().Contains(gm.getCurrentOrder().getIngredientList()[x]))
            {
                for (int y = 1; y < gm.meter.transform.childCount - 1; y++)
                {
                    gm.meter.transform.GetChild(y).GetComponent<TextMesh>().text = gm.meter.transform.GetChild(y + 1).GetComponent<TextMesh>().text;
                }
            }
        }
    }

    public bool isGrabbingFood() { return currentState != State.neutral; }
}
