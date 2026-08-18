using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SHG.AnimatorCoder;
using Unity.Burst;

public class PlayerAnims : AnimatorCoder
{
    [SerializeField] private float movementSpeed;
    public static PlayerAnims instance;
    private SpriteRenderer sprite;
    private CharStats cStats;
    private BoxCollider2D pCollider;
    public static int winner;
    public bool flipAnims = false;  //p2's anims will be flipped, defined by LoadArena.cs
    private CooldownBarUpdate cooldownScript;

    private void Awake()
    {
        instance = this;
        cooldownScript = GameObject.Find("CanvasController").GetComponent<CooldownBarUpdate>();
        winner = 0; //gets set by HealthBarUpdate.cs to 1 or 2
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        sprite = GetComponent<SpriteRenderer>();
        pCollider = GetComponent<BoxCollider2D>();
        cStats = GetComponent<CharStats>();
        if (flipAnims) { sprite.flipX = true; }
    }

    // Update is called once per frame
    void Update()
    {
        if (winner == 0)
        {
            CheckSpcAttack();
            CheckStdAttack();
            CheckDefMove();
            CheckJump();
            DefaultAnimation(0);
        }
        else
        {
            if (!flipAnims)
            {
                //p1 wins and this is p1
                if (winner == 1) { Play(new(Animations.Win, true)); }
                //p2 wins and this is p1
                else if (winner == 2) { Play(new(Animations.Lose, true)); }
            }
            else
            {
                //p1 wins and this is p2
                if (winner == 1) { Play(new(Animations.Lose, true)); }
                //p2 wins and this is p2
                else if (winner == 2) { Play(new(Animations.Win, true)); }
            }
            print(cStats.charName + " " + cStats.cooldown + " / " + cStats.maxCooldown);
        }

        void CheckSpcAttack()
        {
            if (cStats.cooldown != cStats.maxCooldown) return;
            if (!flipAnims && !Input.GetKeyDown(KeyCode.E)) return;
            if (flipAnims && !Input.GetKeyDown(KeyCode.Return)) return;

            Play(new(Animations.Spc_Attack, true, new()));
            cooldownScript.UseCharge(flipAnims ? 2 : 1);
        }
        
        void CheckStdAttack()
        {
            if (!flipAnims && !Input.GetKeyDown(KeyCode.Q)) return;
            if (flipAnims && !Input.GetKeyDown(KeyCode.RightShift)) return;

            Play(new(Animations.Std_Attack, true, new()));
        }

        void CheckDefMove()
        {
            if (cStats.cooldown != cStats.maxCooldown) return;
            if (!flipAnims && !Input.GetKeyDown(KeyCode.Tab)) return;
            if (flipAnims && !Input.GetKeyDown(KeyCode.Slash)) return;

            Play(new(Animations.Def_Move, true, new()));
            cooldownScript.UseCharge(flipAnims ? 2 : 1);
        }

        void CheckJump()
        {
            if (GetBool(Parameters.GROUNDED))
            {
                if (!flipAnims && Input.GetKeyDown(KeyCode.W)) { Play(new(Animations.Jump, true, new())); }
                else if (flipAnims && Input.GetKeyDown(KeyCode.UpArrow)) { Play(new(Animations.Jump, true, new())); }
                
            }
        }
    }

    private void FixedUpdate()
    {
        SetBool(Parameters.GROUNDED, Physics2D.BoxCast(pCollider.bounds.center, pCollider.bounds.size, 0f, Vector2.down, 0.5f));
    }

    //basic logic for animations
    public override void DefaultAnimation(int layer)
    {
        //forward
        if (!flipAnims && Input.GetKey(KeyCode.D)) { Play(new(Animations.Forward)); }
        else if (flipAnims && Input.GetKey(KeyCode.LeftArrow)) { Play(new(Animations.Forward)); }
        
        //backward
        else if (!flipAnims && Input.GetKey(KeyCode.A)) { Play(new(Animations.Backward)); }
        else if (flipAnims && Input.GetKey(KeyCode.RightArrow)) { Play(new(Animations.Backward)); }

        //idle
        else { Play(new(Animations.Idle)); }
    }
}
