using UnityEngine;
using System.Collections;

public class BodyPart : MonoBehaviour {

    public int index;
    GameObject foodHolding;
    GameManager gm;

    bool grabbingFood = false;

	void Start () {
        gm = FindObjectOfType<GameManager>();
	}
	
	void Update () {
        if (grabbingFood) {
            grabbingFood = false;
        }   
	}

    public void grabFood() {
        grabbingFood = true;
        //TODO - enable tentacle animation
        Ingredient food = GameManager.indexToIngredient[index];
        //TODO - add food to current Order, bool result returns whether the ingredient was correct or not
        gm.addToOrder(food);
        //TODO - timer on cooldown for grabbingFood
    }

    public bool isGrabbingFood() { return grabbingFood; }
}
