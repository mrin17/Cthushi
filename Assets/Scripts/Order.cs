using UnityEngine;
using System.Collections.Generic;

public class Order {

    List<Ingredient> ingredientList;
    List<Ingredient> currentPlate;

    public Order(List<Ingredient> ingredients) {
        ingredientList = ingredients;
        currentPlate = new List<Ingredient>();
    }

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

    public List<Ingredient> getIngredientList() {
        return ingredientList;
    }

    public List<Ingredient> getCurrentPlate()
    {
        return currentPlate;
    }
}
