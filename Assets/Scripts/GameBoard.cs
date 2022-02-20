using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;
public class GameBoard : MonoBehaviour {

    [SerializeField] GameObject tilePrefab;
    [SerializeField] int size = 4;
    [SerializeField] Material[] tileMaterials = new Material[7];
    HexMap<GameTile> hexMap;
    
    
    void Start() {
        hexMap = new HexMap<GameTile>(HexMapBuilder.CreateHexagonalShapedMap(size), null);
        MakeBoard();
    }

    private void MakeBoard() {
        foreach (var tile in hexMap.Tiles) {
            GameTile gameTile = new GameTile(
                (GameTile.Type) Random.Range(0, 7), 
                Random.Range(2, 13)
            );
            tile.Data = gameTile;

            GameObject tileInstance = GameObject.Instantiate(tilePrefab);
            tileInstance.name = "" + tile.Position;
            tileInstance.GetComponent<Renderer>().material = tileMaterials[(int) gameTile.type];
            tileInstance.transform.position = tile.CartesianPosition;
        }
    }
}
