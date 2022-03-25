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

    public enum Resource {
        BRICK,
        GRAIN,
        LUMBER,
        ORE,
        WOOL
    }

    public Type type {get; private set;}
    public Dictionary<Resource, int> resources {get; private set;}

    public static String ResourceToString(Resource resource) {
        switch (resource) {
            case Resource.BRICK:
            return "Brick";

            case Resource.GRAIN:
            return "Grain";

            case Resource.LUMBER:
            return "Lumber";

            case Resource.ORE:
            return "Ore";

            case Resource.WOOL:
            return "Wool";

            default:
            throw new ArgumentException();
        }
    }

    public Settlement() {
        type = Type.NONE;
        resources = new Dictionary<Resource, int>();
        foreach (Resource resource in Enum.GetValues(typeof(Resource))) {
            resources.Add(resource, 0);
        }
    }

    public void SetType(Type type) {
        this.type = type;
    }

    public void ProduceResource(Resource resource) {
        if (type == Type.VILLAGE) {
            resources[resource] += 1;
        }
        else if (type == Type.TOWN) {
            resources[resource] += 2;
        }

        PrintResources();
    }

    public void AddResource(Resource resource, int amount) {
        if (amount < 0) {
            throw new ArgumentException();
        }

        resources[resource] += amount;
    }

    private void PrintResources() {
        String output = "";

        foreach (var resource in resources.Keys) {
            output += ResourceToString(resource);
            output += ": ";
            output += resources[resource];
            output += "\n";
        }

        Debug.Log(output);
    }
}

