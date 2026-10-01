using System;
using System.Collections.Generic;
using Strangeman.Utils.Service;
using Strangeman.Utils.Visitor;
using UnityEngine;

namespace Strangeman.Utils.Interaction
{
    //Generic handles the heavy lifting in this pattern
    public abstract class InteractionRegistry<T> : MonoBehaviour where T : UnityEngine.Object
    {
        [SerializeField] protected T InteractionPoint;
        
        protected EntityId InteractionPointID;
        
        HashSet<IVisitable> _visitables = new(); //do not add visitables at runtime
        GlobalInteractionRegistry _globalRegistry;

        protected virtual void SetInteractionID() => InteractionPointID = InteractionPoint.GetEntityId();

        protected void Awake()
        {
            SetInteractionID();
            
            foreach (var visitable in GetComponents<IVisitable>())
            {
                _visitables.Add(visitable);
            }
        }

        protected virtual void Start()
        {
            if (_globalRegistry == null)
            {
                ServiceLocator.Global.GetMonoService(out _globalRegistry);
            }
            
            _globalRegistry.RegisterInteractable(InteractionPointID, _visitables);
        }

        protected virtual void OnDestroy()
        {
            if (_globalRegistry != null)
            {
                _globalRegistry.UnregisterInteractable(InteractionPointID);
            }
        }
    }
}