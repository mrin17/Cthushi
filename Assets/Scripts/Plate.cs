﻿using UnityEngine;
using System.Collections.Generic;

public class Plate : MonoBehaviour {

    enum PlateState { spawn, queue, center, outTheDoor };

    Vector3 spawnPosition = new Vector3(-6, -4.75f, 0);
    Vector3 queuePosition = new Vector3(-3, -4.75f, 0);
    Vector3 centerPosition = new Vector3(.9f, -4.75f, 0);
    Vector3 outTheDoorPosition = new Vector3(8, -4.75f, 0);
    float movementSpeed = .25f;
    float movementSpeedOutDoor = .5f;
    PlateState currentState = PlateState.spawn;

    const float H_OFFSET = .25f;
    const float V_OFFSET = .2f;
    static List<Vector3> positions = new List<Vector3>() {
        new Vector3(-H_OFFSET, V_OFFSET, 0), new Vector3(H_OFFSET, V_OFFSET, 0),
        new Vector3(-H_OFFSET, -V_OFFSET, 0), new Vector3(H_OFFSET, -V_OFFSET, 0) };
    List<GameObject> objectsOnPlate;


    void Start() {
        objectsOnPlate = new List<GameObject>();
        transform.position = spawnPosition;
    }

    void Update() {
        float s = movementSpeed;
        if (currentState == PlateState.outTheDoor) {
            s = movementSpeedOutDoor;
        }
        Vector3 newPos = Vector3.MoveTowards(transform.position, getTargetPosition(), s);
        transform.position = newPos;
    }

    public Vector3 getNextPositionToMoveTowards() {
        int whichLoc = objectsOnPlate.Count % 4;
        return transform.position + positions[whichLoc];
    }

    //repeats the first four locations
    public void AddObjectToPlate(GameObject obj) {
        int whichLoc = objectsOnPlate.Count % 4;
        obj.transform.position = transform.position + positions[whichLoc];
        obj.transform.parent = transform;
        obj.GetComponent<Food>().enabled = false; //so it doesnt move when its not supposed to
        objectsOnPlate.Add(obj);
    }

    public void ClearPlate() {
        foreach(GameObject o in objectsOnPlate) {
            Destroy(o);
        }
        objectsOnPlate = new List<GameObject>();
    }
	
    public void MoveForward() {
        switch (currentState) {
            case PlateState.queue:
                currentState = PlateState.center;
                break;
            case PlateState.center:
                currentState = PlateState.outTheDoor;
                break;
            case PlateState.outTheDoor:
                Destroy(gameObject);
                break;
            case PlateState.spawn:
            default:
                currentState = PlateState.queue;
                break;
        }
    }

    Vector3 getTargetPosition() {
        switch (currentState) {
            case PlateState.queue:
                return queuePosition;
            case PlateState.center:
                return centerPosition;
            case PlateState.outTheDoor:
                return outTheDoorPosition;
            case PlateState.spawn:
            default:
                return spawnPosition;
        }
    }
}
