#pragma warning disable 414
using UnityEngine;

namespace BardicBytes.TemplateNamespace
    {
    public class Template_NewBehaviourScript : MonoBehaviour
    {
        /*
         * Data Members
         */

        // Serialized Data Members for the Unity Inspector

        // for inspector fields that need serialization and a public accessor.
        [field: SerializeField] public int ExampleSerializedAutoProperty { get; private set; } = 1;

        // for values that need serialization but no accessor
        [SerializeField] public int exampleSerializedField = 0;

        public string ExampleStringAccessor => "example";

        // private data members
        private float examplePrivateFloatField = 0;
        private bool examplePrivateBoolField = true;

        /*
         * Public Methods
         */

        // Add public methods

        private void Initialize()
        {
        }

        private void Deinitialize()
        {
        }

        /*
         * Unity Message Handlers
         */
        private void OnValidate()
        {
            // Don't go overboard
        }

        private void Awake()
        {
            // Do as much initialization here as possible.
            // -cache references
            // start async operations
        }

        private void OnEnable()
        {
            //subscribe to events that should be subscribed to while the object is enabled.
        }

        private void OnDisable()
        {
            // Remove Event histeners and handlers that were added EnEnable
        }

        private void Start()
        {
            // Initialize things that couldn't be initialized in Awake

        }

        private void Update()
        {
        }

        private void LateUpdate()
        {
        }

        private void FixedUpdate()
        {
        }

        /*
         * Private Methods, Non-Unity Messages
         */

        
    }
}
#pragma warning restore 414