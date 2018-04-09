using BehaviorTree;
using UnityEngine;

public class EnemyBtree : EnemyBase
{
    private Tree<EnemyBtree> _tree;
    public float pulseTimer;

    public SpriteRenderer sprite;
    public float fadeoutSpeed;

    bool FXTriggered;

    private void Start ()
    {
        base.Start();
        _tree = new Tree<EnemyBtree>(new Selector<EnemyBtree>(

            // (highest priority)
            //Seek
            new Sequence<EnemyBtree>( 
                new IsPlayerAway(),
                new Seek() 
            ),
            
            // Flee Behavior
            new Sequence<EnemyBtree>( 
                new IsInDanger(), 
                new IsPlayerInRange(),
                new FleeAction() 
            ),


            new Sequence<EnemyBtree>( 
                new IsPlayerInRange(), 
                new IsPulseNotFinished(),
                new PulseEffect(),
                new PreAttackAction()

                
            ),

            new Sequence<EnemyBtree>(
                new IsPlayerInRange(), 
                new AttackAction() // Attack               
            ),
        
         
            new Idle()
        ));
    }

    private void Update (){
        _tree.Update(this);
        if (sprite.color.a > 0) {
            sprite.color = new Color(1f, 0.3f, 0.3f, sprite.color.a - fadeoutSpeed * Time.deltaTime);
        }
    }

    protected override void Move() {
        LookPlayer();
        FollowPlayer();
    }

    protected override void Attack() {
        LookPlayer();
        ShootBullet(-10);
        ShootBullet(0);
        ShootBullet(10);
    }



    private void OnCollisionEnter(Collision coll)
    {
        //if (coll.gameObject.GetComponent<Player>() == null) return;
        //_health = Mathf.Max(_health - _damagePerHit, 0);
        //var body = GetComponent<Rigidbody>();
        //var collisionNormal = (transform.position - coll.gameObject.transform.position).normalized;
        //body.AddForce(collisionNormal * 7, ForceMode.Impulse);
    }

    private float _lastRange;
  

    bool ActivatePulseTimer(){
        pulseTimer += Time.deltaTime;
        return (pulseTimer < 3);
    }

    void ResetPulseTimer(){
        pulseTimer = 0;
    }

    void triggerFX() {
        if(!FXTriggered){
            sprite.color = new Color(1f, 0.3f, 0.3f, 1f);
            FXTriggered = true;
        }
    }

    void ResetFXTrigger() {
        FXTriggered = false;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // NODES
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    ////////////////////
    // Conditions
    ////////////////////
    private class IsInDanger : Node<EnemyBtree>{
        public override bool Update(EnemyBtree enemy){
            return enemy.TargetedByPlayer();
        }
    }

    private class IsPlayerInRange : Node<EnemyBtree>{
        public override bool Update(EnemyBtree enemy){
            return enemy.PlayerInRange();
        }
    }

    private class IsPlayerAway : Node<EnemyBtree> {
        public override bool Update(EnemyBtree enemy) {
            return !enemy.PlayerInRange();
        }
    }

    private class IsPulseNotFinished: Node<EnemyBtree> {
        public override bool Update(EnemyBtree enemy) {
            return enemy.ActivatePulseTimer();
        }
    }

    ///////////////////
    /// Actions
    ///////////////////
    /// 
    private class PulseEffect : Node<EnemyBtree> {
        public override bool Update(EnemyBtree enemy) {
            
            return true;
        }
    }

    private class FleeAction : Node<EnemyBtree>{
        public override bool Update(EnemyBtree enemy){
            enemy.LookPlayer();
            enemy.Flee();
            return true;
        }
    }

    private class PreAttackAction : Node<EnemyBtree> {
        public override bool Update(EnemyBtree enemy) {
            enemy.LookPlayer();
            enemy.triggerFX();
            return true;
        }
    }

    private class AttackAction : Node<EnemyBtree>{
        public override bool Update(EnemyBtree enemy){
            if (enemy.CheckAttackTimer()) {
                enemy.Attack();
            }
            return true;
        }
    }

    private class Idle : Node<EnemyBtree>{
        public override bool Update(EnemyBtree enemy){
            enemy.LookPlayer();
            return true;
        }
    }

    class Seek: Node<EnemyBtree>{
        public override bool Update(EnemyBtree enemy) {
            enemy.FollowPlayer();
            enemy.ResetPulseTimer();
            enemy.ResetFXTrigger();
            return true;
        }
    }



        




}

