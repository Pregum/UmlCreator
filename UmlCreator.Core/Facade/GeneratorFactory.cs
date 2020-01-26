using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;
using UmlCreator.Core.Builder;

namespace UmlCreator.Core.Facade
{
    public static class GeneratorFactory
    {
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
