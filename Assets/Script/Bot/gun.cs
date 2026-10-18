using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun : MonoBehaviour
{
    public Transform pointFaer;
   // public BotController3 batRS;
    public float late;
    public ParticleSystem PS;
    public ParticleSystem PS1;
    public bool isray;
    public LayerMask hitmask;
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 100);
        pointFaer.position = hit.point;
        PS1.transform.position = hit.point;
    }

    IEnumerator SpawnEnemies(RaycastHit2D hit)
    {
        while (isray)
        {
            PS.Play();
            Debug.Log("Нанесён урон игроку!");
            CarDamage.Instance.TakeDamage(1f);

            if (hit.collider != null)
            {
               

                PS1.Play();
            }
            yield return new WaitForSeconds(1);


        }
    }
}