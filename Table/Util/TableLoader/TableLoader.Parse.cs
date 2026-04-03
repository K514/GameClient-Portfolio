using System;
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
        /// 테이블 어휘 데이터를 테이블 메타 데이터 및 테이블 레코드 컬렉션으로 파싱하여 테이블 인스턴스에 초기화시키는 메서드
        /// </summary>
        public async UniTask<TableTool.TableDataImage<Meta, Key, Record>> ParseTableLexicalData<Meta, Key, Record>(TableTool.TableLexicalData<Key> p_TableData, CancellationToken p_Cancellation)
            where Meta : TableMetaData
            where Record : class, ITableRecord, new()
        {
            var metaLexData = p_TableData.TableMetaLexicalData;
            var metaSet = await ParseTableMetaLexicalData<Meta>(metaLexData, p_Cancellation);
            p_Cancellation.ThrowIfCancellationRequested();
   
            var recordLexData = p_TableData.TableRecordLexicalData;
            var recordSet = await ParseTableRecordLexicalData<Key, Record>(recordLexData, p_Cancellation);
    
            return new TableTool.TableDataImage<Meta, Key, Record>(metaSet, recordSet);
        }
        
        /// <summary>
        /// 테이블 어휘 데이터를 테이블 메타 데이터로 파싱하여 테이블 인스턴스에 초기화시키는 메서드
        /// </summary>
        public async UniTask<Meta> ParseTableMetaLexicalData<Meta>(Dictionary<string, string> p_MetaData, CancellationToken p_Cancellation)
            where Meta : TableMetaData
        {
            if (p_MetaData.CheckCollectionSafe())
            {
                p_Cancellation.ThrowIfCancellationRequested();
                
                var metaDataType = typeof(Meta);
                var result = (Meta) metaDataType.GetConstructor(Type.EmptyTypes).Invoke(null);
                
                /* 정적 멤버 외의 모든 필드를 가져온다. */
                var fieldInfoGroup = metaDataType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                /* 정적 멤버 외의 모든 프로퍼티를 가져온다. */
                var propertyInfoGroup = metaDataType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        
                /* 테이블 데이터로부터 테이블 데이터 인스턴스에 값을 담는다. */
                /* 필드 서치 */
                for (var i = 0; i < fieldInfoGroup.Length; i++)
                {
                    var tryFieldInfo = fieldInfoGroup[i];
                    var tryFieldName = tryFieldInfo.Name;
                    var tryFieldType = tryFieldInfo.FieldType;
                    var fieldInfo = tryFieldInfo.GetCustomAttribute<TableTool.TableRecordAttribute>();

                    if (ReferenceEquals(null, fieldInfo) && p_MetaData.ContainsKey(tryFieldName))
                    {
                        var tryValue = p_MetaData[tryFieldName];
                        tryFieldInfo.SetValue(result, tryValue.DecodeValue(tryFieldType));
                    }
                }

                /* 프로퍼티 서치 */
                for (var i = 0; i < propertyInfoGroup.Length; i++)
                {
                    var tryPropertyInfo = propertyInfoGroup[i];
                    var tryPropertyName = tryPropertyInfo.Name;
                    var tryPropertyType = tryPropertyInfo.PropertyType;
                    var attributeInfo = tryPropertyInfo.GetCustomAttribute<TableTool.TableRecordAttribute>();

                    if (!ReferenceEquals(null, tryPropertyInfo.SetMethod) 
                        && ReferenceEquals(null, attributeInfo) 
                        && p_MetaData.ContainsKey(tryPropertyName))
                    {
                        var tryValue = p_MetaData[tryPropertyName];
                        tryPropertyInfo.SetValue(result, tryValue.DecodeValue(tryPropertyType));
                    }
                }
                
                return result;
            }
            else
            {
                return default;
            }
        }
                
        /// <summary>
        /// 테이블 어휘 데이터를 테이블 레코드 컬렉션으로 파싱하여 리턴하는 메서드
        /// </summary>
        private async UniTask<Dictionary<Key, Record>> ParseTableRecordLexicalData<Key, Record>(Dictionary<Key, Dictionary<string, string>> p_RecordData, CancellationToken p_Cancellation)
            where Record : class, ITableRecord, new()
        {
            if (p_RecordData.CheckCollectionSafe())
            {
                p_Cancellation.ThrowIfCancellationRequested();

                var tableRecordType = typeof(Record);
                var result = new Dictionary<Key, Record>();
                
                /* 정적 멤버 외의 모든 필드를 가져온다. */
                var fieldInfoGroup = tableRecordType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                /* 정적 멤버 외의 모든 프로퍼티를 가져온다. */
                var propertyInfoGroup = tableRecordType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                foreach (var tryCollectionPair in p_RecordData)
                {
                    /* 테이블 데이터 인스턴스 생성 */
                    var tryKey = tryCollectionPair.Key;
                    var recordData = tryCollectionPair.Value;
                    var tryRecord = new Record();

                    /* 테이블 데이터로부터 테이블 데이터 인스턴스에 값을 담는다. */
                    /* 필드 서치 */
                    for (var i = 0; i < fieldInfoGroup.Length; i++)
                    {
                        var tryFieldInfo = fieldInfoGroup[i];
                        var tryFieldName = tryFieldInfo.Name;
                        var tryFieldType = tryFieldInfo.FieldType;
                        var attributeInfo = tryFieldInfo.GetCustomAttribute<TableTool.TableRecordAttribute>();
          
                        switch (tryFieldName)
                        {
                            /* 필드명이 KEY인 경우, 키값을 세트한다. */
                            case var _ when tryFieldName == TableTool.TableKeyTagName:
                            {
                                tryFieldInfo.SetValue(tryRecord, tryCollectionPair.Key);
                                break;
                            }
                            /* 일치하는 필드명이 있다면 해당 필드에 값을 세트한다. */
                            case var _ when (ReferenceEquals(null, attributeInfo) || attributeInfo.AttributeType != TableTool.TableRecordAttributeType.Runtime) 
                                            && recordData.ContainsKey(tryFieldName):
                            {
                                var tryValue = recordData[tryFieldName];
                                tryFieldInfo.SetValue(tryRecord, tryValue.DecodeValue(tryFieldType));
                                break;
                            }
                        }
                    }

                    /* 프로퍼티 서치 */
                    for (var i = 0; i < propertyInfoGroup.Length; i++)
                    {
                        var tryPropertyInfo = propertyInfoGroup[i];
                        var tryPropertyName = tryPropertyInfo.Name;
                        var tryPropertyType = tryPropertyInfo.PropertyType;
                        var attributeInfo = tryPropertyInfo.GetCustomAttribute<TableTool.TableRecordAttribute>();
                        
                        switch (tryPropertyName)
                        {
                            /* 프로퍼티명이 KEY인 경우, 키값을 세트한다. */
                            case var _ when tryPropertyName == TableTool.TableKeyTagName 
                                            && !ReferenceEquals(null, tryPropertyInfo.SetMethod):
                            {
                                tryPropertyInfo.SetValue(tryRecord, tryCollectionPair.Key);
                                break;
                            }
                            /* 일치하는 프로퍼티명이 있다면 해당 프로퍼티에 값을 세트한다. */
                            case var _ when !ReferenceEquals(null, tryPropertyInfo.SetMethod)
                                            && (ReferenceEquals(null, attributeInfo) || attributeInfo.AttributeType != TableTool.TableRecordAttributeType.Runtime) 
                                            && recordData.ContainsKey(tryPropertyName) :
                            {
                                var tryValue = recordData[tryPropertyName];
                                tryPropertyInfo.SetValue(tryRecord, tryValue.DecodeValue(tryPropertyType));
                                break;
                            }
                        }
                    }

                    if (!result.ContainsKey(tryKey))
                    {
                        await tryRecord.OnRecordDecoded(p_Cancellation);
                        result.Add(tryKey, tryRecord);
                    }
                }

                return result;
            }
            else
            {
                return default;
            }
        } 
    }
}