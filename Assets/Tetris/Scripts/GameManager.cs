using System;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Function")]
    public TetrisBlockGenerator generator;
    public TetrisBlockController controller;

    [Header("Sound")]
    public AudioSource bgm;

    [Header("Tetris Plate & BackGround")]
    public TetrisPlate tetrisPlate;
    public Transform spaceMovementTarget;

    [Header("Tetris Menu UI")]
    public TetrisMenuUI menuUI;

    [Header("Setting Data")]
    public bool isPlay;
    public float dropInterval;
    public float dropInterval_MinValue;
    public float dropInterval_AdjustedValue;

    [Header("Runtime Data")]
    public float delayTime;
    public float totalScore;
    public float initDropInterval;
    public float y_Axis_MinValue = float.MaxValue;
    public float y_Axis_MaxValue = float.MinValue;

    private TetrisBlock mainTetrisBlock;
    private Dictionary<string , Transform> tetrisBlockCubesTransform = new Dictionary<string, Transform>();

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (isPlay == true)
        {
            delayTime += Time.deltaTime;

            DropTetrisBlockWithInterval();
            controller.ControlWithInputkey(mainTetrisBlock);
            spaceMovementTarget.position += Vector3.down * Time.deltaTime * totalScore;
        }

        if(Input.GetKeyDown(KeyCode.Escape) && mainTetrisBlock != null)
        {
            menuUI.gameObject.SetActive(!menuUI.gameObject.activeSelf);

            if (menuUI.gameObject.activeSelf == true)
            {
                PauseGame();
            }
            else
            {
                PlayGame();
            }
        }
    }

    public void StartGame()
    {
        initDropInterval = dropInterval;

        generator.GeneratePreviewTetrisBlocks();
        mainTetrisBlock = generator.GetMainTetrisBlock();
    }
    public void PlayGame()
    {
        isPlay = true;
    }

    public void PauseGame()
    {
        isPlay = false;
    }

    public bool IsExistCubeOnPosition(Vector2 position)
    {
        bool isExist = tetrisBlockCubesTransform.ContainsKey(position.ToString()) || tetrisPlate.IsExistPlateCubeOnPosition(position);

        return isExist;
    }
    
    private void SaveTetirsBlockCubesPosition()
    {
        foreach (var cube in mainTetrisBlock.cubes)
        {
            Vector2 cubePosition = cube.position;

            tetrisBlockCubesTransform.TryAdd(cubePosition.ToString(), cube);
        }
    }

    private void DropTetrisBlockWithInterval()
    {
        if(delayTime >= dropInterval)
        {
            bool isDropRange = controller.IsMoveRange(Vector3.down, mainTetrisBlock);

            if (isDropRange == true)
            {
                controller.MoveTetrisBlock(Vector3.down , mainTetrisBlock);
            }
            else
            {
                CheckGameOver();
            }

            delayTime = 0.0f;
        }
    }

    private void DeleteLines()
    {
        y_Axis_MinValue = float.MaxValue;

        foreach (var cube in mainTetrisBlock.cubes)
        {
            float y = cube.position.y;

            if (y >= y_Axis_MaxValue)
                y_Axis_MaxValue = y;

            if (y < y_Axis_MinValue)
                y_Axis_MinValue = y;
        }

        for (float y = y_Axis_MinValue; y <= y_Axis_MaxValue; y++)
        {
            bool isDeleteLine = true;
            Dictionary<string, GameObject> deleteCubes = new Dictionary<string, GameObject>();

            for (float x = tetrisPlate.GetX_Axis_MinValue() + 1; x < tetrisPlate.GetX_Axis_MaxValue(); x++)
            {
                Vector2 cubePosition = new Vector2(x, y);

                if (!tetrisBlockCubesTransform.ContainsKey(cubePosition.ToString()))
                {
                    isDeleteLine = false;

                    break;
                }
                else
                {
                    deleteCubes.TryAdd(cubePosition.ToString(), tetrisBlockCubesTransform[cubePosition.ToString()].gameObject);
                }
            }

            if (isDeleteLine)
            {
                foreach (var cube in deleteCubes)
                {
                    tetrisBlockCubesTransform.Remove(cube.Key);
                    Destroy(cube.Value);
                }

                DropCubes();

                y--;
                y_Axis_MaxValue--;
                totalScore++;
            }
        }
    }
  
    private void DropCubes()
    {
        for (float y = y_Axis_MinValue + 1; y <= y_Axis_MaxValue; y++)
        {
            for (float x = tetrisPlate.GetX_Axis_MinValue() + 1; x < tetrisPlate.GetX_Axis_MaxValue(); x++)
            {
                Vector2 cubePosition = new Vector2(x, y);

                if (tetrisBlockCubesTransform.ContainsKey(cubePosition.ToString()))
                {
                    Transform cube = tetrisBlockCubesTransform[cubePosition.ToString()];

                    cube.position += Vector3.down;

                    tetrisBlockCubesTransform.Remove(cubePosition.ToString());

                    cubePosition = cube.position;
                    tetrisBlockCubesTransform.TryAdd(cubePosition.ToString(), cube);
                }
            }
        }
    }

    private void ChangeGameDifficulty()
    {
        dropInterval = initDropInterval - ( dropInterval_AdjustedValue * totalScore);

        if(dropInterval < dropInterval_MinValue)
        {
            dropInterval = dropInterval_MinValue;
        }
        else
        {
            bgm.pitch = 1 + (1 - dropInterval);
        }
    }

    private void PrepareNextTetrisBlock()
    {
        SaveTetirsBlockCubesPosition();

        DeleteLines();
        
        ChangeGameDifficulty();

        Action changeMainTetrisBlockHandler = () => mainTetrisBlock = generator.GetMainTetrisBlock();
        tetrisPlate.ChangeBackGroundHeight(y_Axis_MaxValue , changeMainTetrisBlockHandler);
    }

    private void CheckGameOver()
    {
        bool isGameOver = y_Axis_MaxValue == generator.transform.position.y;

        if (isGameOver)
        {
            Debug.Log("GameOver");
        }
        else
        {
            PrepareNextTetrisBlock();
        }
    }
}
