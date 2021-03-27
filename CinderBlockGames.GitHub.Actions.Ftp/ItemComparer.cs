using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CinderBlockGames.GitHub.Actions.Ftp
{
    internal class ItemComparer : IEqualityComparer<Item>
    {

        public static ItemComparer Default { get; } = new ItemComparer();

        public bool Equals(Item left, Item right)
        {
            return string.Equals(left?.LocalPath, right?.LocalPath, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode([DisallowNull] Item item)
        {
            return item.LocalPath.GetHashCode();
        }

    }
}