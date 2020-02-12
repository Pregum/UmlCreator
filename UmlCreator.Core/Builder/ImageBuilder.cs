using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Media.Imaging;
using UmlCreator.Core.Diagram;

namespace UmlCreator.Core.Builder
{
    internal class ImageBuilder : IBuilder<Bitmap>
    {
        /// <summary>
        /// BitmapImageでクラス図を生成します。
        /// </summary>
        /// <param name="rootNode">根本のノード</param>
        /// <returns>クラス図</returns>
        Bitmap IBuilder<Bitmap>.MakeDiagram(IRootNode rootNode)
        {
            throw new NotImplementedException();
        }
    }
}
