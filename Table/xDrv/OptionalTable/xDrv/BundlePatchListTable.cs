using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514
{
    /// <summary>
    /// 에셋 번들 생성 시, 해당 번들의 정보를 기록하는 테이블 클래스
    /// </summary>
    public class BundlePatchListTable : OptionalTable<BundlePatchListTable, BundlePatchListTable.BundlePatchMetaData, string, BundlePatchListTable.TableRecord>
    {
        #region <Consts>
        
        /// <summary>
        /// 서버 인증용 공개키
        /// </summary>
        private const string __PEM
            = "-----BEGIN PUBLIC KEY-----\n" +
              "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAsTtwXmynyiImxMa1qP4+\n" +
              "E9KikeVqeU9lgpSRdHuTZydHYWdNO/aa0qyLj7v9M4V8fYjAj+GUCROadl2EXNef\n" +
              "ngCV5MH2gfpa+MLJdYw6s/DAKNOnC4a9wdpyijLDVu9niKfIXjQHW6RSvDQVRem4\n" +
              "0OYnFjQVIsI2GKwwHcWoL1F0hx7g9et+NTF4GXvGyxdwO4E86nCEElFo5g7/F23O\n" +
              "9kX7MlM9Hs+F7MWkwjAdwRBM+NWSK5x1ZsxgHO/qQ/L0oH4idzQX698vV/AhJ2Z3\n" +
              "k99NzA5FnrLpW46VVULWo5r88aFiqw1SzUmcmxwJaOGovHsO0TDr1tAIYPDaK8Tb\n" +
              "vwIDAQAB\n" +
              "-----END PUBLIC KEY-----";

        #endregion
        
        #region <Meta>

        /// <summary>
        /// 에셋 번들의 생성 정보를 기록하는 테이블 메타 클래스
        /// </summary>
        public class BundlePatchMetaData : TableMetaData
        {
            #region <Fields>

            public int BundleVersion;
            public string PatchDescription;
            public string VersionDate;
            public string Hash;
            public string Signature;

            #endregion
        }

        #endregion

        #region <Record>

        public class TableRecord : OptionalTableRecord
        {
            public int LatestVersion;
            public long CRC;
            public string Hash;

            public override async UniTask SetRecord(string p_Key, object[] p_RecordField, CancellationToken p_CancellationToken)
            {
                await base.SetRecord(p_Key, p_RecordField, p_CancellationToken);

                LatestVersion = (int)p_RecordField.GetElementSafe(0);
                CRC = (long)p_RecordField.GetElementSafe(1);
                Hash = (string)p_RecordField.GetElementSafe(2);
            }
        }

        #endregion

        #region <Methods>

        public bool VerifySignature()
        {
            /*Debug.LogError(__PEM);
            Debug.LogError(_MetaData.Hash);
            Debug.LogError(_MetaData.Signature);*/
            
            return RsaVerify.VerifyHashHexWithPemPublicKey(__PEM, _MetaData.Hash, _MetaData.Signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }

        #endregion
    }
}