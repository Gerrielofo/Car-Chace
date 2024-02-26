using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop Upgrade")]
public class ShopUpgrade : ScriptableObject
{
    public int pointCost;
    public int upgradePrc;

    public UpgradeType upgradeType;

    public enum UpgradeType
    {
        SPEED, DAMAGE, CARMOD
    }
}
