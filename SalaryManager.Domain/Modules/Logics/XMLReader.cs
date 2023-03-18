using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using SalaryManager.Domain.Exceptions;

namespace SalaryManager.Domain.Modules.Logics
{
    public sealed class XMLReader : IDisposable
    {
        /// <summary> シリアライザー </summary>
        private XmlSerializer _xmlSerializer;

        /// <summary> ライター </summary>
        private StreamReader _reader;

        public XMLReader(string filePath, Type type)
        {
            try
            {
                if (File.Exists(filePath)) 
                {
                    _xmlSerializer = new XmlSerializer(type);
                    _reader        = new StreamReader(filePath, new UTF8Encoding(false));
                }
            }
            catch (Exception ex)
            {
                throw new FileWriterException("XMLファイルの作成に失敗しました。", ex);
            }
        }

        /// <summary>
        /// デシリアライズ
        /// </summary>
        /// <exception cref="FileWriterException">書き込み失敗</exception>
        public object Deserialize()
        {
            try
            {
                return _xmlSerializer?.Deserialize(_reader);
            }
            catch (Exception ex)
            {
                throw new FileReaderException("XMLファイルの書き込みに失敗しました。", ex);
            }
        }

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            _reader?.Close();
        }
    }
}
