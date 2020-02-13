using System;
using System.Drawing;
using System.IO;
using UmlCreator.Core.Facade;

namespace UmlCreator.Cui
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
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
