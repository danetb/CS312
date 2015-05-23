using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;

namespace VisualIntelligentScissors
{
	public class SimpleScissors : Scissors
	{
		public SimpleScissors() { }

        /// <summary>
        /// constructor for SimpleScissors. 
        /// </summary>
        /// <param name="image">the image you are going to segment including methods for getting gradients.</param>
        /// <param name="overlay">a bitmap on which you can draw stuff.</param>
		public SimpleScissors(GrayBitmap image, Bitmap overlay) : base(image, overlay) { }

        // this is a class you need to implement in CS 312. 

        /// <summary>
        ///  this is the class to implement for CS 312. 
        /// </summary>
        /// <param name="points">the list of segmentation points parsed from the pgm file</param>
        /// <param name="pen">a pen for writing on the overlay if you want to use it.</param>
		public override void FindSegmentation(IList<Point> points, Pen pen)
		{
            // this is the entry point for this class when the button is clicked for 
            // segmenting the image using the simple greedy algorithm. 
            // the points
            
			if (Image == null) throw new InvalidOperationException("Set Image property first.");
            int i;
            Console.WriteLine("Width/Height");
            Console.WriteLine(Overlay.Width);
            Console.WriteLine(Overlay.Height);
            using (Graphics g = Graphics.FromImage(Overlay))
            {
                for (i = 0; i < points.Count - 1; i++)
                {
                    g.DrawEllipse(pen, points[i].X, points[i].Y, 5, 5);
                    traceGreedyPath(points[i], points[i + 1]);
                    Program.MainForm.RefreshImage();
                }
                g.DrawEllipse(pen, points[i].X, points[i].Y, 5, 5);
            }
            traceGreedyPath(points[i], points[0]); //Complete the cycle.
		}

        //Case 1: Goal is reached.
        //Case 2: Path cannot continue - all 4 adjacent points visited
        private void traceGreedyPath(Point a, Point b)
        {
            Point currentPoint = a;
            HashSet<Point> phash = new HashSet<Point>();
            while(!currentPoint.Equals(b)) {
                List<Point> adjacentPoints = new List<Point>();
                //Go through points clock-wise start at north point.
                adjacentPoints.Add(new Point(currentPoint.X, currentPoint.Y - 1)); //N
                adjacentPoints.Add(new Point(currentPoint.X - 1, currentPoint.Y)); //W
                adjacentPoints.Add(new Point(currentPoint.X, currentPoint.Y + 1)); //S
                adjacentPoints.Add(new Point(currentPoint.X + 1, currentPoint.Y)); //E
                int ci = 0;
                Point nextPoint = adjacentPoints[ci];
                while(phash.Contains(nextPoint)) {
                    ci++;
                    if(ci > 3) {
                        //CASE 2 reached.
                        Console.WriteLine("Path cannot continue - all 4 adjacent points visited");
                        Console.WriteLine("Stopped at: " + nextPoint);
                        return;
                    }
                    nextPoint = adjacentPoints[ci];
                }
                while(ci < 3) {
                    ci++;
                    if(!phash.Contains(adjacentPoints[ci])) {
                        if (GetPixelWeight(adjacentPoints[ci]) < GetPixelWeight(nextPoint))
                            nextPoint = adjacentPoints[ci];
                    }
                }
                phash.Add(currentPoint);
                Overlay.SetPixel(currentPoint.X,currentPoint.Y, Color.White);
                currentPoint = nextPoint;
            }
        }
	}
}
