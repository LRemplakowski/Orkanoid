namespace Orkanoid.Game
{
    public interface IBrick
    {
        void TakeHit(int damage);

        int GetHealthLeft();

        int GetPointValue();
    }
}