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
                DeleteTetrisBlockCubesWithHorizontalLine();

                tetrisBlock = generator.GenerateTetrisBlock();
            }

            delayTime = 0.0f;
        }
    }

    private void DeleteTetrisBlockCubesWithHorizontalLine()
    {
        int deleteLineCount = 0;
        Dictionary<string, GameObject> checkedTetrisBlockCubes = new Dictionary<string, GameObject>();

        foreach (var cube in tetrisBlock.cubes)
        {
            Vector2 cubePosition = cube.position;
            Dictionary<string, GameObject> tetrisBlockCubes = new Dictionary<string, GameObject>();

            // 같은 세로축일 때 왼쪽 블록이 먼저 체크된 경우 해당 가로줄은 체크할 필요가 없음.
            if (checkedTetrisBlockCubes.ContainsKey(cubePosition.ToString()))
                continue;

            bool isHorizontalLine = true;
            float x = cubePosition.x;

            // x 축 범위 0.5 ~ 9.5
            for (float i = left.position.x + 1; i < x; i++)
            {
                cubePosition.x = i;

                if (!tetrisBlockCubesTransform.ContainsKey(cubePosition.ToString()))
                {
                    isHorizontalLine = false;
                    tetrisBlockCubes.Clear();

                    break;
                }
                else
                {
                    tetrisBlockCubes.TryAdd(cubePosition.ToString() , tetrisBlockCubesTransform[cubePosition.ToString()].gameObject);
                }
            }

            for (float i = x; i < right.position.x; i++)
            {
                cubePosition.x = i;

                if (!tetrisBlockCubesTransform.ContainsKey(cubePosition.ToString()))
                {
                    isHorizontalLine = false;

                    break;
                }
                else
                {
                    tetrisBlockCubes.TryAdd(cubePosition.ToString(), tetrisBlockCubesTransform[cubePosition.ToString()].gameObject);
                }
            }

            if(isHorizontalLine)
            {
                foreach (var tetrisBlockCube in tetrisBlockCubes)
                {
                    checkedTetrisBlockCubes.TryAdd(tetrisBlockCube.Key , tetrisBlockCube.Value);
                }

                deleteLineCount++;
            }
        }

        foreach (var checkedTetrisBlockCube in checkedTetrisBlockCubes)
        {
            Destroy(checkedTetrisBlockCube.Value);
            tetrisBlockCubesTransform.Remove(checkedTetrisBlockCube.Key);
        }
    }
}
