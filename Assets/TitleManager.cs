using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [Header("遷移先のシーン名")]
    [SerializeField] private string firstSceneName = "taiki Scene";
    [SerializeField] private string titleSceneName = "Title Scene";

    private void Start()
    {
        // シーンが読み込まれたらマウスカーソルを表示＆ロック解除する
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnStartButtonClicked()
    {
        DayManager.currentDay = 1;
        AnomalyManager.ResetAnomalyHistory(); // 異変履歴のリセット
        SceneManager.LoadScene(firstSceneName);
    }

    public void OnTitleButtonClicked()
    {
        SceneManager.LoadScene(titleSceneName);
    }

    public void OnQuitButtonClicked()
    {
        Debug.Log("ゲームを終了します");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}