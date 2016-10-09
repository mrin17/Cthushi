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

    const float V_INCREMENT = .1f;
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

    public Vector3 getNextPositionToMoveTowards(Ingredient i) {
        if (i.Equals(Ingredient.ginger)) {
            return transform.position + new Vector3(-.7f, .2f);
        } else if (i.Equals(Ingredient.wasabi)) {
            return transform.position + new Vector3(.7f, .2f);
        } else {
            return transform.position + new Vector3(0, .2f + V_INCREMENT * objectsOnPlate.Count);
        }
    }

    //repeats the first four locations
    public void AddObjectToPlate(GameObject obj) {
        Food objFood = obj.GetComponent<Food>();
        obj.transform.localScale = new Vector3(.5f, .5f, 1);
        obj.transform.position = getNextPositionToMoveTowards(objFood.getIngredient());
        objFood.enabled = false; //so it doesnt move when its not supposed to       
        obj.transform.parent = transform;       
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
