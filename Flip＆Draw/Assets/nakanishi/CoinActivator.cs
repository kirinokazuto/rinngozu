using UnityEngine;

public class CoinActivator : MonoBehaviour
{
    public GameDirector gameDirector; // GameDirectorを参照
    public string coinTag = "Coin"; // Unityで設定したタグ名
    public Board board; // Boardへの参照を追加

    public void ActivateCoinHandler()
    {
        //マーカー削除
        GameObject[] markers = GameObject.FindGameObjectsWithTag("EligibleMarker");
        foreach (GameObject marker in markers)
        {
            Destroy(marker);
        }

        //キャッシュクリア
        if (board != null)
        {
            board.ClearCachedPoints();
        }

        //コインのクリック処理を有効化
        GameObject[] coinObjects = GameObject.FindGameObjectsWithTag(coinTag);
        foreach (GameObject coinObj in coinObjects)
        {
            CoinClickHandler handler = coinObj.GetComponent<CoinClickHandler>();
            if (handler != null)
            {
                handler.EnableHandler();
            }
        }
    }
}
