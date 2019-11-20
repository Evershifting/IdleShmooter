public interface IZombie
{
    float MoveSpeed { get; }
    float Reward { get; }

    void Init(float health, float moveSpeed, float reward, IMoveBehaviour moveBehaviour, IMoveBehaviour meleeBehaviour);
    void ReceiveDamage(float value);
}