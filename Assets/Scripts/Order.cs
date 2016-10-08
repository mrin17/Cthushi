using UnityEngine;
using System.Collections.Generic;

public enum Ingredient { tofu, tuna, crab, ginger, wasabi, soySauce, whiteRice, brownRice}

public class Order {

    //General list of ingredients, this will be the ordering of the ingredients on the counter
    //Can be changed to fit design
    public static List<Ingredient> indexToIngredient = new List<Ingredient>() {
        Ingredient.tofu, Ingredient.tuna, Ingredient.crab,
        Ingredient.ginger, Ingredient.wasabi, Ingredient.soySauce,
        Ingredient.whiteRice, Ingredient.brownRice };

    List<Ingredient> ingredientList = new List<Ingredient>();
    List<Ingredient> currentPlate = new List<Ingredient>();

    public bool addIngredient(Ingredient ingredient) {
        int location = currentPlate.Count;
        if (ingredientList[location] == ingredient) {
            currentPlate.Add(ingredient);
            return true;
        } else {
            return false;
        }
    }

    public bool isCompleted() {
        if (currentPlate.Count != ingredientList.Count) {
            return false;
        }

        for (int i = 0; i < currentPlate.Count; i++) {
            if (!currentPlate[i].Equals(ingredientList[i])) {
                return false;
            }
        }
        return true;
    }
}
