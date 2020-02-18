using System;
using System.Drawing;
using UmlCreator.Core.Param;

namespace UmlCreator.Core.Builder
{
    internal class ImageBuilder : IBuilder<Bitmap>
    {
        /// <summary>
        /// Bitmapでクラス図を生成します。
        /// </summary>
        /// <param name="diagram">根本のノード</param>
        /// <returns>クラス図</returns>
        Bitmap IBuilder<Bitmap>.MakeDiagram(DiagramParam diagram)
        {
            if (diagram is null)
            {
                throw new ArgumentNullException(nameof(diagram));
            }

            throw new NotImplementedException();
        }
    }
}
