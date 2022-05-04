using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RSVForTagPrint.Helpers
{
    class Persistence
    {
        public class Container
        {
            /// <summary>
            /// 公開鍵と秘密鍵を作成し、キーコンテナに格納する
            /// </summary>
            /// <param name="containerName">キーコンテナ名</param>
            /// <returns>作成された公開鍵(XML形式)</returns>
            public static string CreatePubKeyIn(string containerName)
            {
                var rsa = NewRSACryptoServiceProvider(containerName);

                //公開鍵をXML形式で取得して返す
                return rsa.ToXmlString(false);
            }

            /// <summary>
            /// キーコンテナに格納された秘密鍵を使って、文字列を復号化する
            /// </summary>
            /// <param name="str">Encryptメソッドにより暗号化された文字列</param>
            /// <param name="containerName">キーコンテナ名</param>
            /// <returns>復号化された文字列</returns>
            public static string Decrypt(string str, string containerName)
            {
                var rsa = NewRSACryptoServiceProvider(containerName);

                //復号化する
                byte[] data = System.Convert.FromBase64String(str);
                byte[] decryptedData = rsa.Decrypt(data, false);
                return System.Text.Encoding.UTF8.GetString(decryptedData);
            }

            /// <summary>
            /// 指定されたキーコンテナを削除する
            /// </summary>
            /// <param name="containerName">キーコンテナ名</param>
            public static void DeleteKeys(string containerName)
            {
                var rsa = NewRSACryptoServiceProvider(containerName);

                //キーコンテナを削除
                rsa.PersistKeyInCsp = false;
                rsa.Clear();
            }

            private static RSACryptoServiceProvider NewRSACryptoServiceProvider(string containerName)
            {
                //CspParametersオブジェクトの作成
                var cp = new CspParameters();
                cp.Flags = CspProviderFlags.UseMachineKeyStore;
                //キーコンテナ名を指定する
                cp.KeyContainerName = containerName;
                //CspParametersを指定してRSACryptoServiceProviderオブジェクトを作成
                return new RSACryptoServiceProvider(cp);
            }
        }

        /// <summary>
        /// 公開鍵と秘密鍵を作成して返す
        /// </summary>
        /// <param name="publicKey">作成された公開鍵(XML形式)</param>
        /// <param name="privateKey">作成された秘密鍵(XML形式)</param>
        public static void CreateKeys(out string publicKey, out string privateKey)
        {
            //RSACryptoServiceProviderオブジェクトの作成
            var rsa = new RSACryptoServiceProvider();

            //公開鍵をXML形式で取得
            publicKey = rsa.ToXmlString(false);
            //秘密鍵をXML形式で取得
            privateKey = rsa.ToXmlString(true);
        }

        /// <summary>
        /// 公開鍵を使って文字列を暗号化する
        /// </summary>
        /// <param name="str">暗号化する文字列</param>
        /// <param name="publicKey">暗号化に使用する公開鍵(XML形式)</param>
        /// <returns>暗号化された文字列</returns>
        public static string Encrypt(string str, string publicKey)
        {
            //RSACryptoServiceProviderオブジェクトの作成
            var rsa = new RSACryptoServiceProvider();

            //公開鍵を指定
            rsa.FromXmlString(publicKey);

            //暗号化する文字列をバイト配列に
            byte[] data = System.Text.Encoding.UTF8.GetBytes(str);
            //暗号化する
            //（XP以降の場合のみ2項目にTrueを指定し、OAEPパディングを使用できる）
            byte[] encryptedData = rsa.Encrypt(data, false);

            //Base64で結果を文字列に変換
            return System.Convert.ToBase64String(encryptedData);
        }

        /// <summary>
        /// 秘密鍵を使って文字列を復号化する
        /// </summary>
        /// <param name="str">Encryptメソッドにより暗号化された文字列</param>
        /// <param name="privateKey">復号化に必要な秘密鍵(XML形式)</param>
        /// <returns>復号化された文字列</returns>
        public static string Decrypt(string str, string privateKey)
        {
            //RSACryptoServiceProviderオブジェクトの作成
            var rsa = new RSACryptoServiceProvider();

            //秘密鍵を指定
            rsa.FromXmlString(privateKey);

            //復号化する文字列をバイト配列に
            byte[] data = System.Convert.FromBase64String(str);
            //復号化する
            byte[] decryptedData = rsa.Decrypt(data, false);

            //結果を文字列に変換
            return System.Text.Encoding.UTF8.GetString(decryptedData);
        }
    }
}
