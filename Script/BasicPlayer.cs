using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class BasicPlayer : MonoBehaviour
{
    public SpriteRenderer sr;
    private Level_9 level9;
    private TaskBar bar;
    public PlayerWeapon weapon;
    public Rigidbody2D rb2D;
    public float Speed;
    public float nowHp;
    public float MaxHp;
    private Vector3 targetPosition;
    private bool IsMoving = false;
    public bool isInvincible = false;
    public void Init(Level_9 level , TaskBar taskbar)
    {
        level9 = level;
        bar = taskbar;
        nowHp = MaxHp;
        bar.taskBarImage.fillAmount = 1f;
    }


    public void FixedUpdate()
    {
        if( nowHp  < MaxHp /2)
        {
            nowHp += Time.fixedDeltaTime * 4;
            bar.taskBarImage.fillAmount = nowHp / MaxHp;
        }
        if(Pointer.current.press.IsPressed() == true)
        {
            IsMoving = true;
            targetPosition = Camera.main.ScreenToWorldPoint((Vector3)Pointer.current.position.ReadValue());
        }

        if (IsMoving == true)
        {
            Vector2 deltaPosition = (targetPosition - transform.position);

            if (deltaPosition.magnitude > 36)
            {
                rb2D.velocity += deltaPosition.normalized * (Speed * 4 * Time.fixedDeltaTime);
                if(rb2D.velocity.magnitude > Speed)
                {
                    rb2D.velocity = rb2D.velocity.normalized * Speed;
                }
            }
            else
            {
                rb2D.velocity = Vector2.zero;
                IsMoving = false;
            }
        }

    }
    public void OnDestroy()
    {
        if(bar != null && bar.taskBarImage != null)
        {
            bar.taskBarImage.fillAmount = 1f;
        }
    }
    public void UpGrade()
    {
        MaxHp += 100;
        nowHp += 100;
        Speed += 50;
        weapon.UpGrade();
    }
    public void GetDamage(float damage, Vector2 force)
    {
        if(isInvincible == false)
        {
            AudioManager.Instance.PlaySfxByName("Hurt");
            isInvincible = true;
            nowHp -= damage;
            bar.taskBarImage.fillAmount = nowHp / MaxHp;
            rb2D.AddForce(force, ForceMode2D.Impulse);
            if (nowHp < 0)
            {
                level9.Restart();
            }
            else
            {
                StartCoroutine(Flash());
            }
        }
    }
    IEnumerator Flash()
    {
        Color color = sr.color;
        for(float time = 0; time < 0.5f; time += Time.deltaTime)
        {
            color.a = 0.5f + 0.5f * Mathf.Cos(time * Mathf.PI * 4);
            sr.color = color;
            yield return null;
        }
        color.a = 1f;
        sr.color = color;
        isInvincible = false;
    }

}
