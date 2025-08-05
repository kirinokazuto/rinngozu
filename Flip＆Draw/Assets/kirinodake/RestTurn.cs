using UnityEngine;
using UnityEngine.UI;

public class RestTurn : MonoBehaviour
{
    [SerializeField] private SkillButtonController skillController; // 対象のスキルコントローラー
    [SerializeField] private Text statusText; // 表示用の UI テキスト

    void Update()
    {
        int cooldown = skillController.GetCooldownRemaining();
        int currentUses = skillController.GetCurrentUses();
        int maxUses = skillController.GetMaxUses();

        string display = $"使用回数: {currentUses}/{maxUses}\n";
        display += cooldown > 0 ? $"再使用まで:{cooldown}ターン":"使用可能";

        statusText.text = display;
    }
}
