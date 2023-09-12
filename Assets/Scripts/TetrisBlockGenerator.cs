using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlockGenerator : MonoBehaviour
{
    public Color[] tetrisBlockColors; 
    public TetrisBlock [] tetrisBlockPrefabs;

    public TetrisBlock GenerateTetrisBlock()
    {
        int colorRandomRange = Random.Range(0 , tetrisBlockColors.Length);
        int blockRandomRange = Random.Range(0 , tetrisBlockPrefabs.Length);

        TetrisBlock _tetrisBlock = Instantiate(tetrisBlockPrefabs[blockRandomRange] , transform);

        _tetrisBlock.SetCubesColor(tetrisBlockColors[colorRandomRange]);

        return _tetrisBlock;
    }
}
