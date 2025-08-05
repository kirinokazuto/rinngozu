using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ボタンがクリックされたときに効果音を再生するコンポーネント。
/// ボタンにアタッチすることで、自動的に AudioSource を追加し、
/// 指定した効果音を再生するようになる。
/// </summary>
public class ButtonClickSound : MonoBehaviour
{
    // ----------------------
    // フィールド
    // ----------------------

    // 再生するクリック音（インスペクターで設定）
    [SerializeField] private AudioClip _clip;

    // AudioSource コンポーネント（内部的に追加される）
    private AudioSource _as;

    // ----------------------
    // プライベートメソッド
    // ----------------------

    /// <summary>
    /// コンポーネントが有効化されたときに呼ばれるメソッド。
    /// AudioSource を追加し、ボタンクリック時にサウンドを再生するようリスナーを登録。
    /// </summary>
    private void OnEnable()
    {
        // AudioSource コンポーネントをこの GameObject に追加
        _as = gameObject.AddComponent<AudioSource>();

        // 再生する AudioClip を設定
        _as.clip = _clip;

        // オブジェクトが生成された時点で自動再生しないように設定
        _as.playOnAwake = false;

        // Button コンポーネントを取得し、クリック時にサウンドを再生するリスナーを登録
        gameObject.GetComponent<Button>().onClick.AddListener(() => {
            _as.Play();  // クリックされたら効果音を再生
        });
    }
}