using UnityEngine;
using System.Collections.Generic;

public enum Ingredient { tofu, tuna, crab, ginger, wasabi, soySauce, whiteRice, brownRice }

public class GameManager : MonoBehaviour {

    //General list of ingredients, this will be the ordering of the ingredients on the counter
    //Can be changed to fit design
    public static List<Ingredient> meats = new List<Ingredient>() { Ingredient.tofu, Ingredient.tuna, Ingredient.crab };
    public static List<Ingredient> condiments = new List<Ingredient>() { Ingredient.ginger, Ingredient.wasabi, Ingredient.soySauce };
    public static List<Ingredient> rice = new List<Ingredient>() { Ingredient.whiteRice, Ingredient.brownRice };
    public static List<Ingredient> indexToIngredient = new List<Ingredient>() {
        Ingredient.tofu, Ingredient.tuna, Ingredient.crab,
        Ingredient.ginger, Ingredient.wasabi, Ingredient.soySauce,
        Ingredient.whiteRice, Ingredient.brownRice };


    //For easy, medium, hard, and unlimited
    public List<int> highScores = new List<int>() { 0, 0, 0, 0 };
    int clientsFed = 0;
    int maxClientsThisLevel = 10;
    int difficulty = 1;

    Order currentOrder;

	void Start () {
        DontDestroyOnLoad(this);
	}

    public void getNewOrder() {
        //TODO - set order to be something
        currentOrder = OrderCreator.getRandomOrder(difficulty, clientsFed, maxClientsThisLevel);
    }

    public bool addToOrder(Ingredient i) {
        return currentOrder.addIngredient(i);
    }
	
    public static bool isRice(Ingredient i) { return rice.Contains(i); }
    public static bool isMeat(Ingredient i) { return meats.Contains(i); }
    public static bool isCondiment(Ingredient i) { return condiments.Contains(i); }
    public static Ingredient getRandomRice() { return rice[Random.Range(0, rice.Count)]; }
    public static Ingredient getRandomMeat() { return meats[Random.Range(0, meats.Count)]; }
    public static Ingredient getRandomCondiment() { return condiments[Random.Range(0, condiments.Count)]; }

}
