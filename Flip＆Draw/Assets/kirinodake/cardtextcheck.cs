using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// 特定のボタンを右クリックすると指定された画像を表示するスクリプト。
/// このスクリプトはボタンのGameObjectにアタッチしてください。
/// </summary>
public class cardtextcheck : MonoBehaviour, IPointerClickHandler
{
    UnityEngine.UI.Image myImage; // UnityのImageを使う場合

    // 表示させたいImage（Inspectorで設定）
    public Image targetImage;

    public Image targetImage2;

    public Image targetImage3;

    public Image targetImage4;  

    // マウスクリックイベント処理

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (targetImage != null)
            {
                // targetImage を表示・非表示切り替え
                targetImage.gameObject.SetActive(!targetImage.gameObject.activeSelf);

                // 他の Image を非表示にする
                if (targetImage2 != null)
                    targetImage2.gameObject.SetActive(false);

                if (targetImage3 != null)
                    targetImage3.gameObject.SetActive(false);

                if (targetImage4 != null)
                    targetImage4.gameObject.SetActive(false);

            }

        }
    }
}