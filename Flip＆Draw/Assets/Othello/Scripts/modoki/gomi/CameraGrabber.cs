/*using UnityEngine;

/// <summary>
/// このスクリプトは、Canvas コンポーネントが存在する場合に、
/// 自動的にメインカメラ（Camera.main）を割り当てる役割を持つ。
/// 通常は、World Space モードの Canvas に使用される。
/// </summary>
public class CameraGrabber : MonoBehaviour
{
    /// <summary>
    /// オブジェクトが生成されたとき（Awake 時）に呼び出される。
    /// Canvas コンポーネントを取得し、存在する場合はメインカメラを設定する。
    /// </summary>
    private void Awake()
    {
        // このオブジェクトに Canvas コンポーネントがあるか調べ、
        // あればメインカメラ（Camera.main）を Canvas に割り当てる
        if (TryGetComponent(out Canvas canvas))
            canvas.worldCamera = Camera.main;
    }
}*/