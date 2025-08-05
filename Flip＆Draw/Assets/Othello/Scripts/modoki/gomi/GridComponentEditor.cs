/*using Lacobus.Grid;
using UnityEditor;
using UnityEngine;

namespace Lacobus_Editors.Grid
{
    // GridComponent�̃J�X�^���G�f�B�^
    [CustomEditor(typeof(GridComponent))]
    public class GridComponentEditor : EditorUtils<GridComponent>
    {
        // �f�o�b�O�ݒ�\���p�̃t���O
        private static bool _enableDebugSettings = false;

        // �eSerializedProperty�̒�`
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

        // �J�X�^��GUI�̕`�揈��
        public override void CustomOnGUI()
        {
            // �v���C���܂��͈ꎞ��~���͕ҏW�s��
            bool shouldDisable = EditorApplication.isPlaying || EditorApplication.isPaused;
            if (shouldDisable)
                Info("Exit playmode to edit fields", MessageType.Warning);

            EditorGUI.BeginDisabledGroup(shouldDisable);

            // �v���p�e�B�̎擾
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

            // �Z�N�V�����^�C�g��
            Heading("Grid Settings");
            Space(10);

            // �f�o�b�O�ݒ�̕\���g�O��
            _enableDebugSettings = EditorGUILayout.Toggle("Show debug settings : ", _enableDebugSettings);

            // �O���b�h�\����ON/OFF�{�^��
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

            // �Z���X�v���C�g�����_�����O�ݒ�
            PropertyField(_useSimpleSpriteRendering, "Create sprite grid on Awake :", "Set this as true if you need some kind of representation of cells");
            if (_useSimpleSpriteRendering.boolValue)
                PropertyField(_defaultSimpleSprite, "Default sprite :", "This will be the default sprite for all the cells");

            Space(20);

            // �O���b�h�T�C�Y�ƃZ���T�C�Y
            PropertyField(_gridDimesion, "Grid Dimensions : ", "Width and height of the grid");
            PropertyField(_cellDimesion, "Cell Dimensions : ", "The size of a single cell");
            PropertyField(_offsetType, "Pivot Type : ", "");

            // �O���b�h���_�̃I�t�Z�b�g���v�Z
            int h = _gridDimesion.vector2IntValue.x;
            int v = _gridDimesion.vector2IntValue.y;
            Vector2 cd = _cellDimesion.vector2Value;

            switch (_offsetType.enumValueIndex)
            {
                case 0: // �v���Z�b�g�s�{�b�g
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
                case 1: // �}�j���A���I�t�Z�b�g�w��
                    PropertyField(_gridOffset, "Pivot Point : ", "");
                    break;
            }

            // �f�o�b�O�ݒ�̕`��
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

    // �ėp�I�ȃG�f�B�^���[�e�B���e�B�N���X
    public class EditorUtils<TType> : Editor where TType : Object
    {
        // �ΏۃI�u�W�F�N�g
        public TType Root => (TType)target;

        // Inspector�̕`�揈��
        public override void OnInspectorGUI()
        {
            serializedObject.Update();   // �v���p�e�B�̍X�V
            CustomOnGUI();              // �h����ŃI�[�o�[���C�h
            serializedObject.ApplyModifiedProperties(); // �ύX�𔽉f
        }

        public virtual void CustomOnGUI() { }

        // �v���p�e�B�擾
        public SerializedProperty GetProperty(string propertyName)
            => serializedObject.FindProperty(propertyName);

        // �v���p�e�B�t�B�[���h�̕`��
        public void PropertyField(SerializedProperty property)
            => PropertyField(property, "", "");

        public void PropertyField(SerializedProperty property, string propertyName, string tooltip)
            => EditorGUILayout.PropertyField(property, new GUIContent(propertyName, tooltip));

        // ���{�b�N�X�\��
        public void Info(string info, MessageType type = MessageType.Info)
            => EditorGUILayout.HelpBox(info, type);

        // �X���C�_�[�\��
        public void PropertySlider(SerializedProperty property, float min, float max, string label)
            => EditorGUILayout.Slider(property, min, max, label);

        // �X�y�[�X�̑}��
        public void Space(float val)
            => GUILayout.Space(val);

        // �Z�N�V�����̌��o��
        public void Heading(string label)
        {
            var style = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold
            };
            EditorGUILayout.LabelField(label, style, GUILayout.ExpandWidth(true));
        }

        // �{�^���̕`��
        public bool Button(string content)
            => GUILayout.Button(content);

        public bool Button(string content, float height)
            => GUILayout.Button(content, GUILayout.Height(height));

        public bool Button(string content, float height, float width)
            => GUILayout.Button(content, GUILayout.Height(height), GUILayout.Width(width));

        // �h���b�v�_�E���̕`��
        public int DropdownList(string label, int index, string[] choices)
            => EditorGUILayout.Popup(label, index, choices);

        // ���C�A�E�g����
        public void BeginVertical() => EditorGUILayout.BeginVertical();
        public void EndVertical() => EditorGUILayout.EndVertical();
        public void BeginHorizontal() => EditorGUILayout.BeginHorizontal();
        public void EndHorizontal() => EditorGUILayout.EndHorizontal();

        // ���x���̕`��
        public void Label(string labelContent)
            => EditorGUILayout.LabelField(labelContent);
    }
}*/