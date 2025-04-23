using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Decoy : MonoBehaviour, IInteractable
{
    [Header("Decoy Settings")]
    [SerializeField] float decoyDuration = 5f;
    [SerializeField] float decoyCooldown = 10f;
    [SerializeField] float DecoyRadius = 10f;
    [SerializeField] Transform Monsterpos;

    [SerializeField] bool DecoyActive;
    [SerializeField] bool CanUseDecoy;
    [SerializeField] float decoyTimer;
    float MonsterDistance;
    void Start()
    {
        decoyTimer = decoyDuration;
        DecoyActive = false;
        CanUseDecoy = true;
    }
    public void Interact()
    {
        if (DecoyActive == false && CanUseDecoy == true)
        {
            ActivateDecoy();
        }
    }
    void Update()
    {
        MonsterDistance = Vector3.Distance(Monsterpos.position, this.transform.position);

        if (DecoyActive == true)
        {
            if (MonsterDistance <= DecoyRadius)
            {
                MonsterController.chasing = true;
                MonsterController.wandering = false;
                MonsterController.IgnorePlayer = true;
                MonsterController.target = this.transform.position;
            }

            decoyTimer -= Time.deltaTime;

            if (decoyTimer <= 0f)
            {
                DeactivateDecoy();
            }
        }
    }
    void ActivateDecoy()
    {
        DecoyActive = true;
        CanUseDecoy = false;
    }
    void DeactivateDecoy()
    {
        decoyTimer = decoyDuration;
        DecoyActive = false;
        CanUseDecoy = true;
        MonsterController.wandering = true;
        MonsterController.IgnorePlayer = false;
        StartCoroutine(DecoyCooldown());
    }

    IEnumerator DecoyCooldown()
    {
        yield return new WaitForSeconds(decoyCooldown);
        CanUseDecoy = true;
    }
}
