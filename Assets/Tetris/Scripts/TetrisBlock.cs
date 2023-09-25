using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    public Transform cubeContent;
    public Transform [] cubes;

    public Transform previewEmptyContent;
    public Transform [] previewEmpties;

    public int angleIndex;
    public Vector3 [] angles;

    public void SetCubesColor(Color color)
    {
        foreach (var cube in cubes)
        {
            cube.GetComponent<MeshRenderer>().material.color = color;
        }
    }

    public void SetRandomAngle()
    {
        angleIndex = Random.Range(0, angles.Length);

        Vector3 angle = angles[angleIndex];

        cubeContent.eulerAngles = angle;
        previewEmptyContent.eulerAngles = angle;
    }

    public void RotateCubeContentAngle()
    {
        Vector3 angle = angles[angleIndex];

        cubeContent.eulerAngles = angle;
    }

    public void RotatePreviewEmptyContentAngle()
    {
        angleIndex++;
        angleIndex %= angles.Length;

        Vector3 angle = angles[angleIndex];

        previewEmptyContent.eulerAngles = angle;
    }

    public void RevertPreviewEmptyContentAngle()
    {
        if (angleIndex == 0)
            angleIndex = angles.Length - 1;
        else
            angleIndex--;

        Vector3 angle = angles[angleIndex];

        previewEmptyContent.eulerAngles = angle;

        if(cubeContent.eulerAngles != previewEmptyContent.eulerAngles)
        {
            Debug.LogError("Check Angles , cubeContent and previewEmptyContent's eulerAngles is difference");
        }
    }
}
