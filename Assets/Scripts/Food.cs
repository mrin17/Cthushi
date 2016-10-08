using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour {

    bool gettingThrown = false;
    Vector3 add = new Vector3(.5f, 0, 0);

    // Update is called once per frame
    void Update() {
        if (gettingThrown) {
            transform.Translate(add);
        }
        if (transform.position.x > 8) {
            Destroy(this.gameObject);
        }
    }

    public void throwFood() {
        gettingThrown = true;
    }
}
