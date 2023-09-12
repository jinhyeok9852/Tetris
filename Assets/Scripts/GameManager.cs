using UnityEngine;
using System.Collections;
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

    private float _delayTime;
    private TetrisBlock _tetrisBlock;
    public Dictionary<Vector3 , GameObject> tetrisBlockCubes = new Dictionary<Vector3, GameObject>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetTetirsBlockCubes(left);
        SetTetirsBlockCubes(right);
        SetTetirsBlockCubes(bottom);

        _tetrisBlock = generator.GenerateTetrisBlock();
    }

    private void Update()
    {
        _delayTime += Time.deltaTime;

        //DropTetrisBlockWithInterval();
        controller.ControlWithInputkey(_tetrisBlock);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            SetTetirsBlockCubes(_tetrisBlock);

            _tetrisBlock = generator.GenerateTetrisBlock();
        }
    }

    private void SetTetirsBlockCubes(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            tetrisBlockCubes.TryAdd(parent.GetChild(i).position , parent.GetChild(i).gameObject);
        }
    }

    public void SetTetirsBlockCubes(TetrisBlock tetrisBlock)
    {
        foreach (var cube in tetrisBlock.cubes)
        {
            if (!tetrisBlockCubes.ContainsKey(cube.position))
            {
                tetrisBlockCubes.TryAdd(cube.position, cube.gameObject);
            }
        }
    }

    private void DropTetrisBlockWithInterval()
    {
        if(_delayTime >= dropInterval)
        {
            controller.MoveTetrisBlock(Vector3.down, _tetrisBlock);

            _delayTime = 0.0f;
        }
    }

    private void GenerateTetrisBlockWithDelay()
    {
        if(_delayTime >= generationTime)
        {
            generator.GenerateTetrisBlock();

            _delayTime = 0.0f;
        }
    }
}
