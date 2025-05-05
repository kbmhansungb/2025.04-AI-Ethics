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

    private void Awake()
    {
        playButton.onClick.AddListener(PlayVideo);
        optionAButton.onClick.AddListener(() => SelectOption("A"));
        optionBButton.onClick.AddListener(() => SelectOption("B"));


        videoPlayer.loopPointReached += EndReached;
    }

    void OnEnable()
    {
        playButton.gameObject.SetActive(true); // 비디오 재생 버튼 활성화

        userInfoManager = UserInfoManager.Instance; // Singleton 사용
        if (userInfoManager == null)
        {
            Debug.LogError("UserInfoManager를 찾을 수 없습니다.");
            return; // UserInfoManager가 없으면 이후 코드를 실행하지 않음
        }

        optionAButton.gameObject.SetActive(false);
        optionBButton.gameObject.SetActive(false);


        rawImage.gameObject.SetActive(true);
        videoPlayer.Stop();        // 재생 중이었다면 멈추고
        videoPlayer.StepForward();  // 0프레임을 바로 디코딩
        videoPlayer.Prepare();

        // 파일 경로 설정 (실행 파일이 있는 경로에 저장)
        filePath = Path.Combine(Application.persistentDataPath, "data.csv");
        Debug.Log("CSV 파일 경로: " + filePath); // 파일 경로 출력
    }

    void PlayVideo()
    {
        rawImage.gameObject.SetActive(true);
        videoPlayer.Play();
        rawImage.gameObject.SetActive(false);

        playButton.gameObject.SetActive(false); // 비디오 재생 후 버튼 숨김
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
        //SceneManager.LoadScene("event");
        GlobalManager.Instance.SetState(EState.Event); // GlobalManager를 통해 상태 변경
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
