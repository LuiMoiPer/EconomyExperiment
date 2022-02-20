using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;
public class GameBoard : MonoBehaviour {

    [SerializeField]
    GameObject tilePrefab;
    HexMap<int> hexMap;
    [SerializeField]
    int size = 4;
    
    void Start() {
        hexMap = new HexMap<int>(HexMapBuilder.CreateHexagonalShapedMap(size), null);

        MakeBoard();
    }

    private void MakeBoard() {
        foreach (var tile in hexMap.Tiles) {
            GameObject tileInstance = GameObject.Instantiate(tilePrefab);
            tileInstance.name = "" + tile.Position;
            tileInstance.transform.position = tile.CartesianPosition;
        }
    }
}
