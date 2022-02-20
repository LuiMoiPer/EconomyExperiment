using System.Collections;
using System.Collections.Generic;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;
public class GameTile {
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
    }
}

