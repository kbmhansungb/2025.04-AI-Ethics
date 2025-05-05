using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.IO; // 파일 입출력을 위한 네임스페이스 추가

public class EventManager : MonoBehaviour
{
    public Button restartButton;
    public Text resultText1; // 첫 번째 결과 메시지를 표시할 Text
    public Text resultText2; // 두 번째 결과 메시지를 표시할 Text
    public VideoPlayer videoPlayer; // VideoPlayer 컴포넌트 추가

    private int currentMessageIndex; // 현재 메시지 인덱스

    private void Awake()
    {
        restartButton.onClick.AddListener(RestartGame);

        // 비디오가 끝났을 때 호출될 이벤트 등록
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnEnable()
    {
        // PlayerPrefs에서 이전 실행 인덱스를 가져오기
        currentMessageIndex = PlayerPrefs.GetInt("CurrentMessageIndex", 0);

        // 결과 텍스트 설정
        UpdateResultTexts();
        
        // 버튼과 텍스트 숨김
        restartButton.gameObject.SetActive(false);
        resultText1.gameObject.SetActive(false);
        resultText2.gameObject.SetActive(false);
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        // 비디오가 끝났을 때 텍스트와 버튼 활성화
        UpdateResultTexts();
        restartButton.gameObject.SetActive(true); // 버튼 활성화
    }

    void RestartGame()
    {
        UserInfoManager.Instance.ResetUserInfo();
        
        // 메시지 인덱스 변경 (0 또는 1로 토글)
        currentMessageIndex = 1 - currentMessageIndex; // 0이면 1로, 1이면 0으로 변경
        
        // PlayerPrefs에 현재 인덱스 저장
        PlayerPrefs.SetInt("CurrentMessageIndex", currentMessageIndex);
        PlayerPrefs.Save();

        // 결과 텍스트 업데이트
        UpdateResultTexts();

        // CSV 파일로 결과 저장
        SaveResultsToCSV();

        //SceneManager.LoadScene("info"); // "info" 씬으로 전환
        GlobalManager.Instance.SetState(EState.Info); // "info" 씬으로 전환
    }

    void UpdateResultTexts()
    {
        // 결과 텍스트를 업데이트
        resultText1.gameObject.SetActive(currentMessageIndex == 0);
        resultText2.gameObject.SetActive(currentMessageIndex == 1);
        
        // 비디오가 재생 중일 때는 버튼을 숨김
        restartButton.gameObject.SetActive(false);
    }

    void SaveResultsToCSV()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "results.csv");

        // 결과 데이터 생성 (여기서는 메시지 인덱스와 텍스트를 저장합니다)
        string[] lines = new string[2];
        lines[0] = $"Index,Message\n{currentMessageIndex},{(currentMessageIndex == 0 ? resultText1.text : resultText2.text)}";

        // 파일 쓰기
        File.WriteAllLines(filePath, lines);
        Debug.Log($"결과가 {filePath}에 저장되었습니다.");
    }

    void OnDestroy()
    {
        // 이벤트 구독 해제
        videoPlayer.loopPointReached -= OnVideoFinished;
    }
}
