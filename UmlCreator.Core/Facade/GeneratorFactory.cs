using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Media.Imaging;
using UmlCreator.Core.Builder;
using UmlCreator.Core.Serializer;

namespace UmlCreator.Core.Facade
{
    /// <summary>
    /// クラス図を作成する為に使用するFactoryクラスです。
    /// </summary>
    public static class GeneratorFactory
    {
        /// <summary>
        /// クラス図を作成する為に必要なインスタンスを生成します。
        /// </summary>
        /// <typeparam name="T"> 出力するファイルの形式 </typeparam>
        /// <returns></returns>
        public static ClassDiagramGenerator<T> Create<T>()
        {
            if (typeof(T) == typeof(string))
            {
                return new ClassDiagramGenerator<T>((IBuilder<T>)new AsciiBuilder(), (ISerializer<T>)new AsciiSerializer());
            }
            else if (typeof(T) == typeof(Bitmap))
            {
                return new ClassDiagramGenerator<T>((IBuilder<T>)new ImageBuilder(), (ISerializer<T>)new ImageSerializer());
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
