using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

using UmlCreator.Core.Facade;
using System.Diagnostics;

namespace UmlCreator.Core.Test
{
    public class AsciiDiagramTest
    {
        private ClassDiagramGenerator<string> _classDiagramGenerator;
        public AsciiDiagramTest()
        {
            _classDiagramGenerator = GeneratorFactory.Create<string>();
        }

        [Theory(DisplayName = "クラス図テスト")]
        [InlineData("class Hoge{+Create():void\nDestroy():void}")]
        public void ClassDiagramTest1(string testData)
        {
            // Arrange

            // Act
            _classDiagramGenerator.GenerateClassDiagram(testData);
            var classDiagram = _classDiagramGenerator.OutputDiagram;

            // Assert
            string result = 
@"|--------------------|
| Hoge               |
|--------------------|
|--------------------|
| + Create() : void  |
| ~ Destroy() : void |
|--------------------|
";
            Assert.Equal(result, classDiagram);
        }

        [Theory(DisplayName = "クラス図テスト")]
        [InlineData("class Hoge{-_className:string\n+Create():void\nDestroy():void}")]
        public void ClassDiagramTest2(string testData)
        {
            // Arrange

            // Act
            _classDiagramGenerator.GenerateClassDiagram(testData);
            var classDiagram = _classDiagramGenerator.OutputDiagram;

            // Assert
            string result = 
@"|-----------------------|
| Hoge                  |
|-----------------------|
| - _className : string |
|-----------------------|
| + Create() : void     |
| ~ Destroy() : void    |
|-----------------------|
";
            Assert.Equal(result, classDiagram);
        }
    }
}
