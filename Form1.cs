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
            public ENode(double ymax1, double xofymin1, double dx1, double dy1, double carr1)
            {
                ymax = ymax1;
                xofymin = xofymin1;
                dx = dx1;
                dy = dy1;
                carrier = carr1;
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
            public int countItem()
            {
                int temp = countItem(head);
                return temp;
            }
            public int countItem(ENode n)
            {
                int temp = 0;
                ENode ptr = n;
                while(ptr != null)
                {
                    temp++;
                    ptr = ptr.next;
                }
                return temp;
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
            public void sortList()
            {
                ENode ptr = head;
                for(int i=0; i<countItem()-1; i++)
                {
                    if (ptr.xofymin > ptr.next.xofymin)
                    {
                        if(i == 0)
                        {
                            ptr = ptr.next.next;
                            ptr.next = ptr;
                        }
                        else
                        {
                            ptr.next = ptr.next.next.next;
                            ptr.next.next = ptr.next;
                            ptr = ptr.next.next;
                        }
                    }
                }
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
        public int selectedObj;

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
            if(p1p2dy < 0)
            {
                p1p2dx *= -1;
                p1p2dy *= -1;
            }
            p2p3dx = p3.x - p2.x;
            p2p3dy = p3.y - p2.y;
            if(p2p3dy < 0)
            {
                p2p3dx *= -1;
                p2p3dy *= -1;
            }
            p3p1dx = p1.x - p3.x;
            p3p1dy = p1.y - p3.y;
            if(p3p1dy < 0)
            {
                p3p1dx *= -1;
                p3p1dy *= -1;
            }
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
                    ENode nodeTemp = new ENode(p1p2ymax, p1p2xofymin, p1p2dx, p1p2dy, 0);
                    temp[i].appendNode(nodeTemp);
                }
                if (i == p2p3ymin)
                {
                    ENode nodeTemp = new ENode(p2p3ymax, p2p3xofymin, p2p3dx, p2p3dy, 0);
                    temp[i].appendNode(nodeTemp);
                }
                if (i == p3p1ymin)
                {
                    ENode nodeTemp = new ENode(p3p1ymax, p3p1xofymin, p3p1dx, p3p1dy, 0);
                    temp[i].appendNode(nodeTemp);
                }
            }
            return temp;
        }

        public BucketList processCurrentList(BucketList B)
        {
            BucketList temp = new BucketList();
            ENode ptr = B.head;
            while(ptr != null)
            {
                ptr.carrier += ptr.dx;
                if(ptr.carrier >= ptr.dy)
                {
                    while(ptr.carrier >= ptr.dy)
                    {
                        ptr.xofymin++;
                        ptr.carrier -= ptr.dy;
                    }
                }
                else if(ptr.carrier < 0)
                {
                    while(ptr.carrier < 0)
                    {
                        ptr.xofymin--;
                        ptr.carrier += ptr.dy;
                    }
                }
                ENode tempNode = new ENode(ptr.ymax, ptr.xofymin, ptr.dx, ptr.dy, ptr.carrier);
                temp.appendNode(tempNode);
                ptr = ptr.next;
            }
            return temp;
        }

        public BucketList[] generateAEL(BucketList[] SET)
        {
            BucketList[] temp = new BucketList[SET.Length];
            for (int i = 0; i < temp.Length; i++) temp[i] = new BucketList();
;           for (int i = 0; i < SET.Length; i++) if (SET[i].head != null) temp[i] = SET[i];
            for (int i = 1; i < temp.Length; i++)
            {
                if (temp[i - 1].head != null)
                {
                    ENode ptr = temp[i - 1].head;
                    BucketList tempList = new BucketList();
                    while(ptr != null)
                    {
                        ENode tempNode = new ENode(ptr.ymax, ptr.xofymin, ptr.dx, ptr.dy, ptr.carrier);
                        tempList.appendNode(tempNode);
                        ptr = ptr.next;
                    }
                    BucketList tempList1 = processCurrentList(tempList);
                    tempList1.sortList();
                    temp[i] = tempList1;
                }
            }
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
            //for (int i = 0; i < SET.Length; i++)
            //{
            //    debugText += i.ToString() + " => ";
            //    debugText += SET[i].printList();
            //    debugText += Environment.NewLine;
            //}
            BucketList[] AEL = new BucketList[Convert.ToInt32(maximumY)];
            AEL = generateAEL(SET);
            for (int i = 0; i < AEL.Length; i++)
            {
                debugText += i.ToString() + " => ";
                debugText += AEL[i].printList();
                //debugText += AEL[i].countItem();
                debugText += Environment.NewLine;
            }
        }


        public void draw(TPoint[] VS, TLine[] E, TSurface[] S)
        {
            //for(int i=0; i<S.Length; i++)
            //{
            //    scanlineFill(S[i], VS);
            //}
            //scanlineFill(S[0], VS);
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bmp);
            Pen redPen = new Pen(Color.Red);
            Pen blackPen = new Pen(Color.Black);
            TPoint p1, p2;
            for (int i = 3; i < 6; i++)
            {
                p1 = VS[E[i].p1];
                p2 = VS[E[i].p2];
                g.DrawLine(blackPen, (float)p1.x, (float)p1.y, (float)p2.x, (float)p2.y);
            }
            for (int i = 0; i < 3; i++)
            {
                p1 = VS[E[i].p1];
                p2 = VS[E[i].p2];
                g.DrawLine(redPen, (float)p1.x, (float)p1.y, (float)p2.x, (float)p2.y);
            }
            pictureBox1.Image = bmp;
        }

        TObject[] obj = new TObject[2];

        public void display()
        {
            for (int i = 0; i < obj.Length; i++)
            {
                // Calculate N
                obj[i].N = unitVector(obj[i].VPN);

                // Calculate up Unit
                obj[i].upUnit = unitVector(obj[i].VUP);

                // Calculate up Vector
                double temp;
                TPoint tempPoint = new TPoint();
                temp = dotProduct(obj[i].upUnit, obj[i].N);
                tempPoint.x = temp * obj[i].N.x;
                tempPoint.y = temp * obj[i].N.y;
                tempPoint.z = temp * obj[i].N.z;
                setPoint(ref obj[i].upVec, obj[i].upUnit.x - tempPoint.x, obj[i].upUnit.y - tempPoint.y, obj[i].upUnit.z - tempPoint.z);

                // Calculate v
                obj[i].v = unitVector(obj[i].upVec);

                // Calculate u
                obj[i].u = crossProduct(obj[i].v, obj[i].N);

                setPoint(ref obj[i].CW, (obj[i].windowUmax + obj[i].windowUmin) / 2, (obj[i].windowVmax + obj[i].windowVmin) / 2, 0);
                setPoint(ref obj[i].DOP, (obj[i].CW.x - obj[i].COP.x), (obj[i].CW.y - obj[i].COP.y), (obj[i].CW.z - obj[i].COP.z));

                double rx = obj[i].VRP.x;
                double ry = obj[i].VRP.y;
                double rz = obj[i].VRP.z;
                setRowMatrix(ref obj[i].T1, 0, 1, 0, 0, 0);
                setRowMatrix(ref obj[i].T1, 1, 0, 1, 0, 0);
                setRowMatrix(ref obj[i].T1, 2, 0, 0, 1, 0);
                setRowMatrix(ref obj[i].T1, 3, -rx, -ry, -rz, 1);

                setRowMatrix(ref obj[i].T2, 0, obj[i].u.x, obj[i].v.x, obj[i].N.x, 0);
                setRowMatrix(ref obj[i].T2, 1, obj[i].u.y, obj[i].v.y, obj[i].N.y, 0);
                setRowMatrix(ref obj[i].T2, 2, obj[i].u.z, obj[i].v.z, obj[i].N.z, 0);
                setRowMatrix(ref obj[i].T2, 3, 0, 0, 0, 1);

                setRowMatrix(ref obj[i].T3, 0, 1, 0, 0, 0);
                setRowMatrix(ref obj[i].T3, 1, 0, 1, 0, 0);
                setRowMatrix(ref obj[i].T3, 2, 0, 0, 1, 0);
                setRowMatrix(ref obj[i].T3, 3, -obj[i].COP.x, -obj[i].COP.y, -obj[i].COP.z, 1);

                double shx = -obj[i].DOP.x / obj[i].DOP.z;
                double shy = -obj[i].DOP.y / obj[i].DOP.z;
                setRowMatrix(ref obj[i].T4, 0, 1, 0, 0, 0);
                setRowMatrix(ref obj[i].T4, 1, 0, 1, 0, 0);
                setRowMatrix(ref obj[i].T4, 2, shx, shy, 1, 0);
                setRowMatrix(ref obj[i].T4, 3, 0, 0, 0, 1);

                double w, h;
                w = ((obj[i].COP.z - obj[i].BP) * (obj[i].windowUmax - obj[i].windowUmin)) / (2 * obj[i].COP.z);
                h = ((obj[i].COP.z - obj[i].BP) * (obj[i].windowVmax - obj[i].windowVmin)) / (2 * obj[i].COP.z);
                double sx = 1 / w;
                double sy = 1 / h;
                double sz = -1 / (obj[i].BP - obj[i].COP.z);
                setRowMatrix(ref obj[i].T5, 0, sx, 0, 0, 0);
                setRowMatrix(ref obj[i].T5, 1, 0, sy, 0, 0);
                setRowMatrix(ref obj[i].T5, 2, 0, 0, sz, 0);
                setRowMatrix(ref obj[i].T5, 3, 0, 0, 0, 1);

                setRowMatrix(ref obj[i].T7, 0, 1, 0, 0, 0);
                setRowMatrix(ref obj[i].T7, 1, 0, 1, 0, 0);
                setRowMatrix(ref obj[i].T7, 2, 0, 0, 1, 0);
                setRowMatrix(ref obj[i].T7, 3, 0, 0, -(-obj[i].COP.z / (obj[i].COP.z - obj[i].BP)), 1);

                setRowMatrix(ref obj[i].T8, 0, (obj[i].COP.z - obj[i].BP) / obj[i].COP.z, 0, 0, 0);
                setRowMatrix(ref obj[i].T8, 1, 0, (obj[i].COP.z - obj[i].BP) / obj[i].COP.z, 0, 0);
                setRowMatrix(ref obj[i].T8, 2, 0, 0, 1, 0);
                setRowMatrix(ref obj[i].T8, 3, 0, 0, 0, 1);

                setRowMatrix(ref obj[i].T9, 0, 1, 0, 0, 0);
                setRowMatrix(ref obj[i].T9, 1, 0, 1, 0, 0);
                setRowMatrix(ref obj[i].T9, 2, 0, 0, 1, (-1 / (obj[i].COP.z / (obj[i].COP.z - obj[i].BP))));
                setRowMatrix(ref obj[i].T9, 3, 0, 0, 0, 1);

                if (translateRB.Checked)
                {
                    setRowMatrix(ref matrixTemp, 0, 1, 0, 0, 0);
                    setRowMatrix(ref matrixTemp, 1, 0, 1, 0, 0);
                    setRowMatrix(ref matrixTemp, 2, 0, 0, 1, 0);
                    setRowMatrix(ref matrixTemp, 3, obj[i].Wtdx, obj[i].Wtdy, obj[i].Wtdz, 1);
                }
                else if (rotateRB.Checked)
                {
                    if (onX)
                    {
                        setRowMatrix(ref matrixTemp, 0, 1, 0, 0, 0);
                        setRowMatrix(ref matrixTemp, 1, 0, Math.Cos(obj[i].WtAngle * Math.PI / 180), Math.Sin(obj[i].WtAngle * Math.PI / 180), 0);
                        setRowMatrix(ref matrixTemp, 2, 0, -Math.Sin(obj[i].WtAngle * Math.PI / 180), Math.Cos(obj[i].WtAngle * Math.PI / 180), 0);
                        setRowMatrix(ref matrixTemp, 3, 0, 0, 0, 1);
                    }
                    else if (onY)
                    {
                        setRowMatrix(ref matrixTemp, 0, Math.Cos(obj[i].WtAngle * Math.PI / 180), 0, -Math.Sin(obj[i].WtAngle * Math.PI / 180), 0);
                        setRowMatrix(ref matrixTemp, 1, 0, 1, 0, 0);
                        setRowMatrix(ref matrixTemp, 2, Math.Sin(obj[i].WtAngle * Math.PI / 180), 0, Math.Cos(obj[i].WtAngle * Math.PI / 180), 0);
                        setRowMatrix(ref matrixTemp, 3, 0, 0, 0, 1);
                    }
                    else if (onZ)
                    {
                        setRowMatrix(ref matrixTemp, 0, Math.Cos(obj[i].WtAngle * Math.PI / 180), Math.Sin(obj[i].WtAngle * Math.PI / 180), 0, 0);
                        setRowMatrix(ref matrixTemp, 1, -Math.Sin(obj[i].WtAngle * Math.PI / 180), Math.Cos(obj[i].WtAngle * Math.PI / 180), 0, 0);
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

                obj[i].Wt = matrixMultiplication(WtTemp, matrixTemp);

                obj[i].Pr1 = matrixMultiplication(matrixMultiplication(matrixMultiplication(matrixMultiplication(matrixMultiplication(matrixMultiplication(matrixMultiplication(obj[i].T1, obj[i].T2), obj[i].T3), obj[i].T4), obj[i].T5), obj[i].T7), obj[i].T8), obj[i].T9);

                for (int j = 0; j < 4; j++)
                {
                    obj[i].VW[j] = multiplyMatrix(obj[i].V[j], obj[i].Wt);
                    obj[i].VPr1[j] = multiplyMatrix(obj[i].VW[j], obj[i].Pr1);

                }

                obj[i].Snew = backFaceCulling(obj[i].S, obj[i].VW);


                for (int j = 0; j < 4; j++)
                {
                    obj[i].VV[j] = multiplyMatrix(obj[i].VPr1[j], Pr2);
                    obj[i].VS[j] = multiplyMatrix(obj[i].VV[j], St);
                }

                draw(obj[i].VS, obj[i].E, obj[i].S);
            }
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

            for (int i = 0; i < obj.Length; i++)
            {
                obj[i] = new TObject();
                obj[i].id = i;
                selectListBox.Items.Add(i);

                setPoint(ref obj[i].V[0], -1, -1, 1);
                setPoint(ref obj[i].V[1], 1, -1, 1);
                setPoint(ref obj[i].V[2], 0, 1, 0);
                setPoint(ref obj[i].V[3], 0, -1, -1);

                setLine(ref obj[i].E[0], 0, 1);
                setLine(ref obj[i].E[1], 1, 2);
                setLine(ref obj[i].E[2], 2, 0);
                setLine(ref obj[i].E[3], 2, 3);
                setLine(ref obj[i].E[4], 0, 3);
                setLine(ref obj[i].E[5], 1, 3);

                setSurface(ref obj[i].S[0], 0, 1, 2, Color.Red);
                setSurface(ref obj[i].S[1], 1, 3, 2, Color.Orange);
                setSurface(ref obj[i].S[2], 3, 0, 2, Color.Yellow);
                setSurface(ref obj[i].S[3], 3, 0, 1, Color.Green);

                setPoint(ref obj[i].VRP, 0, 0, 0);
                setPoint(ref obj[i].VPN, 0, 0, 1);
                setPoint(ref obj[i].VUP, 0, 1, 0);
                setPoint(ref obj[i].COP, 0, 0, 4);

                obj[i].windowUmin = -2;
                obj[i].windowVmin = -2;
                obj[i].windowUmax = 2;
                obj[i].windowVmax = 2;
                obj[i].FP = 2;
                obj[i].BP = -2;
            }

            display();
            debugTextBox.Text = debugText;
        }

        private void upBtn_Click(object sender, EventArgs e)
        {
            selectedObj = selectListBox.SelectedIndex;
            if (translateRB.Checked) obj[selectedObj].Wtdy++;
            else
            {
                onY = true;
                onX = onZ = false;
                obj[selectedObj].WtAngle += 1;
            }
            display();
        }

        private void downBtn_Click(object sender, EventArgs e)
        {
            if (translateRB.Checked) obj[selectedObj].Wtdy--;
            else
            {
                onY = true;
                onX = onZ = false;
                obj[selectedObj].WtAngle -= 1;
            }
            display();
        }

        private void frontBtn_Click(object sender, EventArgs e)
        {
            if (translateRB.Checked) obj[selectedObj].Wtdz++;
            else
            {
                onX = true;
                onY = onZ = false;
                obj[selectedObj].WtAngle += 1;
            }
            display();
        }

        private void backBtn_Click(object sender, EventArgs e)
        {
            if (translateRB.Checked) obj[selectedObj].Wtdz--;
            else
            {
                onX = true;
                onY = onZ = false;
                obj[selectedObj].WtAngle -= 1;
            }
            display();
        }

        private void leftBtn_Click(object sender, EventArgs e)
        {
            if (translateRB.Checked) obj[selectedObj].Wtdx--;
            else
            {
                onZ = true;
                onX = onY = false;
                obj[selectedObj].WtAngle -= 1;
            }
            display();
        }

        private void rightBtn_Click(object sender, EventArgs e)
        {
            if (translateRB.Checked) obj[selectedObj].Wtdx++;
            else
            {
                onZ = true;
                onX = onY = false;
                obj[selectedObj].WtAngle += 1;
            }
            display();
        }
    }
}
