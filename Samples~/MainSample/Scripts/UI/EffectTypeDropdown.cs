using UnityEngine;

namespace DualSenseSample.UI
{
    public class EffectTypeDropdown : MonoBehaviour
    {
        public GameObject[] options;

        public void SetOption(int index)
        {
            foreach (var option in options)
            {
                option.SetActive(false);
            }

            options[index].SetActive(true);
        }
    }
}
