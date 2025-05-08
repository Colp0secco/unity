using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpButton : MonoBehaviour
{

    public TMP_Text weaponName;
    public TMP_Text weaponDescription;
    public Image weaponIcon;

    private Weapon assignedWeapon;

    public void AcrivateButton(Weapon weapon)
    {
        weaponName.text = weapon.name;
        weaponDescription.text = weapon.stats[weapon.weaponLevel].description;
        weaponIcon.sprite = weapon.weaponImage;
    }

    public void SelectUpgrade()
    {
        assignedWeapon.LevelUp();
        UiController.Instance.LevelUpPanelClose();
    }
}
