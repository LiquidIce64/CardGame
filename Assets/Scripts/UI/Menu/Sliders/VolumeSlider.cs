using UnityEngine;
using Utils;

namespace UI
{
    public class VolumeSlider : BaseSlider
    {
        protected override float GetValue() => UserSettings.Volume;

        protected override void SetValue(float value)
        {
            AudioListener.volume = value;
            UserSettings.Volume = value;
            UserSettings.Save();
        }
    }
}
