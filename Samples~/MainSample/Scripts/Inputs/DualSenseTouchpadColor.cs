using UnityEngine;

namespace DualSenseSample.Inputs
{
    /// <summary>
    /// Component to set the DualSense Touchpad light bar color.
    /// <para>Set the <see cref="LightBarColor"/> Property to see the change.</para>
    /// </summary>
    public class DualSenseTouchpadColor : AbstractDualSenseBehaviour
    {
        public Color LightBarColor
        {
            get => lightBarColor;
            set
            {
                lightBarColor = value;
                DualSense?.SetLightBarColor(lightBarColor);
            }
        }

        private Color lightBarColor;

        public void UpdateRedColor(float red)
        {
            lightBarColor.r = red;
            LightBarColor = lightBarColor;
        }

        public void UpdateGreenColor(float green)
        {
            lightBarColor.g = green;
            LightBarColor = lightBarColor;
        }

        public void UpdateBlueColor(float blue)
        {
            lightBarColor.b = blue;
            LightBarColor = lightBarColor;
        }
    }
}
