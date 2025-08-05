using UnityEngine;

public class CoinActivator : MonoBehaviour
{
    public GameDirector gameDirector; // GameDirector���Q��
    public string coinTag = "Coin"; // Unity�Őݒ肵���^�O��
    public Board board; // Board�ւ̎Q�Ƃ�ǉ�

    public void ActivateCoinHandler()
    {
        //�}�[�J�[�폜
        GameObject[] markers = GameObject.FindGameObjectsWithTag("EligibleMarker");
        foreach (GameObject marker in markers)
        {
            Destroy(marker);
        }

        //�L���b�V���N���A
        if (board != null)
        {
            board.ClearCachedPoints();
        }

        //�R�C���̃N���b�N������L����
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
