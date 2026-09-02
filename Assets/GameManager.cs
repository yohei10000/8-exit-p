using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // どこからでも GameManager.Instance で呼び出せるようにする設定
    public static GameManager Instance { get; private set; }

    [Header("シーン設定")]
    [SerializeField] private string normalSceneName = "map Scene";      // 異常なしシーン
    [SerializeField] private string anomalySceneName = "map Scene 1";    // 異変ありシーン

    // 初回プレイかどうかを判定するフラグ
    private static bool isFirstRun = true;

    private void Awake()
    {
        // シングルトンの設定（重複防止）
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーン切り替え時も破棄しない
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Gキーなどで呼ばれるマップ遷移処理
    /// </summary>
    /// <summary>
    /// Gキーなどで呼ばれるマップ遷移処理
    /// </summary>
    public void GoToRandomMap()
    {
        if (DayManager.currentDay == 1)
        {
            // Day1クリア ➔ Day2へ進めて最初のマップへ
            SceneManager.LoadScene(normalSceneName);
        }
        else
        {
            // 2回目以降（Day2〜）： 75%の確率で異変あり、25%で異常なし
            // Random.Range(0, 100) で 0〜99 の数値を取得
            int randomValue = Random.Range(0, 100);

            if (randomValue < 75)
            {
                // 75% (0〜74) ➔ 異変ありシーンへ
                SceneManager.LoadScene(anomalySceneName);
            }
            else
            {
                // 25% (75〜99) ➔ 異常なしシーンへ
                SceneManager.LoadScene(normalSceneName);
            }
        }
    }
}