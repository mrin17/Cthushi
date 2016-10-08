using UnityEngine;
using System.Collections.Generic;

public class OrderCreator {

    //Difficulty is in number of ingredients
    //Level 1: 2-4
    //Level 2: 4-6
    //Level 3: 6-8, etc
	public static Order getRandomOrder(int difficulty, int numClientsFed, int maxClients) {
        int numIngredients = difficulty * 2 + (int) (numClientsFed / maxClients * 2) + Random.Range(0, 1);
        List<Ingredient> ingredients = new List<Ingredient>();

        //Add a random ingredient for each numIngredient
        int possibleIngredients = GameManager.indexToIngredient.Count;
        int lastPickedIngredient = -1;
        bool containsRice = false;
        bool containsMeat = false;

        for (int i = 0; i < numIngredients; i++) {
            int nextIngredient = Random.Range(0, possibleIngredients);
            if (nextIngredient != lastPickedIngredient) {
                //this makes it less likely that two of the same ingredients would be placed in a row
                nextIngredient = Random.Range(0, possibleIngredients);
            }
            Ingredient ingredient = GameManager.indexToIngredient[nextIngredient];
            ingredients.Add(ingredient);
            if (GameManager.isRice(ingredient)) {
                containsRice = true;
            }
            if (GameManager.isMeat(ingredient)) {
                containsMeat = true;
            }
        }

        //TODO - if doesnt contain a rice, add rice (dont overwrite any meat added unless necessary)
        //TODO - if doesnt contain a meat, add meat (dont overwrite any rice added unless necessary)
        //Only manually override on the last ingredient
        if (!containsMeat || !containsRice) {
            for (int i = 0; i < ingredients.Count; i++) {
                if (!containsMeat && (!GameManager.isRice(ingredients[i]) || i == ingredients.Count - 1)) {
                    ingredients[i] = GameManager.getRandomMeat();
                    containsMeat = true;
                }
                if (!containsRice && (!GameManager.isMeat(ingredients[i]) || i == ingredients.Count - 1)) {
                    ingredients[i] = GameManager.getRandomRice();
                    containsRice = true;
                }
            }
        }

        return new Order(ingredients);
    }
}
