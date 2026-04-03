using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using xk514;

namespace k514
{
    public static partial class TableTool
    {
        #region <Consts>

        /// <summary>
        /// xml 메타 태그 명
        /// </summary>
        public const string XmlMetaTagName = "Meta";
        
        /// <summary>
        /// xml 테이블 태그 명
        /// </summary>
        public const string XmlTableTagName = "Table";
        
        /// <summary>
        /// xml 레코드 태그 명
        /// </summary>
        public const string XmlRecordTagName = "Record";
        
        /// <summary>
        /// 테이블 데이터 클래스의 키 필드 명
        /// </summary>
        public const string TableKeyTagName = "KEY";

        /// <summary>
        /// 프로퍼티에 의해 가려진 내부 필드 이름 규칙 명
        /// </summary>
        public const string BackingFieldRear = "BackingField";
        
        /// <summary>
        /// xml 테이블 디렉터리 중간 브랜치명
        /// </summary>
        public static readonly string XML_PATH = $"{TableDataType.Xml}/";

        /// <summary>
        /// xml 테이블 파일 확장자
        /// </summary>
        public const string XML_EXT = ".xml";

        /// <summary>
        /// json 테이블 디렉터리 중간 브랜치명
        /// </summary>
        public static readonly string JSON_PATH = $"{TableDataType.JSON}/";

        /// <summary>
        /// json 테이블 파일 확장자
        /// </summary>
        public const string JSON_EXT = ".json";
        
        /// <summary>
        /// csv 테이블 디렉터리 중간 브랜치명
        /// </summary>
        public static readonly string CSV_PATH = $"{TableDataType.CSV}/";

        /// <summary>
        /// csv 테이블 파일 확장자
        /// </summary>
        public const string CSV_EXT = ".csv";
        
        /// <summary>
        /// 바이트 테이블 디렉터리 중간 브랜치명
        /// </summary>
        public static readonly string BYTES_PATH = $"{TableDataType.Bytes}/";

        /// <summary>
        /// 바이트 파일 확장자
        /// </summary>
        public const string BYTES_EXT = ".bytes";

        #endregion

        #region <Constructor>

        static TableTool()
        {
            TableNameQueryTypeEnumerator =
                EnumFlag.GetEnumEnumerator<TableNameQueryType>(EnumFlag.GetEnumeratorType.ExceptMaskNone);
        }

        #endregion

        #region <Enums>

        public enum TableType
        {
            GameTable,
            OptionalTable,
            EditorOnlyTable,
            SystemTable,
        }

        public enum TableFileType
        {
            Xml,
            JSON,
            CSV,
        }
        
        public enum TableDataType
        {
            Xml,
            JSON,
            CSV,
            Bytes,
        }

        [Flags]
        public enum TableNameQueryType
        {
            None = 0,
            
            WithMainTableName = 1 << 0,
            WithBranchHeader = 1 << 1,
            WithTableExtension = 1 << 2,
            WithByteExtension = 1 << 3,

            WithBranchHeaderMain = WithBranchHeader | WithMainTableName,
            WithMainTableExt = WithMainTableName | WithTableExtension,
            WithMainByteExt = WithMainTableName | WithByteExtension,
            WithBranchHeaderMainTableExt = WithMainTableName | WithBranchHeader | WithTableExtension,
            WithBranchHeaderMainByteExt = WithMainTableName | WithBranchHeader | WithByteExtension
        }

        public static TableNameQueryType[] TableNameQueryTypeEnumerator;

        [Flags]
        public enum TableStateFlag
        {
            None = 0,
            
            /// <summary>
            /// 씬 전환중에 세트되는 플래그
            /// </summary>
            SceneTerminate = 1 << 0,
                  
            /// <summary>
            /// 테이블이 특정 상위 경로를 가지는 경우 세트되는 플래그
            /// </summary>
            HasBranchHeader = 1 << 1,
            
