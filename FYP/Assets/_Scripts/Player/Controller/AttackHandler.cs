using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AttackHandler : MonoBehaviour
{
    [SerializeField] Weapon attackStats;
    [SerializeField] LayerMask hitLayer;
    [SerializeField] Vector3 offset, halfExtents;
    [SerializeField] float attackForce;
    Vector3 yEliminator = new Vector3(0, 0, 0);
    WaitForEndOfFrame nextFrame = new WaitForEndOfFrame();
    Animator anim;
    Collider[] hits;
    [SerializeField] Image[] weaponImages;

    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if (Input.GetKeyDown(KeyCode.Mouse0))
            StartCoroutine(attack());
    }

    IEnumerator attack()
    {

        anim.SetTrigger("Attack");
        print("attacking");


        hits = Physics.OverlapBox(
            transform.position + offset,
            halfExtents,
            new Quaternion(),
            hitLayer);


        foreach (Collider x in hits)
        {
            if (x.tag == "Enemy")
            {
                print(Vector3.Scale(((transform.position + x.transform.position).normalized * attackForce), yEliminator));
                EnemyHandler enemy = x.gameObject.GetComponentInParent<EnemyHandler>();
                StartCoroutine(enemy.takeDamage(
                    attackStats.damage, 
                    Vector3.Scale(((transform.position + x.transform.position).normalized * attackForce), yEliminator),
                    0.5f
                    ));
            } else if (x.tag == "Player")
            {

            }
        }

        yield return nextFrame;

        anim.ResetTrigger("Attack");
    }

    public void flipHitCheckOffset() => offset.x = offset.x * -1;

    public Weapon getAttackStats() => attackStats;

    public void setWeapon(Weapon newWeapon)
    {
        attackStats = newWeapon;
        foreach (Image i in weaponImages)
            i.sprite = attackStats.image;
    }

    public void OnDrawGizmos() => Gizmos.DrawWireCube(
        transform.position + offset,
        new Vector3(halfExtents.x * 2, halfExtents.y * 2, halfExtents.z * 2)
        );


}
