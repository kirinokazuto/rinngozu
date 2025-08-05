using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// コインに関連するサウンドを管理するクラス。
/// 配置音（ランダム再生）と反転音を制御。
/// </summary>
public class CoinSoundHandler : MonoBehaviour
{
    // ----------------------
    // フィールド
    // ----------------------

    // コインを置いたときに再生されるサウンドクリップ（複数からランダムに選択）
    [SerializeField] private List<AudioClip> _placingSounds = new List<AudioClip>();

    // コインを反転させたときに再生されるサウンドクリップ
    [SerializeField] private AudioClip _flippingSound;

    // サウンドを再生するための AudioSource コンポーネント
    private AudioSource _audioSource;

    // ----------------------
    // パブリックメソッド
    // ----------------------

    /// <summary>
    /// コインを配置したときのサウンドをランダムで再生する。
    /// </summary>
    public void PlayCoinPlaceSound()
    {
        // 0 以上 _placingSounds.Count 未満のインデックスをランダムに選ぶ
        var index = getRandomFrom(0, _placingSounds.Count);

        // 選ばれたサウンドを再生
        _audioSource.clip = _placingSounds[index];
        _audioSource.Play();
    }

    /// <summary>
    /// コインを反転させたときのサウンドを再生する。
    /// </summary>
    public void PlayCoinFlipSound()
    {
        _audioSource.clip = _flippingSound;
        _audioSource.Play();
    }

    // ----------------------
    // プライベートメソッド
    // ----------------------

    /// <summary>
    /// AudioSource コンポーネントを取得する（初期化処理）。
    /// </summary>
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 指定された範囲内でランダムな整数値を返す。
    /// フレーム数を使って乱数の初期化を行うことで毎回異なる結果を得る。
    /// </summary>
    /// <param name="min">最小値（含む）</param>
    /// <param name="max">最大値（含まない）</param>
    /// <returns>min 以上 max 未満のランダムな整数</returns>
    private int getRandomFrom(int min, int max)
    {
        Random.InitState(Time.frameCount); // 毎フレーム異なるシードで初期化
        return Random.Range(min, max);
    }
}