            /// <summary>
            /// 테이블이 별명을 가지는 경우 세트되는 플래그
            /// </summary>
            HasAlterTableName = 1 << 2,
            
            /// <summary>
            /// 테이블의 키 타입이 레퍼런스 타입인 경우 세트되는 플래그
            /// </summary>
            ReferenceTypeKey = 1 << 3,
        }

        public enum TableSerializeType
        {
            /// <summary>
            /// 직렬화 없이 테이블 파일로부터 테이블 인스턴스를 초기화하는 타입
            /// </summary>
            NoneSerialize,
            
            /// <summary>
            /// 바이너리화된 테이블 이미지로부터 테이블 인스턴스를 초기화하는 타입
            /// </summary>
            SerializeBinaryTableImage,
        }

        /// <summary>
        /// 테이블 혹은 레코드 클래스를 구성하는 필드/프로퍼티에 어트리뷰트를 부여할 때 사용하는 열거형 상수
        /// </summary>
        public enum TableRecordAttributeType
        {
            /// <summary>
            /// 기본 상태
            /// </summary>
            None,
            
            /// <summary>
            /// 해당 어트리뷰트를 보유한 필드는 런타임에서 선정되는 값이므로, 인코딩시 직렬화되지 않는다.
            /// </summary>
            Runtime,
        }

        #endregion

        #region <Methods>

        public static bool IsUnityResourceTable(this ITable p_Table)
        {
            switch (p_Table.TableType)
            {
                case TableType.GameTable:
                case TableType.OptionalTable:
                    return ResourceType.Table.GetResourceLoadType() == ResourceLoadType.FromUnityResource;;
                default:
                case TableType.EditorOnlyTable:
                case TableType.SystemTable:
                    return true;
            }
        }
        
        public static ResourceLoadType GetAssetLoadType(this ITable p_Table)
        {
            return p_Table.IsUnityResourceTable() ? ResourceLoadType.FromUnityResource : ResourceLoadType.FromAssetBundle;
        }

        /// <summary>
        /// 테이블 타입에 따른 파일 확장자를 리턴하는 메서드
        /// </summary>
        public static string GetTableExtension(this TableFileType p_Type)
        {
            switch (p_Type)
            {
                case TableFileType.Xml :
                    return XML_EXT;
                case TableFileType.JSON :
                    return JSON_EXT;
                case TableFileType.CSV :
                    return CSV_EXT;
            }
            return string.Empty;
        }

