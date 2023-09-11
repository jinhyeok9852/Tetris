using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlockGenerator : MonoBehaviour
{
    public TetrisBlock [] tetrisBlockPrefabs;

    public TetrisBlock GenerateTetrisBlock()
    {
        int _randomRange = Random.Range(0 , tetrisBlockPrefabs.Length);

        TetrisBlock _randomTetrisBlock = Instantiate(tetrisBlockPrefabs[_randomRange] , transform);

        return _randomTetrisBlock;
    }
}
