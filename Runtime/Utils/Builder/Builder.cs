using Strangeman.Utils.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Strangeman.Utils.Builder
{
    public abstract class Builder<B> : IBuild<B>
    {
        protected readonly static ConcurrentDictionary<string, MemberInfo[]> _memberCache = new();
        protected Dictionary<MemberInfo, object> _memberRegistry = new(new MemberInfoEqualityComparer());
        public virtual IBuild<B> With<T>(string memberName, T value)
        {
            var members = GetCachedMembers(memberName);

            var member = members.FirstOrDefault(m => m.GetMemberInfoType() == typeof(T));

            if (member != null)
            {
                _memberRegistry[member] = value;
                return this;
            }

            throw new Exception($"Member '{memberName}' was not found in builder.");
        }

        public virtual T Get<T>(string memberName)
        {
            var memberInfo = GetCachedMembers(memberName);

            var member = memberInfo.FirstOrDefault(m => m.GetMemberInfoType() == typeof(T));

            if (member != null && _memberRegistry.TryGetValue(member, out var value))
            {
                return (T)value;
            }

            return default;
        }

        protected static MemberInfo[] GetCachedMembers(string memberName)
        {
            if (!_memberCache.TryGetValue(memberName, out var members))
            {
                members = typeof(B).GetMember(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                _memberCache[memberName] = members;
            }

            return members;
        }

        public abstract B Build();
    }

    public class MemberInfoEqualityComparer : IEqualityComparer<MemberInfo>
    {
        public bool Equals(MemberInfo x, MemberInfo y)
        {
            return x.MetadataToken == y.MetadataToken && x.Module == y.Module;
        }

        public int GetHashCode(MemberInfo obj)
        {
            return obj.MetadataToken.GetHashCode() ^ obj.Module.GetHashCode();
        }
    }
}