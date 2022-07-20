using System.Collections.Generic;
using UnityEngine;

public class GameManager: MonoBehaviour
{
   public const int tetreminoBlocksArrayLen = 3; // can't by changed

    private int curentFallingTetremino_Id;

    private float lastTickTime;

    [SerializeField] private float timeBetweenTicks;

    [SerializeField] private Collider2D gameArea; // this canvas represent the area 

    [SerializeField] GameObject loseScreen;

    private float tetreminoBlockScale;

    private float gameArea_yMin;
    private float gameArea_yMax;

    private float gameArea_xMin;
    private float gameArea_xMax;

    private const int numOfLines = 20; // needs to be an even number
    private int numOfColoms;

    private int startLine;
    private int startColom;

    private GameBoard gameBoard;


    private void Start()
    {
        loseScreen.SetActive(false);

        gameArea_yMin = gameArea.bounds.min.y;
        gameArea_yMax = gameArea.bounds.max.y;

        gameArea_xMin = gameArea.bounds.min.x;
        gameArea_xMax = gameArea.bounds.max.x;

        tetreminoBlockScale = CalculateTetreminoBlockScale();

        numOfColoms = CalculateNumOfColumns();

        startLine = numOfLines;
        startColom = numOfColoms / 2;

        gameBoard = CrateGameBoard();

        CreateRandomTereminoAndSetAsCurrent();

        lastTickTime = Time.time;
    }

    private void Update()
    {
        HandeMoveCurentFallingTtreminoBasedOnInput();

        float curentTime = Time.time;

        if (lastTickTime + timeBetweenTicks > curentTime)
        {
            return;
        }

        OnTick();
    }

    private void OnTick()
    {
        if (!gameBoard.TryMoveDownTetremino(curentFallingTetremino_Id))
        {
            HandelCurentFallingTetreminoStopFall();
        }

        lastTickTime = Time.time;
    }

    private void HandelCurentFallingTetreminoStopFall()
    {
        gameBoard.HandleDestroyLinesAndMoveDowndBlocks();
        CreateRandomTereminoAndSetAsCurrent();
    }

    private void HandelLose()
    {
        loseScreen.SetActive(true);
        Destroy(this);
    }

    private void HandeMoveCurentFallingTtreminoBasedOnInput()
    {
        if (!Input.anyKeyDown)
        {
            return;
        }

        else if (Input.GetKey(KeyCode.DownArrow))
        {
            if (!gameBoard.TryMoveDownTetremino(curentFallingTetremino_Id))
            {
                HandelCurentFallingTetreminoStopFall();
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            gameBoard.TryMoveRightTetremino(curentFallingTetremino_Id);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            gameBoard.TryMoveLeftTetremino(curentFallingTetremino_Id);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            gameBoard.TryRotateTetremino(curentFallingTetremino_Id);
        }
    }

    private GameBoard CrateGameBoard()
    {
        GameObject gameObject = new GameObject("game board");

        GameBoard gameBoard = gameObject.AddComponent<GameBoard>() as GameBoard;
        gameBoard.CreateBoard(gameArea_xMin, gameArea_yMin, numOfColoms + 1, numOfLines + 1, tetreminoBlockScale);

        return gameBoard;
    }

    private void CreateRandomTereminoAndSetAsCurrent()
    {
        TetreminoBlock block_1 = new TetreminoBlock(Color.red, tetreminoBlockScale);
        TetreminoBlock[,] tetremino_1_blocks = {    { null   , null    , null },
                                                    { null   , block_1  , null },
                                                    { block_1 , block_1  ,block_1}};


        TetreminoBlock block_2 = new TetreminoBlock(Color.blue, tetreminoBlockScale);
        TetreminoBlock[,] tetremino_2_blocks = {    {null    ,  null ,     null } ,
                                                    {block_2 ,  null ,     null } ,
                                                    {block_2 , block_2 , block_2 } };

        TetreminoBlock block_3 = new TetreminoBlock(Color.red, tetreminoBlockScale);
        TetreminoBlock[,] tetremino_3_blocks = {    { null , block_3 , null} ,
                                                    { null , block_3 , null } ,
                                                    { null , block_3 , null} };

        TetreminoBlock block_4 = new TetreminoBlock(Color.yellow, tetreminoBlockScale);
        TetreminoBlock[,] tetremino_4_blocks = {    { null , null , null} ,
                                                    { block_4 , block_4 , null } ,
                                                    { null , block_4 , block_4} };

        TetreminoBlock block_5 = new TetreminoBlock(Color.red, tetreminoBlockScale);
        TetreminoBlock[,] tetremino_5_blocks = { { null , null , null } ,
                                                 {null , block_5 , block_5 } ,
                                                 { block_5 , block_5 , null } };

        List<TetreminoBlock[,]> tetreminosBlocks = new List<TetreminoBlock[,]>();

        tetreminosBlocks.Add(tetremino_1_blocks);
        tetreminosBlocks.Add(tetremino_2_blocks);
        tetreminosBlocks.Add(tetremino_3_blocks);
        tetreminosBlocks.Add(tetremino_4_blocks);
        tetreminosBlocks.Add(tetremino_5_blocks);

        System.Random random = new System.Random();
        int randomTetreminoIndex = random.Next(tetreminosBlocks.Count - 1);


        curentFallingTetremino_Id = gameBoard.AddTetreminoToBoard(tetreminosBlocks[randomTetreminoIndex], startColom , startLine, out bool genOnOtherBlock);

        if (genOnOtherBlock)
        {
            HandelLose();
        }
    }

    private int CalculateNumOfColumns()
    {
        int numOfColumns = (int)((Mathf.Abs(gameArea_xMin) + Mathf.Abs(gameArea_xMax) - (tetreminoBlockScale / 2)) / tetreminoBlockScale);
        return numOfColumns;
    }


    private float CalculateTetreminoBlockScale()
    {
        float blocksScaleInHight = (Mathf.Abs(gameArea_yMin) + Mathf.Abs(gameArea_yMax)) / numOfLines;
        return blocksScaleInHight;
    }
}
