using StarterAssets;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private ThirdPersonController personController;
    private StarterAssetsInputs _input;
    private Animator _animator;

    private int _animIDAttackCombo;
    private int _animIDIsAttacking;
    private int _animIDHoldHeavy;
    private int _animIDHoldAttack;
    private int _animIDAttackAir;
    private int _animIDAttackHeavy;

    bool hasAnimator;

    int indexCombo = 0;
    int indexHeavy = 0;

    float attackStart = 0;
    float delaycombo = 0.3f;
    float durationAttack = 1f;
    float timeHoldAttackHeavy = 0.5f;

    int maxCombo = 4;
    public static bool isAttacking = false;
    public static bool isAttacAir = false;
    public static bool isAttacHeavy = false;
    bool holdAttack;

    bool IsBaseAttack { get { return _animator.GetCurrentAnimatorStateInfo(0).IsName(StringConfig.rig_Attack_Combo1) ||
                _animator.GetCurrentAnimatorStateInfo(0).IsName(StringConfig.rig_Attack_Combo2) ||
                _animator.GetCurrentAnimatorStateInfo(0).IsName(StringConfig.rig_Attack_Combo3) ||
                _animator.GetCurrentAnimatorStateInfo(0).IsName(StringConfig.rig_Attack_Combo4); } }
    private void Start()
    {
        personController = GetComponent<ThirdPersonController>();
        hasAnimator = TryGetComponent(out _animator);
        _input = GetComponent<StarterAssetsInputs>();


        _animIDAttackCombo = Animator.StringToHash("AttackCombo");
        _animIDIsAttacking = Animator.StringToHash("IsAttacking");
        _animIDHoldHeavy = Animator.StringToHash("HoldHeavy");
        _animIDHoldAttack = Animator.StringToHash("HoldAttack");
        _animIDAttackAir = Animator.StringToHash("AttackAir");
        _animIDAttackHeavy = Animator.StringToHash("AttackHeavy");
    }
    private void Update()
    {
        ResetAttackCombo();
        AttackBehaviour();
    }
    void ResetAttackCombo()
    {
        if (isAttacking)
        {
            if (Time.time - attackStart > durationAttack)
            {
                ResetCombo();
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) || _animator.GetCurrentAnimatorStateInfo(0).IsName("rig_Idle2") || indexCombo > maxCombo)
        {
            if (indexCombo != 0)
                ResetCombo();
        }
    }
    private void AttackBehaviour()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (personController.Grounded)
            {
                if (attackStart + delaycombo < Time.time)
                {
                    attackStart = Time.time;
                    isAttacking = true;
                    indexCombo = 1;
                    if (_animator.GetCurrentAnimatorStateInfo(0).IsName(StringConfig.rig_Attack_Combo1))
                    {
                        indexCombo = 2;
                    }
                    else if (_animator.GetCurrentAnimatorStateInfo(0).IsName(StringConfig.rig_Attack_Combo2))
                    {
                        indexCombo = 3;
                    }
                    else if (_animator.GetCurrentAnimatorStateInfo(0).IsName(StringConfig.rig_Attack_Combo3))
                    {
                        indexCombo = 4;
                    }
                    //else if (_animator.GetCurrentAnimatorStateInfo(0).IsName(StringConfig.rig_Attack_Combo4))
                    //{
                    //    indexCombo = 5;
                    //}
                    if (hasAnimator)
                    {
                        _animator.SetInteger(_animIDAttackCombo, indexCombo);
                        _animator.SetBool(_animIDIsAttacking, isAttacking);
                    }
                }
            }
            else
            {
                _animator.SetTrigger(_animIDAttackAir);
                personController.GetComponent<Rigidbody>().isKinematic = true;
                Invoke("ResetGravity", 1f);
            }
        }
        if (Input.GetMouseButton(0))
        {
            timeHoldAttackHeavy += Time.deltaTime;
            _animator.SetFloat(_animIDHoldHeavy, timeHoldAttackHeavy);
            if (attackStart + 0.5f <= Time.time)
            {
                holdAttack = true;
                _animator.SetBool(_animIDHoldAttack, holdAttack);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (holdAttack == true)
            {
                timeHoldAttackHeavy = 0;
                holdAttack = false;
                _animator.SetBool(_animIDHoldAttack, holdAttack);
            }
        }
    }
    public void ResetCombo()
    {
        indexCombo = 0;
        isAttacking = false;
        _animator.SetInteger(_animIDAttackCombo, indexCombo);
        _animator.SetBool(_animIDIsAttacking, isAttacking);
        Debug.LogError("ResetCombo");
    }
    public void ResetHeavy()
    {
        indexHeavy = 0;
        holdAttack = false;
    }
    public void ResetGravity()
    {
        //personController.Gravity = -15f;
        personController.GetComponent<Rigidbody>().isKinematic = false;
    }

    public enum PlayerState
    {
        Idle,
        Move,
        Jump,
        AttackBase,
        AttackHeavy,
        AttackAir,
    }
}
