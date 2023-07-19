using Microsoft.Xna.Framework;

namespace SpaceSimulation.Helpers
{
    static class WorldSpaceMath
    {
        public static Point GetWorldPointForMousePoint(Point viewpoint, Point mousePoint, int mapViewSize, double viewportHeight, double viewportWidth)
        {
            var heightScale = viewportHeight / mapViewSize;
            var widthScale = viewportWidth / mapViewSize;

            var viewCorner_x = viewpoint.X - (mapViewSize / 2);
            var viewCorner_y = viewpoint.Y - (mapViewSize / 2);
            var xpos = mousePoint.X;
            var ypos = mousePoint.Y;
            // Convert these positions to the world state map, as integers
            var mapX = (int)((xpos / widthScale) + viewCorner_x);
            var mapY = (int)((ypos / heightScale) + viewCorner_y);

            return new Point(mapX, mapY);
        }
    }

}
