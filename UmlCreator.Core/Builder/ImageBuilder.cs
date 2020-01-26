using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;
using UmlCreator.Core.Diagram;

namespace UmlCreator.Core.Builder
{
    internal class ImageBuilder : IBuilder<BitmapImage>
    {
        /// <summary>
        /// BitmapImageでクラス図を生成します。
        /// </summary>
        /// <param name="rootNode">根本のノード</param>
        /// <returns>クラス図</returns>
        public BitmapImage MakeDiagram(IRootNode rootNode)
        {
            throw new NotImplementedException();
        }
    }
}
