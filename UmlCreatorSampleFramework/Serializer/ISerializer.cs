using System;
using System.Collections.Generic;
using System.Text;

namespace UmlCreator.Core.Serializer
{
    /// <summary>
    /// 外部に出力する機能を持ったインタフェースです。
    /// </summary>
    /// <typeparam name="T"> 出力する型です。 </typeparam>
    internal interface ISerializer<T>
    {
        /// <summary>
        /// 外部に出力します。
        /// </summary>
        /// <typeparam name="T"> 出力する型 </typeparam>
        /// <param name="obj"> 出力する元のオブジェクトです。 </param>
        /// <param name="fileInfo"> ファイルです。 </param>
        void Serialize(T obj, System.IO.FileInfo fileInfo);
    }
}
