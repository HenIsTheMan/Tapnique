using Genesis.Wisdom;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Genesis.Creation {
    internal sealed class GameButtonLink: MonoBehaviour { //POD
        [field: SerializeField]
        internal Image Img {
            get;
            private set;
        }

        [field: SerializeField]
        internal Rigidbody MyRigidbody {
            get;
            private set;
        }

        [field: SerializeField]
        internal EventTrigger MyEventTrigger {
            get;
            private set;
        }

        [field: SerializeField]
        internal AbstractAnim PtrDownAnim {
            get;
            private set;
        }

        [field: SerializeField]
        internal AbstractAnim PtrUpAnim {
            get;
            private set;
        }
    }
}