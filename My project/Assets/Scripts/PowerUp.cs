using UnityEngine;
public enum PowerUpType { None, Heal, DoubleDamage, DoubleSpeed, Immunity }
public class PowerUp : MonoBehaviour
{
    public PowerUpType powerUpType;
}
