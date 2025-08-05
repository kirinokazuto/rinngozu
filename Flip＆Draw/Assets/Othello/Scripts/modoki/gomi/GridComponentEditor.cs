/*using Lacobus.Grid;
using UnityEditor;
using UnityEngine;

namespace Lacobus_Editors.Grid
{
    // GridComponentのカスタムエディタ
    [CustomEditor(typeof(GridComponent))]
    public class GridComponentEditor : EditorUtils<GridComponent>
    {
        // デバッグ設定表示用のフラグ
        private static bool _enableDebugSettings = false;

        // 各SerializedPropertyの定義
        SerializedProperty _gcData;
        SerializedProperty _gridDimesion;
        SerializedProperty _cellDimesion;
        SerializedProperty _gridOffset;
        SerializedProperty _offsetType;
        SerializedProperty _presetType;
        SerializedProperty _shouldDrawGizmos;
        SerializedProperty _gridLineColor;
        SerializedProperty _crossLineColor;
        SerializedProperty _useSimpleSpriteRendering;
        SerializedProperty _defaultSimpleSprite;

        // カスタムGUIの描画処理
        public override void CustomOnGUI()
        {
            // プレイ中または一時停止中は編集不可
            bool shouldDisable = EditorApplication.isPlaying || EditorApplication.isPaused;
            if (shouldDisable)
                Info("Exit playmode to edit fields", MessageType.Warning);

            EditorGUI.BeginDisabledGroup(shouldDisable);

            // プロパティの取得
            _gcData = GetProperty("_gcData");
            _useSimpleSpriteRendering = GetProperty("_useSimpleSpriteRendering");
            _defaultSimpleSprite = GetProperty("_defaultSimpleSprite");

            _gridDimesion = _gcData.FindPropertyRelative("gridDimension");
            _cellDimesion = _gcData.FindPropertyRelative("cellDimension");
            _gridOffset = _gcData.FindPropertyRelative("gridOffset");
            _offsetType = _gcData.FindPropertyRelative("offsetType");
            _presetType = _gcData.FindPropertyRelative("presetType");
            _shouldDrawGizmos = _gcData.FindPropertyRelative("shouldDrawGizmos");
            _gridLineColor = _gcData.FindPropertyRelative("gridLineColor");
            _crossLineColor = _gcData.FindPropertyRelative("crossLineColor");

            // セクションタイトル
            Heading("Grid Settings");
            Space(10);

            // デバッグ設定の表示トグル
            _enableDebugSettings = EditorGUILayout.Toggle("Show debug settings : ", _enableDebugSettings);

            // グリッド表示のON/OFFボタン
            if (_shouldDrawGizmos.boolValue == false)
            {
                if (Button("Enable Grid View"))
                    _shouldDrawGizmos.boolValue = true;
            }
            else
            {
                if (Button("Disable Grid View"))
                    _shouldDrawGizmos.boolValue = false;
            }

            Space(10);

            // セルスプライトレンダリング設定
            PropertyField(_useSimpleSpriteRendering, "Create sprite grid on Awake :", "Set this as true if you need some kind of representation of cells");
            if (_useSimpleSpriteRendering.boolValue)
                PropertyField(_defaultSimpleSprite, "Default sprite :", "This will be the default sprite for all the cells");

            Space(20);

            // グリッドサイズとセルサイズ
            PropertyField(_gridDimesion, "Grid Dimensions : ", "Width and height of the grid");
            PropertyField(_cellDimesion, "Cell Dimensions : ", "The size of a single cell");
            PropertyField(_offsetType, "Pivot Type : ", "");

            // グリッド原点のオフセットを計算
            int h = _gridDimesion.vector2IntValue.x;
            int v = _gridDimesion.vector2IntValue.y;
            Vector2 cd = _cellDimesion.vector2Value;

            switch (_offsetType.enumValueIndex)
            {
                case 0: // プリセットピボット
                    PropertyField(_presetType, "Select Preset Pivot : ", "");
                    switch (_presetType.enumValueIndex)
                    {
                        case 0: _gridOffset.vector2Value = new Vector2(-h * cd.x, -v * cd.y); break;          // BottomLeft
                        case 1: _gridOffset.vector2Value = new Vector2(-h * cd.x / 2, -v * cd.y); break;     // BottomCenter
                        case 2: _gridOffset.vector2Value = new Vector2(0, -v * cd.y); break;                 // BottomRight
                        case 3: _gridOffset.vector2Value = new Vector2(-h * cd.x, -v * cd.y / 2); break;     // MiddleLeft
                        case 4: _gridOffset.vector2Value = new Vector2(-h * cd.x / 2, -v * cd.y / 2); break; // MiddleCenter
                        case 5: _gridOffset.vector2Value = new Vector2(0, -v * cd.y / 2); break;             // MiddleRight
                        case 6: _gridOffset.vector2Value = new Vector2(-h * cd.x, 0); break;                 // TopLeft
                        case 7: _gridOffset.vector2Value = new Vector2(-h * cd.x / 2, 0); break;             // TopCenter
                        case 8: _gridOffset.vector2Value = new Vector2(0, 0); break;                         // TopRight
                    }
                    break;
                case 1: // マニュアルオフセット指定
                    PropertyField(_gridOffset, "Pivot Point : ", "");
                    break;
            }

            // デバッグ設定の描画
            if (_enableDebugSettings)
            {
                Space(15);
                Heading("Debug Settings");
                Space(10);
                _gridLineColor.colorValue = EditorGUILayout.ColorField("Grid line color : ", _gridLineColor.colorValue);
                _crossLineColor.colorValue = EditorGUILayout.ColorField("Cross line color : ", _crossLineColor.colorValue);
            }

            EditorGUI.EndDisabledGroup();
        }
    }

    // 汎用的なエディタユーティリティクラス
    public class EditorUtils<TType> : Editor where TType : Object
    {
        // 対象オブジェクト
        public TType Root => (TType)target;

        // Inspectorの描画処理
        public override void OnInspectorGUI()
        {
            serializedObject.Update();   // プロパティの更新
            CustomOnGUI();              // 派生先でオーバーライド
            serializedObject.ApplyModifiedProperties(); // 変更を反映
        }

        public virtual void CustomOnGUI() { }

        // プロパティ取得
        public SerializedProperty GetProperty(string propertyName)
            => serializedObject.FindProperty(propertyName);

        // プロパティフィールドの描画
        public void PropertyField(SerializedProperty property)
            => PropertyField(property, "", "");

        public void PropertyField(SerializedProperty property, string propertyName, string tooltip)
            => EditorGUILayout.PropertyField(property, new GUIContent(propertyName, tooltip));

        // 情報ボックス表示
        public void Info(string info, MessageType type = MessageType.Info)
            => EditorGUILayout.HelpBox(info, type);

        // スライダー表示
        public void PropertySlider(SerializedProperty property, float min, float max, string label)
            => EditorGUILayout.Slider(property, min, max, label);

        // スペースの挿入
        public void Space(float val)
            => GUILayout.Space(val);

        // セクションの見出し
        public void Heading(string label)
        {
            var style = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold
            };
            EditorGUILayout.LabelField(label, style, GUILayout.ExpandWidth(true));
        }

        // ボタンの描画
        public bool Button(string content)
            => GUILayout.Button(content);

        public bool Button(string content, float height)
            => GUILayout.Button(content, GUILayout.Height(height));

        public bool Button(string content, float height, float width)
            => GUILayout.Button(content, GUILayout.Height(height), GUILayout.Width(width));

        // ドロップダウンの描画
        public int DropdownList(string label, int index, string[] choices)
            => EditorGUILayout.Popup(label, index, choices);

        // レイアウト制御
        public void BeginVertical() => EditorGUILayout.BeginVertical();
        public void EndVertical() => EditorGUILayout.EndVertical();
        public void BeginHorizontal() => EditorGUILayout.BeginHorizontal();
        public void EndHorizontal() => EditorGUILayout.EndHorizontal();

        // ラベルの描画
        public void Label(string labelContent)
            => EditorGUILayout.LabelField(labelContent);
    }
}*/