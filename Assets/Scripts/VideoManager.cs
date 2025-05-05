using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.IO;

public class VideoManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Button playButton;
    public Button optionAButton;
    public Button optionBButton;
    public RawImage rawImage;
    private UserInfoManager userInfoManager;

    private string filePath; // CSV 파일 경로

    void Start()
    {
        userInfoManager = UserInfoManager.Instance; // Singleton 사용
        if (userInfoManager == null)
        {
            Debug.LogError("UserInfoManager를 찾을 수 없습니다.");
            return; // UserInfoManager가 없으면 이후 코드를 실행하지 않음
        }

        playButton.onClick.AddListener(PlayVideo);
        optionAButton.onClick.AddListener(() => SelectOption("A"));
        optionBButton.onClick.AddListener(() => SelectOption("B"));

        optionAButton.gameObject.SetActive(false);
        optionBButton.gameObject.SetActive(false);
        
        videoPlayer.Prepare();

        // 파일 경로 설정 (실행 파일이 있는 경로에 저장)
        filePath = Path.Combine(Application.persistentDataPath, "data.csv");
        Debug.Log("CSV 파일 경로: " + filePath); // 파일 경로 출력
    }

    void PlayVideo()
    {
        MoveButtonOffScreen();
        rawImage.gameObject.SetActive(true);
        videoPlayer.Play();
        videoPlayer.loopPointReached += EndReached;
        rawImage.gameObject.SetActive(false);
    }

    void MoveButtonOffScreen()
    {
        RectTransform rectTransform = playButton.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, 1000);
    }

    void EndReached(VideoPlayer vp)
    {
        optionAButton.gameObject.SetActive(true);
        optionBButton.gameObject.SetActive(true);
        rawImage.gameObject.SetActive(false);
    }

    void SelectOption(string option)
    {
        userInfoManager.selectedOption = option; // 선택된 옵션 저장
        Debug.Log("선택한 옵션: " + option);

        // CSV에 옵션 저장
        SaveToCSV(option);

        // "event" 씬으로 전환
        SceneManager.LoadScene("event");
    }

    private void SaveToCSV(string option)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(option); // 선택된 옵션을 CSV에 저장
            }
            Debug.Log("옵션이 CSV에 저장되었습니다: " + option);
        }
        catch (System.Exception e)
        {
            Debug.LogError("CSV 저장 실패: " + e.Message);
        }
    }
}
