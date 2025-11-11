using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Decoy : MonoBehaviour, IInteractable
{
    [Header("Decoy Settings")]
    [SerializeField] float DecoyDuration = 5f;
    [SerializeField] float decoyCooldown = 50f;
    [SerializeField] float DecoyRadius = 10f;

    [Header("References")]
    [SerializeField] MonsterController MonsterScript;
    [SerializeField] Transform Monsterpos;
    [SerializeField] GameObject DecoyReadyLight, DecoyInActiveLight, DecoyOnIndicator;

    bool DecoyActive;
    bool CanUseDecoy;
    float DistanceFromMonster;
    void Start()
    {
        DecoyActive = false;
        CanUseDecoy = true;
    }
    public void Interact()
    {
        if (DecoyActive == false && CanUseDecoy == true)
        {
            StartCoroutine(DecoyCooldown());
        }
    }
    void Update()
    {
        DistanceFromMonster = Vector3.Distance(Monsterpos.position, this.transform.position);

        if (DecoyActive == true)
        {
            DecoyOnIndicator.SetActive(true);
            if (DistanceFromMonster <= DecoyRadius)
            {
                MonsterScript.targetState = MonsterController.TargetState.Chasing;
                MonsterScript.IgnorePlayer = true;
                MonsterController.target = this.transform.position;
            }
        }

        if (CanUseDecoy)
        {
            DecoyReadyLight.SetActive(true);
            DecoyInActiveLight.SetActive(false);
        }
        else
        {
            DecoyReadyLight.SetActive(false);
            DecoyInActiveLight.SetActive(true);
        }
    }
    void ActivateDecoy()
    {
        DecoyActive = true;
        CanUseDecoy = false;
    }
    void DeactivateDecoy()
    {
        DecoyActive = false;

        DecoyOnIndicator.SetActive(false);

        MonsterScript.targetState = MonsterController.TargetState.Wandering;
        MonsterScript.IgnorePlayer = false;
    }
    IEnumerator DecoyCooldown()
    {
        ActivateDecoy();
        yield return new WaitForSeconds(DecoyDuration);
        DeactivateDecoy();
        yield return new WaitForSeconds(decoyCooldown);
        CanUseDecoy = true;
    }
}
