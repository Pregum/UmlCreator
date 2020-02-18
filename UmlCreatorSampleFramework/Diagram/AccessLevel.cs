using System;
using System.Collections.Generic;
using System.Text;

namespace UmlCreator.Core.Diagram
{
    /// <summary>
    /// アクセスレベル
    /// </summary>
    public enum AccessLevel
    {
        /// <summary>
        /// 自クラスのみアクセス可能
        /// </summary>
        Private,
        /// <summary>
        /// 自クラスと派生クラスのみアクセス可能
        /// </summary>
        Protected,
        /// <summary>
        /// 同パッケージのみアクセス可能
        /// </summary>
        Package,
        /// <summary>
        /// 自クラスと同アセンブリ内の派生クラスのみアクセス可能(未使用)
        /// </summary>
        PrivateProtected,
        /// <summary>
        /// 同アセンブリまたは派生クラスのみアクセス可能(未使用)
        /// </summary>
        ProtectedInternal,
        /// <summary>
        /// 同アセンブリのみアクセス可能(未使用)
        /// </summary>
        Internal,
        /// <summary>
        /// 全てのアセンブリからアクセス可能
        /// </summary>
        Public
    }
}
