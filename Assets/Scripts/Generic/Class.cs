namespace Class
{
    public class Player
    {
        public int level;

        public Player(int level)
        {
            this.level = level;
        }

        public void finishLevel(int level)
        {
            if (level > this.level)
                this.level = level;
        }
    }

}
