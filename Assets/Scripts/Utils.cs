using System;

public class Utils {
    public static Resource TileToResouce(GameTile tile) {
        switch (tile.type) {
            case GameTile.Type.FIELD:
                return Resource.Grain();
            
            case GameTile.Type.FOREST:
                return Resource.Lumber();

            case GameTile.Type.HILL:
                return Resource.Brick();

            case GameTile.Type.MOUNTAIN:
                return Resource.Ore();

            case GameTile.Type.PASTURE:
                return Resource.Wool();

            case GameTile.Type.DESERT:
            case GameTile.Type.OCEAN:
                return null;
            
            default:
                throw new ArgumentException();
        }
    }
}