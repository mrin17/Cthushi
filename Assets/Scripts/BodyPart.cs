using UnityEngine;
using System.Collections;

public class BodyPart : MonoBehaviour {

    enum State { neutral, grabbing, placing, throwing, cooldown };

    public int index;
    GameObject foodHolding;
    GameManager gm;
    public float GRAB_TIME = .5f;
    public float DROP_ON_PLATE_TIME = .5f;
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
                        timer = THROW_AWAY_TIME;
                    }
                    gm.anims[index].SetBool("ButtonPressed?", true);
                    foodHolding = (GameObject)Instantiate(Resources.Load("food"), transform.position + new Vector3(0, .5f, 0), Quaternion.identity);
                    foodHolding.GetComponent<SpriteRenderer>().sprite = gm.ingredientsOnPlate[index];
                    foodHolding.GetComponent<Food>().moveTowardsLocation(gm.getCurrentPlate().getNextPositionToMoveTowards(GameManager.indexToIngredient[index]));
                    foodHolding.GetComponent<Food>().setIngredient(GameManager.indexToIngredient[index]);
                    break;
                case State.placing:
                    gm.getCurrentPlate().AddObjectToPlate(foodHolding);
                    foodHolding = null;
                    currentState = State.cooldown;
                    timer = COOLDOWN_TIME;
                    break;
                case State.throwing:
                    gm.anims[index].SetBool("ButtonPressed?", false);
                    foodHolding.GetComponent<Food>().throwFood();
                    foodHolding = null;
                    //TODO - maybe he does his knife animation just to push the food off the plate?
                    currentState = State.cooldown;
                    timer = COOLDOWN_TIME;
                    break;
                case State.cooldown:
                    gm.anims[index].SetBool("ButtonPressed?", false);
                    currentState = State.neutral;
                    timer = 0;
                    break;
            }
        }
	}

    public void grabFood() {
        currentState = State.grabbing;
        timer = GRAB_TIME;
        Ingredient food = GameManager.indexToIngredient[index];
        //gm.anims[index].SetBool("ButtonPressed?", true);
        grabbingCorrectFood = gm.addToOrder(food);
        if (grabbingCorrectFood)
            {
                for (int y = 0; y < gm.meter.transform.childCount - 1; y++)
                {
                    gm.meter.transform.GetChild(y).GetComponent<SpriteRenderer>().sprite = gm.meter.transform.GetChild(y + 1).GetComponent<SpriteRenderer>().sprite;
                }
        }
    }

    public bool isGrabbingFood() { return currentState != State.neutral; }
}
