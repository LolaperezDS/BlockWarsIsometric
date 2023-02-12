﻿using System;
using UnityEngine;


public enum BoardActionEnum
{
    BuildBuilding,
    OwnTile,
    Shoot,
    DestroyBuilding,
    ChangeTurn,
    SendMessage,
    RepairBuilding
}

public static class BoardActionsEnumMethods
{
    public static int ToInt(BoardAction action) => action switch
    {
        BuildBuilding => (int) BoardActionEnum.BuildBuilding,
        OwnTile => (int) BoardActionEnum.OwnTile,
        Shoot => (int) BoardActionEnum.Shoot,
        DestroyBuilding => (int) BoardActionEnum.DestroyBuilding,
        ChangeTurn => (int) BoardActionEnum.ChangeTurn,
        SendMessage => (int) BoardActionEnum.SendMessage,
        RepairBuilding => (int) BoardActionEnum.RepairBuilding,
        _ => throw new Exception("Not Implemented action")
    };

    public static BoardActionEnum ToEnum(int actionId)
    {
        switch (actionId)
        {
            case (int) BoardActionEnum.BuildBuilding: return BoardActionEnum.BuildBuilding;
            case (int) BoardActionEnum.OwnTile: return BoardActionEnum.OwnTile;
            case (int) BoardActionEnum.Shoot: return BoardActionEnum.Shoot;
            case (int) BoardActionEnum.DestroyBuilding: return BoardActionEnum.DestroyBuilding;
            case (int) BoardActionEnum.ChangeTurn: return BoardActionEnum.ChangeTurn;
            case (int) BoardActionEnum.SendMessage: return BoardActionEnum.SendMessage;
            case (int) BoardActionEnum.RepairBuilding: return BoardActionEnum.RepairBuilding;
            default: throw new Exception("Not Implemented action");
        }
    }
}

public abstract class BoardAction
{
}

public class BuildBuilding : BoardAction
{
    private readonly Vector2Int _buildingCoords;
    public Vector2Int BuildingCoord { get; }

    private readonly BuildingType _buildingType;
    public BuildingType Building { get; }

    public BuildBuilding(Vector2Int coords, BuildingType buildingType)
    {
        _buildingCoords = coords;
        _buildingType = buildingType;
    }
}

public class OwnTile : BoardAction
{
    private readonly Vector2Int _tileCoords;
    public Vector2Int TileCoord { get; }

    public OwnTile(Vector2Int coords)
    {
        _tileCoords = coords;
    }
}

public class Shoot : BoardAction
{
    private readonly Vector2Int _source;
    public Vector2Int Source { get; }

    private readonly Vector2Int _destination;
    public Vector2Int Destination { get; }

    private readonly int _amountOfDamage;
    public int AmountOfDamage { get; }

    public Shoot(Vector2Int sourceCoords, Vector2Int destinationCoords, int amountOfDamage)
    {
        _source = sourceCoords;
        _destination = destinationCoords;
        _amountOfDamage = amountOfDamage;
    }
}

public class DestroyBuilding : BoardAction
{
    private readonly Vector2Int _buildingCoords;

    public Vector2Int BuildingCoord { get; }

    public DestroyBuilding(Vector2Int coords)
    {
        _buildingCoords = coords;
    }
}

public class ChangeTurn : BoardAction
{
}

public class SendMessage : BoardAction
{
    private readonly string _message;
    public string Message { get; }

    public SendMessage(string message)
    {
        _message = message;
    }
}

public class RepairBuilding : BoardAction
{
    private readonly Vector2Int _buildingCoord;
    public Vector2Int BuildingCoord { get; }

    private readonly int _repairAmount;
    public int RepairAmount { get; }

    private readonly ProduceValue _neededResources;
    public ProduceValue NeededResources { get; }

    public RepairBuilding(Vector2Int buildingCoord, int repairAmount, ProduceValue neededResources)
    {
        _buildingCoord = buildingCoord;
        _repairAmount = repairAmount;
        _neededResources = neededResources;
    }
}