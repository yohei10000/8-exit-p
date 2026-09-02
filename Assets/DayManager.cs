using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance { get; private set; }

    [Header("表示時間設定")]
    [SerializeField] private float waitTime = 1.0f;     // 完全表示で待つ時間（秒）
    [SerializeField] private float fadeDuration = 1.0f; // 徐々に消える時間（秒）

    [Header("エンディング設定")]
    [SerializeField] private string endingSceneName = "EndingScene"; // エンディングシーン名

    public static int currentDay = 1;
    private static bool shouldShowDayPanel = true;

    private GameObject dayPanel;
    private TextMeshProUGUI dayText;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindUIElements();

        if (shouldShowDayPanel)
        {
            StartCoroutine(ShowDayAndFadeOut());
        }
        else
        {
            if (dayPanel != null) dayPanel.SetActive(false);
        }
    }

    private void FindUIElements()
    {
        GameObject panelObj = GameObject.Find("DayFadePanel");
        if (panelObj != null)
        {
            dayPanel = panelObj;
            dayText = dayPanel.GetComponentInChildren<TextMeshProUGUI>();

            canvasGroup = dayPanel.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = dayPanel.AddComponent<CanvasGroup>();
            }
        }
    }

    public IEnumerator ShowDayAndFadeOut()
    {
        shouldShowDayPanel = false;

        if (dayPanel != null)
        {
            dayPanel.SetActive(true);
            if (dayText != null) dayText.text = "DAY " + currentDay;

            if (canvasGroup != null) canvasGroup.alpha = 1f;

            yield return new WaitForSeconds(waitTime);

            float elapsedTime = 0f;
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                if (canvasGroup != null)
                {
                    canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
                }
                yield return null;
            }

            if (canvasGroup != null) canvasGroup.alpha = 0f;
            dayPanel.SetActive(false);
        }
    }

    /// <summary>
    /// 正解して次のDayへ進む処理
    /// </summary>
    public void ProceedToNextDay(string nextSceneName)
    {
        currentDay++; // Day数を進める

        // DAY 8 をクリア（DAY 9 に到達）したらエンディングシーンへ
        if (currentDay > 8)
        {
            shouldShowDayPanel = false; // エンディングではDAYパネルを表示しない
            SceneManager.LoadScene(endingSceneName);
            return;
        }

        shouldShowDayPanel = true;  // 通常時はDAYパネルを表示
        SceneManager.LoadScene(nextSceneName);
    }

   
    public void ResetToDay1(string nextSceneName)
    {
        currentDay = 1;
        shouldShowDayPanel = true;
        SceneManager.LoadScene(nextSceneName);
    }
}