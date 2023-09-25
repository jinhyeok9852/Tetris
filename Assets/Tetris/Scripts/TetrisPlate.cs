using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TetrisPlate : MonoBehaviour
{
    public Transform left;
    public Transform right;
    public Transform bottom;

    [Header("Gradient Value")]
    public float heightChangeSpeed;
    public Material backGroundMaterial;
    public Color[] backGroundColors;

    float colorChangeSpeed;
    private Coroutine changeGradientHeightCoroutine;
    private Dictionary<string, Transform> tetrisPlateCubesTransform = new Dictionary<string, Transform>();

    private void Start()
    {
        SaveTetirsPlateCubesPosition(left);
        SaveTetirsPlateCubesPosition(right);
        SaveTetirsPlateCubesPosition(bottom);

        backGroundMaterial.SetFloat("_TopLine", 0);

        StartCoroutine(ChangeBackGroundColorCoroutine());
    }

    public float GetX_Axis_MinValue()
    {
        return left.position.x;
    }

    public float GetX_Axis_MaxValue()
    {
        return right.position.x;
    }

    public bool IsExistPlateCubeOnPosition(Vector2 position)
    {
        bool isExist = tetrisPlateCubesTransform.ContainsKey(position.ToString()) == true;

        return isExist;
    }

    private IEnumerator ChangeBackGroundColorCoroutine()
    {
        int colorIndex = 0;

        while(true)
        {
            float time = 0.0f;

            while (time < 1.0f)
            {
                time += Time.deltaTime * colorChangeSpeed;
                Color from = backGroundMaterial.GetColor("_GradientBottomColor");
                Color to = backGroundColors[colorIndex];

                Color lerpValue = Color.Lerp(from, to, time);

                backGroundMaterial.SetColor("_GradientBottomColor", lerpValue);

                yield return null;
            }

            colorIndex++;
            colorIndex %= backGroundColors.Length;
        }
    }

    public void ChangeBackGroundHeight(float targetHeight , Action changeMainTetrisBlockAction)
    {
        if(changeGradientHeightCoroutine != null)
        {
            StopCoroutine(changeGradientHeightCoroutine);
        }

        colorChangeSpeed = 1 + targetHeight;
        changeGradientHeightCoroutine = StartCoroutine(ChangeBackGroundHeightCoroutine(targetHeight, changeMainTetrisBlockAction));
    }

    private IEnumerator ChangeBackGroundHeightCoroutine(float targetHeight, Action changeMainTetrisBlockAction)
    {
        float time = 0.0f;
        float currentHeight = backGroundMaterial.GetFloat("_TopLine"); 

        while (time < 1.0f)
        {
            time += Time.deltaTime * heightChangeSpeed;
            float lerpValue = Mathf.Lerp(currentHeight, targetHeight, time);

            backGroundMaterial.SetFloat("_TopLine", lerpValue + 2);

            if(time > 0.9f && changeMainTetrisBlockAction != null)
            {
                changeMainTetrisBlockAction.Invoke();
                changeMainTetrisBlockAction = null;
            }

            yield return null;
        }
    }

    private void SaveTetirsPlateCubesPosition(Transform tetrisPlate)
    {
        for (int i = 0; i < tetrisPlate.childCount; i++)
        {
            Vector2 cubePosition = tetrisPlate.GetChild(i).position;

            tetrisPlateCubesTransform.TryAdd(cubePosition.ToString(), tetrisPlate.GetChild(i));
        }
    }
}
