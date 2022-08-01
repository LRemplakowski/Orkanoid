namespace Orkanoid.Level.Bricks
{
    public interface IBrick
    {
        void TakeHit(int damage);

        int GetHealthLeft();

        int GetPointValue();
    }
}