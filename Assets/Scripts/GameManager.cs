using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;

public class GameManager : MonoBehaviour
{
    private Tetremino curentFallingTetremino;

    private float lastTickTime;

    [SerializeField] private float timeBetweenTicks;

    [SerializeField] private Collider2D gameArea; // this canvar represent the area 

    private float tetreminoBlockScale;

    private float gameArea_yMin;
    private float gameArea_yMax;

    private float gameArea_xMin;
    private float gameArea_xMax;

    private Vector2 tetreminoStartPosition; // the position were all the tetreminos are crated

    private int curenFalingTetreminoColom;
    private int curentFallingTetreminoLine;

    private int tetreminoStartLine;
    private int tetreminoStartColom;

    private const int numOfLines = 25;
    private int numOfColoms;

    private const int aditionalStartLines = 5;

    private float tetreminoMoveSideUnit;
    private float tetreminoMoveDownUnit;

    private GameBord gameBord;

    private void Start()
    {
        gameArea_yMin = gameArea.bounds.min.y;
        gameArea_yMax = gameArea.bounds.max.y;

        gameArea_xMin = gameArea.bounds.min.x;
        gameArea_xMax = gameArea.bounds.max.x;

        tetreminoBlockScale = CalculteTetreminoBlockScale();

        numOfColoms = CalculateNumOfColoms(out tetreminoMoveSideUnit);

        tetreminoMoveDownUnit = tetreminoBlockScale;

        tetreminoStartPosition = CalculateTetreminoStartPosition(out tetreminoStartLine , out tetreminoStartColom);

        Tetremino.SetTetreminosMoveToSideUnit(tetreminoMoveSideUnit);
        Tetremino.SetTetremionoMoveDownUnit(tetreminoMoveDownUnit);

        Tetremino.SetTetreminoBlockScale(tetreminoBlockScale);

        gameBord = CrateGameBord();

        CrateRandomTereminoAndSetAsCurent(); // crate the first falling tetremino

        lastTickTime = Time.time;
    }


    private void Update()
    {
        HandeMoveCurentFallingTtreminoBasedOnIput();

        float curentTime = Time.time;

        if (lastTickTime + timeBetweenTicks > curentTime)
        {
            return;
        }

        OnTick();
    }

    private void OnTick()
    {
        MoveDownCurentTetremino();
        curentFallingTetreminoLine--;

        if (TetremiinoShouldStopFalling())
        {
            
            AddCurentFallngTetreminoToBord();

            Destroy(curentFallingTetremino.gameObject);

            gameBord.HandelDestroyLines();

            CrateRandomTereminoAndSetAsCurent();
        }

        lastTickTime = Time.time;
    }

    private void HandeMoveCurentFallingTtreminoBasedOnIput()
    {
        if (!Input.anyKeyDown)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (curenFalingTetreminoColom == 0)
            {
                return;
            }

            curentFallingTetremino.MoveLeft();
            curenFalingTetreminoColom--;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (curenFalingTetreminoColom == numOfColoms - Tetremino.TetreminoVerticesSize - 1)
            {
                return;
            }

            curentFallingTetremino.MoveRight();
            curenFalingTetreminoColom++;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            curentFallingTetremino.RotateRight();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            curentFallingTetremino.RotatateLeft();
        }
    }
    private void MoveDownCurentTetremino()
    {
        curentFallingTetremino.MoveDown();
    }
    private bool TetremiinoShouldStopFalling()
    {
        int[] tetreminoBlocksHights = new int[Tetremino.TetreminoVerticesSize];

        for(int x = 0; x < Tetremino.TetreminoVerticesSize; x++)
        {
            for(int y = 0; y < Tetremino.TetreminoVerticesSize; y++)
            {
                if(curentFallingTetremino.TetreminoBlocks[(Tetremino.TetreminoVerticesSize - 1) - y,x] != null)
                {
                    tetreminoBlocksHights[x] = y;
                    break;
                }
                else if(y == Tetremino.TetreminoVerticesSize - 1)
                {
                    tetreminoBlocksHights[x] = -1000;
                }
            }
        }

        for(int i = 0; i < Tetremino.TetreminoVerticesSize; i++)
        {
            if(tetreminoBlocksHights[i] != -1000)
            {
                if (gameBord.IsContainBlock(curenFalingTetreminoColom + i, (curentFallingTetreminoLine + tetreminoBlocksHights[i])))
                {
                    return true;
                }
            }
        }

        if(curentFallingTetreminoLine == 0)
        {
            return true;
        }
        return false;
    }



    private GameBord CrateGameBord()
    {
        GameObject gameObject = new GameObject("game bord");

        GameBord gameBord = gameObject.AddComponent<GameBord>() as GameBord;
        gameBord.CrateBord(gameArea_xMin , gameArea_yMin , numOfColoms, numOfLines + aditionalStartLines, tetreminoBlockScale);

        return gameBord;

    }


    private void AddCurentFallngTetreminoToBord()
    {
        gameBord.AddBlocksToBord(curentFallingTetremino.TetreminoBlocks, curenFalingTetreminoColom + 1, curentFallingTetreminoLine);
    }

    private void CrateRandomTereminoAndSetAsCurent()
    {
        Type[] tetreminosTypes = GetTetreminos();

        System.Random random = new System.Random();

        int randomTetreminoIndex = random.Next(0 , tetreminosTypes.Length);

        curentFallingTetremino = CrateTetremino(tetreminosTypes[randomTetreminoIndex]);
    }

    Type[] GetTetreminos()
    {
        IEnumerable tetremonosTypes = Assembly.GetAssembly(typeof(Tetremino)).GetTypes().Where(type => type.IsSubclassOf(typeof(Tetremino)));

        Type[] types = tetremonosTypes.Cast<Type>().ToArray();

        return types;

    }


    private Tetremino CrateTetremino(Type t)
    {
        GameObject gameObject = new GameObject("tetremino");
        gameObject.transform.position = tetreminoStartPosition;

        Tetremino tetremino = gameObject.AddComponent(t) as Tetremino;

        tetremino.CrateTetremino();

        curentFallingTetreminoLine = tetreminoStartLine;
        curenFalingTetreminoColom = tetreminoStartColom;

        return tetremino;
    }

    private Vector2 CalculateTetreminoStartPosition(out int tetreminoStartLine , out int tetreminoStartColom)
    {
        tetreminoStartLine = ((numOfLines + aditionalStartLines) - 1) - (Tetremino.TetreminoVerticesSize);
        tetreminoStartColom = 0;

        float y = gameArea_yMin + ((tetreminoStartLine) * tetreminoBlockScale) + ((Tetremino.TetreminoVerticesSize * tetreminoBlockScale) / 2);

        float x = gameArea_xMin + ((Tetremino.TetreminoVerticesSize * tetreminoBlockScale) / 2);

        return new Vector2(x, y);
    }
    
    private int CalculateNumOfColoms(out float tetereminoSideMoveUnit)
    {
        tetereminoSideMoveUnit = tetreminoBlockScale;

        int numOfColoms = (int)((Mathf.Abs(gameArea_xMin) + Mathf.Abs(gameArea_xMax)) / (tetreminoMoveSideUnit));

        return numOfColoms;
    }
 

    private float CalculteTetreminoBlockScale()
    {
        float blocksScaleInHight = (Mathf.Abs(gameArea_yMin) + Mathf.Abs(gameArea_yMax)) / numOfLines;

        return blocksScaleInHight;


    }

}
