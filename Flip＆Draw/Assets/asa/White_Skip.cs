using UnityEngine;

public class White_Skip : MonoBehaviour
{
    [SerializeField] private GameDirector gameDirector;
    [SerializeField] private Board board;

    private bool pendingOverride = false;
    private bool wasPlayerTurn = false;

    void Update()
    {
        if (gameDirector.IsGameOver()) return;

        bool currentTurn = !gameDirector.IsPlayerTurn(); // ���^�[������͍��̋t�Ȃ̂ł������܂�

        if (wasPlayerTurn && !currentTurn && pendingOverride)
        {
            Debug.Log("�����X�L�������I�����I�Ɏ����̃^�[���ɖ߂��܂�");
            ForceTurnBack();
            pendingOverride = false;

            UpdateMarkers();
        }

        wasPlayerTurn = currentTurn;
    }

    public void ActivateSkill()
    {
        if (!(!gameDirector.IsPlayerTurn())) // �����̃^�[�������^�[�����ǂ���
        {
            Debug.Log("�����̃^�[���ȊO�̓X�L�����g���܂���");
            return;
        }

        Debug.Log("�����X�L�b�v�X�L���g�p�\��I");
        pendingOverride = true;
    }

    private void ForceTurnBack()
    {
        var selectorField = typeof(GameDirector).GetField("_playerSelector", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (selectorField != null)
        {
            bool current = (bool)selectorField.GetValue(gameDirector);
            selectorField.SetValue(gameDirector, !current);
            Debug.Log("�����^�[���������I�ɖ߂���܂����I");
        }
        else
        {
            Debug.LogError("GameDirector��_playerSelector�ɃA�N�Z�X�ł��܂���");
        }
    }

    private void UpdateMarkers()
    {
        gameDirector.ClearMarkers();

        ClearCachedEligiblePositions();

        bool success = board.UpdateEligiblePositions(gameDirector.getFace());
        Debug.Log(success ? "�}�[�J�[���Đ������܂���" : "���@��Ȃ�");
    }

    private void ClearCachedEligiblePositions()
    {
        var blackPointsField = typeof(Board).GetField("_cachedBlackPoints", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var whitePointsField = typeof(Board).GetField("_cachedWhitePoints", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (blackPointsField != null)
            blackPointsField.SetValue(board, null);
        if (whitePointsField != null)
            whitePointsField.SetValue(board, null);
    }
}
