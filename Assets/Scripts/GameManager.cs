using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public enum Ingredient { calimari, tuna, shrimp, ginger, wasabi, soySauce, whiteRice, kelp }

public class GameManager : MonoBehaviour {

    //General list of ingredients, this will be the ordering of the ingredients on the counter
    //Can be changed to fit design
    public static List<Ingredient> meats = new List<Ingredient>() { Ingredient.calimari, Ingredient.tuna, Ingredient.shrimp };
    public static List<Ingredient> condiments = new List<Ingredient>() { Ingredient.ginger, Ingredient.wasabi };
    public static List<Ingredient> others = new List<Ingredient>() { Ingredient.soySauce, Ingredient.whiteRice, Ingredient.kelp };
    public static List<Ingredient> indexToIngredient = new List<Ingredient>() {
        Ingredient.calimari, Ingredient.tuna, Ingredient.shrimp,
        Ingredient.ginger, Ingredient.wasabi,
        Ingredient.soySauce, Ingredient.whiteRice, Ingredient.kelp };
    public static Dictionary<Ingredient, string> ingredientsToSpriteNames = new Dictionary<Ingredient, string>() {
        { Ingredient.calimari, "Calimari" }, { Ingredient.tuna, "Tuna" }, { Ingredient.shrimp, "Shrimp" },
        { Ingredient.ginger, "Ginger" }, { Ingredient.wasabi, "Wasabi" },
        { Ingredient.soySauce, "SoySauce" }, { Ingredient.whiteRice, "Rice" }, { Ingredient.kelp, "Kelp" }
    };
    public static Dictionary<Ingredient, Color> ingredientsToColors = new Dictionary<Ingredient, Color>() {
        { Ingredient.calimari, new Color(102f/255, 1f/255, 160f/255) }, { Ingredient.tuna, new Color(165f/255, 183f/255, 187f/255) }, { Ingredient.shrimp, new Color(231f/255, 114f/255, 134f/255) },
        { Ingredient.ginger, new Color(1, 1, 1) }, { Ingredient.wasabi, new Color(1, 1, 1) },
        { Ingredient.soySauce, new Color(231f/255, 114f/255, 134f/255) }, { Ingredient.whiteRice, new Color(1, 1, 1) }, { Ingredient.kelp, new Color(50f/255, 88f/255, 12f/255) }
    };
    public static Color starWinColor = new Color(240f / 255, 217f / 255, 217f / 255);
    public static Color starLoseColor = new Color(69f / 255, 69f / 255, 69f / 255);
    public List<Sprite> ingredientsOnPlate;
    public List<AudioClip> musics;
    public GameObject meter;
    Score scoreScript;
    public Animator[] anims;


    Order currentOrder;
    //in the order of spawn, queue, center (one we are working on), and out the door
    //So current plate is currentPlates[2]
    List<Plate> currentPlates;

    //Level information------------------------------------
    //CLIENTS PER LEVEL = 10, 15, 20
    int clientsFed = 0;
    int maxClientsThisLevel = 10;
    bool hasWon = false;
    bool hasLost = false;
    bool levelComplete = false;

    //Scoring------------------------------------------------
    int currentScore = 0;
    //ALGORITHM
    // - Under NUM_FOOD_ITEMS * SECONDS_PER_FOOD_ITEM seconds = Great
    // - Under NUM_FOOD_ITEMS * SECONDS_PER_FOOD_ITEM * 2 seconds = Meh
    // - Under NUM_FOOD_ITEMS * SECONDS_PER_FOOD_ITEM * 3 seconds = Bad
    // - Beyond Bad - They leave
    const int MAX_FOOD_ITEMS_TRACKED_FOR_SCORE = 5;
    const float SECONDS_PER_FOOD_ITEM = 1.5f;
    float timeSpentOnOrder = 0;
    bool freezeTimeSpent = false;

    ScoreAndDifficulty scoreAndDifficulty;
    CharacterPortrait portraitScript;

    void Start()
    {
        //DontDestroyOnLoad(this);
        scoreAndDifficulty = FindObjectOfType<ScoreAndDifficulty>();
        portraitScript = FindObjectOfType<CharacterPortrait>();
        setUpPlates();
        scoreScript = FindObjectOfType<Score>();
        if (scoreAndDifficulty.getUnlimitedMode()) {
            GetComponent<AudioSource>().clip = musics[2];
        } else {
            GetComponent<AudioSource>().clip = musics[scoreAndDifficulty.getDifficulty()];
            maxClientsThisLevel = 5 + scoreAndDifficulty.getDifficulty() * 5;
        }
        GetComponent<AudioSource>().Play();
    }

    void setUpPlates() {
        getNewOrder();
        currentPlates = new List<Plate>();
        advancePlates();
        advancePlates();
        advancePlates();
    }

