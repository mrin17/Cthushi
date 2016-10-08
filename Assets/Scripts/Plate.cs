using UnityEngine;
using System.Collections.Generic;

public class Plate : MonoBehaviour {

    static Vector3 spawnPosition = new Vector3(0, 0, 0);
    static Vector3 queuePosition = new Vector3(0, 0, 0);
    static Vector3 centerPosition = new Vector3(0, 0, 0);
    static Vector3 outTheDoorPosition = new Vector3(0, 0, 0);

    static List<Vector3> positions = new List<Vector3>() {
        new Vector3(-5, 5, 0), new Vector3(5, 5, 0),
        new Vector3(-5, -5, 0), new Vector3(5, -5, 0) };
    List<GameObject> objectsOnPlate;

    // Use this for initialization
    void Start() {
        objectsOnPlate = new List<GameObject>();
        transform.position = spawnPosition;
    }

    //repeats the first four locations
    public void AddObjectToPlate(GameObject obj) {
        int whichLoc = objectsOnPlate.Count % 4;
        obj.transform.position = transform.position + positions[whichLoc];
        objectsOnPlate.Add(obj);
    }

    public void ClearPlate() {
        foreach(GameObject o in objectsOnPlate) {
            Destroy(o);
        }
        objectsOnPlate = new List<GameObject>();
    }
	
    //Movement methods that move the plate to various locations
	public void MoveToQueue() {

    }

    //plate currently working on
    public void MoveToCenter() {

    }

    public void MoveOutDoor() {

    }
}
