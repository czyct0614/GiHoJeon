using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float hoverScaleFactor = 1.1f; // 마우스를 올려놓았을 때 버튼의 확대 비율

    private Vector3 originalScale; // 버튼의 원래 크기
    private RectTransform buttonRectTransform; // 버튼의 RectTransform 컴포넌트

    void Start()
    {
        // 버튼의 RectTransform 컴포넌트 가져오기
        buttonRectTransform = GetComponent<RectTransform>();
        // 버튼의 원래 크기 저장
        originalScale = buttonRectTransform.localScale;
    }

    // 마우스를 올려놓았을 때 호출되는 콜백 함수
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 버튼을 확대
        buttonRectTransform.localScale = originalScale * hoverScaleFactor;
    }

    // 마우스를 빠져나갔을 때 호출되는 콜백 함수
    public void OnPointerExit(PointerEventData eventData)
    {
        // 버튼을 원래 크기로 되돌림
        buttonRectTransform.localScale = originalScale;
    }
}
