using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObjects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public int life = 3;
    public float speed = 5f;
    public int sepias = 0;

    public void ResetStats()
    {
        this.life = 3;
        this.speed = 5f;
        this.sepias = 0;
    }
}
