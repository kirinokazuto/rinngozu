using UnityEngine;
using UnityEngine.UI;

public class SkillButtonController : MonoBehaviour
{
    [SerializeField] private GameDirector gameDirector; // ゲームの進行状況を管理する GameDirector への参照
    [SerializeField] private Button skillButton;        // スキルボタンの UI 要素
    [SerializeField] private int usableTurn = 3;        // スキルが使用可能になるターン数
    [SerializeField] private int maxUses = 3;           // スキルの最大使用回数
    [SerializeField] private int cooldownTurns = 2;     // スキル使用後のクールタイム（ターン数）
    [SerializeField] private bool isPlayerCard = true;  // このボタンがプレイヤー用かどうか

    private int currentUses = 0;        // 現在の使用回数（累積）
    private int cooldownRemaining = 0; // クールタイム残りターン数
    private int lastCheckedTurn = -1;  // 最後にターンをチェックしたターン番号

    void Start()
    {
        // ボタンがクリックされたときのイベントを登録
        skillButton.onClick.AddListener(OnSkillButtonClicked);
    }

    void Update()
    {
        int currentTurn = gameDirector.GetCurrentTurn();     // 現在のターン数を取得
        bool isPlayerTurn = gameDirector.IsPlayerTurn();     // 現在がプレイヤーのターンかどうか
        bool isMyTurn = (isPlayerCard == isPlayerTurn);      // このボタンが自分のターンに対応しているか

        // ターンが進んだらクールタイムを減らす
        if (currentTurn != lastCheckedTurn)
        {
            lastCheckedTurn = currentTurn;

            if (cooldownRemaining > 0)
                cooldownRemaining--;
        }

        // スキルが使用可能かどうかを判定
        bool canUse = currentTurn >= usableTurn &&           // 使用可能ターンに達している
                      currentUses < maxUses &&               // 使用回数の上限に達していない
                      isMyTurn &&                            // 自分のターンである
                      cooldownRemaining == 0;                // クールタイムが終了している

        // ボタンの操作可否を設定
        skillButton.interactable = canUse;
    }

    // スキルが使用されたことを記録する（外部から呼び出す）
    public void MarkSkillAsUsed()
    {
        int currentTurn = gameDirector.GetCurrentTurn();

        // クールタイム中は使用不可
        if (cooldownRemaining > 0)
        {
            return;
        }

        // 使用回数の上限に達していたら使用不可
        if (currentUses >= maxUses)
        {
            skillButton.interactable = false;
            return;
        }

        currentUses++;               // 使用回数をカウント
        cooldownRemaining = cooldownTurns; // クールタイムを設定


        // 使用回数の上限に達したらボタンを無効化
        if (currentUses >= maxUses)
        {
            skillButton.interactable = false;
        }
    }

    // ボタンがクリックされたときの処理
    private void OnSkillButtonClicked()
    {
        MarkSkillAsUsed(); // スキル使用を記録
    }


    public int GetCooldownRemaining()
    {
        return cooldownRemaining;
    }

    public int GetCurrentUses()
    {
        return currentUses;
    }

    public int GetMaxUses()
    {
        return maxUses;
    }


}
