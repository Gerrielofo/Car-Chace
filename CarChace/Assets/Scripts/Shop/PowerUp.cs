using UnityEngine;

[CreateAssetMenu(menuName = "PowerUp")]
public class PowerUp : ScriptableObject
{
    public PowerUpType powerUpType;
    public float powerUpDuration;
    public float powerUpAmount;
    public Sprite powerUpImage;
    public float powerUpPrice;

    public enum PowerUpType
    {
        SPEED, SPIKE, HELICOPTER, REINFORCEMENTS
    }
}
