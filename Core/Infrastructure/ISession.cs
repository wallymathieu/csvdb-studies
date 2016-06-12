using System;
using System.Collections.Generic;

namespace SomeBasicCsvApp.Core.Infrastructure
{
    public interface ISession : IDisposable
    {
        T Get<T>(int key) where T : IIdentifiableByNumber;
        void Save<T>(T value) where T : IIdentifiableByNumber;
        IEnumerable<T> Where<T>(Func<T, bool> predicate) where T : IIdentifiableByNumber;
        bool Any<T>(Func<T, bool> predicate) where T : IIdentifiableByNumber;
        void Commit();
        long Count<T>(Func<T, bool> predicate) where T : IIdentifiableByNumber;
        TRet Max<T, TRet>(Func<T, bool> predicate, Func<T, TRet> map) where T : IIdentifiableByNumber;
        TRet Min<T, TRet>(Func<T, bool> predicate, Func<T, TRet> map) where T : IIdentifiableByNumber;
    }
    public static class SessionExtensions
    {
        private static bool ReturnsTrue<T>(T arg)
        {
            return true;
        }

        public static IEnumerable<T> Where<T>(this ISession session) where T : IIdentifiableByNumber
        {
            return session.Where<T>(ReturnsTrue);
        }

        public static bool Any<T>(this ISession session) where T : IIdentifiableByNumber
        {
            return session.Any<T>(ReturnsTrue);
        }
        public static long Count<T>(this ISession session) where T : IIdentifiableByNumber
        {
            return session.Count<T>(ReturnsTrue);
        }
        public static TRet Max<T, TRet>(this ISession session, Func<T, TRet> map) where T : IIdentifiableByNumber
        {
            return session.Max(ReturnsTrue, map);
        }
        public static TRet Min<T, TRet>(this ISession session, Func<T, TRet> map) where T : IIdentifiableByNumber
        {
            return session.Min(ReturnsTrue, map);
        }

    }
}
