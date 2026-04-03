using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514
{
    public partial class TableLoader
    {
        /// <summary>
        /// 지정한 절대경로에 테이블을 xml파일로 변환하여 저장하는 메서드
        /// </summary>
        public async UniTask<TableTool.TableLexicalData<KeyType>> Extract<Meta, KeyType, Record>(TableTool.TableDataImage<Meta, KeyType, Record> p_TableData, CancellationToken p_Cancellation)
            where Record : ITableRecord
            where Meta : TableMetaData
        {
            p_Cancellation.ThrowIfCancellationRequested();

            var tableMetaLexicalData = new Dictionary<string, string>();
            var tableRecordCollectionLexicalData = new Dictionary<KeyType, Dictionary<string, string>>();
            var result = new TableTool.TableLexicalData<KeyType>(tableMetaLexicalData, tableRecordCollectionLexicalData);
            
            // 테이블 메타 데이터
            var tableMetaData = p_TableData.TableMetaData;
            if (!ReferenceEquals(null, tableMetaData))
            {
                // 메타 데이터 필드
                var fieldInfoSet = typeof(Meta).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
                foreach (var fieldInfo in fieldInfoSet)
                {
                    /* SE Cond */
                    // 1. 프로퍼티 백필드의 경우 외부에 공개할 필요가 없으므로 제외한다.
                    // 2. 필드 어트리뷰트가 RunTime인 경우, RunTime에서 계산되는 값이므로 제외한다.
                    if(fieldInfo.Name.Contains(TableTool.BackingFieldRear)
                       || (fieldInfo.GetCustomAttribute<TableTool.TableRecordAttribute>()?.AttributeType??TableTool.TableRecordAttributeType.None) == TableTool.TableRecordAttributeType.Runtime) continue;
                    
                    var fieldName = fieldInfo.Name;
                    var fieldValue = fieldInfo.GetValue(tableMetaData).EncodeValue(fieldInfo.FieldType);
                    tableMetaLexicalData.Add(fieldName, fieldValue);
                }
                
                // 메타 데이터 프로퍼티
                var propertyInfoSet = typeof(Meta).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
                foreach (var propertyInfo in propertyInfoSet)
                {
                    /* SE Cond */
                    // 1. 프로퍼티 어트리뷰트가 RunTime인 경우, RunTime에서 계산되는 값이므로 제외한다.
                    if((propertyInfo.GetCustomAttribute<TableTool.TableRecordAttribute>()?.AttributeType??TableTool.TableRecordAttributeType.None) == TableTool.TableRecordAttributeType.Runtime) continue;

                    var propertyName = propertyInfo.Name;
                    var propertyValue = propertyInfo.GetValue(tableMetaData).EncodeValue(propertyInfo.PropertyType);
                    tableMetaLexicalData.Add(propertyName, propertyValue);
                }
            }
                        
            // 테이블 레코드 엘리먼트 컬렉션
            var tableRecordCollection = p_TableData.TableRecordData;
            if (!ReferenceEquals(null, tableRecordCollection))
            {
                foreach (var tableRecordPair in tableRecordCollection)
                {
                    var tryRecordKey = tableRecordPair.Key;
                    var tryRecordValue = tableRecordPair.Value;
                    var recordLexicalData = new Dictionary<string, string>();
                    tableRecordCollectionLexicalData.Add(tryRecordKey, recordLexicalData);
                    
                    // 레코드 데이터 필드
                    var fieldInfoSet = typeof(Record).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
                    foreach (var fieldInfo in fieldInfoSet)
                    {
                        /* SE Cond */
                        // 1. 필드명 KEY의 경우, 위 블록에서 따로 처리하므로 제외한다.
                        // 2. 프로퍼티 백필드의 경우 외부에 공개할 필요가 없으므로 제외한다.
                        // 3. 필드 어트리뷰트가 RunTime인 경우, RunTime에서 계산되는 값이므로 제외한다.
                        if(fieldInfo.Name == TableTool.TableKeyTagName 
                           || fieldInfo.Name.Contains(TableTool.BackingFieldRear)
                           || (fieldInfo.GetCustomAttribute<TableTool.TableRecordAttribute>()?.AttributeType??TableTool.TableRecordAttributeType.None) == TableTool.TableRecordAttributeType.Runtime) continue;
                            
                        var fieldName = fieldInfo.Name;
                        var fieldValue = fieldInfo.GetValue(tryRecordValue).EncodeValue(fieldInfo.FieldType);
                        recordLexicalData.Add(fieldName, fieldValue);
                    }
                    
                    // 프로퍼티 데이터 필드
                    var propertyInfoSet = typeof(Record).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
                    foreach (var propertyInfo in propertyInfoSet)
                    {
                        /* SE Cond */
                        // 1. 프로퍼티명 KEY의 경우, 위 블록에서 따로 처리하므로 제외한다.
                        // 2. 프로퍼티 어트리뷰트가 RunTime인 경우, RunTime에서 계산되는 값이므로 제외한다.
                        if(propertyInfo.Name == TableTool.TableKeyTagName
                           || (propertyInfo.GetCustomAttribute<TableTool.TableRecordAttribute>()?.AttributeType??TableTool.TableRecordAttributeType.None) == TableTool.TableRecordAttributeType.Runtime) continue;

                        var propertyName = propertyInfo.Name;
                        var propertyValue = propertyInfo.GetValue(tryRecordValue).EncodeValue(propertyInfo.PropertyType);
                        recordLexicalData.Add(propertyName, propertyValue);
                    }
                }
            }

            return result;
        }
    }
}