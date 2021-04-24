using UniSense;
using UnityEngine;
using DualSenseSample.Inputs;

namespace DualSenseSample.Animations
{
    /// <summary>
    /// Animator controller class for DualSense prefab.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator))]
    public sealed class DualSenseAnimator : AbstractDualSenseBehaviour
    {
        [SerializeField]
        private Animator animator;

        private readonly int isConnectBool = Animator.StringToHash("isConnected");

        private void Reset() => animator = GetComponent<Animator>();

        internal override void OnConnect(DualSenseGamepadHID _) => Connect();

        internal override void OnDisconnect() => Disconnect();

        public void Connect() => SetIsConnected(true);

        public void Disconnect() => SetIsConnected(false);

        public void SetIsConnected(bool isConnected)
            => animator.SetBool(isConnectBool, isConnected);
    }
}