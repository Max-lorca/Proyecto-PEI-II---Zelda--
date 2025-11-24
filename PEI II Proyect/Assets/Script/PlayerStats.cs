using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObjects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public int life = 3;
    public int maxLife = 3;
    public float speed = 5f;
    public int sepias = 0;

    public bool haveShield;
    public bool haveSword;

    //level1
    public bool haveSwordKey;
    //level2
    public bool haveHearthKey;
    public bool haveBossKey;

    public void AddSepias(int cant)
    {
        this.sepias += cant;
    }
    public void AddLife(int cant)
    {
        this.life += cant;

        if(this.life > this.maxLife)
        {
            this.life = this.maxLife;
        }
    }
    public void AddMaxLife(int cant)
    {
        this.maxLife += cant;
    }
    public void ResetStats()
    {
        this.maxLife = 3;
        this.life = maxLife;
        this.speed = 5f;
        this.sepias = 0;

        this.haveShield = false;
        this.haveSword = false;

        this.haveSwordKey = false;
        this.haveHearthKey = false;
        this.haveBossKey = false;
    }
}