        public static string GetTablePath(ResourceLoadType p_ResourceLoadType, AssetPathType p_PathType, TableType p_TableType, TableDataType p_TableDataType)
        {
            switch (p_TableType)
            {
                case TableType.GameTable:
                case TableType.OptionalTable:
                {
                    switch (p_TableDataType)
                    {
                        case TableDataType.Xml :
                            return $"{SystemMaintenance.GetResourcePath(p_ResourceLoadType, ResourceType.Table, p_PathType)}{XML_PATH}";
                        case TableDataType.JSON :
                            return $"{SystemMaintenance.GetResourcePath(p_ResourceLoadType, ResourceType.Table, p_PathType)}{JSON_PATH}";
                        case TableDataType.CSV :
                            return $"{SystemMaintenance.GetResourcePath(p_ResourceLoadType, ResourceType.Table, p_PathType)}{CSV_PATH}";
                        case TableDataType.Bytes :
                            return $"{SystemMaintenance.GetResourcePath(p_ResourceLoadType, ResourceType.Table, p_PathType)}{BYTES_PATH}";
                    }
                    break;
                }
                case TableType.EditorOnlyTable:
                {
                    switch (p_TableDataType)
                    {
                        case TableDataType.Xml :
                            return $"{SystemMaintenance.GetEditorOnlyResourcePath(ResourceType.Table, p_PathType)}{XML_PATH}";
                        case TableDataType.JSON :
                            return $"{SystemMaintenance.GetEditorOnlyResourcePath(ResourceType.Table, p_PathType)}{JSON_PATH}";
                        case TableDataType.CSV :
                            return $"{SystemMaintenance.GetEditorOnlyResourcePath(ResourceType.Table, p_PathType)}{CSV_PATH}";
                    }
                    break;
                }
                case TableType.SystemTable:
                {
                    switch (p_TableDataType)
                    {
                        case TableDataType.Xml :
                            return $"{SystemMaintenance.GetDependencyResourcePath(DependencyResourceSubType.SystemTable, p_PathType)}{XML_PATH}";
                        case TableDataType.JSON :
                            return $"{SystemMaintenance.GetDependencyResourcePath(DependencyResourceSubType.SystemTable, p_PathType)}{JSON_PATH}";
                        case TableDataType.CSV :
                            return $"{SystemMaintenance.GetDependencyResourcePath(DependencyResourceSubType.SystemTable, p_PathType)}{CSV_PATH}";
                        case TableDataType.Bytes :
                            return $"{SystemMaintenance.GetDependencyResourcePath(DependencyResourceSubType.SystemTable, p_PathType)}{BYTES_PATH}";
                    }
                    break;
                }
            }

            return string.Empty;
        }

#if UNITY_EDITOR
        public static async UniTask GenerateAllByteTable(CancellationToken p_Token)
        {
#if APPLY_PRINT_LOG
            CustomDebug.LogError(("Serialize Table Start", Color.green));
#endif
            var systemTableSubClassSet = typeof(SystemTable<,,,>).GetSubTypeSet();
            SingletonTool.DisposeSingleton(systemTableSubClassSet);
            var (systemTableValid, systemTableSingletonSet) = await SingletonTool.CreateSingletonAsync(systemTableSubClassSet, MultiTaskMode.Simultaneous, p_Token);
            if (systemTableValid)
            {
                var asyncList = new List<UniTask>();
                foreach (var table in systemTableSingletonSet)
                {
                    asyncList.Add(((ITable)table).SerializeTable(p_Token));
                }

                await asyncList;
            }

            var gameTableSubClassSet = typeof(GameTable<,,,>).GetSubTypeSet();
            SingletonTool.DisposeSingleton(gameTableSubClassSet);
            var (gameTableValid, gameTableSingletonSet) = await SingletonTool.CreateSingletonAsync(gameTableSubClassSet, MultiTaskMode.Simultaneous, p_Token);
            if (gameTableValid)
            {
                var asyncList = new List<UniTask>();
                foreach (var table in gameTableSingletonSet)
                {
                    asyncList.Add(((ITable)table).SerializeTable(p_Token));
                }
                
                await asyncList;
            }

            await LanguageDataTableQuery.GetInstanceSafe(p_Token);
            var enumerator = EnumFlag.GetEnumEnumerator<LanguageType>(EnumFlag.GetEnumeratorType.GetAll);
            foreach (var languageType in enumerator)
            {
                var languageTableSet = await LanguageDataTableQuery.GetInstanceUnsafe.ReloadLanguageTable(languageType, p_Token);
                var asyncList = new List<UniTask>();
                foreach (var table in languageTableSet)
                {
                    asyncList.Add(table.SerializeTable(p_Token));
                }
                
                await asyncList;
            }
            await LanguageDataTableQuery.GetInstanceUnsafe.ReloadLanguageTable(p_Token);
            
            // 생성된 바이트 테이블을 리소스리스트에 등록시켜준다.
            await ResourceListTable.GetInstanceUnsafe.WriteDefaultTable(true, p_Token);
            await ResourceListTable.GetInstanceUnsafe.SerializeTable(p_Token);
            
#if APPLY_PRINT_LOG
            CustomDebug.LogError(($"{(!gameTableValid ? "Singleton Create Fail" : "Serialize Table Success")}", gameTableValid ? Color.green : Color.red));
#endif
        }

