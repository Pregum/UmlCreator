using Sprache;
using System;
using System.Linq;
using UmlCreator.Core.Diagram;
using UmlCreator.Core.Facade;
using UmlCreator.Core.Param;
using UmlCreator.Core.Parser;
using Xunit;

namespace UmlCreator.Core.Test
{
    public class ParseTest
    {
        private readonly string case1 = @"class Hoge {
}";
        private readonly string case2 = @"class Fuga {
 + test2: string
}";
        private readonly string case4 = @"class Hoge {
# test4 : void
}";
        private readonly string case5 = @"class Hoge {
~test5:float
}";
        private readonly string case6 = @"class Hoge {
-_test6:float
}";
        private readonly string case7 = @"class Hoge {
_test7:float
}";
        private readonly string case8 = @"class Hoge{_test8:float}";

        private ClassDiagramGenerator<string> _classDiagramGenerator;

        public ParseTest()
        {
            _classDiagramGenerator = GeneratorFactory.Create<string>();
        }

        [Theory(DisplayName ="空クラステスト")]
        [InlineData("class Empty{}")]
        public void EmptyClassTest(string testData)
        {
            // Arrange

            // Act
            _classDiagramGenerator.GenerateClassDiagram(testData);

            // Assert
            Assert.Equal("Empty", _classDiagramGenerator.InputDiagram.RootNodes.First().Name);
        }

        [Fact(DisplayName = "クラス名取得テスト")]
        public void ClassNameTest1()
        {
            _classDiagramGenerator.GenerateClassDiagram(case1);
            Assert.Equal("Hoge", _classDiagramGenerator.InputDiagram.RootNodes.First().Name);
            _classDiagramGenerator.GenerateClassDiagram(case2);
            Assert.Equal("Fuga", _classDiagramGenerator.InputDiagram.RootNodes.First().Name);
        }

        [Fact(DisplayName = "アクセス修飾子取得テスト")]
        public void FieldAccessibilityTest1()
        {
            _classDiagramGenerator.GenerateClassDiagram(case2);
            Assert.Equal(AccessLevel.Public, _classDiagramGenerator.InputDiagram.RootNodes.First().DataNodes.First().Accessibility);
        }

        // 書く予定のテストケース
        // 1. 各トークンが離れている場合の解析テスト
        [Fact(DisplayName = "各トークンが離れている場合の解析テスト")]
        public void TokenSeparatedTest1()
        {
            _classDiagramGenerator.GenerateClassDiagram(case4);
            Assert.Equal("Hoge", _classDiagramGenerator.InputDiagram.RootNodes.First().Name);
            var diagram = _classDiagramGenerator.InputDiagram.RootNodes.First().DataNodes.First();
            Assert.Equal(AccessLevel.Protected, diagram.Accessibility);
            Assert.Equal("test4", diagram.Name);
            Assert.Equal("void", diagram.Type);
        }

        // 2. 各トークンがくっついている場合の解析テスト
        [Fact(DisplayName = "各トークンがくっついている場合の解析テスト")]
        public void ChunckTokenTest1()
        {
            _classDiagramGenerator.GenerateClassDiagram(case5);
            var diagram = _classDiagramGenerator.InputDiagram.RootNodes.First().DataNodes.First();
            Assert.Equal(AccessLevel.Package, diagram.Accessibility);
            Assert.Equal("test5", diagram.Name);
            Assert.Equal("float", diagram.Type);
        }

        [Fact(DisplayName = "各トークン+中括弧がくっついている場合の解析テスト")]
        public void ChunckTokenTest2()
        {
            _classDiagramGenerator.GenerateClassDiagram(case8);
            var diagram = _classDiagramGenerator.InputDiagram.RootNodes.First().DataNodes.First();
            Assert.Equal("Hoge", _classDiagramGenerator.InputDiagram.RootNodes.First().Name);
            Assert.Equal(AccessLevel.Package, diagram.Accessibility);
            Assert.Equal("_test8", diagram.Name);
            Assert.Equal("float", diagram.Type);
        }

        // 3. フィールドメンバのテスト
        // 3-1. 名前の先頭が_のテスト
        [Fact(DisplayName = "名前の先頭が_のテスト")]
        public void FieldNameStartWith_Test1()
        {
            _classDiagramGenerator.GenerateClassDiagram(case6);
            var diagram = _classDiagramGenerator.InputDiagram.RootNodes.First().DataNodes.First();
            Assert.Equal(AccessLevel.Private, diagram.Accessibility);
            Assert.Equal("_test6", diagram.Name);
            Assert.Equal("float", diagram.Type);
        }

        // 3-2. アクセス修飾子のテスト
        [Fact(DisplayName = "フィールドメンバのアクセス修飾子省略時のテスト")]
        public void PrivateAccessTest()
        {
            _classDiagramGenerator.GenerateClassDiagram(case7);
            var diagram = _classDiagramGenerator.InputDiagram.RootNodes.First().DataNodes.First();
            Assert.Equal(AccessLevel.Package, diagram.Accessibility);
        }

        [Theory(DisplayName = "メソッド非存在時のHasBehaviorNodesプロパティテスト")]
        [InlineData(@"class Hoge{-_data:float}")]
        public void HasBehaviorNodesTestWhenBehaviorNodeNothing(string testData)
        {
            // Arrange

            // Act
            _classDiagramGenerator.GenerateClassDiagram(testData);

            // Assert
            Assert.False(_classDiagramGenerator.InputDiagram.RootNodes.First().HasBehaviorNodes);
        }

        [Theory(DisplayName = "メソッドのアクセス修飾子省略テスト")]
        [InlineData(@"class Hoge{Test():float}")]
        public void BehaviorAccessModifierOmmitedTest1(string testData)
        {
            // Arrange

            // Act
            _classDiagramGenerator.GenerateClassDiagram(testData);
            var diagram = _classDiagramGenerator.InputDiagram.RootNodes.First().BehaviorNodes.First();

            // Assert
            Assert.Equal(AccessLevel.Package, diagram.Accessibility);
        }

        [Theory(DisplayName = "メソッドのアクセス修飾子テスト(Private)")]
        [InlineData(@"class Hoge{-Test():float}")]
        public void BehaviorAccessModifierPrivateTest1(string testData)
        {
            // Arrange

            // Act
            _classDiagramGenerator.GenerateClassDiagram(testData);
            var diagram = _classDiagramGenerator.InputDiagram.RootNodes.First().BehaviorNodes.First();

            // Assert
            Assert.Equal(AccessLevel.Private, diagram.Accessibility);
        }


        [Theory(DisplayName = "メソッドのアクセス修飾子テスト(Package)")]
        [InlineData(@"class Hoge{~Test():float}")]
        public void BehaviorAccessModifierPackageTest1(string testData)
        {
            // Arrange

            // Act
            _classDiagramGenerator.GenerateClassDiagram(testData);
            var diagram = _classDiagramGenerator.InputDiagram.RootNodes.First().BehaviorNodes.First();

            // Assert
            Assert.Equal(AccessLevel.Package, diagram.Accessibility);
        }


        [Theory(DisplayName = "メソッドのアクセス修飾子テスト(Protected)")]
        [InlineData(@"class Hoge{#Test():float}")]
        public void BehaviorAccessModifierProtectedTest1(string testData)
        {
            // Arrange

            // Act
            _classDiagramGenerator.GenerateClassDiagram(testData);
            var diagram = _classDiagramGenerator.InputDiagram.RootNodes.First().BehaviorNodes.First();

            // Assert
            Assert.Equal(AccessLevel.Protected, diagram.Accessibility);
        }

        [Theory(DisplayName = "メソッドのアクセス修飾子テスト(Public)")]
        [InlineData(@"class Hoge{+Test():float}")]
        public void BehaviorAccessModifierPublicTest1(string testData)
        {
            // Arrange

            // Act
            _classDiagramGenerator.GenerateClassDiagram(testData);
            var diagram = _classDiagramGenerator.InputDiagram.RootNodes.First().BehaviorNodes.First();

            // Assert
            Assert.Equal(AccessLevel.Public, diagram.Accessibility);
        }

        [Theory(DisplayName = "メソッドの名前テスト")]
        [InlineData(@"class Hoge{+Create():void}")]
        public void BehaviorMethodNameMatchTest(string testData)
        {
            // Arrange

            // Act
            _classDiagramGenerator.GenerateClassDiagram(testData);
            var diagram = _classDiagramGenerator.InputDiagram.RootNodes.First().BehaviorNodes.First();

            // Assert
            Assert.Equal("Create", diagram.Name);
            Assert.Equal("Create()", diagram.NameWithArgs);
        }

        [Theory(DisplayName = "メソッドの型テスト")]
        [InlineData(@"class Hoge{+Create():void}")]
        public void BehaviorMethodTypeMatchTest(string testData)
        {
            // Arrange

            // Act
            _classDiagramGenerator.GenerateClassDiagram(testData);
            var diagram = _classDiagramGenerator.InputDiagram.RootNodes.First().BehaviorNodes.First();

            // Assert
            Assert.Equal("void", diagram.Type);
        }

        [Theory(DisplayName = "メソッドの個数確認テスト")]
        [InlineData("class Hoge{+Create():void\nDestroy():void}")]
        public void BehaviorMethodCountTest(string testData)
        {
            // Arrange

            // Act
            _classDiagramGenerator.GenerateClassDiagram(testData);
            var behaviorNodes = _classDiagramGenerator.InputDiagram.RootNodes.First().BehaviorNodes;

            // Assert
            Assert.Equal(2, behaviorNodes.Count);
        }


        [Theory(DisplayName = "メソッドの複数のアクセス修飾子テスト")]
        [InlineData("class Hoge{#Create():void\n-Destroy():void}")]
        public void BehaviorSomeMethodAccessModifierTest(string testData)
        {
            // Arrange

            // Act
            _classDiagramGenerator.GenerateClassDiagram(testData);
            var behaviorNodes = _classDiagramGenerator.InputDiagram.RootNodes.First().BehaviorNodes;

            // Assert
            Assert.Equal(AccessLevel.Protected, behaviorNodes[0].Accessibility);
            Assert.Equal(AccessLevel.Private, behaviorNodes[1].Accessibility);
        }

        [Theory(DisplayName = "メソッドの複数の名前テスト")]
        [InlineData("class Hoge{+Create():void\nDestroy():void}")]
        public void BehaviorSomeMethodNameTest(string testData)
        {
            // Arrange

            // Act
            _classDiagramGenerator.GenerateClassDiagram(testData);
            var behaviorNodes = _classDiagramGenerator.InputDiagram.RootNodes.First().BehaviorNodes;

            // Assert
            Assert.Equal("Create", behaviorNodes[0].Name);
            Assert.Equal("Create()", behaviorNodes[0].NameWithArgs);
            Assert.Equal("Destroy", behaviorNodes[1].Name);
            Assert.Equal("Destroy()", behaviorNodes[1].NameWithArgs);
        }


        [Theory(DisplayName = "メソッドの複数の型テスト")]
        [InlineData("class Hoge{#Create():int\n-Destroy():string}")]
        public void BehaviorSomeMethodTypeTest(string testData)
        {
            // Arrange

            // Act
            _classDiagramGenerator.GenerateClassDiagram(testData);
            var behaviorNodes = _classDiagramGenerator.InputDiagram.RootNodes.First().BehaviorNodes;

            // Assert
            Assert.Equal("int", behaviorNodes[0].Type);
            Assert.Equal("string", behaviorNodes[1].Type);
        }

        [Theory(DisplayName = "パース失敗時テスト")]
        [InlineData("class ff #Create():int}")]
        public void ParseFailureTest(string testData)
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => _classDiagramGenerator.GenerateClassDiagram(testData));
        }

        [Theory(DisplayName = "引数テスト1個")]
        [InlineData("class Hoge{+Build(para: string): void}")]
        public void ArgumentParserTestInBehaviorNode1(string value)
        {
            // Arrange

            // Act
            _classDiagramGenerator.GenerateClassDiagram(value);
            var behaviorNodes = _classDiagramGenerator.InputDiagram.RootNodes.First().BehaviorNodes;

            // Assert
            Assert.Equal("para", behaviorNodes[0].Arguments[0].Name);
            Assert.Equal("string", behaviorNodes[0].Arguments[0].Type);
            Assert.Equal("Build", behaviorNodes[0].Name);
            Assert.Equal("void", behaviorNodes[0].Type);
            Assert.Equal("Build(para : string)", behaviorNodes[0].NameWithArgs);
            Assert.Equal("+ Build(para : string) : void", behaviorNodes[0].FullName);
        }

        [Theory(DisplayName = "引数テスト2個")]
        [InlineData("class Hoge{-First(elem: object): int\n +Build(para:string, time : DateTime): void}")]
        public void ArgumentParserTestInBehaviorNode2(string value)
        {
            // Arrange

            // Act
            _classDiagramGenerator.GenerateClassDiagram(value);
            var behaviorNodes = _classDiagramGenerator.InputDiagram.RootNodes.First().BehaviorNodes;

            // Assert
            Assert.Equal("First", behaviorNodes[0].Name);
            Assert.Equal("First(elem : object)", behaviorNodes[0].NameWithArgs);
            Assert.Equal("- First(elem : object) : int", behaviorNodes[0].FullName);
            Assert.Equal("int", behaviorNodes[0].Type);
            Assert.Equal("elem", behaviorNodes[0].Arguments[0].Name);
            Assert.Equal("object", behaviorNodes[0].Arguments[0].Type);
            Assert.Equal("Build", behaviorNodes[1].Name);
            Assert.Equal("Build(para : string, time : DateTime)", behaviorNodes[1].NameWithArgs);
            Assert.Equal("+ Build(para : string, time : DateTime) : void", behaviorNodes[1].FullName);
            Assert.Equal("void", behaviorNodes[1].Type);
            Assert.Equal("para", behaviorNodes[1].Arguments[0].Name);
            Assert.Equal("string", behaviorNodes[1].Arguments[0].Type);
            Assert.Equal("time", behaviorNodes[1].Arguments[1].Name);
            Assert.Equal("DateTime", behaviorNodes[1].Arguments[1].Type);
        }

        [Theory(DisplayName ="Edgeのノード名とArrowTypeテスト1")]
        [InlineData(@"A --> B")]
        public void EdgeParseTest1(string input)
        {
            // Arrange
            var parser = new InputParser();

            // Act
            var edge = parser.ParseEdge(input);

            // Assert
            Assert.Equal("A", edge.SourceNodeName);
            Assert.Equal("B", edge.TargetNodeName);
            Assert.Equal(ArrowType.Dependency, edge.ForwardArrowType);
            Assert.Equal(ArrowType.None, edge.BackArrowType);
        }

        [Theory(DisplayName ="Edgeのノード名とArrowTypeテスト2")]
        [InlineData(@"A <|.. B")]
        public void EdgeParseTest2(string input)
        {
            // Arrange
            var parser = new InputParser();

            // Act
            var edge = parser.ParseEdge(input);

            // Assert
            Assert.Equal("B", edge.SourceNodeName);
            Assert.Equal("A", edge.TargetNodeName);
            Assert.Equal(ArrowType.Extend, edge.ForwardArrowType);
            Assert.Equal(ArrowType.None, edge.BackArrowType);
        }

        [Theory(DisplayName ="Edgeのノード名とArrowTypeテスト2")]
        [InlineData(@"A <|..> B")]
        public void EdgeParseTest3(string input)
        {
            // Arrange
            var parser = new InputParser();

            // Act

            // Assert
            Assert.Throws<InvalidOperationException>(() => parser.ParseEdge(input));
        }

        [Theory(DisplayName ="Edgeのノード名とArrowTypeテスト3")]
        [InlineData(@"A .. B")]
        public void EdgeParseTest4(string input)
        {
            // Arrange
            var parser = new InputParser();

            // Act
            EdgeNode edge = parser.ParseEdge(input);

            // Assert
            Assert.Equal("A", edge.SourceNodeName);
            Assert.Equal("B", edge.TargetNodeName);
            Assert.Equal(ArrowType.None, edge.ForwardArrowType);
            Assert.Equal(ArrowType.None, edge.BackArrowType);
        }

        [Theory(DisplayName ="DiagramParamのパーステスト1")]
        [InlineData("class ABC { + Name: string\n - Age: int\n + Say(msg: string): void} \n class DEF { + fuga: int} \n ABC --> DEF")]
        public void DiagramParamParseTest1(string input)
        {
            // Arrange
            var parser = new InputParser();

            // Act
            DiagramParam param = parser.ParseDiagrams(input);

            // Assert
            Assert.Equal(2, param.RootNodes.Count);
            Assert.Equal(1, param.Edges.Count);
            Assert.Equal("ABC", param.RootNodes[0].Name);
            Assert.Equal("Say", param.RootNodes[0].BehaviorNodes[0].Name);
            Assert.Equal("DEF", param.Edges[0].TargetNodeName);
        }
    }
}
