using UnityEngine;

public abstract class Command
{
    public abstract void Execute(Animator animator, Rigidbody rb);
}

public class DoMoveLeft : Command
{
    public override void Execute(Animator animator, Rigidbody rb)
    {
        rb.MovePosition(rb.position + Vector3.left * 2);
    }
}

public class DoMoveRight : Command
{
    public override void Execute(Animator animator, Rigidbody rb)
    {
        rb.MovePosition(rb.position + Vector3.right * 2);
    }
}

public class DoJump : Command
{
    public override void Execute(Animator animator, Rigidbody rb)
    {
        animator.SetTrigger("Jump");
    }
}

public class DoSpecial : Command
{
    public override void Execute(Animator animator, Rigidbody rb)
    {
        animator.SetTrigger("Special");
    }
}

public class DoCrouch : Command
{
    public override void Execute(Animator animator, Rigidbody rb)
    {
        animator.SetTrigger("Crouch");
    }
}

public class DoDie : Command
{
    public override void Execute(Animator animator, Rigidbody rb)
    {
        animator.SetTrigger("Die");
    }
}

public class DoRun : Command
{
    public override void Execute(Animator animator, Rigidbody rb)
    {
        animator.SetTrigger("Run");
    }
}

public class DoNothing : Command
{
    public override void Execute(Animator animator, Rigidbody rb) { }
}