        public static async UniTask RemoveAllByteTable(CancellationToken p_Token)
        {
            var enumerator = EnumFlag.GetEnumEnumerator<TableType>(EnumFlag.GetEnumeratorType.GetAll);
            foreach (var tableType in enumerator)
            {
                var tryFullPath = GetTablePath(ResourceLoadType.FromUnityResource, AssetPathType.ProjectAbsolutePath, tableType, TableDataType.Bytes);
                if (Directory.Exists(tryFullPath))
                {
                    Directory.Delete(tryFullPath, true);
                }
            }
            
            AssetDatabase.Refresh();
            
            // 삭제된 바이트 테이블을 리소스리스트에 등록시켜준다.
            await ResourceListTable.GetInstanceUnsafe.WriteDefaultTable(true, p_Token);
        }
        
        public static async UniTask RemoveAllJsonTable(CancellationToken p_Token)
        {
            var enumerator = EnumFlag.GetEnumEnumerator<TableType>(EnumFlag.GetEnumeratorType.GetAll);
            foreach (var tableType in enumerator)
            {
                var tryFullPath = GetTablePath(ResourceLoadType.FromUnityResource, AssetPathType.ProjectAbsolutePath, tableType, TableDataType.JSON);
                if (Directory.Exists(tryFullPath))
                {
                    Directory.Delete(tryFullPath, true);
                }
            }
            
            AssetDatabase.Refresh();
            
            // 삭제된 바이트 테이블을 리소스리스트에 등록시켜준다.
            await ResourceListTable.GetInstanceUnsafe.WriteDefaultTable(true, p_Token);
        }
#endif

        #endregion

        #region <Struct>

        /// <summary>
        /// 문서로부터 메모리로 로드된 디코드 되기전 테이블 데이터
        /// </summary>
        [Serializable]
        public struct TableLexicalData<Key>
        {
            #region <Fields>

            /// <summary>
            /// 테이블 클래스의 필드. 테이블을 기술하는 메타데이터
            /// </summary>
            public Dictionary<string, string> TableMetaLexicalData;
            
            /// <summary>
            /// 테이블 클래스 내부에 포함되어 있는 레코드 클래스
            /// </summary>
            public Dictionary<Key, Dictionary<string, string>> TableRecordLexicalData;

            #endregion

            #region <Constructor>

            public TableLexicalData(Dictionary<string, string> p_TableMetaLexicalData, Dictionary<Key, Dictionary<string, string>> p_TableRecordLexicalData)
            {
                TableMetaLexicalData = p_TableMetaLexicalData;
                TableRecordLexicalData = p_TableRecordLexicalData;
            }

            #endregion
        }

        /// <summary>
        /// 테이블 이미지
        /// </summary>
        [Serializable]
        public struct TableDataImage<Meta, Key, Record> where Meta : TableMetaData
        {
            #region <Fields>

            /// <summary>
            /// 테이블 클래스의 필드. 테이블을 기술하는 메타데이터
            /// </summary>
            public Meta TableMetaData;
            
            /// <summary>
            /// 테이블 클래스 내부에 포함되어 있는 레코드 클래스
            /// </summary>
            public Dictionary<Key, Record> TableRecordData;

            /// <summary>
            /// 유효성 키
            /// </summary>
            public readonly bool ValidFlag;
            
            #endregion

            #region <Constructor>

            public TableDataImage(Meta p_TableMetaData, Dictionary<Key, Record> p_TableRecordData)
            {
                TableMetaData = p_TableMetaData;
                TableRecordData = p_TableRecordData;
                ValidFlag = true;
            }

            #endregion
        }

        #endregion

        #region <Class>
        
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
        public class TableRecordAttribute : Attribute
        {
            #region <Fields>

            public TableRecordAttributeType AttributeType;

            #endregion

            #region <Constructor>

            public TableRecordAttribute(TableRecordAttributeType p_Type)
            {
                AttributeType = p_Type;
            }

            #endregion

            #region <Operator>

            public static implicit operator TableRecordAttribute(TableRecordAttributeType p_Type)
            {
                return new TableRecordAttribute(p_Type);
            }

            #endregion
        }

        #endregion
    }
}