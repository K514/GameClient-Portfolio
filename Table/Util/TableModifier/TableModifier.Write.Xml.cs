#if UNITY_EDITOR

using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml;
using Cysharp.Threading.Tasks;

namespace k514
{
    public partial class TableModifier
    {
        /// <summary>
        /// 지정한 절대경로에 테이블을 xml파일로 변환하여 저장하는 메서드
        /// </summary>
        private async UniTask WriteXmlTable<Meta, Key, Record>(string p_WriteFullPath, TableTool.TableDataImage<Meta, Key, Record> p_TableData, CancellationToken p_Cancellation) 
            where Record : ITableRecord
            where Meta : TableMetaData
        {
            p_Cancellation.ThrowIfCancellationRequested();

            var xmlWriter = XmlWriter.Create(p_WriteFullPath, xmlWriterSettings);
            {
                // xml 시작
                xmlWriter.WriteStartDocument();
                {
                    // xml 루트 태그 : Table
                    xmlWriter.WriteStartElement(TableTool.XmlTableTagName);
                    {
                        // 테이블 메타 데이터
                        var tableMetaData = p_TableData.TableMetaData;
                        if (!ReferenceEquals(null, tableMetaData))
                        {
                            // xml 태그 : Meta   
                            xmlWriter.WriteStartElement(TableTool.XmlMetaTagName);
                            {
                                // 필드 벨류 태그 기술
                                var fieldInfoSet = typeof(Meta).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
                                foreach (var fieldInfo in fieldInfoSet)
                                {
                                    /* SE Cond */
                                    // 1. 프로퍼티 백필드의 경우 외부에 공개할 필요가 없으므로 제외한다.
                                    // 2. 필드 어트리뷰트가 RunTime인 경우, RunTime에서 계산되는 값이므로 제외한다.
                                    if(fieldInfo.Name.Contains(TableTool.BackingFieldRear)
                                       || (fieldInfo.GetCustomAttribute<TableTool.TableRecordAttribute>()?.AttributeType??TableTool.TableRecordAttributeType.None) == TableTool.TableRecordAttributeType.Runtime) continue;
                                    
                                    // xml 태그 : 필드명
                                    xmlWriter.WriteStartElement(fieldInfo.Name);
                                    xmlWriter.WriteString(fieldInfo.GetValue(tableMetaData).EncodeValue(fieldInfo.FieldType));
                                    xmlWriter.WriteEndElement();
                                }
                                
                                // 프로퍼티 벨류 태그 기술
                                var propertyInfoSet = typeof(Meta).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
                                foreach (var propertyInfo in propertyInfoSet)
                                {
                                    /* SE Cond */
                                    // 1. 프로퍼티 어트리뷰트가 RunTime인 경우, RunTime에서 계산되는 값이므로 제외한다.
                                    if((propertyInfo.GetCustomAttribute<TableTool.TableRecordAttribute>()?.AttributeType??TableTool.TableRecordAttributeType.None) == TableTool.TableRecordAttributeType.Runtime) continue;

                                    // xml 태그 : 프로퍼티명
                                    xmlWriter.WriteStartElement(propertyInfo.Name);
                                    xmlWriter.WriteString(propertyInfo.GetValue(tableMetaData).EncodeValue(propertyInfo.PropertyType));
                                    xmlWriter.WriteEndElement();
                                }
                            }
                            xmlWriter.WriteEndElement();
                        }
                    }
                    {
                        // 테이블 레코드 엘리먼트 컬렉션
                        var tableRecordCollection = p_TableData.TableRecordData;
                        if (!ReferenceEquals(null, tableRecordCollection))
                        {
                            foreach (var tableRecordPair in tableRecordCollection)
                            {
                                var tryRecordKey = tableRecordPair.Key;
                                var tryRecordValue = tableRecordPair.Value;

                                // xml 태그 : Record
                                xmlWriter.WriteStartElement(TableTool.XmlRecordTagName);
                                {
                                    // xml 태그 : Key
                                    xmlWriter.WriteStartElement(TableTool.TableKeyTagName);
                                    xmlWriter.WriteString(tryRecordKey.EncodeValue(typeof(Key)));
                                    xmlWriter.WriteEndElement();
                                    
                                    // 필드 벨류 태그 기술
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
                                        
                                        // xml 태그 : 필드명
                                        xmlWriter.WriteStartElement(fieldInfo.Name);
                                        xmlWriter.WriteString(fieldInfo.GetValue(tryRecordValue).EncodeValue(fieldInfo.FieldType));
                                        xmlWriter.WriteEndElement();
                                    }
                                    
                                    // 프로퍼티 벨류 태그 기술
                                    var propertyInfoSet = typeof(Record).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
                                    foreach (var propertyInfo in propertyInfoSet)
                                    {
                                        /* SE Cond */
                                        // 1. 프로퍼티명 KEY의 경우, 위 블록에서 따로 처리하므로 제외한다.
                                        // 2. 프로퍼티 어트리뷰트가 RunTime인 경우, RunTime에서 계산되는 값이므로 제외한다.
                                        if(propertyInfo.Name == TableTool.TableKeyTagName
                                           || (propertyInfo.GetCustomAttribute<TableTool.TableRecordAttribute>()?.AttributeType??TableTool.TableRecordAttributeType.None) == TableTool.TableRecordAttributeType.Runtime) continue;

                                        // xml 태그 : 프로퍼티명
                                        xmlWriter.WriteStartElement(propertyInfo.Name);
                                        xmlWriter.WriteString(propertyInfo.GetValue(tryRecordValue).EncodeValue(propertyInfo.PropertyType));
                                        xmlWriter.WriteEndElement();
                                    }
                                }
                                xmlWriter.WriteEndElement();
                            }
                        }
                    }
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndDocument();
            }
            xmlWriter.Close();
        }

