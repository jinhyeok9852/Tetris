using UnityEngine;

public class TetrisBlockGenerator : MonoBehaviour
{
    public Color[] tetrisBlockColors; 
    public TetrisBlock [] tetrisBlockPrefabs;

    public Transform generationPosition;
    public Transform [] previewPositions;
    public TetrisBlock[] previewTetrisBlocks;

    private TetrisBlock mainTetirsBlock;

    private void Start()
    {
        GeneratePreviewTetrisBlocks();
    }

    private void GeneratePreviewTetrisBlocks()
    {
        previewTetrisBlocks = new TetrisBlock[previewPositions.Length];

        for (int i = 0; i < previewPositions.Length; i++)
        {
            TetrisBlock previewTetrisBlock = GenerateRandomTetrisBlock();

            previewTetrisBlocks[i] = previewTetrisBlock;
            previewTetrisBlock.transform.SetParent(previewPositions[i] , false);
        }
    }

    private TetrisBlock GenerateRandomTetrisBlock()
    {
        int colorRandomRange = Random.Range(0, tetrisBlockColors.Length);
        int blockRandomRange = Random.Range(0, tetrisBlockPrefabs.Length);

        TetrisBlock tetrisBlock = Instantiate(tetrisBlockPrefabs[blockRandomRange]);

        tetrisBlock.SetRandomAngle();
        tetrisBlock.SetCubesColor(tetrisBlockColors[colorRandomRange]);

        return tetrisBlock;
    }

    private void RenewalPreviewTetrisBlocks()
    {
        mainTetirsBlock = previewTetrisBlocks[0];
        mainTetirsBlock.transform.SetParent(generationPosition, false);

        for (int i = 0; i < previewTetrisBlocks.Length - 1; i++)
        {
            previewTetrisBlocks[i] = previewTetrisBlocks[i + 1];
            previewTetrisBlocks[i].transform.SetParent(previewPositions[i], false);
        }

        int lastPreviewIndex = previewTetrisBlocks.Length - 1;

        previewTetrisBlocks[lastPreviewIndex] = GenerateRandomTetrisBlock();
        previewTetrisBlocks[lastPreviewIndex].transform.SetParent(previewPositions[lastPreviewIndex] , false);
    }

    public TetrisBlock GetMainTetrisBlock()
    {
        RenewalPreviewTetrisBlocks();

        return mainTetirsBlock;
    }
}
