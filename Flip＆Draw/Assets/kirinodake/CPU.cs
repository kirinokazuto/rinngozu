//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Reflection;
//using Unity.Mathematics;
//using UnityEngine;

//public class CPU : MonoBehaviour
//{
//    private Board _board;

//    [SerializeField] private GameDirector gameDirector; // ゲームの進行状況を管理する GameDirector への参照

//    public static int rand;//ランダム
//    List<Vector2Int> points = new();////2->3



//    // Start is called before the first frame update
//    void Start()
//    {
//        _board = FindObjectOfType<Board>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        bool isPlayerTurn = gameDirector.IsPlayerTurn();     // 現在がプレイヤーのターンかどうか

//        if (!isPlayerTurn) // 自分のターン＝白ターンかどうか
//        {
//            rand = UnityEngine.Random.Range(1,Board.white_count);//白のマーカーの位置をランダムで設定


//            GameObject[] markers = GameObject.FindGameObjectsWithTag("EligibleMarker");
//            foreach (GameObject marker in markers)
//            {
//                Destroy(marker);
//            }

//            //キャッシュクリア
//            if (_board != null)
//            {
//                _board.ClearCachedPoints();
//            }
//        }
//    }


//}