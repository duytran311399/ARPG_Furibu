using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using StarterAssets;

public class PlayerCombat : MonoBehaviour
{
    private ThirdPersonController personController;
    public Weapon sword;
    public Transform weaponEquipmentHolder;
    public Transform weaponUnEquipHolder;
    public List<AttackSO> combos;
    StateAttack stateAttack;
    [SerializeField] float lastClickedTime;
    [SerializeField] float lastComboEnd;
    [SerializeField] int comboCounter;
    Animator anim;
    private int attackNorAID;

    private void Start()
    {
        personController = GetComponent<ThirdPersonController>();
        anim = GetComponent<Animator>();
        attackNorAID = Animator.StringToHash("Attack");
    }
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            AttackNormal();
        }
        ExitAttackNormal();
    }
    void AttackNormal()
    {
        float currentState = 0;
        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            currentState = anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        if (Time.time - lastComboEnd > 0.5f && comboCounter < combos.Count)
        {
            CancelInvoke(nameof(EndCombo));
            if (Time.time - lastClickedTime >= currentState * 0.4f)
            {
                SetSwordCombat();
                anim.runtimeAnimatorController = combos[comboCounter].animatorOV;
                anim.Play(attackNorAID, 0, 0);
                ChangeStateAttack(StateAttack.Attack);
                comboCounter++;
                lastClickedTime = Time.time;
                if(comboCounter >= combos.Count)
                {
                    Invoke(nameof(EndCombo), currentState * 0.95f);
                }
            }
        } 
    }
    void ExitAttackNormal()
    {
        if(stateAttack == StateAttack.Attack)
        if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            EndCombo();
            ChangeStateAttack(StateAttack.EndAttack);
            Debug.Log("ExitAttackNormal");
            Invoke(nameof(SetSwordExitAttack), 0.2f);
        }
    }
    void EndCombo()
    {
        comboCounter = 0;
        lastComboEnd = Time.time;
        Debug.LogError("EndCombo");
    }
    void SetSwordExitAttack()
    {
        sword.transform.SetParent(weaponUnEquipHolder);
        sword.transform.localPosition = Vector3.zero;
        sword.transform.localRotation = Quaternion.identity;
    }
    void SetSwordCombat()
    {
        CancelInvoke(nameof(SetSwordExitAttack));
        sword.transform.SetParent(weaponEquipmentHolder);
        sword.transform.localPosition = Vector3.zero;
        sword.transform.localRotation = Quaternion.identity;
    }
    void ChangeStateAttack(StateAttack stateChange)
    {
        stateAttack = stateChange;
    }
    enum StateAttack
    {
        Ready, Start, Attack, EndAttack, ResetAttack
    }
}
