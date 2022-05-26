using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameBord : MonoBehaviour
{
    private Block[,] blocks;

    private float blockScale;

    private MinMaxGridRender gridRender;

    public void CrateBord(float xMin, float yMin, int bordWidth, int bordHight, float blocksScale)
    {
        gridRender = gameObject.AddComponent<MinMaxGridRender>() as MinMaxGridRender;
        gridRender.SetBordMin(xMin, yMin);

        blocks = new Block[bordWidth, bordHight];

        this.blockScale = blocksScale;
    }

    public void AddBlocksToBord(Block[,] blocks, int colom, int line)
    {
        for (int x = 0; x < blocks.GetLength(0); x++)
        {
            for (int y = 0; y < blocks.GetLength(1); y++)
            {

                if (blocks[y, x] == null)
                {
                    continue;
                }

                this.blocks[colom + x, line + (int)(Tetremino.TetreminoVerticesSize) - y] = blocks[y, x];


            }
        }

        gridRender.ClearGridRender();
        gridRender.SetGridBlocks(this.blocks, blockScale);
    }

    public bool IsContainBlock(int colom, int line)
    {
        if (blocks[colom + 1, line] == null)
        {
            return false;
        }
        return true;
    }

    public void HandelDestroyLines()
    {
        for (int y = 1; y < blocks.GetLength(1); y++) // starts placing baocks at line 1
        {
            for (int x = 1; x < blocks.GetLength(0); x++) // starts placing blocks in colom 1
            {
                if (blocks[x, y] == null)
                {
                    break;
                }
                else if (x == blocks.GetLength(0) - 1)
                {
                    DestroyLine(y);
                    MoveBlocksDown();

                    gridRender.ClearGridRender();
                    gridRender.SetGridBlocks(this.blocks, blockScale);

                    break;
                }
            }
        }
    }

    private void DestroyLine(int line)
    {
        for (int i = 1; i < blocks.GetLength(0); i++)
        {
            blocks[i, line] = null;
        }
    }

    private void MoveBlocksDown()
    {
           for (int y = blocks.GetLength(1) - 1; y > 1; y--)
        {
            for (int x = 1; x < blocks.GetLength(0); x++)
            {
                if (blocks[x, y - 1] == null)

                    blocks[x, y - 1] = blocks[x, y];
                blocks[x, y] = null;
            }
        }
    }
}

