using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour {

    public List<BodyPart> items = new List<BodyPart>();
    public List<bool> downs = new List<bool>();
    List<KeyCode> keys = new List<KeyCode>();
    CthulhuScript cs;
    GameManager gm;
    // Use this for initialization
    void Awake () {
        cs = FindObjectOfType<CthulhuScript>();
        gm = FindObjectOfType<GameManager>();
        for (int x = 0; x < 8; x++) {
            downs.Add(false);
        }
        keys.Add(KeyCode.A);
        keys.Add(KeyCode.S);
        keys.Add(KeyCode.D);
        keys.Add(KeyCode.F);
        keys.Add(KeyCode.H);
        keys.Add(KeyCode.J);
        keys.Add(KeyCode.K);
        keys.Add(KeyCode.L);
    }
	
	// Update is called once per frame
	void Update () {
        if (gm.isLevelComplete()) {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            cs.finishPlate();
        }
        for (int x = 0; x < keys.Count; x++)
        {
            if (Input.GetKeyDown(keys[x])) {
                cs.grabFood(x);
            }
            /*
            if (Input.GetKeyDown(keys[x]) && items[x].transform.position.y >= 3)
            {
                cs.grabFood(x);
                downs[x] = true;
            }
            if (downs[x] && items[x].transform.position.y > -3)
            {
                items[x].transform.Translate(0, -0.1f, 0);
            }
            if (items[x].transform.position.y <= -3)
            {
                downs[x] = false;
            }
            if (!downs[x] && items[x].transform.position.y < 3)
            {
                items[x].transform.Translate(0, 0.1f, 0);
            }*/
        }
    }
}
