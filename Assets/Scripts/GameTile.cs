using System;
using System.Collections;
using System.Collections.Generic;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;
public class GameTile {
    private List<Settlement> settlements;

    public static Random random = new Random();
    public static Type RandomLandType() {
        Type[] landTypes = {
            Type.DESERT,
            Type.FIELD,
            Type.FOREST,
            Type.HILL,
            Type.MOUNTAIN,
            Type.PASTURE
        };

        int index = random.Next(landTypes.Length);
        return landTypes[index];
    }

    public enum Type {
        DESERT = 0,
        FIELD = 1,
        FOREST = 2,
        HILL = 3,
        MOUNTAIN = 4,
        OCEAN = 5,
        PASTURE = 6
    }
    public Type type {get; private set;}
    public int produceOn {get; private set;}

    public GameTile() {}

    public GameTile(Type type, int produceOn) {
        this.type = type;
        this.produceOn = produceOn;
        this.settlements = new List<Settlement>();
    }

    public bool IsLand() {
        return type != Type.OCEAN;
    }

    public void Produce(int roll) {
        if (roll != produceOn) {
            return;
        }

        foreach (var settlement in settlements) {
            settlement.ProduceResource(Utils.TileToResouce(this));
        }
    }

    public void AddSettlement(Settlement settlement) {
        if (settlements.Count >= 3) {
            throw new ArgumentException("Tile already has 3 settlements on it");
        }

        settlements.Add(settlement);
    }
}

