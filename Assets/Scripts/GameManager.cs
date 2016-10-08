using UnityEngine;
using System.Collections.Generic;

public enum Ingredient { calimari, tuna, shrimp, ginger, wasabi, soySauce, whiteRice, kelp }

public class GameManager : MonoBehaviour {

    //General list of ingredients, this will be the ordering of the ingredients on the counter
    //Can be changed to fit design
    public static List<Ingredient> meats = new List<Ingredient>() { Ingredient.calimari, Ingredient.tuna, Ingredient.shrimp };
    public static List<Ingredient> condiments = new List<Ingredient>() { Ingredient.ginger, Ingredient.wasabi, Ingredient.soySauce };
    public static List<Ingredient> rice = new List<Ingredient>() { Ingredient.whiteRice, Ingredient.kelp };
    public static List<Ingredient> indexToIngredient = new List<Ingredient>() {
        Ingredient.calimari, Ingredient.tuna, Ingredient.shrimp,
        Ingredient.ginger, Ingredient.wasabi, Ingredient.soySauce,
        Ingredient.whiteRice, Ingredient.kelp };
    public static Dictionary<Ingredient, string> ingredientsToSpriteNames = new Dictionary<Ingredient, string>() {
        { Ingredient.calimari, "Calimari" }, { Ingredient.tuna, "Tuna" }, { Ingredient.shrimp, "Shrimp" },
        { Ingredient.ginger, "Ginger" }, { Ingredient.wasabi, "Wasabi" }, { Ingredient.soySauce, "SoySauce" },
        { Ingredient.whiteRice, "Rice" }, { Ingredient.kelp, "Kelp" }
    };
    public GameObject meter;


    Order currentOrder;
    //in the order of spawn, queue, center (one we are working on), and out the door
    //So current plate is currentPlates[2]
    List<Plate> currentPlates;

    //Level information------------------------------------
    int clientsFed = 0;
    int maxClientsThisLevel = 10;
    int difficulty = 1;
    bool unlimitedMode = false;

    //Scoring------------------------------------------------
    //For easy, medium, hard, and unlimited
    public List<int> highScores = new List<int>() { 0, 0, 0, 0 };
    int currentScore = 0;
    //ALGORITHM
    // - Under NUM_FOOD_ITEMS * SECONDS_PER_FOOD_ITEM seconds = Great
    // - Under NUM_FOOD_ITEMS * SECONDS_PER_FOOD_ITEM * 2 seconds = Meh
    // - Under NUM_FOOD_ITEMS * SECONDS_PER_FOOD_ITEM * 3 seconds = Bad
    // - Beyond Bad - They leave
    const int MAX_FOOD_ITEMS_TRACKED_FOR_SCORE = 7;
    const int SECONDS_PER_FOOD_ITEM = 1;
    float timeSpentOnOrder = 0;


    void Start()
    {
        DontDestroyOnLoad(this);
        getNewOrder();
        currentPlates = new List<Plate>();
        advancePlates();
        advancePlates();
        advancePlates();
    }

    void Update() {
        timeSpentOnOrder += Time.deltaTime;
    }

    //SCORING----------------------------------------------------
    //Best satisfaction is 0, 1 is meh, 2 is Bad, 3 is they leave
    public int getSatisfaction() {
        return (int) (timeSpentOnOrder / getScoredFoodItems() * SECONDS_PER_FOOD_ITEM);
    }

    public int getScoredFoodItems() {
        return Mathf.Min(currentOrder.getIngredientList().Count, MAX_FOOD_ITEMS_TRACKED_FOR_SCORE);
    }

    public int getScore() {
        int foodNum = getScoredFoodItems();
        return 0; //TODO fix
    }

    //ORDERS----------------------------------------------------
    public Order getCurrentOrder() {
        return currentOrder;
    }

    public void getNewOrder() {
        timeSpentOnOrder = 0;
        currentOrder = OrderCreator.getRandomOrder(difficulty, clientsFed, maxClientsThisLevel);
        string totalOrder = "Target Order is: ";
        foreach (Ingredient ingredient in currentOrder.getIngredientList()) {
            totalOrder += ingredient + " ";
        }
        Debug.Log(totalOrder);
        for (int x = 0; x < currentOrder.getIngredientList().Count; x++)
        {
            meter.transform.GetChild(x).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(ingredientsToSpriteNames[currentOrder.getIngredientList()[x]]);
        }
        for (int x = currentOrder.getIngredientList().Count + 1; x < meter.transform.childCount; x++)
        {
            meter.transform.GetChild(x).GetComponent<SpriteRenderer>().sprite = null;
        }
    }

    public bool addToOrder(Ingredient i)
    {
        string totalOrder = "";
        foreach (Ingredient ingredient in currentOrder.getCurrentPlate()) {
            totalOrder += ingredient + " ";
        }
        print(i + " being added to (" + totalOrder + ")");
        return currentOrder.addIngredient(i);
    }

    //PLATES--------------------------------------------
    public void advancePlates() {
        //Moves all the plates forward
        foreach (Plate p in currentPlates) {
            p.MoveForward();
        }
        GameObject plate = (GameObject)Instantiate(Resources.Load("Plate"));
        currentPlates.Insert(0, plate.GetComponent<Plate>());
        if (currentPlates.Count > 4) {
            currentPlates.RemoveAt(4);
        }
    }

    public void dropFoodOnPlate(GameObject food) {
        currentPlates[2].AddObjectToPlate(food);
    }

    //INGREDIENTS--------------------------------------------------
    public static bool isRice(Ingredient i) { return rice.Contains(i); }
    public static bool isMeat(Ingredient i) { return meats.Contains(i); }
    public static bool isCondiment(Ingredient i) { return condiments.Contains(i); }
    public static Ingredient getRandomRice() { return rice[Random.Range(0, rice.Count)]; }
    public static Ingredient getRandomMeat() { return meats[Random.Range(0, meats.Count)]; }
    public static Ingredient getRandomCondiment() { return condiments[Random.Range(0, condiments.Count)]; }

}
