using System.Linq;

using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using xk514;

namespace k514.Mono.Common
{
    public static class SceneTool
    {
        #region <Consts>

        public const string LOBBY_SCENE_NAME = "LobbyScene.unity";
        public const int LOBBY_SCENE_ENTRY_INDEX = 1;

        #endregion
        
        #region <Enums>

        public enum SceneLocationType
        {
            None,
            
            ZakoSpawner,
            ChampSpawner,
            BossSpawner,
            GearSpawner,
            MessageSpawner,
            VictoryLocation,
        }

        public enum SceneLocationWaveType
        {
            Default,
            FirstOnce,
        }
        
        public enum SystemSceneType
        {
            BootScene,
            PatchScene,
            InitScene,
            TitleScene,
        }

        public enum LoadingSceneType
        {
            Black,
            SolidImage,
        }

#if UNITY_EDITOR
        public enum EditorSceneType
        {
            Dexter,
        }
#endif

        public enum SceneShortCutType
        {
            EntryScene,
            LobbyScene,
        }

        [Flags]
        public enum SceneControlFlag
        {
            None = 0,
            
            /// <summary>
            /// 해당 씬 전이 이후 플레이어 제어가 필요한 경우
            /// </summary>
            HasSceneStartPreset = 1 << 0,
        }

        /// <summary>
        /// 씬 전이시 이벤트 타입
        /// </summary>
        public enum SceneChangeEventType
        {
            None,
            SceneTransition,
            ScenePreload,
            SceneStart,
            SceneTerminate,
        }
        
        [Flags]
        public enum SceneEventType
        {
            None = 0,
            OnSceneEnvironmentPreload = 1 << 0,
        }

        #endregion
        
        #region <Methods>
        
        public static string GetActiveScenePath()
        {
            return SceneManager.GetActiveScene().path;
        }
        
        public static string GetActiveSceneName(bool p_AddExt)
        {
            return GetActiveScenePath().GetFileNameFromPath(p_AddExt);
        }
        
        public static string GetSystemSceneLoadPath(SystemSceneType p_Type)
        {
            return SystemScenePathTable.GetInstanceUnsafe.GetRecord(p_Type).SceneFullPath;
        }
        
        public static string GetLoadingSceneLoadPath(LoadingSceneType p_Type)
        {
            return LoadingScenePathTable.GetInstanceUnsafe.GetRecord(p_Type).SceneFullPath;
        }

#if UNITY_EDITOR
        public static string GetEditorSceneLoadPath(EditorSceneType p_Type)
        {
            return EditorScenePathTable.GetInstanceUnsafe.GetRecord(p_Type).SceneFullPath;
        }
#endif

        public static void TurnToSystemScene(SystemSceneType p_Type)
        {
            SceneManager.LoadScene(GetSystemSceneLoadPath(p_Type), LoadSceneMode.Single);
        }

        public static void TurnToLoadingScene(LoadingSceneType p_Type)
        {
            SceneManager.LoadScene(GetLoadingSceneLoadPath(p_Type), LoadSceneMode.Single);
        }
        
        #endregion

        #region <Structs>

        public readonly struct SceneInfo
        {
            #region <Fields>

            /// <summary>
            /// 씬의 풀패스(Assets/Resources/..)
            /// </summary>
            public readonly string SceneFullPath;

            /// <summary>
            /// 로딩씬에서 비동기 로딩할 씬의 이름
            /// </summary>
            public readonly string SceneName;

            /// <summary>
            /// 유효성 검증 플래그
            /// </summary>
            public readonly bool ValidFlag;
            
            #endregion

            #region <Constructors>

            public SceneInfo(string p_SceneFullPath)
            {
                if (string.IsNullOrWhiteSpace(p_SceneFullPath))
                {
                    SceneFullPath = null;
                    SceneName = null;
                    ValidFlag= false;
                }
                else
                {
                    SceneFullPath = p_SceneFullPath;
                    SceneName = SceneFullPath.GetFileNameFromPath(true);
                    ValidFlag= true;
                }
            }

            #endregion

            #region <Operator>
                        
#if UNITY_EDITOR
            public override string ToString()
            {
                return $"[SceneName : {SceneName}]\n[SceneFullPath : {SceneFullPath}]\n";
            }
#endif

            #endregion
        }
        
