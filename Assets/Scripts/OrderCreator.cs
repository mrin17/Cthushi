using UnityEngine;
using System.Collections.Generic;

public class OrderCreator {

    //Difficulty is in number of ingredients
    //Level 1: 2-4
    //Level 2: 4-6
    //Level 3: 6-8, etc
	public static Order getRandomOrder(int difficulty, int numClientsFed, int maxClients) {
        int numIngredients = difficulty * 2 + (int) (numClientsFed * 1.0f / maxClients * 2) + Random.Range(0, 2);
        List<Ingredient> ingredients = new List<Ingredient>();

        //Add a random ingredient for each numIngredient
        int possibleIngredients = GameManager.indexToIngredient.Count;
        int lastPickedIngredient = -1;
        bool containsMeat = false;

        for (int i = 0; i < numIngredients; i++) {
            int nextIngredient = Random.Range(0, possibleIngredients);
            if (nextIngredient != lastPickedIngredient) {
                //this makes it less likely that two of the same ingredients would be placed in a row
                nextIngredient = Random.Range(0, possibleIngredients);
            }
            Ingredient ingredient = GameManager.indexToIngredient[nextIngredient];
            ingredients.Add(ingredient);
            if (GameManager.isMeat(ingredient)) {
                containsMeat = true;
            }
        }

        //TODO - if doesnt contain a meat, add meat (dont overwrite any rice added unless necessary)
        if (!containsMeat) {
            ingredients[0] = GameManager.getRandomMeat();
            containsMeat = true;
        }

        return new Order(ingredients);
    }
}
