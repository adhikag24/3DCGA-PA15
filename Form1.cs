using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _3DCGA_PA15
{
    public partial class Form1 : Form
    {
        public struct TPoint
        {
            public double x, y, z, w;
        }
        public struct TLine
        {
            public int p1, p2;
        }
        public struct TSurface
        {
            public int p1, p2, p3;
            public Color c;
        }

        public class ENode
        {
            public double ymax, xofymin, dx, dy, carrier;
            public ENode next;
            public ENode()
            {
                carrier = 0;
                next = null;
            }
            public ENode(double ymax1, double xofymin1, double dx1, double dy1)
            {
                ymax = ymax1;
                xofymin = xofymin1;
                dx = dx1;
                dy = dy1;
                carrier = 0;
                next = null;
            }
        }

        public class BucketList
        {
            public ENode head;
            public BucketList()
            {
                head = null;
            }
            public BucketList(ENode n)
            {
                head = n;
            }
            public void appendNode(ENode n)
            {
                if (head == null) head = n;
                else
                {
                    ENode ptr = head;
                    while (ptr.next != null) ptr = ptr.next;
                    ptr.next = n;
                }
            }
            public string printList()
            {
                string temp = "";
                if (head == null) temp += "Empty";
                else
                {
                    ENode ptr = head;
                    while (ptr != null)
                    {
                        temp += "(" + ptr.ymax + "|" + ptr.xofymin + "|" + ptr.dx + "|" + ptr.dy + "|" + ptr.carrier + ") --> ";
                        ptr = ptr.next;
                    }
                }
                return temp;
            }
            public void sort()
            {
                bubbleSort(head);
            }
            public void bubbleSort(ENode n)
            {

            }
        }


        public class TObject
        {
            public int id;
            public double windowUmin, windowVmin, windowUmax, windowVmax, FP, BP;
            public TPoint[] V = new TPoint[4];
            public TPoint[] VW = new TPoint[4];
            public TPoint[] VPr1 = new TPoint[4];
            public TPoint[] VV = new TPoint[4];
            public TPoint[] VS = new TPoint[4];
            public TLine[] E = new TLine[6];
            public TSurface[] S = new TSurface[4];
            public TSurface[] Snew = new TSurface[4];
            public double[,] Wt = new double[4, 4];
            public double[,] T1 = new double[4, 4];
            public double[,] T2 = new double[4, 4];
            public double[,] T3 = new double[4, 4];
            public double[,] T4 = new double[4, 4];
            public double[,] T5 = new double[4, 4];
            public double[,] T7 = new double[4, 4];
            public double[,] T8 = new double[4, 4];
            public double[,] T9 = new double[4, 4];
            public double[,] Pr1 = new double[4, 4];
            public TPoint VRP, VPN, VUP, COP, N, upUnit, upVec, v, u, CW, DOP = new TPoint();
            public double Wtdx = 0, Wtdy = 0, Wtdz = 0, WtAngle = 0;
        }

        Bitmap bmp;
        Graphics g;
        public double[,] St = new double[4, 4];
        public double[,] Pr2 = new double[4, 4];
        public double[,] WtTemp = new double[4, 4];
        public double[,] matrixTemp = new double[4, 4];
        public bool onX = false, onY = false, onZ = false;
        public string debugText;

        public void setPoint(ref TPoint V, double x, double y, double z)
        {
            V.x = x;
            V.y = y;
            V.z = z;
            V.w = 1;
        }
        public void setLine(ref TLine E, int p1, int p2)
        {
            E.p1 = p1;
            E.p2 = p2;
        }
        public void setSurface(ref TSurface S, int p1, int p2, int p3, Color c)
        {
            S.p1 = p1;
            S.p2 = p2;
            S.p3 = p3;
            S.c = c;
        }
        public void setRowMatrix(ref double[,] M, int row, double a, double b, double c, double d)
        {
            M[row, 0] = a;
            M[row, 1] = b;
            M[row, 2] = c;
            M[row, 3] = d;
        }
        public TPoint multiplyMatrix(TPoint P, double[,] M)
        {
            TPoint temp;
            double w = P.x * M[0, 3] + P.y * M[1, 3] + P.z * M[2, 3] + P.w * M[3, 3];
            temp.x = (P.x * M[0, 0] + P.y * M[1, 0] + P.z * M[2, 0] + P.w * M[3, 0]) / w;
            temp.y = (P.x * M[0, 1] + P.y * M[1, 1] + P.z * M[2, 1] + P.w * M[3, 1]) / w;
            temp.z = (P.x * M[0, 2] + P.y * M[1, 2] + P.z * M[2, 2] + P.w * M[3, 2]) / w;
            temp.w = 1;
            return temp;
        }
        public double[,] matrixMultiplication(double[,] M1, double[,] M2)
        {
            double[,] temp = new double[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    temp[i, j] = 0;
                    for (int k = 0; k < 4; k++)
                    {
                        temp[i, j] += M1[i, k] * M2[k, j];
                    }
                }
            }
            return temp;
        }

        public TPoint findVector(TPoint p1, TPoint p2)
        {
            TPoint temp;
            temp.x = p2.x - p1.x;
            temp.y = p2.y - p1.y;
            temp.z = p2.z - p1.z;
            temp.w = 1;
            return temp;
        }

        public double dotProduct(TPoint P1, TPoint P2)
        {
            double temp;
            temp = P1.x * P2.x + P1.y * P2.y + P1.z * P2.z;
            return temp;
        }

        public TPoint crossProduct(TPoint P1, TPoint P2)
        {
            TPoint tempPoint;
            tempPoint.x = (P1.y * P2.z) - (P2.y * P1.z);
            tempPoint.y = (P1.z * P2.x) - (P2.z * P1.x);
            tempPoint.z = (P1.x * P2.y) - (P2.x * P1.y);
            tempPoint.w = 1;
            return tempPoint;
        }

        public TPoint unitVector(TPoint P)
        {
            TPoint tempPoint;
            double temp;
            temp = Math.Sqrt(Math.Pow(P.x, 2) + Math.Pow(P.y, 2) + Math.Pow(P.z, 2));
            tempPoint.x = P.x / temp;
            tempPoint.y = P.y / temp;
            tempPoint.z = P.z / temp;
            tempPoint.w = 1;
            return tempPoint;
        }


        public TSurface[] backFaceCulling(TSurface[] surfaces, TPoint[] points)
        {
            int count = 0;
            TSurface[] temp = new TSurface[4];
            for (int i=0; i<4; i++)
            {
                TPoint p1 = points[surfaces[i].p1];
                TPoint p2 = points[surfaces[i].p2];
                TPoint p3 = points[surfaces[i].p3];
                TPoint vec1 = findVector(p1, p2);
                TPoint vec2 = findVector(p2, p3);
                TPoint N = crossProduct(vec1, vec2);
                TPoint VN = new TPoint();
                setPoint(ref VN, 0, 0, -1);
                double res = dotProduct(VN, N);
                //if (i == 0)
                //{
                //    debugTextBox.Text = "";
                //    debugTextBox.AppendText(p1.x.ToString() + "  " + p1.y.ToString() + "  " + p1.z.ToString());
                //    debugTextBox.AppendText(Environment.NewLine);
                //    debugTextBox.AppendText(p2.x.ToString() + "  " + p2.y.ToString() + "  " + p2.z.ToString());
                //    debugTextBox.AppendText(Environment.NewLine);
                //    debugTextBox.AppendText(p3.x.ToString() + "  " + p3.y.ToString() + "  " + p3.z.ToString());
                //    debugTextBox.AppendText(Environment.NewLine);
                //    debugTextBox.AppendText(Environment.NewLine);
                //    debugTextBox.AppendText(vec1.x.ToString() + "  " + vec1.y.ToString() + "  " + vec1.z.ToString());
                //    debugTextBox.AppendText(Environment.NewLine);
                //    debugTextBox.AppendText(vec2.x.ToString() + "  " + vec2.y.ToString() + "  " + vec2.z.ToString());
                //    debugTextBox.AppendText(Environment.NewLine);
                //    debugTextBox.AppendText(Environment.NewLine);
                //    debugTextBox.AppendText(N.x.ToString() + "  " + N.y.ToString() + "  " + N.z.ToString());
                //    debugTextBox.AppendText(Environment.NewLine);
                //    debugTextBox.AppendText(Environment.NewLine);
                //    debugTextBox.AppendText(VN.x.ToString() + "  " + VN.y.ToString() + "  " + VN.z.ToString());
                //    debugTextBox.AppendText(Environment.NewLine);
                //    debugTextBox.AppendText(Environment.NewLine);
                //    debugTextBox.AppendText(res.ToString());
                //}
                if(res < 0)
                {
                    setSurface(ref temp[count], surfaces[i].p1, surfaces[i].p2, surfaces[i].p3, surfaces[i].c);
                    count++;
                }
            }
            return temp;
        }

        public BucketList[] generateSET(TPoint p1, TPoint p2, TPoint p3, double maximumY)
        {
            BucketList[] temp = new BucketList[Convert.ToInt32(maximumY)];
            for (int i = 0; i < temp.Length; i++) temp[i] = new BucketList();
            double p1p2ymax, p1p2xofymin, p1p2dx, p1p2dy, p1p2ymin;
            double p2p3ymax, p2p3xofymin, p2p3dx, p2p3dy, p2p3ymin;
            double p3p1ymax, p3p1xofymin, p3p1dx, p3p1dy, p3p1ymin;
            p1p2dx = p2.x - p1.x;
            p1p2dy = p2.y - p1.y;
            p2p3dx = p3.x - p2.x;
            p2p3dy = p3.y - p2.y;
            p3p1dx = p1.x - p3.x;
            p3p1dy = p1.y - p3.y;
            if (p1.y > p2.y)
            {
                p1p2ymax = p1.y;
                p1p2xofymin = p2.x;
                p1p2ymin = p2.y;
            }
            else
            {
                p1p2ymax = p2.y;
                p1p2xofymin = p1.x;
                p1p2ymin = p1.y;
            }
            if(p2.y > p3.y)
            {
                p2p3ymax = p2.y;
                p2p3xofymin = p3.x;
                p2p3ymin = p3.y;
            }
            else
            {
                p2p3ymax = p3.y;
                p2p3xofymin = p2.x;
                p2p3ymin = p2.y;
            }
            if(p3.y > p1.y)
            {
                p3p1ymax = p3.y;
                p3p1xofymin = p1.x;
                p3p1ymin = p1.y;
            }
            else
            {
                p3p1ymax = p1.y;
                p3p1xofymin = p3.x;
                p3p1ymin = p3.y;
            }

            for (int i = 0; i < Convert.ToInt32(maximumY); i++)
            {
                if (i == p1p2ymin)
                {
                    ENode nodeTemp = new ENode(p1p2ymax, p1p2xofymin, p1p2dx, p1p2dy);
                    temp[i].appendNode(nodeTemp);
                }
                if (i == p2p3ymin)
                {
                    ENode nodeTemp = new ENode(p2p3ymax, p2p3xofymin, p2p3dx, p2p3dy);
                    temp[i].appendNode(nodeTemp);
                }
                if (i == p3p1ymin)
                {
                    ENode nodeTemp = new ENode(p3p1ymax, p3p1xofymin, p3p1dx, p3p1dy);
                    temp[i].appendNode(nodeTemp);
                }
            }
            return temp;
        }

        public BucketList[] generateAEL(BucketList[] SET)
        {
            BucketList[] temp;

            return temp;
        }

        public void scanlineFill(TSurface S, TPoint[] P)
        {
            TPoint p1 = P[S.p1];
            TPoint p2 = P[S.p2];
            TPoint p3 = P[S.p3];
            Color c1 = S.c;
            double maximumY = Math.Max(p1.y, Math.Max(p2.y, p3.y));
            BucketList[] SET = new BucketList[Convert.ToInt32(maximumY)];
            SET = generateSET(p1, p2, p3, maximumY);
            //for(int i=0; i<SET.Length; i++)
            //{
            //    debugText += i.ToString() + " => ";
            //    debugText += SET[i].printList();
            //    debugText += Environment.NewLine;
            //}
        }


        public void draw(TPoint[] VS, TLine[] E, TSurface[] S)
        {
            //for(int i=0; i<S.Length; i++)
            //{
            //    scanlineFill(S[i], VS);
            //}
            scanlineFill(S[0], VS);
            //bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            //g = Graphics.FromImage(bmp);
            //Pen redPen = new Pen(Color.Red);
            //Pen blackPen = new Pen(Color.Black);
            //TPoint p1, p2;
            //for (int i = 3; i < 6; i++)
            //{
            //    p1 = VS[E[i].p1];
            //    p2 = VS[E[i].p2];
            //    g.DrawLine(blackPen, (float)p1.x, (float)p1.y, (float)p2.x, (float)p2.y);
            //}
            //for (int i = 0; i < 3; i++)
            //{
            //    p1 = VS[E[i].p1];
            //    p2 = VS[E[i].p2];
            //    g.DrawLine(redPen, (float)p1.x, (float)p1.y, (float)p2.x, (float)p2.y);
            //}
            //pictureBox1.Image = bmp;
        }

        TObject obj = new TObject();

        public void display()
        {
            
            obj.id = 0;
            setPoint(ref obj.V[0], -1, -1, 1);
            setPoint(ref obj.V[1], 1, -1, 1);
            setPoint(ref obj.V[2], 0, 1, 0);
            setPoint(ref obj.V[3], 0, -1, -1);

            setLine(ref obj.E[0], 0, 1);
            setLine(ref obj.E[1], 1, 2);
            setLine(ref obj.E[2], 2, 0);
            setLine(ref obj.E[3], 2, 3);
            setLine(ref obj.E[4], 0, 3);
            setLine(ref obj.E[5], 1, 3);

            setSurface(ref obj.S[0], 0, 1, 2, Color.Red);
            setSurface(ref obj.S[1], 1, 3, 2, Color.Orange);
            setSurface(ref obj.S[2], 3, 0, 2, Color.Yellow);
            setSurface(ref obj.S[3], 3, 0, 1, Color.Green);

            


            setPoint(ref obj.VRP, 0, 0, 0);
            setPoint(ref obj.VPN, 0, 0, 1);
            setPoint(ref obj.VUP, 0, 1, 0);
            setPoint(ref obj.COP, 0, 0, 4);

            obj.windowUmin = -2;
            obj.windowVmin = -2;
            obj.windowUmax = 2;
            obj.windowVmax = 2;
            obj.FP = 2;
            obj.BP = -2;

            // Calculate N
            obj.N = unitVector(obj.VPN);

            // Calculate up Unit
            obj.upUnit = unitVector(obj.VUP);

            // Calculate up Vector
            double temp;
            TPoint tempPoint = new TPoint();
            temp = dotProduct(obj.upUnit, obj.N);
            tempPoint.x = temp * obj.N.x;
            tempPoint.y = temp * obj.N.y;
            tempPoint.z = temp * obj.N.z;
            setPoint(ref obj.upVec, obj.upUnit.x - tempPoint.x, obj.upUnit.y - tempPoint.y, obj.upUnit.z - tempPoint.z);

            // Calculate v
            obj.v = unitVector(obj.upVec);

            // Calculate u
            obj.u = crossProduct(obj.v, obj.N);

            setPoint(ref obj.CW, (obj.windowUmax + obj.windowUmin) / 2, (obj.windowVmax + obj.windowVmin) / 2, 0);
            setPoint(ref obj.DOP, (obj.CW.x - obj.COP.x), (obj.CW.y - obj.COP.y), (obj.CW.z - obj.COP.z));

            double rx = obj.VRP.x;
            double ry = obj.VRP.y;
            double rz = obj.VRP.z;
            setRowMatrix(ref obj.T1, 0, 1, 0, 0, 0);
            setRowMatrix(ref obj.T1, 1, 0, 1, 0, 0);
            setRowMatrix(ref obj.T1, 2, 0, 0, 1, 0);
            setRowMatrix(ref obj.T1, 3, -rx, -ry, -rz, 1);

            setRowMatrix(ref obj.T2, 0, obj.u.x, obj.v.x, obj.N.x, 0);
            setRowMatrix(ref obj.T2, 1, obj.u.y, obj.v.y, obj.N.y, 0);
            setRowMatrix(ref obj.T2, 2, obj.u.z, obj.v.z, obj.N.z, 0);
            setRowMatrix(ref obj.T2, 3, 0, 0, 0, 1);

            setRowMatrix(ref obj.T3, 0, 1, 0, 0, 0);
            setRowMatrix(ref obj.T3, 1, 0, 1, 0, 0);
            setRowMatrix(ref obj.T3, 2, 0, 0, 1, 0);
            setRowMatrix(ref obj.T3, 3, -obj.COP.x, -obj.COP.y, -obj.COP.z, 1);

            double shx = -obj.DOP.x / obj.DOP.z;
            double shy = -obj.DOP.y / obj.DOP.z;
            setRowMatrix(ref obj.T4, 0, 1, 0, 0, 0);
            setRowMatrix(ref obj.T4, 1, 0, 1, 0, 0);
            setRowMatrix(ref obj.T4, 2, shx, shy, 1, 0);
            setRowMatrix(ref obj.T4, 3, 0, 0, 0, 1);

            double w, h;
            w = ((obj.COP.z - obj.BP) * (obj.windowUmax - obj.windowUmin)) / (2 * obj.COP.z);
            h = ((obj.COP.z - obj.BP) * (obj.windowVmax - obj.windowVmin)) / (2 * obj.COP.z);
            double sx = 1 / w;
            double sy = 1 / h;
            double sz = -1 / (obj.BP - obj.COP.z);
            setRowMatrix(ref obj.T5, 0, sx, 0, 0, 0);
            setRowMatrix(ref obj.T5, 1, 0, sy, 0, 0);
            setRowMatrix(ref obj.T5, 2, 0, 0, sz, 0);
            setRowMatrix(ref obj.T5, 3, 0, 0, 0, 1);

            setRowMatrix(ref obj.T7, 0, 1, 0, 0, 0);
            setRowMatrix(ref obj.T7, 1, 0, 1, 0, 0);
            setRowMatrix(ref obj.T7, 2, 0, 0, 1, 0);
            setRowMatrix(ref obj.T7, 3, 0, 0, -(-obj.COP.z / (obj.COP.z - obj.BP)), 1);

            setRowMatrix(ref obj.T8, 0, (obj.COP.z - obj.BP) / obj.COP.z, 0, 0, 0);
            setRowMatrix(ref obj.T8, 1, 0, (obj.COP.z - obj.BP) / obj.COP.z, 0, 0);
            setRowMatrix(ref obj.T8, 2, 0, 0, 1, 0);
            setRowMatrix(ref obj.T8, 3, 0, 0, 0, 1);

            setRowMatrix(ref obj.T9, 0, 1, 0, 0, 0);
            setRowMatrix(ref obj.T9, 1, 0, 1, 0, 0);
            setRowMatrix(ref obj.T9, 2, 0, 0, 1, (-1 / (obj.COP.z / (obj.COP.z - obj.BP))));
            setRowMatrix(ref obj.T9, 3, 0, 0, 0, 1);

            if (translateRB.Checked)
            {
                setRowMatrix(ref matrixTemp, 0, 1, 0, 0, 0);
                setRowMatrix(ref matrixTemp, 1, 0, 1, 0, 0);
                setRowMatrix(ref matrixTemp, 2, 0, 0, 1, 0);
                setRowMatrix(ref matrixTemp, 3, obj.Wtdx, obj.Wtdy, obj.Wtdz, 1);
            }
            else if(rotateRB.Checked)
            {
                if(onX)
                {
                    setRowMatrix(ref matrixTemp, 0, 1, 0, 0, 0);
                    setRowMatrix(ref matrixTemp, 1, 0, Math.Cos(obj.WtAngle * Math.PI / 180), Math.Sin(obj.WtAngle * Math.PI / 180), 0);
                    setRowMatrix(ref matrixTemp, 2, 0, -Math.Sin(obj.WtAngle * Math.PI / 180), Math.Cos(obj.WtAngle * Math.PI / 180), 0);
                    setRowMatrix(ref matrixTemp, 3, 0, 0, 0, 1);
                }
                else if(onY)
                {
                    setRowMatrix(ref matrixTemp, 0, Math.Cos(obj.WtAngle * Math.PI / 180), 0, -Math.Sin(obj.WtAngle * Math.PI / 180), 0);
                    setRowMatrix(ref matrixTemp, 1, 0, 1, 0, 0);
                    setRowMatrix(ref matrixTemp, 2, Math.Sin(obj.WtAngle * Math.PI / 180), 0, Math.Cos(obj.WtAngle * Math.PI / 180), 0);
                    setRowMatrix(ref matrixTemp, 3, 0, 0, 0, 1);
                }
                else if(onZ)
                {
                    setRowMatrix(ref matrixTemp, 0, Math.Cos(obj.WtAngle * Math.PI / 180), Math.Sin(obj.WtAngle * Math.PI / 180), 0, 0);
                    setRowMatrix(ref matrixTemp, 1, -Math.Sin(obj.WtAngle * Math.PI / 180), Math.Cos(obj.WtAngle * Math.PI / 180), 0, 0);
                    setRowMatrix(ref matrixTemp, 2, 0, 0, 1, 0);
                    setRowMatrix(ref matrixTemp, 3, 0, 0, 0, 1);
                }
            }
            else
            {
                setRowMatrix(ref matrixTemp, 0, 1, 0, 0, 0);
                setRowMatrix(ref matrixTemp, 1, 0, 1, 0, 0);
                setRowMatrix(ref matrixTemp, 2, 0, 0, 1, 0);
                setRowMatrix(ref matrixTemp, 3, 0, 0, 0, 1);
            }

            obj.Wt = matrixMultiplication(WtTemp, matrixTemp);

            obj.Pr1 = matrixMultiplication(matrixMultiplication(matrixMultiplication(matrixMultiplication(matrixMultiplication(matrixMultiplication(matrixMultiplication(obj.T1, obj.T2), obj.T3), obj.T4), obj.T5), obj.T7), obj.T8), obj.T9);

            for (int i = 0; i < 4; i++)
            {
                obj.VW[i] = multiplyMatrix(obj.V[i], obj.Wt);
                obj.VPr1[i] = multiplyMatrix(obj.VW[i], obj.Pr1);

            }

            obj.Snew = backFaceCulling(obj.S, obj.VW);


            for (int i = 0; i < 4; i++)
            {
                obj.VV[i] = multiplyMatrix(obj.VPr1[i], Pr2);
                obj.VS[i] = multiplyMatrix(obj.VV[i], St);
            }

            draw(obj.VS, obj.E, obj.S);
        }

        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            translateRB.Checked = true;

            setRowMatrix(ref WtTemp, 0, 1, 0, 0, 0);
            setRowMatrix(ref WtTemp, 1, 0, 1, 0, 0);
            setRowMatrix(ref WtTemp, 2, 0, 0, 1, 0);
            setRowMatrix(ref WtTemp, 3, 0, 0, 0, 1);

            setRowMatrix(ref St, 0, 100, 0, 0, 0);
            setRowMatrix(ref St, 1, 0, -100, 0, 0);
            setRowMatrix(ref St, 2, 0, 0, 0, 0);
            setRowMatrix(ref St, 3, 200, 200, 0, 1);

            setRowMatrix(ref Pr2, 0, 1, 0, 0, 0);
            setRowMatrix(ref Pr2, 1, 0, 1, 0, 0);
            setRowMatrix(ref Pr2, 2, 0, 0, 0, 0);
            setRowMatrix(ref Pr2, 3, 0, 0, 0, 1);

            display();
            debugTextBox.Text = debugText;
        }

        private void upBtn_Click(object sender, EventArgs e)
        {
            if (translateRB.Checked) obj.Wtdy++;
            else
            {
                onY = true;
                onX = onZ = false;
                obj.WtAngle += 1;
            }
            display();
        }

        private void downBtn_Click(object sender, EventArgs e)
        {
            if (translateRB.Checked) obj.Wtdy--;
            else
            {
                onY = true;
                onX = onZ = false;
                obj.WtAngle -= 1;
            }
            display();
        }

        private void frontBtn_Click(object sender, EventArgs e)
        {
            if (translateRB.Checked) obj.Wtdz++;
            else
            {
                onX = true;
                onY = onZ = false;
                obj.WtAngle += 1;
            }
            display();
        }

        private void backBtn_Click(object sender, EventArgs e)
        {
            if (translateRB.Checked) obj.Wtdz--;
            else
            {
                onX = true;
                onY = onZ = false;
                obj.WtAngle -= 1;
            }
            display();
        }

        private void leftBtn_Click(object sender, EventArgs e)
        {
            if (translateRB.Checked) obj.Wtdx--;
            else
            {
                onZ = true;
                onX = onY = false;
                obj.WtAngle -= 1;
            }
            display();
        }

        private void rightBtn_Click(object sender, EventArgs e)
        {
            if (translateRB.Checked) obj.Wtdx++;
            else
            {
                onZ = true;
                onX = onY = false;
                obj.WtAngle += 1;
            }
            display();
        }
    }
}
