using System.Collections.Generic;
using UnityEngine;
using System;

public class GameBoard: MonoBehaviour
{
    private TetreminoBlock[,] blocks;

    private float blockScale;

    private MinMaxGridRender gridRender;

    private int numOfTetreminos = 0;

    private int numOfColumns;
    private int numOfLines;


    public void CreateBoard(float xMin, float yMin, int numOfColoumns, int numOfLines, float blocksScale)
    {
        gridRender = gameObject.AddComponent<MinMaxGridRender>() as MinMaxGridRender;
        gridRender.SetBoardMin(xMin, yMin);

        blocks = new TetreminoBlock[numOfColoumns, numOfLines];

        this.numOfColumns = numOfColoumns;
        this.numOfLines = numOfLines;

        this.blockScale = blocksScale;
    }

    public bool TryMoveDownTetremino(int id)
    {
        List<Tuple<int, int>> tetreminoBlocksIndexesToMove = new List<Tuple<int, int>>();

        for (int x = 0; x < numOfColumns; x++)
        {
            for (int y = 0; y < numOfLines; y++)
            {

                if (blocks[x, y] == null || blocks[x, y].tetreminoId != id)
                {
                    continue;
                }

                if ((y != 0) && (blocks[x, y - 1] == null || blocks[x, y - 1].tetreminoId == blocks[x, y].tetreminoId))
                {
                    tetreminoBlocksIndexesToMove.Add(new Tuple<int, int>(x, y));
                }
                else
                {
                    return false;
                }
            }
        }

        if(tetreminoBlocksIndexesToMove.Count == 0)
        {
            return false;
        }

        for (int i = 0; i < tetreminoBlocksIndexesToMove.Count; i++)
        {
            int x = tetreminoBlocksIndexesToMove[i].Item1;
            int y = tetreminoBlocksIndexesToMove[i].Item2;

            blocks[x, y - 1] = blocks[x, y];
            blocks[x, y] = null;

        }

        gridRender.ClearGridRender();
        gridRender.SetGridBlocks(this.blocks, blockScale);

        return true;
    }
    public void TryMoveRightTetremino(int id)
    {
        List<Tuple<int, int>> tetreminoBlocksIndexesToMove = new List<Tuple<int, int>>();

        for (int x = 0; x < numOfColumns; x++)
        {
            for (int y = 0; y < numOfLines; y++)
            {
                if (blocks[x, y] == null || blocks[x, y].tetreminoId != id)
                {
                    continue;
                }

                if ((x != numOfColumns - 1) && (blocks[x + 1, y] == null || blocks[x + 1, y].tetreminoId == blocks[x, y].tetreminoId))
                {
                    tetreminoBlocksIndexesToMove.Add(new Tuple<int, int>(x, y));
                }
                else
                {
                    return;
                }
            }
        }

        for (int i = tetreminoBlocksIndexesToMove.Count - 1; i >= 0; i--)
        {
            int x = tetreminoBlocksIndexesToMove[i].Item1;
            int y = tetreminoBlocksIndexesToMove[i].Item2;

            blocks[x + 1, y] = blocks[x, y];
            blocks[x, y] = null;

        }

        gridRender.ClearGridRender();
        gridRender.SetGridBlocks(this.blocks, blockScale);
    }

    public void TryMoveLeftTetremino(int id)
    {
        List<Tuple<int, int>> tetreminoBlocksIndexesToMove = new List<Tuple<int, int>>();

        for (int x = 0; x < numOfColumns; x++)
        {
            for (int y = 0; y < numOfLines; y++)
            {

                if (blocks[x, y] == null || blocks[x, y].tetreminoId != id)
                {
                    continue;
                }

                if ((x != 0) && (blocks[x - 1, y] == null || blocks[x - 1, y].tetreminoId == blocks[x, y].tetreminoId))
                {
                    tetreminoBlocksIndexesToMove.Add(new Tuple<int, int>(x, y));
                }
                else
                {
                    return;
                }
            }
        }

        for (int i = 0; i < tetreminoBlocksIndexesToMove.Count; i++)
        {
            int x = tetreminoBlocksIndexesToMove[i].Item1;
            int y = tetreminoBlocksIndexesToMove[i].Item2;

            blocks[x - 1, y] = blocks[x, y];
            blocks[x, y] = null;

        }

        gridRender.ClearGridRender();
        gridRender.SetGridBlocks(this.blocks, blockScale);
    }


