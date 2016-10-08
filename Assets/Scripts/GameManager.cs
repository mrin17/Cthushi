using UnityEngine;
using System.Collections.Generic;

public enum Ingredient { tofu, tuna, crab, ginger, wasabi, soySauce, whiteRice, brownRice }

public class GameManager : MonoBehaviour {

    //General list of ingredients, this will be the ordering of the ingredients on the counter
    //Can be changed to fit design
    public static List<Ingredient> indexToIngredient = new List<Ingredient>() {
        Ingredient.tofu, Ingredient.tuna, Ingredient.crab,
        Ingredient.ginger, Ingredient.wasabi, Ingredient.soySauce,
        Ingredient.whiteRice, Ingredient.brownRice };

    //For easy, medium, hard, and unlimited
    public List<int> highScores = new List<int>() { 0, 0, 0, 0 };

    Order currentOrder;

	void Start () {
        DontDestroyOnLoad(this);
	}

    public void getNewOrder() {
        //TODO - set order to be something
        currentOrder = new Order(new List<Ingredient>() { Ingredient.tuna, Ingredient.wasabi, Ingredient.whiteRice });
    }

    public bool addToOrder(Ingredient i) {
        return currentOrder.addIngredient(i);
    }
	
}
