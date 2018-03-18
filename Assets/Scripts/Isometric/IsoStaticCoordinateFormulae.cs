using UnityEngine;

namespace iso
{
    /**
     @brief formulae
     x,y,z is space coord.left handle coord system.x point left,y point top,z point front
     x',y' is pix coord
     ux,uy,uz is space unit to pix
 
     x'=x*ux-z*uz;
     y'=y*uy+(x*ux+z*uz)/2;
 
     x=(x'/2+y'-y*uy)/ux;
     z=(-x'/2+y'-y*uy)/uz
 
     如果单位坐标值相等
     x=x'/TileWidth+y'/TileHeight;
     z=-x'/TileWidth+y'/TileHeight;
     */
    public class IsoStaticCoordinateFormulae
    {

        public static void initTileSize(float tileWidth, float tileHeight)
        {
            sTileWidth = tileWidth;
            sTileHeight = tileHeight;

            sXUnit = tileWidth * 0.5f;
            sYUnit = tileHeight;
            sZUnit = tileHeight;

            sHalfYUnit = tileHeight * 0.5f;
            sXUnitDerivative = 1.0f/ sXUnit;
            sYUnitDerivative = 1.0f / sYUnit;
        }

        /**
         *@brief 通常各分轴单位是一样的。
         */
        public static void initCoordinateUnit(float xUnit, float yUnit, float zUnit)
        {
            sXUnit = xUnit;
            sYUnit = yUnit;
            sZUnit = zUnit;

            sTileWidth = xUnit + yUnit;
            sTileHeight = sTileWidth * 0.5f;

            sHalfYUnit = yUnit * 0.5f;
            sXUnitDerivative = 1.0f / sXUnit;
            sYUnitDerivative = 1.0f / sYUnit;
        }

        //==============使用传值的方式==========================//
        public static Vector2 ViewToGame3F(float x, float y, float gameZ)
        {
            float px = (x * 0.5f + y - gameZ * sZUnit) * sXUnitDerivative;//x=(x'/2+y'-z*uz)/ux;
            float py = (y - 0.5f * x - gameZ * sZUnit) * sYUnitDerivative;//y=(-x'/2+y'-z*uz)/uy
            return new Vector2(px,py);
        }

        static Vector2 viewToGame2F(float x, float y)
        {
            float px = (x * 0.5f + y) *sXUnitDerivative;//x=(x'/2+y'-z*uz)/ux;
            float py = (y - 0.5f * x) *sYUnitDerivative;//y=(-x'/2+y'-z*uz)/uy
            return new Vector2(px, py);
        }

        public static Vector2 viewToGamePoint(Vector2 point)
        {
            return viewToGame2F(point.x, point.y);
        }

        public static Vector2 viewToGameCell2F(float x, float y)
        {
            Vector2 p = viewToGame2F(x, y);
            p.x =Mathf.Floor(p.x);
            p.y = Mathf.Floor(p.y);
            return p;
        }

        public static Vector2 viewToGameCellPoint(Vector2 point)
        {
            return viewToGameCell2F(point.x, point.y);
        }

        public static Vector2 gameToView3F(float x, float y, float z)
        {
            float px = (x * sXUnit - y * sYUnit);//x'=x*ux-y*uy;
            float py = (0.5f * (x * sXUnit + y * sYUnit) + z * sZUnit);//y'=z*uz+(x*ux+y*uy)/2;
            return new Vector2(px,py);
        }

        public static Vector2 gameToView2F(float x, float y)
        {
            float px = (x * sXUnit - y * sYUnit);//x'=x*ux-y*uy;
            float py = 0.5f * (x * sXUnit + y * sYUnit);//y'=z*uz+(x*ux+y*uy)/2;
            return new Vector2(px,py);
        }

        public static Vector2 gameToViewPoint(Vector2 point)
        {
            return gameToView2F(point.x, point.y);
        }
        

        static float sTileWidth=0.64f;
        static float sTileHeight=0.32f;
        //空间坐标的单位，对应空间的像素值。正常坐标系下，x,y,z单位值是一致的。
        static float sXUnit=sTileWidth*0.5f;
        static float sYUnit=sTileHeight;
        static float sZUnit=sTileHeight;
        static float sHalfYUnit=sTileHeight*0.5f;
        static float sXUnitDerivative = 1.0f / sXUnit;
        static float sYUnitDerivative = 1.0f / sYUnit;
    }
}