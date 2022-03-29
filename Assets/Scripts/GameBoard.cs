using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;
public class GameBoard : MonoBehaviour {

    [SerializeField] GameObject tilePrefab;
    [SerializeField] GameObject villagePrefab;
    [SerializeField] GameObject townPrefab;
    [SerializeField] GameObject roadPrefab;
    [SerializeField] Material[] tileMaterials = new Material[7];
    Transform tileParent;
    Transform settlementParent;
    Transform roadParent;

    [SerializeField] int size = 4;
    HexMap<GameTile, bool, Settlement> hexMap;
    Dictionary<Settlement.Type, HashSet<Corner<Settlement>>> cornerBySettlementType = new Dictionary<Settlement.Type, HashSet<Corner<Settlement>>>();
    Dictionary<bool, HashSet<Edge<bool>>> edgesByExistingRoad = new Dictionary<bool, HashSet<Edge<bool>>>();
    List<Tile<GameTile>> landTiles = new List<Tile<GameTile>>();
    HashSet<Corner<Settlement>> validSettlementLocations = new HashSet<Corner<Settlement>>();
    HashSet<Edge<bool>> validRoadLocations = new HashSet<Edge<bool>>();
    
    
    void Start() {
        tileParent = new GameObject("Tile Parent").transform;
        tileParent.parent = this.gameObject.transform;

        roadParent = new GameObject("Road Parent").transform;
        roadParent.parent = this.gameObject.transform;

        settlementParent = new GameObject("Settlement Parent").transform;
        settlementParent.parent = this.gameObject.transform;

        hexMap = new HexMap<GameTile, bool, Settlement>(HexMapBuilder.CreateHexagonalShapedMap(size), null);
        MakeBoard();
        AddRandomSettlement();
    }

    void Update() {
        if (Input.GetKeyDown("space")) {
            RandomProduction();
        }
        
        if (Input.GetKeyDown(KeyCode.S)) {
            AddRandomSettlement();
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            AddRandomRoad();
        }
    }

    public void RandomProduction() {
        int roll = Random.Range(1, 7) + Random.Range(1, 7);
        Produce(roll);
    }

    public void Produce(int number) {
        foreach (Tile<GameTile> tile in hexMap.Tiles) {
            tile.Data.Produce(number);
        }
    }

    private void MakeBoard() {
        MakeBoardData();
        MakeBoardVisuals();
    }

    private void MakeBoardData() {
        foreach (var tile in hexMap.Tiles) {
            GameTile gameTile = new GameTile(
                GameTile.RandomLandType(), 
                Random.Range(1, 7) + Random.Range(1, 7)
            );
            tile.Data = gameTile;
        }

        foreach (var tile in hexMap.GetTiles.Ring(Vector3Int.zero, size, 1)) {
            tile.Data = new GameTile(GameTile.Type.OCEAN, 0);
        }

        // Set up land tile list
        foreach (var tile in hexMap.Tiles) {
            if (tile.Data.IsLand()) {
                landTiles.Add(tile);
            }
        }

        foreach (var tile in landTiles) {
            foreach (Corner<Settlement> corner in hexMap.GetCorners.OfTile(tile)) {
                // Set up the list of valid settlement locations
                validSettlementLocations.Add(corner);
                // Set up settlements by type
                AddToCornerDictionary(corner);
            }
        }

        // Set up edges dictionary
        foreach (var tile in landTiles) {
            foreach (Edge<bool> edge in hexMap.GetEdges.OfTile(tile)) {
                AddToEdgeDictionary(edge);
            }
        }
    }

    private void AddToCornerDictionary(Corner<Settlement> corner) {
        Settlement settlement = corner.Data;
        if (cornerBySettlementType.ContainsKey(settlement.type) == false) {
            cornerBySettlementType.Add(settlement.type, new HashSet<Corner<Settlement>>());
        }

        cornerBySettlementType[settlement.type].Add(corner);
    }

    private void AddToEdgeDictionary(Edge<bool> edge) {
        bool hasRoad = edge.Data;
        if (edgesByExistingRoad.ContainsKey(hasRoad) == false) {
            edgesByExistingRoad.Add(hasRoad, new HashSet<Edge<bool>>());
        }
        edgesByExistingRoad[hasRoad].Add(edge);
    }

