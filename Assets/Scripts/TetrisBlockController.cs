using UnityEngine;

public class TetrisBlockController : MonoBehaviour
{
    public float getKeyTime;
    public float getKeyDelay;

    public bool IsMoveRange(Vector3 direction , TetrisBlock tetrisBlock)
    {
        foreach (var previewEmpty in  tetrisBlock.previewEmpties)
        {
            Vector2 previewPosition = previewEmpty.position + direction;

            if (GameManager.instance.IsExistCubeOnPosition(previewPosition) == true)
            {
                return false;
            }
        }

        return true;
    }

    public void MoveTetrisBlock(Vector3 direction, TetrisBlock tetrisBlock)
    {
        if (IsMoveRange(direction, tetrisBlock) == true)
        {
            tetrisBlock.transform.position += direction;
        }
    }

    public bool IsRotateRange(TetrisBlock tetrisBlock)
    {
        tetrisBlock.RotatePreviewEmptyContentAngle();

        foreach (var previewEmpty in tetrisBlock.previewEmpties)
        {
            Vector2 previewPosition = previewEmpty.position;

            if (GameManager.instance.IsExistCubeOnPosition(previewPosition) == true)
            {
                tetrisBlock.RevertPreviewEmptyContentAngle();

                return false;
            }
        }

        return true;
    }

    public void RotateTetrisBlock(TetrisBlock tetrisBlock)
    {
        if (IsRotateRange(tetrisBlock))
        {
            tetrisBlock.RotateCubeContentAngle();
        }
    }

    public void ControlWithInputkey(TetrisBlock tetrisBlock)
    {
        // DownArrow
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveTetrisBlock(Vector3.down , tetrisBlock);
        }
        else if(Input.GetKey(KeyCode.DownArrow))
        {
            getKeyTime += Time.deltaTime;

            if (getKeyTime > getKeyDelay)
                MoveTetrisBlock(Vector3.down , tetrisBlock);
        }
        else if(Input.GetKeyUp(KeyCode.DownArrow))
        {
            getKeyTime = 0.0f;
        }

        // RightArrow
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveTetrisBlock(Vector3.right , tetrisBlock);
        }
        else if(Input.GetKey(KeyCode.RightArrow))
        {
            getKeyTime += Time.deltaTime;

            if (getKeyTime > getKeyDelay)
                MoveTetrisBlock(Vector3.right, tetrisBlock);
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            getKeyTime = 0.0f;
        }

        // LeftArrow
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveTetrisBlock(Vector3.left, tetrisBlock);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            getKeyTime += Time.deltaTime;

            if (getKeyTime > getKeyDelay)
                MoveTetrisBlock(Vector3.left, tetrisBlock);
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            getKeyTime = 0.0f;
        }

        // Z
        if (Input.GetKeyDown(KeyCode.Z))
        {
            RotateTetrisBlock(tetrisBlock);
        }
        else if(Input.GetKey(KeyCode.Z))
        {
            getKeyTime += Time.deltaTime;

            if (getKeyTime > getKeyDelay)
                RotateTetrisBlock(tetrisBlock);
        }
        else if (Input.GetKeyUp(KeyCode.Z))
        {
            getKeyTime = 0.0f;
        }
    }
}
