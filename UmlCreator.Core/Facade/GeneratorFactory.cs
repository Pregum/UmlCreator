using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;
using UmlCreator.Core.Builder;

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
                return new ClassDiagramGenerator<T>((IBuilder<T>)new AsciiBuilder());
            }
            else if (typeof(T) == typeof(BitmapImage))
            {
                return new ClassDiagramGenerator<T>((IBuilder<T>)new ImageBuilder());
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
