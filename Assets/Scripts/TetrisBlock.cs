using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    public Transform cubeContent;
    public Transform [] cubes;

    public Transform previewEmptyContent;
    public Transform [] previewEmpties;

    public int anglesIndex;
    public Vector3 [] angles;

    public void RotateCubes()
    {
        Vector3 angle = angles[anglesIndex];

        cubeContent.eulerAngles = angle;
    }

    public void RotatePreviewEmpties()
    {
        anglesIndex++;
        anglesIndex %= angles.Length;

        Vector3 angle = angles[anglesIndex];

        previewEmptyContent.eulerAngles = angle;
    }

    public void RevertPreviewEmpties()
    {
        anglesIndex = anglesIndex > 0 ? anglesIndex-- : angles.Length - 1;

        Vector3 angle = angles[anglesIndex];

        previewEmptyContent.eulerAngles = angle;
    }

    public void SetCubesColor(Color color)
    {
        foreach (var cube in cubes)
        {
            cube.GetComponent<MeshRenderer>().material.color = color;
        }
    }
}
