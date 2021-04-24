using UnityEngine;

namespace DualSenseSample.UI
{
    public class DemoUI : MonoBehaviour
    {
        public GameObject touchpadPanel;
        public GameObject rumblePanel;
        public GameObject triggerPanel;

        public void ToggleTouchpadPanel()
            => touchpadPanel.SetActive(!touchpadPanel.activeInHierarchy);

        public void ToggleRumblePanel()
            => rumblePanel.SetActive(!rumblePanel.activeInHierarchy);

        public void ToggleTriggerPanel()
            => triggerPanel.SetActive(!triggerPanel.activeInHierarchy);
    }
}
