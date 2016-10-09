using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {

    public float TARGET_SCALE = 2;
    float currentScale = .1f;
    const float SCALE_INCREMENT = .1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (currentScale < TARGET_SCALE) {
            currentScale += SCALE_INCREMENT;
        }
        transform.localScale = new Vector3(currentScale, currentScale, 1);
	}
}