        /// <summary>
        /// xml데이터로 변환하여 리턴하는 메서드
        /// </summary>
        private async UniTask<string> WriteXmlTable<Meta, Key, Record>(TableTool.TableDataImage<Meta, Key, Record> p_TableData, CancellationToken p_Cancellation)
            where Record : ITableRecord
            where Meta : TableMetaData
        {
            p_Cancellation.ThrowIfCancellationRequested();

            StringBuilder stringBuilder = new StringBuilder();
            var xmlWriter = XmlWriter.Create(stringBuilder, xmlWriterSettings);
            {
                // xml 시작
                xmlWriter.WriteStartDocument();
                {
                    // xml 루트 태그 : Table
                    xmlWriter.WriteStartElement(TableTool.XmlTableTagName);
                    {
                        // 테이블 메타 데이터
                        var tableMetaData = p_TableData.TableMetaData;
                        if (!ReferenceEquals(null, tableMetaData))
                        {
                            // xml 태그 : Meta   
                            xmlWriter.WriteStartElement(TableTool.XmlMetaTagName);
                            {
                                // 필드 벨류 태그 기술
                                var fieldInfoSet = typeof(Meta).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
                                foreach (var fieldInfo in fieldInfoSet)
                                {
                                    /* SE Cond */
                                    // 1. 프로퍼티 백필드의 경우 외부에 공개할 필요가 없으므로 제외한다.
                                    // 2. 필드 어트리뷰트가 RunTime인 경우, RunTime에서 계산되는 값이므로 제외한다.
                                    if (fieldInfo.Name.Contains(TableTool.BackingFieldRear)
                                       || (fieldInfo.GetCustomAttribute<TableTool.TableRecordAttribute>()?.AttributeType ?? TableTool.TableRecordAttributeType.None) == TableTool.TableRecordAttributeType.Runtime) continue;

                                    // xml 태그 : 필드명
                                    xmlWriter.WriteStartElement(fieldInfo.Name);
                                    xmlWriter.WriteString(fieldInfo.GetValue(tableMetaData).EncodeValue(fieldInfo.FieldType));
                                    xmlWriter.WriteEndElement();
                                }

                                // 프로퍼티 벨류 태그 기술
                                var propertyInfoSet = typeof(Meta).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
                                foreach (var propertyInfo in propertyInfoSet)
                                {
                                    /* SE Cond */
                                    // 1. 프로퍼티 어트리뷰트가 RunTime인 경우, RunTime에서 계산되는 값이므로 제외한다.
                                    if ((propertyInfo.GetCustomAttribute<TableTool.TableRecordAttribute>()?.AttributeType ?? TableTool.TableRecordAttributeType.None) == TableTool.TableRecordAttributeType.Runtime) continue;

                                    // xml 태그 : 프로퍼티명
                                    xmlWriter.WriteStartElement(propertyInfo.Name);
                                    xmlWriter.WriteString(propertyInfo.GetValue(tableMetaData).EncodeValue(propertyInfo.PropertyType));
                                    xmlWriter.WriteEndElement();
                                }
                            }
                            xmlWriter.WriteEndElement();
                        }
                    }
                    {
                        // 테이블 레코드 엘리먼트 컬렉션
                        var tableRecordCollection = p_TableData.TableRecordData;
                        if (!ReferenceEquals(null, tableRecordCollection))
                        {
                            foreach (var tableRecordPair in tableRecordCollection)
                            {
                                var tryRecordKey = tableRecordPair.Key;
                                var tryRecordValue = tableRecordPair.Value;

                                // xml 태그 : Record
                                xmlWriter.WriteStartElement(TableTool.XmlRecordTagName);
                                {
                                    // xml 태그 : Key
                                    xmlWriter.WriteStartElement(TableTool.TableKeyTagName);
                                    xmlWriter.WriteString(tryRecordKey.EncodeValue(typeof(Key)));
                                    xmlWriter.WriteEndElement();

                                    // 필드 벨류 태그 기술
                                    var fieldInfoSet = typeof(Record).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
                                    foreach (var fieldInfo in fieldInfoSet)
                                    {
                                        /* SE Cond */
                                        // 1. 필드명 KEY의 경우, 위 블록에서 따로 처리하므로 제외한다.
                                        // 2. 프로퍼티 백필드의 경우 외부에 공개할 필요가 없으므로 제외한다.
                                        // 3. 필드 어트리뷰트가 RunTime인 경우, RunTime에서 계산되는 값이므로 제외한다.
                                        if (fieldInfo.Name == TableTool.TableKeyTagName
                                           || fieldInfo.Name.Contains(TableTool.BackingFieldRear)
                                           || (fieldInfo.GetCustomAttribute<TableTool.TableRecordAttribute>()?.AttributeType ?? TableTool.TableRecordAttributeType.None) == TableTool.TableRecordAttributeType.Runtime) continue;

                                        // xml 태그 : 필드명
                                        xmlWriter.WriteStartElement(fieldInfo.Name);
                                        xmlWriter.WriteString(fieldInfo.GetValue(tryRecordValue).EncodeValue(fieldInfo.FieldType));
                                        xmlWriter.WriteEndElement();
                                    }

                                    // 프로퍼티 벨류 태그 기술
                                    var propertyInfoSet = typeof(Record).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
                                    foreach (var propertyInfo in propertyInfoSet)
                                    {
                                        /* SE Cond */
                                        // 1. 프로퍼티명 KEY의 경우, 위 블록에서 따로 처리하므로 제외한다.
                                        // 2. 프로퍼티 어트리뷰트가 RunTime인 경우, RunTime에서 계산되는 값이므로 제외한다.
                                        if (propertyInfo.Name == TableTool.TableKeyTagName
                                           || (propertyInfo.GetCustomAttribute<TableTool.TableRecordAttribute>()?.AttributeType ?? TableTool.TableRecordAttributeType.None) == TableTool.TableRecordAttributeType.Runtime) continue;

                                        // xml 태그 : 프로퍼티명
                                        xmlWriter.WriteStartElement(propertyInfo.Name);
                                        xmlWriter.WriteString(propertyInfo.GetValue(tryRecordValue).EncodeValue(propertyInfo.PropertyType));
                                        xmlWriter.WriteEndElement();
                                    }
                                }
                                xmlWriter.WriteEndElement();
                            }
                        }
                    }
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndDocument();
            }
            xmlWriter.Close();

            return stringBuilder.ToString();
        }
    }
}

#endif