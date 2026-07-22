using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SHG.AnimatorCoder;
using Unity.Burst;

public class PlayerAnims : AnimatorCoder
{
    [SerializeField] private float movementSpeed;
    public static PlayerAnims instance;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    public bool flipAnims = false;  //p2's anims will be flipped, defined by LoadArena.cs

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        if (flipAnims) { sprite.flipX = true; }
    }

    // Update is called once per frame
    void Update()
    {
        CheckAttack();
        DefaultAnimation(0);

        void CheckAttack()
        {
            if (!flipAnims && !Input.GetKeyDown(KeyCode.Q)) return;
            if (flipAnims && !Input.GetKeyDown(KeyCode.RightShift)) return;

            Play(new(Animations.Std_Attack, true, new()));
        }
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
