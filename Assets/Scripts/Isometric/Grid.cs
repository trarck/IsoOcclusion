namespace iso
{
    public struct Grid
    {
        public int x;
        public int y;

        public Grid(int _x,int _y)
        {
            x = _x;
            y = _y;
        }
        public static Grid zero=new Grid(0,0);
    }

}