    private void MakeBoardVisuals() {
        foreach (var tile in hexMap.Tiles) {
            GameObject tileInstance = GameObject.Instantiate(tilePrefab, tileParent);
            tileInstance.name = "" + tile.Position;
            tileInstance.GetComponent<Renderer>().material = tileMaterials[(int) tile.Data.type];
            tileInstance.transform.position = tile.CartesianPosition;
        }
    }

    private void AddRandomSettlement() {
        if (validSettlementLocations.Count == 0) {
            Debug.Log("No valid settlement placement locations.");
            return;
        }

        Corner<Settlement>[] emptyCorners = new Corner<Settlement>[validSettlementLocations.Count];
        validSettlementLocations.CopyTo(emptyCorners);
        Corner<Settlement> corner = emptyCorners[Random.Range(0, emptyCorners.Length)];
        RemoveFromValidLocations(corner);

        cornerBySettlementType[Settlement.Type.NONE].Remove(corner);
        corner.Data.SetType(Settlement.Type.VILLAGE);
        AddToCornerDictionary(corner);
        AddToTileSettlements(corner);
        UpdateValidRoadLocations(corner);

        GameObject settlementInstance = GameObject.Instantiate(villagePrefab);
        settlementInstance.name = "" + corner.Index;
        settlementInstance.transform.position = corner.CartesianPosition;
        settlementInstance.transform.parent = settlementParent;
    }

    private void RemoveFromValidLocations(Corner<Settlement> corner) {
        List<Corner<Settlement>> toRemove = hexMap.GetCorners.AdjacentToCorner(corner);
        toRemove.Add(corner);

        validSettlementLocations.ExceptWith(toRemove);
    }

    private void AddRandomRoad() {
        if (validRoadLocations.Count == 0) {
            Debug.Log("No valid road placement locations.");
            return;
        }

        Edge<bool>[] validEdges = new Edge<bool>[validRoadLocations.Count];
        validRoadLocations.CopyTo(validEdges);
        Edge<bool> edge = validEdges[Random.Range(0, validEdges.Length)];
        UpdateValidRoadLocations(edge);

        edgesByExistingRoad[false].Remove(edge);
        edge.Data = true;
        AddToEdgeDictionary(edge);

        GameObject roadInstance = GameObject.Instantiate(roadPrefab);
        roadInstance.name = "" + edge.Index;
        roadInstance.transform.position = edge.CartesianPosition;
        roadInstance.transform.parent = roadParent;

        Corner<Settlement> corner = hexMap.GetCorners.OfEdge(edge)[0];
        roadInstance.transform.LookAt(corner.CartesianPosition);
    }

    private void UpdateValidRoadLocations(Edge<bool> edge) {
        validRoadLocations.Remove(edge);

        HashSet<Edge<bool>> toAdd = new HashSet<Edge<bool>>();
        List<Edge<bool>> adjacentEdges = hexMap.GetEdges.AdjacentToEdge(edge);
        foreach (var adjacentEdge in adjacentEdges) {
            if (adjacentEdge.Data == false && IsTouchingLand(adjacentEdge)) {
                toAdd.Add(adjacentEdge);
            }
        }

        validRoadLocations.UnionWith(toAdd);
    }

    private void UpdateValidRoadLocations(Corner<Settlement> corner) {
        HashSet<Edge<bool>> toAdd = new HashSet<Edge<bool>>();
        List<Edge<bool>> adjacentEdges = hexMap.GetEdges.AdjacentToCorner(corner);
        foreach (var adjacentEdge in adjacentEdges) {
            if (adjacentEdge.Data == false && IsTouchingLand(adjacentEdge)) {
                toAdd.Add(adjacentEdge);
            }
        }

        validRoadLocations.UnionWith(toAdd);
    }

    private bool IsTouchingLand(Edge<bool> edge) {
        List<Tile<GameTile>> adjacentTiles = hexMap.GetTiles.AdjacentToEdge(edge);
        foreach (var tile in adjacentTiles) {
            if (tile.Data.IsLand()) {
                return true;
            }
        }
        return false;
    }

    private void AddToTileSettlements(Corner<Settlement> corner) {
        List<Tile<GameTile>> adjacentTiles = hexMap.GetTiles.AdjacentToCorner(corner);
        foreach (var tile in adjacentTiles) {
            tile.Data.AddSettlement(corner.Data);
        }
    }
}
