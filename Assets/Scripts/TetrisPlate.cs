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
    public float lerpSpeed;
    public Material backGroundMaterial;
    public Color[] backGroundColors;

    int colorIndex = 0;
    float colorlerpSpeed = 1;
    private Coroutine changeGradientHeightCoroutine;
    private Dictionary<string, Transform> tetrisPlateCubesTransform = new Dictionary<string, Transform>();

    private void Start()
    {
        SaveTetirsPlateCubesPosition(left);
        SaveTetirsPlateCubesPosition(right);
        SaveTetirsPlateCubesPosition(bottom);

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
        float time = 0.0f;
        int colorIndex = 0;

        while(true)
        {
            time += Time.deltaTime * colorlerpSpeed;
            Color from = backGroundMaterial.GetColor("_GradientBottomColor");
            Color to = backGroundColors[colorIndex];

            Color lerpValue = Color.Lerp(from, to, time);

            backGroundMaterial.SetColor("_GradientBottomColor" , lerpValue);

            if(backGroundMaterial.GetColor("_GradientBottomColor") == backGroundColors[colorIndex])
            {
                colorIndex++;
                colorIndex %= backGroundColors.Length;
            }

            yield return null;
        }
    }

    public void ChangeBackGroundHeight(float height , Action changeMainTetrisBlockAction)
    {
        if(changeGradientHeightCoroutine != null)
        {
            StopCoroutine(changeGradientHeightCoroutine);
        }

        colorlerpSpeed = 0.1f * height;
        changeGradientHeightCoroutine = StartCoroutine(ChangeBackGroundHeightCoroutine(height , changeMainTetrisBlockAction));
    }

    private IEnumerator ChangeBackGroundHeightCoroutine(float to , Action changeMainTetrisBlockAction)
    {
        float time = 0.0f;
        float from = backGroundMaterial.GetFloat("_TopLine"); 

        while (time < 1.0f)
        {
            time += Time.deltaTime * lerpSpeed;
            float lerpValue = Mathf.Lerp(from, to, time);

            backGroundMaterial.SetFloat("_TopLine", lerpValue);

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