    void Update() {
        if (!freezeTimeSpent) {
            timeSpentOnOrder += Time.deltaTime;
        }
        if (getTimeRemaining() < 0) {
            hasLost = true;
        }
        if (!levelComplete && (hasWon || hasLost)) {
            levelComplete = true;
            scoreScript.gameObject.SetActive(false);
            int highScore;
            if (scoreAndDifficulty.getUnlimitedMode()) {
                highScore = scoreAndDifficulty.getHighScore(3);
            } else {
                highScore = scoreAndDifficulty.getHighScore(scoreAndDifficulty.getDifficulty() - 1);
            }
            GameObject star = (GameObject)Instantiate(Resources.Load("Star"));
            if (currentScore > highScore) {
                star.transform.GetChild(1).GetComponent<TextMesh>().text = "New High Score:";
                if (scoreAndDifficulty.getUnlimitedMode()) {
                    scoreAndDifficulty.setHighScore(3, currentScore);
                } else {
                    scoreAndDifficulty.setHighScore(scoreAndDifficulty.getDifficulty() - 1, currentScore);
                }
            }
            if (hasLost) {
                star.transform.GetChild(0).GetComponent<TextMesh>().text = "You Lost!";
            }
            star.transform.GetChild(1).GetComponent<TextMesh>().text = "" + currentScore;
        }
        if (levelComplete) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                if (scoreAndDifficulty.getUnlimitedMode()) {
                    //TODO - go to intro screen
                    SceneManager.LoadScene("Title");
                } else {
                    if (hasLost) {
                        //TODO - go to intro screen
                        SceneManager.LoadScene("Title");
                    } else {
                        int difficulty = scoreAndDifficulty.getDifficulty();
                        if (difficulty == 1 || difficulty == 2) {
                            scoreAndDifficulty.setDifficulty(difficulty + 1);
                            //TODO - go to the same screen
                            SceneManager.LoadScene("Level");
                        } else {
                            //TODO - go to Cutscene 3
                            SceneManager.LoadScene("Cutscene3");
                        }
                    }
                }
            }
        }
    }

    public bool isLevelComplete() { return levelComplete; }

    //SCORING----------------------------------------------------
    //Best satisfaction is 0, 1 is meh, 2 is Bad, 3 is they leave
    public int getSatisfaction() {
        return (int) (timeSpentOnOrder / getScoredFoodItems() * 1 / SECONDS_PER_FOOD_ITEM);
    }

    public int getScoredFoodItems() {
        return Mathf.Min(currentOrder.getIngredientList().Count, MAX_FOOD_ITEMS_TRACKED_FOR_SCORE);
    }

    public float getTimeRemaining() {
        return getScoredFoodItems() * SECONDS_PER_FOOD_ITEM * 3 - timeSpentOnOrder;
    }

    public int getScore() {
        float timeLeft = getTimeRemaining();
        if (timeLeft <= 0 || !currentOrder.isCompleted()) {
            return 0;
        }
        int foodNum = getScoredFoodItems();
        float timeMultiplier = Mathf.Log(timeLeft + 1); //so the score is always positive
        int finalScore = (int)(foodNum * timeMultiplier * 100) * 10; //so it always ends in a 0
        freezeTimeSpent = true;
        return finalScore;
    }

    public void scorePlate() {
        int score = getScore();
        currentScore += score;
        scoreScript.addScore(score);
        clientsFed++;
    }

    const int INCORRECT_FOOD_SCORE = -500;
    public void addIncorrectFoodScore() {
        currentScore += INCORRECT_FOOD_SCORE;
        scoreScript.addScore(INCORRECT_FOOD_SCORE);
    }

    //ORDERS----------------------------------------------------
    public Order getCurrentOrder() {
        return currentOrder;
    }

    public void getNewOrder() {
        timeSpentOnOrder = 0;
        currentOrder = OrderCreator.getRandomOrder(scoreAndDifficulty.getDifficulty(), clientsFed, maxClientsThisLevel);
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
        portraitScript.setRandomPortrait();
        freezeTimeSpent = false;
    }

    public bool addToOrder(Ingredient i)
    {
        string totalOrder = "";
        foreach (Ingredient ingredient in currentOrder.getCurrentPlate()) {
            totalOrder += ingredient + " ";
        }
        print(i + " being added to (" + totalOrder + ")");
        bool result = currentOrder.addIngredient(i);
        if (!result) {
            addIncorrectFoodScore();
        }
        return result;
    }

    public void finishOrder() {
        advancePlates();
        if (clientsFed != maxClientsThisLevel) {
            getNewOrder();
        } else {
            if (scoreAndDifficulty.getUnlimitedMode()) {
                scoreAndDifficulty.setDifficulty(scoreAndDifficulty.getDifficulty() + 1);
                maxClientsThisLevel = 5 + scoreAndDifficulty.getDifficulty() * 5;
                setUpPlates();
            } else {
                hasWon = true;
            }
        }
    }

    //PLATES--------------------------------------------
    public void advancePlates() {
        //Moves all the plates forward
        foreach (Plate p in currentPlates) {
            p.MoveForward();
        }
        GameObject plate = (GameObject)Instantiate(Resources.Load("Plate"));
        currentPlates.Insert(0, plate.GetComponent<Plate>());
        //dont add extra plates
        if (clientsFed >= maxClientsThisLevel - 2) {
            plate.GetComponent<SpriteRenderer>().enabled = false;
            plate.transform.GetChild(0).gameObject.SetActive(false);
        }
        if (currentPlates.Count > 4) {
            currentPlates.RemoveAt(4);
        }
    }

    public Plate getCurrentPlate() { return currentPlates[2]; }

    //INGREDIENTS--------------------------------------------------
    public static bool isOther(Ingredient i) { return others.Contains(i); }
    public static bool isMeat(Ingredient i) { return meats.Contains(i); }
    public static bool isCondiment(Ingredient i) { return condiments.Contains(i); }
    public static Ingredient getRandomOther() { return others[Random.Range(0, others.Count)]; }
    public static Ingredient getRandomMeat() { return meats[Random.Range(0, meats.Count)]; }
    public static Ingredient getRandomCondiment() { return condiments[Random.Range(0, condiments.Count)]; }

}
