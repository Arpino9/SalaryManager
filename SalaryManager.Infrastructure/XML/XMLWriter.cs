using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using SalaryManager.Domain.Exceptions;

namespace SalaryManager.Infrastructure.XML
{
    /// <summary>
    /// XML書き込み
    /// </summary>
    /// <remarks>
    /// 指定されたパスにXMLを生成し、それぞれのタグに値を書き込む。
    /// コンストラクタ呼び出し時にusingを行うため、Repositoryは不要。
    /// </remarks>
    public sealed class XMLWriter : IDisposable
    {
        /// <summary> シリアライザー </summary>
        private XmlSerializer _xmlSerializer;

        /// <summary> ライター </summary>
        private StreamWriter _writer;

        public XMLWriter(string filePath, Type type)
        {
            try
            {
                _xmlSerializer = new XmlSerializer(type);
                _writer = new StreamWriter(filePath, false, new UTF8Encoding(false));
            }
            catch (Exception ex)
            {
                throw new FileWriterException("XMLファイルの作成に失敗しました。", ex);
            }
        }

        /// <summary>
        /// シリアライズ
        /// </summary>
        /// <param name="source">ソース</param>
        /// <exception cref="FileWriterException">書き込み失敗</exception>
        public void Serialize(object source)
        {
            try
            {
                _xmlSerializer.Serialize(_writer, source);
            }
            catch (Exception ex)
            {
                throw new FileWriterException("XMLファイルの書き込みに失敗しました。", ex);
            }
        }

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            _writer?.Close();
        }
    }
}
