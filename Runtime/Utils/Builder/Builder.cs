using Strangeman.Utils.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Strangeman.Utils.Builder
{
    /// <summary>
    /// Abstract base class for building instances of type B.
    /// Provides mechanisms to register and retrieve member values and manage member information caching.
    /// </summary>
    /// <typeparam name="B">The type of object to build.</typeparam>
    public abstract class Builder<B> : IBuild<B>
    {
        // Cache for storing member information to avoid redundant reflection operations.
        protected readonly static ConcurrentDictionary<string, MemberInfo[]> _memberCache = new();

        // Registry for storing member values.
        protected Dictionary<MemberInfo, object> _memberRegistry = new(new MemberInfoEqualityComparer());

        /// <summary>
        /// Registers a value for a member with the specified name and type.
        /// </summary>
        /// <typeparam name="T">The type of the member value.</typeparam>
        /// <param name="memberName">The name of the member to register.</param>
        /// <param name="value">The value to associate with the member.</param>
        /// <returns>Returns the current builder instance for chaining.</returns>
        /// <exception cref="Exception">Thrown if the member is not found.</exception>
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

        /// <summary>
        /// Retrieves the value of a member with the specified name and type.
        /// </summary>
        /// <typeparam name="T">The type of the member value.</typeparam>
        /// <param name="memberName">The name of the member to retrieve.</param>
        /// <returns>The value associated with the member, or the default value if not found.</returns>
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

        /// <summary>
        /// Retrieves member information from the cache or performs reflection to get it.
        /// </summary>
        /// <param name="memberName">The name of the member to retrieve information for.</param>
        /// <returns>An array of <see cref="MemberInfo"/> corresponding to the member name.</returns>
        protected static MemberInfo[] GetCachedMembers(string memberName)
        {
            if (!_memberCache.TryGetValue(memberName, out var members))
            {
                members = typeof(B).GetMember(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                _memberCache[memberName] = members;
            }

            return members;
        }

        /// <summary>
        /// Abstract method to build and return an instance of type B.
        /// </summary>
        /// <returns>An instance of type B.</returns>
        public abstract B Build();
    }

    /// <summary>
    /// Equality comparer for <see cref="MemberInfo"/> based on metadata token and module.
    /// </summary>
    public class MemberInfoEqualityComparer : IEqualityComparer<MemberInfo>
    {
        /// <summary>
        /// Determines whether two <see cref="MemberInfo"/> instances are equal.
        /// </summary>
        /// <param name="x">The first <see cref="MemberInfo"/> to compare.</param>
        /// <param name="y">The second <see cref="MemberInfo"/> to compare.</param>
        /// <returns>True if the instances are considered equal; otherwise, false.</returns>
        public bool Equals(MemberInfo x, MemberInfo y)
        {
            return x.MetadataToken == y.MetadataToken && x.Module == y.Module;
        }

        /// <summary>
        /// Gets the hash code for a <see cref="MemberInfo"/> instance.
        /// </summary>
        /// <param name="obj">The <see cref="MemberInfo"/> to get the hash code for.</param>
        /// <returns>The hash code for the <see cref="MemberInfo"/>.</returns>
        public int GetHashCode(MemberInfo obj)
        {
            return obj.MetadataToken.GetHashCode() ^ obj.Module.GetHashCode();
        }
    }
}
