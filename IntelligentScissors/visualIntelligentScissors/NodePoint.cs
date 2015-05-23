using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace VisualIntelligentScissors
{
    class NodePoint: IComparable, ITypeGetterSetter<double>
    {
        private static NodePoint[][] nodeArray;
        public static void setUp(int x, int y)
        {
            nodeArray = new NodePoint[x-2][];
            for (int i = 0; i < x-2; i++)
            {
                //Console.WriteLine("x: " + x.ToString());
                nodeArray[i] = new NodePoint[y-2];
                for (int ii = 0; ii < y-2; ii++)
                {
                    //Console.WriteLine("y: " + y.ToString());
                    nodeArray[i][ii] = new NodePoint(i + 1, ii + 1);
                }
            }
            Xmax = x - 2;
            Ymax = y - 2;

        }
        private static int Xmax;
        private static int Ymax;
        public static NodePoint getNodeByXY(int x, int y) {
            if (y > Ymax || y < 1) return null;
            if (x > Xmax || x < 1) return null;
            return nodeArray[x-1][y-1];
        }
        private double dist;
        public const double START_WEIGHT = 1e20; //set very large
        private bool visited;
        private Point point;
        private NodePoint prev;
        public bool isEqual(NodePoint np)
        {
            
            if (np.getPoint().Equals(this.point)) 
                return true;
            return false;
        }
        public HashSet<NodePoint> getNextNodes()
        {

            HashSet<NodePoint> nodes = new HashSet<NodePoint>();
            //nodes.Add(new NodePoint(point.X, point.Y-1)); //N
            //nodes.Add(new NodePoint(point.X-1, point.Y)); //W
            //nodes.Add(new NodePoint(point.X, point.Y + 1)); //S
            //nodes.Add(new NodePoint(point.X+1, point.Y)); //E
            NodePoint n = getNodeByXY(point.X, point.Y - 1);
            NodePoint w = getNodeByXY(point.X - 1, point.Y);
            NodePoint s = getNodeByXY(point.X, point.Y + 1);
            NodePoint e = getNodeByXY(point.X + 1, point.Y);
            if (n != null)
                nodes.Add(n);
            if (w != null)
                nodes.Add(w);
            if (s != null)
                nodes.Add(s);
            if (e != null)
                nodes.Add(e);
            return nodes;
        }
        public bool isVisited()
        {
            return visited;
        }
        public void setVisited()
        {
            visited = true;
        }
        public NodePoint getPrev()
        {
            return prev;
        }
        private NodePoint(int x, int y)
        {
            this.point = new Point(x,y);
            dist = START_WEIGHT;
            prev = null;
            visited = false;
        }
        public Point getPoint()
        {
            return point;
        }
        public double getDist() {
            return dist;
        }
        public void setDist(double d) {
            dist = d;
        }
        public void setPrev(NodePoint np) {
            prev = np;
        }
        public void set(double d) {
            dist = d;
        }
        public double get()
        {
            return dist;
        }
     int  System.IComparable.CompareTo(object obj)
        {
            NodePoint v = (NodePoint) obj;
            if(dist > v.getDist()) return 1;
            else if(dist == v.getDist()) return 0;
            else return -1;
        }
       
    }
}
