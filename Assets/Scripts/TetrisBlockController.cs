using UnityEngine;

public class TetrisBlockController : MonoBehaviour
{
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
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveTetrisBlock(Vector3.down);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveTetrisBlock(Vector3.right);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveTetrisBlock(Vector3.left);
        }
        if(Input.GetKeyDown(KeyCode.Z))
        {
            RotateTetrisBlock();
        }
    }
}