        public readonly struct SceneChangePreset
        {
            #region <Fields>

            /// <summary>
            /// 해당 씬 이름
            /// </summary>
            public readonly SceneInfo SceneInfo;

            /// <summary>
            /// 씬 변수 레코드 인덱스
            /// </summary>
            public readonly int SceneVariableRecordIndex;

            /// <summary>
            /// 씬 컨트롤 프리셋
            /// </summary>
            public readonly SceneControlPreset SceneControlPreset;

            /// <summary>
            /// 유효성 검증 플래그
            /// </summary>
            public readonly bool ValidFlag;
            
            /// <summary>
            /// 씬 상수 데이터
            /// </summary>
            public SceneConstantDataTable.TableRecord SceneConstantDataRecord => SceneConstantDataTable.GetInstanceUnsafe[SceneInfo.SceneName];

            /// <summary>
            /// 씬 변수 데이터
            /// </summary>
            public SceneVariableDataTable.TableRecord SceneVariableDataRecord => SceneConstantDataRecord.GetSceneVariableDataRecord(SceneVariableRecordIndex);
            
            #endregion

            #region <Constructor>

            public SceneChangePreset(SceneInfo p_SceneInfo, int p_SceneVariableIndex, SceneControlPreset p_SceneControlPreset)
            {
                SceneInfo = p_SceneInfo;
                SceneVariableRecordIndex = p_SceneVariableIndex;
                SceneControlPreset = p_SceneControlPreset;
                ValidFlag = p_SceneInfo.ValidFlag;
            }
            
            #endregion

            #region <Methods>

            public bool TryGetSceneName(out string o_SceneName)
            {
                if (ValidFlag)
                {
                    o_SceneName = SceneInfo.SceneName;
                    return true;
                }
                else
                {
                    o_SceneName = null;
                    return false;
                }
            }
            
            public bool TryGetSceneFullPath(out string o_SceneFullPath)
            {
                if (ValidFlag)
                {
                    o_SceneFullPath = SceneInfo.SceneFullPath;
                    return true;
                }
                else
                {
                    o_SceneFullPath = null;
                    return false;
                }
            }

            #endregion
        }

        public struct SceneControlPreset
        {
            #region <Fields>
                    
            /// <summary>
            /// 해당 씬 전이 플래그
            /// </summary>
            private SceneControlFlag _SceneControlFlagMask;
            
            /// <summary>
            /// 씬 로딩 연출 타입
            /// </summary>
            public LoadingSceneType LoadingSceneType { get; private set; }

            /// <summary>
            /// 씬 시작시에 적용할 값 프리셋
            /// </summary>
            public SceneStartPreset SceneStartPreset { get; private set; }
            
            #endregion

            #region <Constructor>

            public SceneControlPreset(LoadingSceneType p_LoadingSceneType = LoadingSceneType.Black)
            {
                LoadingSceneType = p_LoadingSceneType;
                _SceneControlFlagMask = SceneControlFlag.None;
                SceneStartPreset = default;
            }
            
            public SceneControlPreset(SceneStartPreset p_SceneStartPreset, LoadingSceneType p_LoadingSceneType = LoadingSceneType.Black) : this(p_LoadingSceneType)
            {
                _SceneControlFlagMask.AddFlag(SceneControlFlag.HasSceneStartPreset);
                SceneStartPreset = p_SceneStartPreset;
            }
            
            #endregion
            
            #region <Operator>

            public static implicit operator SceneControlPreset(LoadingSceneType p_LoadingSceneType)
            {
                return new SceneControlPreset(p_LoadingSceneType : p_LoadingSceneType);
            }

            public static implicit operator SceneControlPreset(SceneStartPreset p_SceneStartPreset)
            {
                return new SceneControlPreset(p_SceneStartPreset : p_SceneStartPreset);
            }
            
            public static implicit operator SceneControlPreset((LoadingSceneType t_LoadingSceneType, SceneStartPreset t_SceneStartPreset) p_Tuple)
            {
                return new SceneControlPreset(p_LoadingSceneType : p_Tuple.t_LoadingSceneType, p_SceneStartPreset : p_Tuple.t_SceneStartPreset);
            }

            #endregion

            #region <Methods>

            public bool TryGetSceneStartPreset(out SceneStartPreset o_Preset)
            {
                if (_SceneControlFlagMask.HasAnyFlagExceptNone(SceneControlFlag.HasSceneStartPreset))
                {
                    o_Preset = SceneStartPreset;
                    return true;
                }
                else
                {
                    o_Preset = default;
                    return false;
                }
            }

            #endregion
        }

