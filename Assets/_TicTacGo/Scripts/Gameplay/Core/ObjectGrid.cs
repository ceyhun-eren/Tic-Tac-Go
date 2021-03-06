﻿using System.Collections.Generic;
using UnityEngine;

public class Grid 
{
    public int id { get; private set; }

    public float xPos { get; private set; }
    public float yPos { get; private set; }
    //public float zPos { get; private set; }
    public bool isEmpty { get; set; }

    public Grid(float x, float y, int gridId)
    {
        xPos = x;
        yPos = y;
        //zPos = z;

        id = gridId;
        isEmpty = true;
    }

}

public class ObjectGrid : Singleton<ObjectGrid>
{
    public List<Grid> emptyGrids = new List<Grid>();
    public List<Grid> fullGrids = new List<Grid>();

    [HideInInspector, SerializeField] float x_Start = -2f;
    [HideInInspector, SerializeField] float y_Start = 3.3f;
    [HideInInspector, SerializeField] float x_Space = 1.01f;
    [HideInInspector, SerializeField] float y_Space = 1.01f;
    [HideInInspector, SerializeField] int columnLength = 5;
    [HideInInspector, SerializeField] int rowLength = 7;

    void Start()
    {
        CreateGrid();
    }
   
    // Create grids when is called
    private void CreateGrid()
    {
        for (int i = 0; i < columnLength * rowLength; i++)
        {
            var gridX = x_Start + (x_Space * (i % columnLength));
            var gridY = y_Start + (-y_Space * (i / columnLength));
            
            Grid newGrid = new Grid(gridX, gridY, i);

            emptyGrids.Add(newGrid);
        }
    }

    public Vector3 EmptyPoint()
    {
        /*
        // Get empty grids type of IEnumarable
        var empty = from emptyGrid in objectGrid
                    where emptyGrid.isEmpty
                    select emptyGrid;

        // And apply empty grids type to Array
        Grid[] emptySlots = empty.ToArray();
        
        // After select a random element in empty grids type of Array
        var point = emptySlots[Random.Range(0, emptySlots.Length)];*/

        // Get empty grid in emptyGrids
        var random = Random.Range(0, emptyGrids.Count);

        var point = emptyGrids[random];

        // Set selected grid is not empty
        point.isEmpty = false;

        // Move to fullgrids
        fullGrids.Add(point);
        emptyGrids.Remove(point);

        // Finally return random empty grid vector value
        Vector3 emptyPoint = new Vector3(point.xPos, point.yPos, point.id);
        return emptyPoint;
    }

    public void GridEmptied(int emptiedId)
    {
        foreach (Grid item in fullGrids)
        {
            if(item.id == emptiedId)
            {
                emptyGrids.Add(item);
                break;
            }
        }
    }

}
