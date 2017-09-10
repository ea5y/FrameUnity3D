using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public ETCJoystick Joystick;
    public ETCButton BtnAttack;
    public ETCButton BtnSkill_1;

    public Camera MainCamera;
    private PlayerState _state;

    public static PlayerController Inst;

    private void Start ()
    {
        Inst = this;
        //_state = new PlayerState(GetComponentInChildren<Animator>());
        this.GetAnimator();

        Joystick.onMoveStart.AddListener(OnMoveStart);
        Joystick.onMove.AddListener(OnMove);
        Joystick.onMoveEnd.AddListener(OnMoveEnd);

        Joystick.onTouchStart.AddListener(OnTouchStart);
        Joystick.onTouchUp.AddListener(OnTouchEnd);

        //BtnAttack.onPressed.AddListener(OnClickBtnAttack);
        BtnAttack.onDown.AddListener(OnClickBtnAttack);
        BtnSkill_1.onDown.AddListener(OnClickBtnSkill_1);
	}

    public void GetAnimator()
    {
        _state = new PlayerState(GetComponentInChildren<Animator>());
    }

    private void OnMoveStart()
    {
        if (_state == null)
            return;
        Debug.Log("MoveStart");
        _state.Move(0);
    }

    private void OnMove(Vector2 v)
    {
        Vector4 offset = new Vector4(v.x, 0, v.y, 1);
        transform.LookAt(MainCamera.transform.TransformVector(offset) + transform.position);
        transform.Translate(transform.forward * Time.deltaTime * 0.05f, Space.World);
    }

    private void OnMoveEnd()
    {
        if (_state == null)
            return;
        Debug.Log("MoveEnd");
        _state.Stand();
    }

    private void OnTouchStart()
    {
        //Debug.Log("TouchStart");
    }

    private void OnTouchEnd()
    {
        //Debug.Log("TouchStart");
    }

    private void OnClickBtnAttack()
    {
        _state.Attack();
    }

    private void OnClickBtnSkill_1()
    {
        _state.Skill_1();
    }
}

public class PlayerState
{
    private Animator _animator;

    public PlayerState(Animator animator)
    {
        _animator = animator;
    }

    public void Stop()
    {
        _animator.SetBool("Stand", false);
        _animator.SetBool("Move", false);
        _animator.SetBool("Attack", false);
        _animator.SetBool("Skill_1", false);
    }

    public void Stand()
    {
        Stop();
        _animator.SetBool("Stand", true);
    }

    public void Idle()
    {
        
    }
    
    public void Move(float speed)
    {
        Stop();
        _animator.SetBool("Move", true);
    }

    public void Attack()
    {
        Debug.Log("Attack");
        Stop();
        _animator.SetBool("Attack", true);
        OnMontionCompleted("Attack");
    }

    public void Skill_1()
    {
        Debug.Log("Skill_1");
        Stop();
        _animator.SetBool("Skill_1", true);
        OnMontionCompleted("Skill");
    }

    public void Death()
    {

    }

    public void Damage()
    {

    }

    private void OnMontionCompleted(string montion)
    {
        PlayerController.Inst.StartCoroutine(_OnMontionCompleted(montion));
    }

    private IEnumerator _OnMontionCompleted(string montion)
    {
        yield return new WaitForSeconds(0.3f);

        if(_animator.GetCurrentAnimatorStateInfo(0).IsName(montion))
        {
            var msg = _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            Debug.Log("Cur anime name:" + msg);

            Debug.Log("Wait MontionCompleted...");
            while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.9f)
            {
                if(_animator.IsInTransition(0))
                    break;
                yield return null;
            }

            this.Stand();
        }
    }
}

public class Attack
{
    public Attack(string config)
    {
    }
}

public class Skill_1
{
    public Skill_1(string config)
    {
    }
}
