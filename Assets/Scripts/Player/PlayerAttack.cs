using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;
    [SerializeField] private AudioClip fireballSound;

    [SerializeField] private Transform swordPoint;
    [SerializeField] private AudioClip swordSound;

    public bool hasFire = false;
    public bool hasSword = false;

    private Animator anim;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (!hasFire && !hasSword){
            return;
        }
        if (Input.GetMouseButton(0) && hasSword && cooldownTimer > attackCooldown && playerMovement.canAttack()
            && Time.timeScale > 0){
            MeleeAttack();
        }
        if (Input.GetMouseButton(0) && hasFire && cooldownTimer > attackCooldown && playerMovement.canAttack()
            && Time.timeScale > 0)
            Attack();

        cooldownTimer += Time.deltaTime;
    }

    private void MeleeAttack(){
        SoundManager.instance.PlaySound(swordSound);
        anim.SetTrigger("meleeattack");
        cooldownTimer = 0;
        swordPoint.GetComponent<SwordSlash>().SlashAttack();


    }

    private void Attack()
    {
        SoundManager.instance.PlaySound(fireballSound);
        anim.SetTrigger("attack");
        cooldownTimer = 0;

        fireballs[FindFireball()].transform.position = firePoint.position;
        fireballs[FindFireball()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }
    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
}