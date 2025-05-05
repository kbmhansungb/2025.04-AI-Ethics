using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using System.Linq;
using TMPro;

public class UserInfoManager : MonoBehaviour
{
    public static UserInfoManager Instance { get; private set; }

    public ToggleGroup genderToggleGroup;
    public ToggleGroup ageToggleGroup;
    public ToggleGroup aiUsageToggleGroup;
    public ToggleGroup consentToggleGroup; 
    public ToggleGroup marketingToggleGroup;
    public TMP_InputField mbtiInput;
    public TMP_InputField phoneInput;
    public Button submitButton;
    public Text warningMessage;

    public string selectedOption;
    private string filePath;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        submitButton.onClick.AddListener(SubmitUserInfo);

        // 전화번호 입력 필드와 MBTI 입력 필드에 이벤트 추가
        phoneInput.onSelect.AddListener(OnPhoneInputSelected);
        mbtiInput.onSelect.AddListener(OnMbtiInputSelected);
    }

    void OnEnable()
    {
        selectedOption = PlayerPrefs.GetString("SelectedOption", "");
        
        filePath = Path.Combine(Application.persistentDataPath, "user_data.csv");

        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "성별,연령,AI 사용,MBTI,전화번호,선택된 옵션,마케팅 동의\n");
        }

        warningMessage.gameObject.SetActive(false);
        
        // 입력 필드 설정
        phoneInput.contentType = TMP_InputField.ContentType.DecimalNumber; // 숫자 입력 전용
    }

    void OnPhoneInputSelected(string text)
    {
        // 전화번호 입력 시 키보드 자동 표시
        phoneInput.ActivateInputField(); // 입력 필드 활성화
    }

    void OnMbtiInputSelected(string text)
    {
        // MBTI 입력 시 키보드 자동 표시
        mbtiInput.ActivateInputField(); // 입력 필드 활성화
    }

    void SubmitUserInfo()
    {
        string gender = GetSelectedToggleText(genderToggleGroup, "성별이 선택되지 않았습니다.");
        if (gender == null) return;

        string age = GetSelectedToggleText(ageToggleGroup, "연령이 선택되지 않았습니다.");
        if (age == null) return;

        string aiUsage = GetSelectedToggleText(aiUsageToggleGroup, "AI 사용 여부가 선택되지 않았습니다.");
        if (aiUsage == null) return;

        string mbti = mbtiInput.text;
        string phone = phoneInput.text;
        if (string.IsNullOrEmpty(phone))
        {
            ShowWarning("휴대폰 번호를 입력해야 합니다.");
            return;
        }

        string consent = GetSelectedToggleText(consentToggleGroup, "개인정보 처리 동의 여부가 선택되지 않았습니다.");
        if (consent == null || consent == "개인정보 처리 동의 안 함")
        {
            ShowWarning("개인정보 처리 동의서에 동의해야 합니다.");
            return;
        }

        string marketingConsent = GetSelectedToggleText(marketingToggleGroup, "마케팅 동의 여부가 선택되지 않았습니다.");
        if (marketingConsent == null || marketingConsent == "마케팅 동의 안 함")
        {
            ShowWarning("마케팅 동의서에 동의해야 합니다.");
            return;
        }

        string csvLine = $"{gender},{age},{aiUsage},{mbti},{phone},{selectedOption},{marketingConsent}\n";
        PlayerPrefs.SetString("SelectedOption", selectedOption);
        File.AppendAllText(filePath, csvLine);
        Debug.Log("데이터가 CSV 파일에 저장되었습니다: " + csvLine);
        
        //SceneManager.LoadScene("AIvideo");
        // 비디오로 전환
        GlobalManager.Instance.SetState(EState.Video);
    }

    string GetSelectedToggleText(ToggleGroup toggleGroup, string warningMessage)
    {
        var activeToggles = toggleGroup.ActiveToggles();
        if (activeToggles.Count() > 0)
        {
            return activeToggles.First().GetComponentInChildren<Text>().text;
        }
        ShowWarning(warningMessage);
        return null;
    }

    void ShowWarning(string message)
    {
        warningMessage.text = message;
        warningMessage.gameObject.SetActive(true);
    }

    public void ResetUserInfo()
    {
        mbtiInput.text = "";
        phoneInput.text = "";
        ResetToggleGroup(genderToggleGroup);
        ResetToggleGroup(ageToggleGroup);
        ResetToggleGroup(aiUsageToggleGroup);
        ResetToggleGroup(consentToggleGroup);
        ResetToggleGroup(marketingToggleGroup);
        warningMessage.gameObject.SetActive(false);
    }

    private void ResetToggleGroup(ToggleGroup toggleGroup)
    {
        foreach (var toggle in toggleGroup.ActiveToggles())
        {
            toggle.isOn = false;
        }
    }
}
