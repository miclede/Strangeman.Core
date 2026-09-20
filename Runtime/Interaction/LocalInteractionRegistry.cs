using System.Collections.Generic;
using Strangeman.Utils.Service;
using Strangeman.Utils.Visitor;
using UnityEngine;

namespace Strangeman.Core.Interaction
{
    public class LocalInteractionRegistry : MonoBehaviour
    {
        [SerializeField] private Collider2D _2dInteractionPoint;
        private EntityId _interactionPointID;

        private HashSet<IVisitable> _visitables = new(); //do not add visitables at runtime

        GlobalInteractionRegistry _globalRegistry;

        private void Awake()
        {
            _interactionPointID = _2dInteractionPoint.GetEntityId();

            foreach (var visitable in GetComponents<IVisitable>())
            {
                _visitables.Add(visitable);
            }
        }

        private void Start()
        {
            if (_globalRegistry == null)
            {
                ServiceLocator.Global.GetMonoService(out _globalRegistry);
            }

            _globalRegistry.RegisterInteractable(_interactionPointID, _visitables);
        }

        private void OnDestroy()
        {
            if (_globalRegistry != null)
            {
                _globalRegistry.UnregisterInteractable(_interactionPointID);
            }
        }
    }
}
