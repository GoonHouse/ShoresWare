using System;
using UnityEngine;
using JetBrains.Annotations;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ShoresWare {
    public class Data {
        [Serializable]
        public struct ItemInfo {
            public string name;
            public string description;
            public string authorName;
        }
        
        [Serializable]
        public struct AreaMap {
            public Vector3 position;
            public Vector3 size;
        }
        
        [Serializable]
        public struct FrameInfo {
            // @TODO: useTwistBorder
            // @TODO: edge radius?
            public Color borderColor;
            public Sprite borderSprite;
            
            public Color backgroundColor;
            public Sprite backgroundSprite;
        }
        

        public enum GameState {
            GAME_STATE_INDETERMINANT = 0,
            GAME_STATE_WON           = 1,
            GAME_STATE_LOST          = 2
        }

        public enum GameDifficulty {
            GAME_DIFFICULTY_NONE   = 0,
            GAME_DIFFICULTY_EASY   = 1,
            GAME_DIFFICULTY_MEDIUM = 2,
            GAME_DIFFICULTY_HARD   = 3
        }
        
        public enum ControlType {
            CONTROL_TYPE_NONE  = 0,
            CONTROL_TYPE_MASH  = 1,
            CONTROL_TYPE_TWIST = 2,
            CONTROL_TYPE_TOUCH = 4,
            CONTROL_TYPE_MIC   = 8
        }

        public enum GameLength {
            GAME_LENGTH_INFINITE = -1, // Boss games, Minigames
            GAME_LENGTH_QUICK    =  4, // Fronk
            GAME_LENGTH_NORMAL   =  8, // Regular Gameplay (2 bars)
            GAME_LENGTH_LONG     = 16, // Peeping At Things
        }

        public enum CardinalDirection {
            CARDINAL_DIRECTION_NORTH       = 90,
            CARDINAL_DIRECTION_NORTH_EAST  = 45,
            CARDINAL_DIRECTION_EAST        = 0,
            CARDINAL_DIRECTION_SOUTH_EAST  = 315,
            CARDINAL_DIRECTION_SOUTH       = 270,
            CARDINAL_DIRECTION_SOUTH_WEST  = 225,
            CARDINAL_DIRECTION_WEST        = 180,
            CARDINAL_DIRECTION_NORTH_WEST  = 135
        }

        [Serializable]
        public class SceneField {
            [SerializeField] [UsedImplicitly] private Object m_SceneAsset;
            [SerializeField] private string m_ScenePath = "";

            public string ScenePath {
                get { return m_ScenePath; }
            }

            // makes it work with the existing Unity methods (LoadLevel/LoadScene)
            public static implicit operator string(SceneField sceneField) {
                return sceneField.ScenePath;
            }
        }
        
#if UNITY_EDITOR
        [CustomPropertyDrawer( typeof( SceneField ) )]
        public class SceneFieldPropertyDrawer : PropertyDrawer {
            public override void OnGUI( Rect _position, SerializedProperty _property, GUIContent _label ) {
                EditorGUI.BeginProperty( _position, GUIContent.none, _property );
                var sceneAsset = _property.FindPropertyRelative( "m_SceneAsset" );
                var scenePath = _property.FindPropertyRelative( "m_ScenePath" );
                _position = EditorGUI.PrefixLabel( _position, GUIUtility.GetControlID( FocusType.Passive ), _label );

                if( sceneAsset != null ) {
                    EditorGUI.BeginChangeCheck();

                    var value = EditorGUI.ObjectField( _position, sceneAsset.objectReferenceValue, typeof( SceneAsset ), false );
                    if( EditorGUI.EndChangeCheck() ) {
                        sceneAsset.objectReferenceValue = value;
                        if( sceneAsset.objectReferenceValue != null ) {
                            scenePath.stringValue = AssetDatabase.GetAssetPath(value);
                        }
                    }
                }
                EditorGUI.EndProperty();
            }
        }
#endif
    }
}