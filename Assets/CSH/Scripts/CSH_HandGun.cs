using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// ----- Low Poly FPS Pack Free Version -----
public class CSH_HandGun : MonoBehaviour
{
    public HandgunScriptLPFP handgun;

    [Header("UI Weapon Name")]
    [Tooltip("현재 소지하는 무기 이름.")]
    public string weaponName;

    [Header("Ammo")]
    [Tooltip("현재 잔여 탄약")]
    public int currentAmmo;
    [Tooltip("총 장전가능 탄약 수")]
    public int totalAmmo;

    [Header("Text")]
    [Tooltip("UI 텍스트들 가져오기")]
    public Text WeaponNameText;
    public Text currentAmmoText;
    public Text totalAmmoText;

    private void Update()
    {
        // 값 가져오기
        weaponName = handgun.weaponName;
        currentAmmo = handgun.currentAmmo;
        totalAmmo = handgun.ammo;

        // UI에서 가져온 값 보여주기
        WeaponNameText.text = weaponName;
        currentAmmoText.text = currentAmmo + "";
        totalAmmoText.text = totalAmmo + "";
    }
}