using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UmlCreator.Core.Facade;

namespace UmlCreatorSampleFramework
{
    class Program
    {
        static void Main(string[] args)
        {

			try
			{
                Run();
			}
			catch (Exception ex)
			{
                Console.WriteLine($"error occurs: {ex.ToString()}");
			}
        }

        private static void Run()
        {
            var text = File.ReadAllText("test.pu");
            var generator = GeneratorFactory.Create<Bitmap>();

            generator.GenerateClassDiagram(text);
        }
    }
}
