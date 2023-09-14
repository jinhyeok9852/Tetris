using UnityEngine;

public class TetrisBlockController : MonoBehaviour
{
    public float getKeyTime;
    public float getKeyDelay;
    public void MoveTetrisBlock(Vector3 direction)
    {
        bool isMove = GameManager.instance.IsMoveRange(direction);

        if (isMove)
        {
            GameManager.instance.tetrisBlock.transform.position += direction;
        }
    }

    public void RotateTetrisBlock()
    {
        bool isRotate = GameManager.instance.IsRotateRange();

        if (isRotate)
        {
            GameManager.instance.tetrisBlock.RotateCubes();
        }
    }

    public void ControlWithInputkey()
    {
        // DownArrow
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveTetrisBlock(Vector3.down);
        }
        else if(Input.GetKey(KeyCode.DownArrow))
        {
            getKeyTime += Time.deltaTime;

            if (getKeyTime > getKeyDelay)
                MoveTetrisBlock(Vector3.down);
        }
        else if(Input.GetKeyUp(KeyCode.DownArrow))
        {
            getKeyTime = 0.0f;
        }

        // RightArrow
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveTetrisBlock(Vector3.right);
        }
        else if(Input.GetKey(KeyCode.RightArrow))
        {
            getKeyTime += Time.deltaTime;

            if (getKeyTime > getKeyDelay)
                MoveTetrisBlock(Vector3.right);
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            getKeyTime = 0.0f;
        }

        // LeftArrow
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveTetrisBlock(Vector3.left);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            getKeyTime += Time.deltaTime;

            if (getKeyTime > getKeyDelay)
                MoveTetrisBlock(Vector3.left);
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            getKeyTime = 0.0f;
        }

        // Z
        if (Input.GetKeyDown(KeyCode.Z))
        {
            RotateTetrisBlock();
        }
        else if(Input.GetKey(KeyCode.Z))
        {
            getKeyTime += Time.deltaTime;

            if (getKeyTime > getKeyDelay)
                RotateTetrisBlock();
        }
        else if (Input.GetKeyUp(KeyCode.Z))
        {
            getKeyTime = 0.0f;
        }
    }
}
