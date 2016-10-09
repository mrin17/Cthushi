using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour {

    bool gettingThrown = false;
    float moveSpeed = .25f;
    Vector3 locToMoveTowards;
    Ingredient ingredient;

    // Update is called once per frame
    void Update() {
        if (locToMoveTowards != null) {
            transform.position = Vector3.MoveTowards(transform.position, locToMoveTowards, moveSpeed);
        }
        if (transform.position.x > 8) {
            Destroy(this.gameObject);
        }
    }

    public void throwFood() {
        locToMoveTowards = new Vector3(8, transform.position.y, 0);
    }

    public void moveTowardsLocation(Vector3 loc) {
        locToMoveTowards = loc;
    }

    public void setIngredient(Ingredient i) { ingredient = i; }
    public Ingredient getIngredient() { return ingredient; }
}
