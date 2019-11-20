
internal class ZombieMeleeBehaviour : IMoveBehaviour
{
    IZombie _zombie;

    public ZombieMeleeBehaviour(IZombie zombie)
    {
        _zombie = zombie;
    }


    public void Move()
    {
    }
}