using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Media.Imaging;

namespace UmlCreator.Core.Serializer
{
    internal class ImageSerializer : ISerializer<Bitmap>
    {
        public void Serialize(Bitmap obj, FileInfo fileInfo)
        {
            // BitmapImageをファイルに保存する
            // ref: https://stackoverflow.com/questions/35804375/how-do-i-save-a-bitmapimage-from-memory-into-a-file-in-wpf-c/35804806
            //BitmapEncoder encoder = new PngBitmapEncoder();
            //encoder.Frames.Add(BitmapFrame.Create(obj));
            //using FileStream fs = fileInfo.Create();
            //encoder.Save(fs);


            obj.Save(fileInfo.FullName);
        }
    }
}
