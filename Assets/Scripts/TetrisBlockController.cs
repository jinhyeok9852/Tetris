using UnityEngine;

public class TetrisBlockController : MonoBehaviour
{
    private bool CheckMoveRange(Vector3 _direction , TetrisBlock _tetrisBlock)
    {
        foreach (var cube in _tetrisBlock.cubes)
        {
            if(GameManager.instance.tetrisBlockCubes.ContainsKey(cube.position + _direction))
            {
                return false;
            }
        }

        return true;
    }

    public void MoveTetrisBlock(Vector3 _direction , TetrisBlock _tetrisBlock)
    {
        bool isMove = CheckMoveRange(_direction , _tetrisBlock);

        if (isMove)
        {
            _tetrisBlock.transform.position += _direction;
        }
    }

    private bool CheckRotateRange(TetrisBlock _tetrisBlock)
    {
        _tetrisBlock.RotatePreviewEmpties();

        foreach (var previewEmpty in _tetrisBlock.previewEmpties)
        {
            Vector3 previewPosition = previewEmpty.position;

            if (GameManager.instance.tetrisBlockCubes.ContainsKey(previewPosition))
            {
                _tetrisBlock.RevertPreviewEmpties();

                return false;
            }
        }

        return true;
    }

    public void RotateTetrisBlock(TetrisBlock _tetrisBlock)
    {
        bool isRotate = CheckRotateRange(_tetrisBlock);

        if(isRotate)
        {
            _tetrisBlock.RotateCubes();
        }
    }

    public void ControlWithInputkey(TetrisBlock _tetrisBlock)
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveTetrisBlock(Vector3.down, _tetrisBlock);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveTetrisBlock(Vector3.right, _tetrisBlock);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveTetrisBlock(Vector3.left, _tetrisBlock);
        }

        if(Input.GetKeyDown(KeyCode.Z))
        {
            RotateTetrisBlock(_tetrisBlock);
        }

        if(Input.GetKeyDown(KeyCode.X))
        {
            RotateTetrisBlock(_tetrisBlock);
        }
    }
}
