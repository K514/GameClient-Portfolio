using System;
using System.IO;
using UnityEngine;

namespace k514
{
    public partial class TableBase<Table, Meta, Key, Record>
    {
        #region <Methods>

        /// <summary>
        /// 지정한 절대 경로에 해당 테이블 클래스의 xml 파일이 존재하는지 검증하는 메서드
        /// </summary>
        public bool HasTableCollectionFromAbsolutePath(string p_TableAbsolutePath)
        {
            return Directory.Exists(p_TableAbsolutePath) && File.Exists(p_TableAbsolutePath + GetTableFileName(TableTool.TableNameQueryType.WithBranchHeaderMainTableExt));
        }

        /// <summary>
        /// 해당 테이블이 참조하는 문서의 문서타입별 기본 루트 패스를 리턴하는 메서드
        /// </summary>
        private string GetTableFileRootPath(ResourceLoadType p_ResourceLoadType, AssetPathType p_PathType, TableTool.TableDataType p_TableDataType)
        {
            return TableTool.GetTablePath(p_ResourceLoadType, p_PathType, TableType, p_TableDataType);
        }

        /// <summary>
        /// 해당 테이블이 참조하는 문서의 기본 루트 패스를 리턴하는 메서드
        /// </summary>
        public string GetTableFileRootPath(ResourceLoadType p_ResourceLoadType, AssetPathType p_PathType)
        {
            var tableType = GetTableFileType();
            switch (tableType)
            {
                case TableTool.TableFileType.Xml:
                    return GetTableFileRootPath(p_ResourceLoadType, p_PathType, TableTool.TableDataType.Xml);
                case TableTool.TableFileType.JSON:
                    return GetTableFileRootPath(p_ResourceLoadType, p_PathType, TableTool.TableDataType.JSON);
                case TableTool.TableFileType.CSV:
                    return GetTableFileRootPath(p_ResourceLoadType, p_PathType, TableTool.TableDataType.CSV);
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// 해당 테이블이 참조하는 문서의 절대경로를 리턴하는 메서드
        /// </summary>
        public string GetTableFileFullPath(ResourceLoadType p_ResourceLoadType, AssetPathType p_PathType, TableTool.TableNameQueryType p_NameType)
        {
            return GetTableFileRootPath(p_ResourceLoadType, p_PathType) + GetTableFileName(p_NameType);
        }

        /// <summary>
        /// 바이트 에셋의 풀패스를 리턴하는 메서드
        /// </summary>
        public string GetByteTableFullPath()
        {
            return $"{GetTableFileRootPath(ResourceLoadType.FromUnityResource, AssetPathType.ProjectAbsolutePath, TableTool.TableDataType.Bytes)}{GetTableFileName(TableTool.TableNameQueryType.WithMainByteExt)}";
        }
        
        /// <summary>
        /// 바이트 에셋의 상대경로를 리턴하는 메서드. Assets 헤더를 포함하며 확장자를 포함하지 않는다.
        /// </summary>
        public string GetByteTableRelativePath()
        {
            return $"{GetTableFileRootPath(ResourceLoadType.FromUnityResource, AssetPathType.RelativePath, TableTool.TableDataType.Bytes)}{GetTableFileName(TableTool.TableNameQueryType.WithMainTableName)}";
        }
        
        #endregion
    }
}