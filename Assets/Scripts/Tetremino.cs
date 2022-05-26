using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Tetremino : MonoBehaviour
{
    protected const int tetreminoVerticesSize = 3; // the size is the len of the grid vertices in x
    protected static float tetreminoBlockScale { get; private set; }
    protected static float moveToTheSideUnit { get; private set; }
    protected static float moveDownUnit { get; private set; }

    protected static float tetreminoScaleInWorld { get; private set; } = -1000;


    private TetreminoData data;

    protected struct TetreminoData
    {
        public Block[,] blocks;
        public Color color;

        public TetreminoData(Block[,] vertices , Color color)
        {
            if(vertices.GetLength(0) != tetreminoVerticesSize || vertices.GetLength(1) != tetreminoVerticesSize)
            {
                Debug.LogError($"tetremino vertices array should by the len of {tetreminoVerticesSize} , {tetreminoVerticesSize}");

                this.blocks = null;
                this.color = Color.black;

                return;
            }

            this.blocks = vertices;
            this.color = color;
        }
    }

    public void CrateTetremino()
    {
        if (tetreminoScaleInWorld == -1000)
        {
            tetreminoScaleInWorld = tetreminoBlockScale;
        }


        data = GetTetreminoData();
        SetGridRenderBlocks(data.blocks);
    }

    private void SetGridRenderBlocks(Block[,] blocks)
    {

        CenterGridRender centerGridRender = gameObject.AddComponent<CenterGridRender>() as CenterGridRender;

        centerGridRender.ClearGridRender();

        centerGridRender.SetGridBlocks(blocks, tetreminoBlockScale);
    }

    public static void SetTetreminosMoveToSideUnit(float amount)
    {
        moveToTheSideUnit = amount;
    }
    public static void SetTetremionoMoveDownUnit(float amount)
    {
        moveDownUnit = amount;
    }   
    public static void SetTetreminoBlockScale(float scale)
    {
        tetreminoBlockScale = scale;
    }

    protected abstract TetreminoData GetTetreminoData();
    
    public void MoveLeft()
    {
        transform.Translate(new Vector2(-moveToTheSideUnit , 0.0f));
    }

    public void MoveRight()
    {
        transform.Translate(new Vector2(moveToTheSideUnit , 0.0f));
    }
    public void MoveDown()
    {
        transform.Translate(new Vector2(0.0f,  -moveDownUnit));
    }

    public void RotateRight()
    {
        Block[,] newBlocks = new Block[TetreminoVerticesSize, tetreminoVerticesSize];

        for(int x = 0; x < tetreminoVerticesSize; x++)
        {
            for(int y = 0; y < TetreminoVerticesSize; y++)
            {
                newBlocks[y, x] = data.blocks[x, y];
            }
        }

        data.blocks = newBlocks;

        SetGridRenderBlocks(newBlocks);
        
    }
    public void RotatateLeft()
    {
        Block[,] newBlocks = new Block[TetreminoVerticesSize, tetreminoVerticesSize];

        for (int x = 0; x < tetreminoVerticesSize; x++)
        {
            for (int y = 0; y < TetreminoVerticesSize; y++)
            {
                newBlocks[(tetreminoVerticesSize - 1) - y, (tetreminoVerticesSize - 1) - x] = data.blocks[x, y];
            }
        }

        data.blocks = newBlocks;

        SetGridRenderBlocks(newBlocks);
    }

    public Block[,] TetreminoBlocks
    {
        get { return data.blocks; }
        set { }
    }

    public static int TetreminoVerticesSize
    {
        get { return tetreminoVerticesSize; }
        set { }
    }
    /*
    public static float TetreminoBlockScle
    {
        get { return tetreminoBlockScale; }
        set { }
    }

    public static float TetreminoScaleInWorld // the tetremino scale in world coordinates
    {

        get { return tetreminoBlockScale * tetreminoVerticesSize; }
        set { }
    }
   */
}
