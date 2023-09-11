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

    private bool CheckRotateRange(float _rotateValue , TetrisBlock _tetrisBlock)
    {
        _tetrisBlock.previewEmptyContent.Rotate(Vector3.forward * _rotateValue);

        foreach (var previewEmpty in _tetrisBlock.previewEmpties)
        {
            if (GameManager.instance.tetrisBlockCubes.ContainsKey(previewEmpty.position))
            {
                _tetrisBlock.previewEmptyContent.localRotation = Quaternion.identity;

                return false;
            }
        }

        return true;
    }

    public void RotateTetrisBlock(float _rotateValue, TetrisBlock _tetrisBlock)
    {
        bool isRotate = CheckRotateRange(_rotateValue , _tetrisBlock);

        if(isRotate)
        {
            _tetrisBlock.cubeContent.Rotate(Vector3.forward * _rotateValue);
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
            RotateTetrisBlock(90, _tetrisBlock);
        }

        if(Input.GetKeyDown(KeyCode.X))
        {
            RotateTetrisBlock(-90, _tetrisBlock);
        }
    }
}
