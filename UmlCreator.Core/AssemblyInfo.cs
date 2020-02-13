using System.Windows;

// ref: [.net coreでInternalsVisibleTo属性を設定する方法](https://www.nuits.jp/entry/net-standard-internals-visible-to)
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("UmlCreator.Core.Test")]


[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
                                     //(used if a resource is not found in the page,
                                     // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
                                              //(used if a resource is not found in the page,
                                              // app, or any theme specific resource dictionaries)
)]

