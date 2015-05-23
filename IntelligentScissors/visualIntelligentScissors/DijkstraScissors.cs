using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using System.Collections;
using VisualIntelligentScissors;
namespace VisualIntelligentScissors
{
    public class DijkstraScissors : Scissors
    {
        public DijkstraScissors() { }
        /// <summary>
        /// constructor for intelligent scissors. 
        /// </summary>
        /// <param name="image">the image you are oging to segment.  has methods for getting gradient information</param>
        /// <param name="overlay">an overlay on which you can draw stuff by setting pixels.</param>
        public DijkstraScissors(GrayBitmap image, Bitmap overlay) : base(image, overlay) { }
        // this is the class you need to implement in CS 312
        /// <summary>
        /// this is the class you implement in CS 312. 
        /// </summary>
        /// <param name="points">these are the segmentation points from the pgm file.</param>
        /// <param name="pen">this is a pen you can use to draw on the overlay</param>
        public override void FindSegmentation(IList<Point> points, Pen pen)
        {
            if (Image == null) throw new InvalidOperationException("Set Image property first.");
            // this is the entry point for this class when the button is clicked
            // to do the image segmentation with intelligent scissors.
            //NodePoint start = new NodePoint(points[0].X, points[0].Y);
            //NodePoint end = new NodePoint(points[1].X,points[1].Y);
            int i;
            NodePoint start;
            NodePoint end;
            NodePoint trc;
            using (Graphics g = Graphics.FromImage(Overlay))
            {
                for (i = 0; i < points.Count - 1; i++)
                {
                    NodePoint.setUp(Overlay.Width, Overlay.Height);
                    start = NodePoint.getNodeByXY(points[i].X, points[i].Y);
                    end = NodePoint.getNodeByXY(points[i + 1].X, points[i + 1].Y);
                    g.DrawEllipse(pen, points[i].X, points[i].Y, 5, 5);
                    dijkstraOnePath(start, end);
                    //Do tracing
                    Console.WriteLine("finished! starting backtrace.");
                    trc = end;
                    while (trc.getPrev() != null)
                    {

                        Overlay.SetPixel(trc.getPoint().X, trc.getPoint().Y, Color.White);
                        trc = trc.getPrev();
                    }
                    Overlay.SetPixel(trc.getPoint().X, trc.getPoint().Y, Color.White);
                    Program.MainForm.RefreshImage();
                }
                g.DrawEllipse(pen, points[i].X, points[i].Y, 5, 5);
            }
            NodePoint.setUp(Overlay.Width, Overlay.Height); //Complete cycle from last point to first...
            start = NodePoint.getNodeByXY(points[i].X, points[i].Y);
            end = NodePoint.getNodeByXY(points[0].X, points[0].Y);
            dijkstraOnePath(start, end);
            //Do tracing
            Console.WriteLine("finished! starting backtrace.");
            trc = end;
            while (trc.getPrev() != null)
            {

                Overlay.SetPixel(trc.getPoint().X, trc.getPoint().Y, Color.White);
                trc = trc.getPrev();
            }
            Overlay.SetPixel(trc.getPoint().X, trc.getPoint().Y, Color.White);
            Program.MainForm.RefreshImage();
        }
        
        private void dijkstraOnePath(NodePoint strtNode, NodePoint dstNode) 
        {
            BinHeap<NodePoint> binHeap = new BinHeap<NodePoint>();
            binHeap.insert(strtNode); //O(log(n))
            binHeap.decreaseKey(strtNode, 0); //O(log(n))
            strtNode.setVisited();
            while (!binHeap.isEmpty())
            {
                NodePoint u = binHeap.deletemin(); //O(log(n))
                if (u.isEqual(dstNode))
                {
                    Console.WriteLine("goal met w/ dist: " + u.getDist().ToString());
                    break; //Goal met; used in place of boolean statement
                }
                HashSet<NodePoint> setV = u.getNextNodes();
                for (int i = 0; i < setV.Count; i++) //for (u, v) in E; all edges adjacent to u from deletemin.
                {
                    NodePoint v = setV.ElementAt(i);
                    Point b = v.getPoint();
                    double _uv = GetPixelWeight(b); //<Weight for any incoming edge> = MaxGradient - g_{x^y^}
                    double newVal = u.getDist() + _uv;
                    if (!v.isVisited())
                    {
                        binHeap.insert(v);  //O(log(n))
                        binHeap.decreaseKey(v, newVal); //O(log(n))
                        v.setPrev(u); //Done for backtrace.
                        v.setVisited(); //Mark as visited. If encountered later, should *effectively* be ignored, or else something is wrong with our algorithm. See below...
                    }
                    else
                    {
                        if (v.getDist() > newVal)
                        {
                            //Unreachable
                            //Console.WriteLine("Condition Violated!!!!!");
                            v.setPrev(u);
                            binHeap.decreaseKey(v, newVal);
                        }
                    }
                }
            }
        }
     }
}
