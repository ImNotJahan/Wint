using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private Dropdown fullscreenDropdown = null;
    [SerializeField] private Dropdown qualityPresetDropdown = null;

    public void ChangeFullscreenMode()
    {
        switch (fullscreenDropdown.value)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;

            case 1:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;

            case 2:
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                break;

            case 3:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;

            default:
                Debug.LogError("Incorrect Fullscreen Selected");
                break;
        }
    }

    public void ChangeQualityPreset()
    {
        QualitySettings.SetQualityLevel(qualityPresetDropdown.value);
    }
}
