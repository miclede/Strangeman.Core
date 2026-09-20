using System;
using System.Collections.Generic;
using Strangeman.Utils.Service;
using Strangeman.Utils.Visitor;
using UnityEngine;

namespace Strangeman.Core.Interaction
{
    public class GlobalInteractionRegistry : GlobalMonoService<GlobalInteractionRegistry>
    {
        private readonly Dictionary<EntityId, Dictionary<Type, IVisitable>> _registeredVisitables = new();

        public void RegisterInteractable(EntityId id, HashSet<IVisitable> visitables)
        {
            var typeMapping = new Dictionary<Type, IVisitable>();

            foreach (var tm in visitables)
            {
                var type = tm.GetType();

                if (!typeMapping.ContainsKey(type)) typeMapping[type] = tm;
            }

            _registeredVisitables[id] = typeMapping;
        }

        public void UnregisterInteractable(EntityId id) => _registeredVisitables.Remove(id);

        public bool TryGetVisitable<TVisitable>(EntityId targetID, out TVisitable visitable) where TVisitable : class, IVisitable
        {
            visitable = null;

            if (!_registeredVisitables.TryGetValue(targetID, out var visitables)) return false;

            if (visitables.TryGetValue(typeof(TVisitable), out var v))
            {
                visitable = (TVisitable)v;
                return visitable != null; ;
            }

            return false;
        }
    }
}