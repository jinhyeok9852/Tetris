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
    public float generationTime;

    public float delayTime;
    public TetrisBlock tetrisBlock;

    [SerializeField] private float y_axis_min = float.MaxValue;
    [SerializeField] private float y_axis_max = float.MinValue;
    private Dictionary<string , Transform> tetrisBlockCubesTransform = new Dictionary<string, Transform>();
    private Dictionary<string , Transform> tetrisPlateCubesTransform = new Dictionary<string , Transform>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
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
        tetrisBlock.RotatePreviewEmpties();

        foreach (var previewEmpty in tetrisBlock.previewEmpties)
        {
            Vector2 previewPosition = previewEmpty.position;

            if (tetrisBlockCubesTransform.ContainsKey(previewPosition.ToString()) || tetrisPlateCubesTransform.ContainsKey(previewPosition.ToString()))
            {
                tetrisBlock.RevertPreviewEmpties();

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
                SaveTetirsBlockCubesPosition();
                DeleteLines();

                tetrisBlock = generator.GenerateTetrisBlock();
            }

            delayTime = 0.0f;
        }
    }

    private void DeleteLines()
    {
        y_axis_min = float.MaxValue;

        foreach (var cube in tetrisBlock.cubes)
        {
            float y = cube.position.y;

            if (y >= y_axis_max)
                y_axis_max = y;

            if (y < y_axis_min)
                y_axis_min = y;
        }

        bool isDeleted = false;

        for (float y = y_axis_min; y <= y_axis_max; y++)
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
            }

            isDeleted |= isDeleteLine; 
        }

        //if (isDeleted)
        //    DropCubes();
    }

    private void DropCubes()
    {
        for (float y = y_axis_min + 1; y <= y_axis_max; y++)
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
}
