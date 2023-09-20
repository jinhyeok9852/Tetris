using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Tetris Plate")]
    public Transform left;
    public Transform right;
    public Transform bottom;

    [Header("Function")]
    public TetrisBlockGenerator generator;
    public TetrisBlockController controller;

    [Header("Setting Data")]
    public float dropInterval;
    public float dropInterval_MinValue;
    public float dropInterval_AdjustedValue;

    [Header("Runtime Data")]
    public float gameScore;
    public float delayTime;
    public float initDropInterval;
    public float y_Axis_MinValue = float.MaxValue;
    public float y_Axis_MaxValue = float.MinValue;
    public TetrisBlock tetrisBlock;

    private Dictionary<string , Transform> tetrisBlockCubesTransform = new Dictionary<string, Transform>();
    private Dictionary<string , Transform> tetrisPlateCubesTransform = new Dictionary<string , Transform>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        initDropInterval = dropInterval;

        SaveTetirsPlateCubesPosition(left);
        SaveTetirsPlateCubesPosition(right);
        SaveTetirsPlateCubesPosition(bottom);

        tetrisBlock = generator.GenerateTetrisBlock();
    }

    private void Update()
    {
        delayTime += Time.deltaTime;

        DropTetrisBlockWithInterval();
        controller.ControlWithInputkey();
    }

    public bool IsMoveRange(Vector3 direction)
    {
        foreach (var previewEmpty in tetrisBlock.previewEmpties)
        {
            Vector2 previewPosition = previewEmpty.position + direction;

            if (tetrisBlockCubesTransform.ContainsKey(previewPosition.ToString()) || tetrisPlateCubesTransform.ContainsKey(previewPosition.ToString()))
            {
                return false;
            }
        }

        return true;
    }
    public bool IsRotateRange()
    {
        tetrisBlock.RotatePreviewEmptyContentAngle();

        foreach (var previewEmpty in tetrisBlock.previewEmpties)
        {
            Vector2 previewPosition = previewEmpty.position;

            if (tetrisBlockCubesTransform.ContainsKey(previewPosition.ToString()) || tetrisPlateCubesTransform.ContainsKey(previewPosition.ToString()))
            {
                tetrisBlock.RevertPreviewEmptyContentAngle();

                return false;
            }
        }

        return true;
    }

    private void SaveTetirsBlockCubesPosition()
    {
        foreach (var cube in tetrisBlock.cubes)
        {
            Vector2 cubePosition = cube.position;

            tetrisBlockCubesTransform.TryAdd(cubePosition.ToString(), cube);
        }
    }
    private void SaveTetirsPlateCubesPosition(Transform tetrisPlate)
    {
        for (int i = 0; i < tetrisPlate.childCount; i++)
        {
            Vector2 cubePosition = tetrisPlate.GetChild(i).position;

            tetrisPlateCubesTransform.TryAdd(cubePosition.ToString(), tetrisPlate.GetChild(i));
        }
    }

    private void DropTetrisBlockWithInterval()
    {
        if(delayTime >= dropInterval)
        {
            if(IsMoveRange(Vector3.down))
            {
                controller.MoveTetrisBlock(Vector3.down);
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

        foreach (var cube in tetrisBlock.cubes)
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

            for (float x = left.position.x + 1; x < right.position.x; x++)
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
                gameScore++;
            }
        }
    }

    private void DropCubes()
    {
        for (float y = y_Axis_MinValue + 1; y <= y_Axis_MaxValue; y++)
        {
            for (float x = left.position.x + 1; x < right.position.x; x++)
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
        dropInterval = initDropInterval - ( dropInterval_AdjustedValue * gameScore );

        if(dropInterval < dropInterval_MinValue)
        {
            dropInterval = dropInterval_MinValue;
        }
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
            SaveTetirsBlockCubesPosition();
            DeleteLines();
            ChangeGameDifficulty();

            tetrisBlock = generator.GenerateTetrisBlock();
        }
    }
}
