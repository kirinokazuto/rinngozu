//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Reflection;
//using Unity.Mathematics;
//using UnityEngine;

//public class CPU : MonoBehaviour
//{
//    private Board _board;

//    [SerializeField] private GameDirector gameDirector; // �Q�[���̐i�s�󋵂��Ǘ����� GameDirector �ւ̎Q��

//    public static int rand;//�����_��
//    List<Vector2Int> points = new();////2->3



//    // Start is called before the first frame update
//    void Start()
//    {
//        _board = FindObjectOfType<Board>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        bool isPlayerTurn = gameDirector.IsPlayerTurn();     // ���݂��v���C���[�̃^�[�����ǂ���

//        if (!isPlayerTurn) // �����̃^�[�������^�[�����ǂ���
//        {
//            rand = UnityEngine.Random.Range(1,Board.white_count);//���̃}�[�J�[�̈ʒu�������_���Őݒ�


//            GameObject[] markers = GameObject.FindGameObjectsWithTag("EligibleMarker");
//            foreach (GameObject marker in markers)
//            {
//                Destroy(marker);
//            }

//            //�L���b�V���N���A
//            if (_board != null)
//            {
//                _board.ClearCachedPoints();
//            }
//        }
//    }


//}