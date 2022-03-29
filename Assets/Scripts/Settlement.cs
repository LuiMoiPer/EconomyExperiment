using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;

public class Settlement {

    public enum Type {
        NONE = 0,
        VILLAGE = 1,
        TOWN = 2
    }

    public Type type {get; private set;}
    public Dictionary<Resource.Type, int> resources {get; private set;}

    public Settlement() {
        type = Type.NONE;
        resources = new Dictionary<Resource.Type, int>();
        foreach (Resource.Type resource in Enum.GetValues(typeof(Resource.Type))) {
            resources.Add(resource, 0);
        }
    }

    public void SetType(Type type) {
        this.type = type;
    }

    public void ProduceResource(Resource resource) {
        if (resource == null) {
            return;
        }

        if (type == Type.VILLAGE) {
            resources[resource.type] += 1;
        }
        else if (type == Type.TOWN) {
            resources[resource.type] += 2;
        }

        PrintResources();
    }

    public void AddResource(Resource resource, int amount) {
        if (amount < 0) {
            throw new ArgumentException();
        }

        resources[resource.type] += amount;
    }

    private void PrintResources() {
        String output = "";

        foreach (var resource in resources.Keys) {
            output += resource.ToString();
            output += ": ";
            output += resources[resource];
            output += "\n";
        }

        Debug.Log(output);
    }
}

