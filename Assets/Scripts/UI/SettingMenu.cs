using UnityEngine;

public class SettingMenu : MonoBehaviour
{
    [SerializeField] private GameObject buttonKeyboard;
    [SerializeField] private GameObject buttonGamepad;
    [SerializeField] private GameObject buttonMisc;

    public void Keyboard()
    {
        buttonGamepad.SetActive(false);
        buttonMisc.SetActive(false);
        buttonKeyboard.SetActive(true);
    }

    public void Gamepad()
    {
        buttonKeyboard.SetActive(false);
        buttonMisc.SetActive(false);
        buttonGamepad.SetActive(true);
    }

    public void Misc()
    {
        buttonKeyboard.SetActive(false);
        buttonGamepad.SetActive(false);
        buttonMisc.SetActive(true);
    }
}