    public void TryRotateTetremino(int id)
    {
        List<Tuple<int, int>> tetreminoBlocksIndexesToRotate = new List<Tuple<int, int>>();

        for (int x = 0; x < numOfColumns; x++)
        {
            for (int y = 0; y < numOfLines; y++)
            {

                if (blocks[x, y] == null || blocks[x, y].tetreminoId != id)
                {
                    continue;
                }


                tetreminoBlocksIndexesToRotate.Add(new Tuple<int, int>(x, y));
            }
        }

        int highestColumn = 0;
        int highestLine = 0;

        int lowestColumn = numOfColumns;
        int lowestLine = numOfLines;

        for (int i = 0; i < tetreminoBlocksIndexesToRotate.Count; i++)
        {
            int x = tetreminoBlocksIndexesToRotate[i].Item1;
            int y = tetreminoBlocksIndexesToRotate[i].Item2;

            if (x > highestColumn)
            {
                highestColumn = x;
            }
            if (x < lowestColumn)
            {
                lowestColumn = x;
            }

            if (y > highestLine)
            {
                highestLine = y;
            }
            if (y < lowestLine)
            {
                lowestLine = y;
            }
        }

        if (highestColumn - lowestColumn < GameManager.tetreminoBlocksArrayLen - 1)
        {
            lowestColumn--;
        }
        if (highestLine - lowestLine < GameManager.tetreminoBlocksArrayLen - 1)
        {
            lowestLine--;
        }

        if(lowestColumn < 0 || lowestLine < 0 || highestColumn >= numOfColumns || highestLine >= numOfLines)
        {
            return;
        }

        for (int x = lowestColumn; x <= highestColumn; x++)
        {
            for (int y = lowestLine; y <= highestLine; y++)
            {
                if (blocks[x, y] != null && blocks[x, y].tetreminoId != id)
                {
                    return;
                }
            }
        }

        TetreminoBlock[,] rotatedBlocks = new TetreminoBlock[GameManager.tetreminoBlocksArrayLen, GameManager.tetreminoBlocksArrayLen];
        for (int x = 0; x < GameManager.tetreminoBlocksArrayLen; x++)
        {
            for (int y = 0; y < GameManager.tetreminoBlocksArrayLen; y++)
            {
                rotatedBlocks[x, GameManager.tetreminoBlocksArrayLen - 1 - y] = blocks[lowestColumn + y, lowestLine + x];
            }
        }

        for (int x = 0; x < GameManager.tetreminoBlocksArrayLen; x++)
        {
            for (int y = 0; y < GameManager.tetreminoBlocksArrayLen; y++)
            {
                blocks[lowestColumn + x, lowestLine + y] = rotatedBlocks[x, y];
            }
        }

        gridRender.ClearGridRender();
        gridRender.SetGridBlocks(this.blocks, blockScale);
    }

    public int AddTetreminoToBoard(TetreminoBlock[,] tetreminoBlocks, int coloumn, int line, out bool genOnOtherBlock) // returns the tetremino id
    {

        for (int x = 0; x < tetreminoBlocks.GetLength(0); x++)
        {
            for (int y = 0; y < tetreminoBlocks.GetLength(1); y++)
            {

                if (tetreminoBlocks[y, x] == null)
                {
                    continue;
                }

                if (this.blocks[coloumn + x, line - 1 - y] != null)
                {
                    genOnOtherBlock = true;
                    return 0;
                }

                blocks[coloumn + x, (line - 1) - y] = tetreminoBlocks[y, x];
                blocks[coloumn + x, (line - 1) - y].SetTetreminoId(numOfTetreminos);

                genOnOtherBlock = false;
            }
        }

        genOnOtherBlock = false;

        gridRender.ClearGridRender();
        gridRender.SetGridBlocks(this.blocks, blockScale);

        int tetreminoId = numOfTetreminos;
        numOfTetreminos++;

        return tetreminoId;
    }

    public bool IsContainBlock(int column, int line)
    {
        if (blocks[column, line] == null)
        {
            return false;
        }
        return true;
    }

    public void HandleDestroyLinesAndMoveDowndBlocks()
    {
        bool isChanged = true;

        while (isChanged)
        {
            isChanged = false;

            if (HandleDestroyLines())
            {
                isChanged = true;

                HandleMoveBlocksDown();

                gridRender.ClearGridRender();
                gridRender.SetGridBlocks(this.blocks, blockScale);
            }
        }
    }

    private bool HandleDestroyLines()
    {
        bool destroyLines = false;

        for (int y = 0; y < blocks.GetLength(1); y++)
        {
            for (int x = 0; x < blocks.GetLength(0); x++)
            {
                if (blocks[x, y] == null)
                {
                    break;
                }
                else if (x == blocks.GetLength(0) - 1)
                {
                    DestroyLine(y);
                    destroyLines = true;
                }
            }
        }

        return destroyLines;
    }

    private void DestroyLine(int line)
    {
        for (int i = 0; i < blocks.GetLength(0); i++)
        {
            blocks[i, line] = null;
        }
    }

    private void HandleMoveBlocksDown()
    {
        bool isChange = true;

        while (isChange)
        {
            isChange = false;
            for (int i = 0; i <= numOfTetreminos; i++)
            {
                if (TryMoveDownTetremino(i))
                {
                    isChange = true;
                }
            }
        }
    }
}
