using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[ExecuteInEditMode] // 에디터 모드에서도 작동하도록 설정
public class ToggleFontSizeAdjuster : MonoBehaviour
{
    public List<Toggle> toggles; // 조정할 여러 토글
    public int fontSize = 14; // 인스펙터에서 조정할 폰트 크기
    public float sizeMultiplier = 1.0f; // 체크마크와 배경 크기 비율
    public Color fontColor = Color.white; // 인스펙터에서 조정할 폰트 색상
    public float labelOffset = 10f; // 라벨과 배경 사이의 여백

    void Start()
    {
        foreach (var toggle in toggles)
        {
            if (toggle != null)
            {
                // 토글에서 텍스트 컴포넌트를 자동으로 찾기
                Text textComponent = toggle.GetComponentInChildren<Text>();
                
                // 기본 크기 설정
                SetInitialToggleSize(toggle);

                // 폰트 크기 및 색상 조정
                AdjustFontSize(toggle, textComponent);
            }
        }
    }

    void OnValidate()
    {
        foreach (var toggle in toggles)
        {
            if (toggle != null)
            {
                Text textComponent = toggle.GetComponentInChildren<Text>();
                AdjustFontSize(toggle, textComponent);
            }
        }
    }

    void SetInitialToggleSize(Toggle toggle)
    {
        if (toggle != null)
        {
            RectTransform toggleRect = toggle.GetComponent<RectTransform>();
            float height = 40; // 기본 높이 설정
            float width = height * 4; // 기본 너비는 높이의 4배
            toggleRect.sizeDelta = new Vector2(width, height); // 기본 크기 설정
        }
    }

    void AdjustFontSize(Toggle toggle, Text textComponent)
    {
        if (toggle != null && textComponent != null)
        {
            // 텍스트 색상 설정
            textComponent.color = fontColor; // 폰트 색상을 인스펙터에서 설정한 값으로 변경

            // 폰트 크기 조정
            textComponent.fontSize = Mathf.Max(1, fontSize); // 폰트 크기를 최소 1로 설정

            // 체크마크와 배경 크기 조정
            RectTransform checkmarkRect = toggle.graphic.GetComponent<RectTransform>();
            RectTransform backgroundRect = toggle.targetGraphic.GetComponent<RectTransform>();

            if (checkmarkRect != null)
            {
                checkmarkRect.sizeDelta = new Vector2(fontSize * sizeMultiplier, fontSize * sizeMultiplier);
            }

            if (backgroundRect != null)
            {
                backgroundRect.sizeDelta = new Vector2(fontSize * sizeMultiplier + 20, fontSize * sizeMultiplier + 10); // 배경 크기 조정
            }

            // 라벨 위치 조정
            if (textComponent != null)
            {
                RectTransform textRect = textComponent.GetComponent<RectTransform>();
                textRect.anchoredPosition = new Vector2(backgroundRect.sizeDelta.x + labelOffset, 0); // 배경 오른쪽으로 이동
            }

            // 토글 크기 조정
            RectTransform toggleRect = toggle.GetComponent<RectTransform>();
            toggleRect.sizeDelta = new Vector2(backgroundRect.sizeDelta.x + 300, backgroundRect.sizeDelta.y); // 토글 크기 조정
        }
        else
        {
            Debug.LogWarning("Toggle or Text Component is not assigned.");
        }
    }
}
