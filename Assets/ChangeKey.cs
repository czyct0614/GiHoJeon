using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class ChangeKey : MonoBehaviour
{
    public InputActionAsset inputActions;    // Input Action Asset
    public InputActionReference actionToRebind; // 바꾸고 싶은 액션을 public으로 설정
    public Button rebindButton;             // 키 재설정 버튼
    public TextMeshProUGUI bindingDisplayText;         // 현재 바인딩된 키를 표시할 텍스트
    public int bindingIndex = 0;            // 바꿀 바인딩 인덱스 (예: Move의 0번째 바인딩)

    private void Start()
    {
        // 현재 바인딩된 키를 UI에 표시
        UpdateBindingDisplay();
        
        // 버튼 클릭 시 Rebind 시작
        rebindButton.onClick.AddListener(() => StartRebinding());
    }

    // 현재 바인딩된 키를 텍스트로 업데이트
    private void UpdateBindingDisplay()
    {
        if (actionToRebind != null)
        {
            bindingDisplayText.text = InputControlPath.ToHumanReadableString(
                actionToRebind.action.bindings[bindingIndex].effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice);
        }
    }

    public void StartRebinding()
    {
        if (actionToRebind == null) return; // 바꿀 액션이 없으면 아무 작업도 안함

        rebindButton.interactable = false;

        // Rebinding 시작
        actionToRebind.action.PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding("<Mouse>")  // 마우스를 제외
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation =>
            {
                // 새로운 바인딩을 확인하고 UI에 반영
                UpdateBindingDisplay();

                // Rebinding 완료 후 PlayerInput의 Action을 리로드
                inputActions.Disable(); // 액션 비활성화
                inputActions.Enable();  // 액션 다시 활성화

                rebindButton.interactable = true;

                // Rebinding 프로세스 정리
                operation.Dispose();
            })
            .Start();
    }

    // 바인딩을 저장
    public void SaveRebinds()
    {
        string rebinds = inputActions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
        PlayerPrefs.Save();
        Debug.Log("키 설정이 저장되었습니다.");
    }

    // 바인딩을 로드
    public void LoadRebinds()
    {
        if (PlayerPrefs.HasKey("rebinds"))
        {
            string rebinds = PlayerPrefs.GetString("rebinds");
            inputActions.LoadBindingOverridesFromJson(rebinds);
            Debug.Log("키 설정이 불러와졌습니다.");
        }
        else
        {
            Debug.Log("저장된 키 설정이 없습니다.");
        }
    }

    // 바인딩을 리셋
    public void ResetRebinds()
    {
        inputActions.RemoveAllBindingOverrides();
        PlayerPrefs.DeleteKey("rebinds");
        UpdateBindingDisplay();  // 기본값으로 리셋된 키 업데이트
        Debug.Log("키 설정이 기본값으로 리셋되었습니다.");
    }
}
