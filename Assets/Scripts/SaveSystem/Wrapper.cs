using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaveData;
using System.Linq;

public static class Wrapper
{
    public static void SaveSnapshot(string fileName) => WrapHandler(fileName);
    public static void SaveSnapshot() => WrapHandler(Random.Range(1, 100000).ToString() + ".json");

    private static void WrapHandler(string fileName)
    {
        BoardStatement boardStatement = new BoardStatement();

        // tiles
        if (TileManager.Tiles == null || TileManager.Tiles[0] == null)
        {
            Debug.LogError("Incorrect tiles topology");
            throw new System.ArgumentNullException();
        }

        boardStatement.Tiles = new TileStatement[TileManager.Tiles.Count * TileManager.Tiles[0].Count];

        for (int i = 0; i < TileManager.Tiles.Count; i++)
        {
            for (int j = 0; j < TileManager.Tiles[0].Count; j++)
            {
                boardStatement.Tiles[i * TileManager.Tiles.Count +  j] = TileManager.Tiles[i][j].Statement();
            }
        }

        // buildings

        int countOfBuildings = 0;

        for (int i = 0; i < BuildingManager.Buildings.Count; i++)
        {
            for (int j = 0; j < BuildingManager.Buildings[0].Count; j++)
            {
                if (BuildingManager.Buildings[i][j] != null) countOfBuildings++;
            }
        }

        boardStatement.Buildings = new BuildingStatement[countOfBuildings];
        int index = 0;

        for (int i = 0; i < BuildingManager.Buildings.Count; i++)
        {
            for (int j = 0; j < BuildingManager.Buildings[0].Count; j++)
            {
                if (BuildingManager.Buildings[i][j] != null)
                {
                    boardStatement.Buildings[index] = BuildingManager.Buildings[i][j].Statement();
                    index++;
                }
            }
        }

        // other
        boardStatement.CurrentTurn = TurnController.CurrentPlayersTurn;

        // TODO ������������ ������� ���������
        boardStatement.Players = PlayerInstanceMethods.GetPlayers().ToArray();
        boardStatement.Wallets = WalletScript.Wallets.ToArray();

        SaveManager.Save(boardStatement, fileName);
    }


    public static void LoadSnapshot(string fileName) => LoadHandler(fileName);

    private static void LoadHandler(string fileName)
    {
        GameObject observer = GameObject.FindGameObjectWithTag("Observer");

        BoardStatement boardStatement = SaveManager.Load(fileName);
        Vector2Int boardTopology = new Vector2Int(boardStatement.Tiles.GetLength(0), boardStatement.Tiles.GetLength(1));

        // tiles
        List<List<AbstractTile>> tiles = new List<List<AbstractTile>>(boardTopology.x);
        for (int i = 0; i < boardTopology.x; i++)
        {
            tiles[i] = new List<AbstractTile>(boardTopology.y);
            for (int j = 0; j < boardTopology.y; j++)
            {
                tiles[i][j] = observer.GetComponent<TileFactory>().CreateTile(boardStatement.Tiles[i * boardTopology.x + j]).GetComponent<AbstractTile>();
            }
        }

        TileManager.Setup(tiles);

        // buildings
        List<List<AbstractBuilding>> buildings = new List<List<AbstractBuilding>>(boardTopology.x);
        for (int i = 0; i < boardTopology.x; i++) buildings[i] = new List<AbstractBuilding>(boardTopology.y);

        foreach (BuildingStatement buildingStatement in boardStatement.Buildings)
        {
            Vector2Int id = buildingStatement.id;
            buildings[id.x][id.y] = observer.GetComponent<BuildingFactory>().CreateBuilding(buildingStatement).GetComponent<AbstractBuilding>();
        }

        BuildingManager.Setup(buildings);

        // other
        
        // ������������� ���� ������
        // TODO ������������ ������� ���������
    }


    public static void DeserializeOnlineBoardStatement(BoardStatement boardStatement)
    {
        GameObject observer = GameObject.FindGameObjectWithTag("Observer");

        // tiles
        // WARNING ������� ������������ ������
        List<List<AbstractTile>> tilesN = new List<List<AbstractTile>>(16);
        for (int i = 0; i < 16; i++)
        {
            tilesN.Add(new List<AbstractTile>(16));
            for (int j = 0; j < 16; j++)
            {
                tilesN[i].Add(null);
            }
        }
        TileManager.Setup(tilesN);

        foreach (TileStatement tileState in boardStatement.Tiles)
        {
            Vector2Int id = tileState.id;
            TileManager.Tiles[id.x][id.y] = observer.GetComponent<TileFactory>().CreateTile(tileState).GetComponentInChildren<AbstractTile>();
        }

        // buildings
        List<List<AbstractBuilding>> buildings = new List<List<AbstractBuilding>>(16);
        for (int i = 0; i < 16; i++)
        {
            buildings.Add(new List<AbstractBuilding>(16));
            for (int j = 0; j < 16; j++)
            {
                buildings[i].Add(null);
            }
        }
        BuildingManager.Setup(buildings);

        foreach (BuildingStatement buildingStatement in boardStatement.Buildings)
        {
            Vector2Int id = buildingStatement.id;
            BuildingManager.Buildings[id.x][id.y] = observer.GetComponent<BuildingFactory>().CreateBuilding(buildingStatement).GetComponent<AbstractBuilding>();
        }


        // Turn
        TurnController.UnsafeSetTurn(boardStatement.CurrentTurn);
        // players
        PlayerInstanceMethods.Setup(boardStatement.Players.Length);
        // wallets
        WalletScript.Setup(boardStatement.Wallets.ToList());
    }
}
