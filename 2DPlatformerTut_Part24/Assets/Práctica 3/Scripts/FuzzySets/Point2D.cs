using System;

namespace FuzzyLogicPCL
{
    /// <summary>
    /// Simple utility class to create, manipulate and sort 2D points
    /// </summary>
    public class Point2D : IComparable
    {
        /// <summary>
        /// X coordinate
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Y coordinate
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        public Point2D(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Comparison method, for the IComparable interface. The order axis used is only the x-axis.
        /// </summary>
        /// <param name="obj">The other point</param>
        /// <returns>0 if equals, negative if smaller, postive if bigger</returns>
        public int CompareTo(object obj)
        {
            return (int)(this.X - ((Point2D)obj).X);
        }

        /// <summary>
        /// Conversion to string
        /// </summary>
        /// <returns>Coordinates in format : (x;y)</returns>
        public override String ToString()
        {
            return "(" + this.X + ";" + this.Y + ")";
        }
    }
}
