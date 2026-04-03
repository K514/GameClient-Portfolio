#if UNITY_EDITOR

using System;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using xk514;
using Color = UnityEngine.Color;

namespace k514
{
    public partial class TableBase<Table, Meta, Key, Record>
    {
        /// <summary>
        /// 테이블 명이 리소스 리스트에 등록되어 있다면, 해당 위치에
        ///
        /// 그렇지 않다면 테이블 클래스가 지정하는 위치에 테이블을 쓴다.
        /// </summary>
        public async UniTask WriteTableTextFileToAutoPath(DataIOTool.WriteType p_WriteType, CancellationToken p_CancellationToken)
        {
            var tryTableName = GetTableFileName(TableTool.TableNameQueryType.WithTableExtension);
            if (ResourceListTable.GetInstanceUnsafe?.TryGetRecord(tryTableName, out var o_Record) ?? false)
            {
                var rootPath = o_Record.ResourceFullPath.CutStringWithPivot("/", true, false);
                await WriteTableTextFileToAbsolutePath(p_WriteType, rootPath, p_CancellationToken);
            }
            else
            {
                var rootPath = GetTableFileRootPath(ResourceLoadType.FromUnityResource, AssetPathType.ProjectAbsolutePath);
                await WriteTableTextFileToAbsolutePath(p_WriteType, rootPath, p_CancellationToken);
            }
        }

        /// <summary>
        /// 지정한 경로에 현재 테이블을 텍스트 파일로 저장하는 메서드
        /// </summary>
        public async UniTask WriteTableTextFileToAbsolutePath(DataIOTool.WriteType p_WriteType, string p_RootPath, CancellationToken p_CancellationToken)
        {
            var fullPath = await TableModifier.GetInstanceUnsafe.WriteTable(this, p_WriteType, p_RootPath, p_CancellationToken);
            p_CancellationToken.ThrowIfCancellationRequested();
            
            await OnWriteTableTextFile(fullPath, p_CancellationToken);
        }

        /// <summary>
        /// 덮어쓰기 파라미터가 참이거나 혹은 클래스에서 지정한 테이블 파일이 없는 경우에
        /// 클래스에서 지정한 기본 레코드를 테이블에 더하여 초기화 시키는 메서드
        /// </summary>
        public async UniTask WriteDefaultTable(bool p_OverlapLegacyTableFlag, CancellationToken p_CancellationTokenToken)
        {
            if (this.IsUnityResourceTable())
            {
                // 해당 테이블 타입에 맞는 디렉터리가 있는지 체크하고 없다면 생성한다.
                SystemMaintenance.CreateTableDirectory(TableType);
                var fullPath = GetTableFileRootPath(ResourceLoadType.FromUnityResource, AssetPathType.ProjectAbsolutePath) + GetTableFileName(TableTool.TableNameQueryType.WithBranchHeaderMainTableExt);
                var rootPath = fullPath.GetUpperPath();
                try
                {
                    // #SE Condition
                    // 1. 덮어쓰기 플래그가 set인 경우
                    // 2. 지정한 위치에 테이블이 존재하지 않는 경우
                    //
                    if (p_OverlapLegacyTableFlag || !File.Exists(fullPath))
                    {
                        // 테이블 컬렉션을 정리한다.
                        ClearTable(true);
                        
                        // 기본 테이블 레코드 인스턴스로 테이블 컬렉션을 채운다.
                        await InitTableDefaultData(p_CancellationTokenToken);
                        await InitTableWriteData(p_CancellationTokenToken);
                        
                        // 테이블 컬렉션을 테이블 파일로 기술한다.
                        await WriteTableTextFileToAbsolutePath(DataIOTool.WriteType.Overlap, rootPath, p_CancellationTokenToken);
                    }
                }
    #if APPLY_PRINT_LOG
                // 파일 탐색에서 예외가 발생한 경우, 관련 메시지를 출력한다.
                catch (Exception e)
                {
                    if (CustomDebug.CustomDebugLogFlag.PrintTableLog.HasOpen())
                    {
                        CustomDebug.LogError((this, $"{fullPath} 테이블을 생성하던 중 에러가 발생했습니다.", e));
                    }
                }
    #else
                catch
                {
                    // do nothing
                }
    #endif
            }
        }

        /// <summary>
        /// 테이블에 써야할 데이터를 초기화하는 메서드
        /// </summary>
        protected virtual async UniTask InitTableWriteData(CancellationToken p_CancellationTokenToken)
        {
            await UniTask.CompletedTask;
        }
    }
}

#endif