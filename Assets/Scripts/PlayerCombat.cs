using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using StarterAssets;

public class PlayerCombat : MonoBehaviour
{
    private ThirdPersonController personController;
    private StarterAssetsInputs _input;
    public Weapon sword;
    public Transform weaponEquipmentHolder;
    public Transform weaponUnEquipHolder;
   
    [Header("Attack Combo")][Space(5)]
    [SerializeField] static StateCombo stateCombo;
    public List<AttackSO> normalCombos;
    public List<AttackSO> airCombos;
    float lastClickedTime;
    float lastComboEnd;
    int comboCounter;
    [SerializeField] float attackDuration = 0.4f;
    [SerializeField] float durationEndCombo = 0.9f;
    Animator anim;

    public static bool isAttacking { get { return stateCombo == StateCombo.Attack ? true : false; } }
    public static bool isCanMove { get { return stateCombo == StateCombo.Ready ? true : false; } }

    private int attackNorAID;
    private int sprintAID;

    private void Start()
    {
        personController = GetComponent<ThirdPersonController>();
        _input = GetComponent<StarterAssetsInputs>();
        anim = GetComponent<Animator>();
        attackNorAID = Animator.StringToHash("Attack");
    }
    private void Update()
    {
        if (_input.attack)
        {
            AttackNormal();
            _input.attack = false;
        }
        else
            ExitAttackNormal();
        Debug.LogWarning(stateCombo.ToString());
    }
    void AttackNormal()
    {
        if (stateCombo == StateCombo.EndAttack)
            return;
        float currentState = 0;
        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            currentState = anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        if (Time.time - lastComboEnd > attackDuration && comboCounter < normalCombos.Count)
        {
            CancelInvoke(nameof(EndCombo));
            if (Time.time - lastClickedTime >= currentState * attackDuration)
            {
                anim.runtimeAnimatorController = normalCombos[comboCounter].animatorOV;
                anim.Play(attackNorAID, 0, 0);
                if (comboCounter == 0)
                    OnChangeStateCombo(StateCombo.StartAttack);
                OnChangeStateCombo(StateCombo.Attack);
                comboCounter++;
                lastClickedTime = Time.time;
                if(comboCounter >= normalCombos.Count)
                {
                    Invoke(nameof(EndCombo), currentState * durationEndCombo);
                    OnChangeStateCombo(StateCombo.EndAttack);
                }
                //Debug.LogError(comboCounter);
            }
        } 
    }
    void ExitAttackNormal()
    {
        if(stateCombo == StateCombo.Attack)
        if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime > durationEndCombo && anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            EndCombo();
            lastComboEnd = Time.time - attackDuration;
            Debug.LogWarning("ExitAttackNormal");
        }
    }
    void EndCombo()
    {
        comboCounter = 0;
        lastComboEnd = Time.time;
        Debug.LogWarning("EndCombo");
        OnChangeStateCombo(StateCombo.ResetAttack);
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
    void OnChangeStateCombo(StateCombo stateChange)
    {
        switch (stateChange)
        {
            case StateCombo.Ready:
                break;
            case StateCombo.StartAttack:
                SetSwordCombat();
                break;
            case StateCombo.Attack:
                break;
            case StateCombo.EndAttack:
                break;
            case StateCombo.ResetAttack:
                Timer.Schedule(this, 0.01f, ()=> stateCombo = StateCombo.Ready);
                Invoke(nameof(SetSwordExitAttack), 5f);
                break;
        }
        stateCombo = stateChange;
    }
    enum StateCombo
    {
        Ready, StartAttack, Attack, EndAttack, ResetAttack
    }
}