        [Serializable]
        public struct SceneStartPreset
        {
            #region <Fields>

            public TableTool.SerializableVector3 StartPosition;
            public TableTool.SerializableVector3 StartRotation;
            
            #endregion

            #region <Constructor>
            
            public SceneStartPreset(Vector3 p_StartPosition = default, Vector3 p_StartRotation = default)
            {
                StartPosition = p_StartPosition;
                StartRotation = p_StartRotation;
            }
            
            public SceneStartPreset(Transform p_Affine)
            {
                StartPosition = p_Affine.position;
                StartRotation = p_Affine.rotation.eulerAngles;
            }

            #endregion
        }

        [Serializable]
        public struct SceneLocationMeta
        {
            #region <Fields>

            public SceneLocationType LocationType;
            public TableTool.SerializableVector3 Position;
            public TableTool.SerializableVector3 Rotation;
            public TableTool.SerializableVector3 Scale;
            public TableTool.SerializableVector3 Center;
            public TableTool.SerializableVector3 Size;
            public List<SceneLocationPivotMeta> SceneLocationPivotMetaSet;
            
            #endregion

            #region <Constructor>

#if UNITY_EDITOR
            public SceneLocationMeta(SceneLocationIndicator p_SceneLocation)
            {
                LocationType = p_SceneLocation.LocationType;

                var affine = p_SceneLocation.transform;
                Position = affine.position;
                Rotation = affine.rotation.eulerAngles;
                Scale = affine.localScale;

                var boxCollider = p_SceneLocation.BoxCollider;
                Center = boxCollider.center;
                Size = boxCollider.size;

                SceneLocationPivotMetaSet = p_SceneLocation.GetComponentsInChildren<SceneLocationPivotIndicator>().Select(pivot => new SceneLocationPivotMeta(pivot)).ToList();
            }
#endif
            public SceneLocationMeta(SceneLocationType p_LocationType, Vector3 p_Position, Vector3 p_Rotation, Vector3 p_Scale,
                Vector3 p_Center, Vector3 p_Size, List<SceneLocationPivotMeta> p_SceneLocationPivotMetaSet)
            {
                LocationType = p_LocationType;
                Position = p_Position;
                Rotation = p_Rotation;
                Scale = p_Scale;
                Center = p_Center;
                Size = p_Size;
                SceneLocationPivotMetaSet = p_SceneLocationPivotMetaSet;
            }
 
            #endregion
        }
        
        [Serializable]
        public struct SceneLocationPivotMeta
        {
            #region <Fields>

            public TableTool.SerializableVector3 Position;
            public TableTool.SerializableVector3 Rotation;
            public float Scale;
            public List<int> SpawnEntityList;
            public SceneLocationWaveType SceneLocationWaveType;
            public int WaveOrder;

            #endregion
    
            #region <Constructor>

#if UNITY_EDITOR
            public SceneLocationPivotMeta(SceneLocationPivotIndicator p_SceneLocationPivot)
            {
                var affine = p_SceneLocationPivot.transform;
                Position = affine.position;
                Rotation = affine.rotation.eulerAngles;
                Scale = affine.localScale.x;
                SpawnEntityList = p_SceneLocationPivot.SpawnEntityList;
                SceneLocationWaveType = p_SceneLocationPivot.SceneLocationWaveType;
                WaveOrder = p_SceneLocationPivot.WaveOrder;
            }
#endif  
            
            public SceneLocationPivotMeta(Vector3 p_Position, List<int> p_SpawnEntityList, SceneLocationWaveType p_SceneLocationWaveType, int p_WaveOrder)
            : this(p_Position, Vector3.zero, 1f, p_SpawnEntityList, p_SceneLocationWaveType, p_WaveOrder)
            {
            }
            
            public SceneLocationPivotMeta(Vector3 p_Position, Vector3 p_Rotation, float p_Scale, List<int> p_SpawnEntityList, SceneLocationWaveType p_SceneLocationWaveType, int p_WaveOrder)
            {
                Position = p_Position;
                Rotation = p_Rotation;
                Scale = p_Scale;
                SpawnEntityList = p_SpawnEntityList;
                SceneLocationWaveType = p_SceneLocationWaveType;
                WaveOrder = p_WaveOrder;
            }

            #endregion

            #region <Methods>

            public AffinePreset GetAffinePreset()
            {
                return new AffinePreset(Position, Rotation, Scale);
            }

            #endregion
        }
        
        #endregion
    }
}