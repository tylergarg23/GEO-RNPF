using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SERFOR.Component.Tools.Cryptography
{
    /// <summary>
    /// Uses to encrypt or decrypt any string by symettric way (AES,RijndaelManaged) .
    /// Limitations:  Encription key genaration hashes are hardcoded in the class.
    /// </summary>
    public class AesBasicCrypter
    {

        private static readonly byte[] _salt = Encoding.ASCII.GetBytes("41fb5b5ae4d57c5ee528adb00e5e8e74");
        private const string ENCRYPT_KEY = "200911381f7899d2482ab61fe8d15684469b17fc690";


        /// <summary>
        /// Método para encriptar cadenas de texto planas.
        /// Puede ser usado múltiples veces si es necesario para mayor seguridad, pero será necesario desencriptar el mismo número de veces.
        /// </summary>
        /// <param name="cadena">Texto que necesita ser encriptado</param>
        /// <returns>Texto encriptado</returns>
        /// <exception cref="System.Exception">Cualquier exception que se origine durante el proceso, será enviada al nivel superior.</exception>
        public static string AesEncrypt(string cadena)
        {
            string output;
            RijndaelManaged aesAlg = null;
            try
            {
                var key = new Rfc2898DeriveBytes(ENCRYPT_KEY, _salt);
                aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var ms = new MemoryStream())
                {
                    ms.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                    ms.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (var sw = new StreamWriter(cs))
                        {
                            sw.Write(cadena);
                        }
                    }
                    output = Convert.ToBase64String(ms.ToArray());
                }
            }
            finally
            {
                if (aesAlg != null)
                {
                    aesAlg.Clear();
                }
            }
            return output;
        }

        /************************************************************************
        Purpose :         This is the method uses to decrypt the encrypted strings.
        Parameters :      cipher_text: decrypted string need to decrypt to plan text
        Returns :         Plan text after decrypted
        Exception :       If any exception while getting values from xml, pass to the upper level
        Algorithm :       C# manage code
        Output :          None
        Notes :           User can encrypt multiple times recursively if more security needs. If
                          that need to decrypt same number of rounds accordingly.
        *************************************************************************/
        public static string aesDecrypt(string cadenaCifrada)
        {
            RijndaelManaged aesAlg = null;
            string plainText;
            try
            {
                var key = new Rfc2898DeriveBytes(ENCRYPT_KEY, _salt);
                byte[] bytes = Convert.FromBase64String(cadenaCifrada);
                using (var ms = new MemoryStream(bytes))
                {
                    aesAlg = new RijndaelManaged();
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                    aesAlg.IV = readByteArray(ms);

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (var sr = new StreamReader(cs))
                            plainText = sr.ReadToEnd();
                    }
                }
            }
            finally
            {
                if (aesAlg != null)
                    aesAlg.Clear();
            }
            return plainText;
        }

        /// <summary>
        /// Use to read first few bytes from cipher string( this helps to generate the same key used in encrypt method again) 
        /// </summary>
        /// <param name="stream">stream that use to read the cipher text bytes from memory</param>
        /// <returns>byte array filled with first few bytes of the cipher string</returns>
        private static byte[] readByteArray(Stream stream)
        {
            var rowLength = new byte[sizeof(int)];
            if (stream.Read(rowLength, 0, rowLength.Length) != rowLength.Length)
            {
                throw new SystemException("encrypted string not contain properly formatted byte array");
            }

            var temp = BitConverter.ToInt32(rowLength, 0);
            var buffer = new byte[temp];

            if (stream.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new SystemException("Did not read encrypted string properly");
            }
            return buffer;
        }
    }
}
