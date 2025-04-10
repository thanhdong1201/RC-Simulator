using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Helicopter")]
public class HelicopterSO : ScriptableObject
{
    public string Name;
    public HelicopterType Type;
}
public enum HelicopterType
{
    Toy,            // Trực thăng đồ chơi
    Firefighting,    // Trực thăng chữa cháy
    Combat,          // Trực thăng chiến đấu
    Transport,       // Trực thăng vận chuyển
    Rescue           // Trực thăng cứu hộ
}

