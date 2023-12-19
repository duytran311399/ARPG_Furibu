using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField] Animator _animator;
    float timeDelayAttack = 0.5f;
    float nextFireTime = 0;
    public static int noOfClicks = 0;
    float maxComboTimeDelay = 1f;
    float lastClickedTime = 0f;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    public void Update()
    {
        if(Time.time > nextFireTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _animator.SetTrigger("Attack");
                OnAttack();
            }
        }
        //Debug.Log(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
    }
    void OnAttack()
    {
        lastClickedTime = Time.time;
        noOfClicks++;
        noOfClicks = Mathf.Clamp(noOfClicks, 0, 4);
        if (noOfClicks == 1)
        {
            _animator.SetBool("rig_Attack_Combo1", true);
        }
        else if (noOfClicks >= 2 && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > timeDelayAttack && _animator.GetCurrentAnimatorStateInfo(0).IsName("rig_Attack_Combo1"))
        {
            _animator.SetBool("rig_Attack_Combo1", false);
            _animator.SetBool("rig_Attack_Combo2", true);
        }
        else if (noOfClicks >= 3 && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > timeDelayAttack && _animator.GetCurrentAnimatorStateInfo(0).IsName("rig_Attack_Combo2"))
        {
            _animator.SetBool("rig_Attack_Combo2", false);
            _animator.SetBool("rig_Attack_Combo3", true);
        }
        else if (noOfClicks >= 4 && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > timeDelayAttack && _animator.GetCurrentAnimatorStateInfo(0).IsName("rig_Attack_Combo3"))
        {
            _animator.SetBool("rig_Attack_Combo3", false);
            _animator.SetBool("rig_Attack_Combo4", true);
        }
    }
}
