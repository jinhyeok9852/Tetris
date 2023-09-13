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
        if (anglesIndex == 0)
            anglesIndex = angles.Length - 1;
        else
            anglesIndex--;

        Vector3 angle = angles[anglesIndex];

        previewEmptyContent.eulerAngles = angle;

        if(cubeContent.eulerAngles != previewEmptyContent.eulerAngles)
        {
            Debug.LogError("Check Angles , cubeContent and previewEmptyContent's eulerAngles is difference");
        }
    }

    public void SetCubesColor(Color color)
    {
        foreach (var cube in cubes)
        {
            cube.GetComponent<MeshRenderer>().material.color = color;
        }
    }
}
