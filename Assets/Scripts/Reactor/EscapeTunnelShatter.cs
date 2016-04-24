using UnityEngine;
using System.Collections;

public class EscapeTunnelShatter : MonoBehaviour
{
    public GameObject shatterInto;
    public int hitPoints = 4;
    BombDetach bd;

    private int hpInternal = 0;
    // Use this for initialization
    void Start()
    {
        hpInternal = hitPoints;
        bd = GameObject.FindGameObjectWithTag("Bomb").GetComponent<BombDetach>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hpInternal <= 0)
        {
            Instantiate(shatterInto, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
    //when the player shoots the thing, reduce HP by 1
    void OnCollisionEnter(Collision collision)
    {
        if (bd.detached == true)
        {
            if (collision.collider.tag == "Player Projectile")
            {
                hpInternal--;
                Destroy(collision.gameObject);
            }
        }
    }
}
