using UnityEngine;

public class TetrisBlockGenerator : MonoBehaviour
{
    public Color[] tetrisBlockColors; 
    public TetrisBlock [] tetrisBlockPrefabs;

    public Transform generationLocation;

    public Transform[] previewLocations;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        foreach (var location in previewLocations)
        {
            TetrisBlock tetrisBlock = GenerateTetrisBlock();

            tetrisBlock.transform.SetParent(location.transform, false);
        }
    }

    public TetrisBlock GenerateTetrisBlock()
    {
        int colorRandomRange = Random.Range(0 , tetrisBlockColors.Length);
        int blockRandomRange = Random.Range(0 , tetrisBlockPrefabs.Length);

        TetrisBlock tetrisBlock = Instantiate(tetrisBlockPrefabs[blockRandomRange] , transform);

        tetrisBlock.SetRandomAngle();
        tetrisBlock.SetCubesColor(tetrisBlockColors[colorRandomRange]);

        return tetrisBlock;
    }
}
