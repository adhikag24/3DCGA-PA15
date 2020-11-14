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
            public int l1, l2, l3;
        }
        public class TObject
        {
            public int id;
            public double windowUmin, windowVmin, windowUmax, windowVmax, FP, BP;
            public TPoint[] V = new TPoint[4];
            public TPoint[] VW = new TPoint[4];
            public TPoint[] VV = new TPoint[4];
            public TPoint[] VS = new TPoint[4];
            public TLine[] E = new TLine[6];
            public TSurface[] S = new TSurface[4];
            public TSurface[] Snew = new TSurface[4];
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
        }

        Bitmap bmp;
        Graphics g;
        public double[,] Wt = new double[4, 4];
        public double[,] Vt = new double[4, 4];
        public double[,] St = new double[4, 4];
        public double[,] Pr2 = new double[4, 4];

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
        public void setSurface(ref TSurface S, int l1, int l2, int l3)
        {
            S.l1 = l1;
            S.l2 = l2;
            S.l3 = l3;
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

        public TSurface[] backFaceCulling(TSurface[] surfaces, TPoint viewerNormal)
        {
            TSurface[] temp = new TSurface[4];
            return temp;
            //Continue...
        }

        public void draw(TPoint[] VS, TLine[] E)
        {
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bmp);
            Pen redPen = new Pen(Color.Red);
            Pen blackPen = new Pen(Color.Black);
            TPoint p1, p2;
            for (int i = 0; i < 6; i++)
            {
                p1 = VS[E[i].p1];
                p2 = VS[E[i].p2];
                g.DrawLine(blackPen, (float)p1.x, (float)p1.y, (float)p2.x, (float)p2.y);
            }
            pictureBox1.Image = bmp;
        }

        public void display()
        {
            TObject obj = new TObject();
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
            setSurface(ref obj.S[0], 0, 1, 2);
            setSurface(ref obj.S[1], 5, 3, 1);
            setSurface(ref obj.S[2], 3, 4, 2);
            setSurface(ref obj.S[3], 4, 0, 5);

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

            double temp;
            TPoint tempPoint;

            temp = Math.Sqrt(Math.Pow(obj.VPN.x, 2) + Math.Pow(obj.VPN.y, 2) + Math.Pow(obj.VPN.z, 2));
            setPoint(ref obj.N, obj.VPN.x / temp, obj.VPN.y / temp, obj.VPN.z / temp);

            temp = Math.Sqrt(Math.Pow(obj.VUP.x, 2) + Math.Pow(obj.VUP.y, 2) + Math.Pow(obj.VUP.z, 2));
            setPoint(ref obj.upUnit, obj.VUP.x / temp, obj.VUP.y / temp, obj.VUP.z / temp);

            tempPoint = new TPoint();
            temp = obj.upUnit.x * obj.N.x + obj.upUnit.y * obj.N.y + obj.upUnit.z * obj.N.z;
            tempPoint.x = temp * obj.N.x;
            tempPoint.y = temp * obj.N.y;
            tempPoint.z = temp * obj.N.z;
            setPoint(ref obj.upVec, obj.upUnit.x - tempPoint.x, obj.upUnit.y - tempPoint.y, obj.upUnit.z - tempPoint.z);

            temp = Math.Sqrt(Math.Pow(obj.upVec.x, 2) + Math.Pow(obj.upVec.y, 2) + Math.Pow(obj.upVec.z, 2));
            setPoint(ref obj.v, obj.upVec.x / temp, obj.upVec.y / temp, obj.upVec.z / temp);

            tempPoint.x = (obj.v.y * obj.N.z) - (obj.N.y * obj.v.z);
            tempPoint.y = (obj.v.z * obj.N.x) - (obj.N.z * obj.v.x);
            tempPoint.z = (obj.v.x * obj.N.y) - (obj.N.x * obj.v.y);
            setPoint(ref obj.u, tempPoint.x, tempPoint.y, tempPoint.z);

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

            obj.Pr1 = matrixMultiplication(matrixMultiplication(matrixMultiplication(matrixMultiplication(matrixMultiplication(matrixMultiplication(matrixMultiplication(obj.T1, obj.T2), obj.T3), obj.T4), obj.T5), obj.T7), obj.T8), obj.T9);

            Vt = matrixMultiplication(obj.Pr1, Pr2);

            for (int i = 0; i < 4; i++)
            {
                obj.VW[i] = multiplyMatrix(obj.V[i], Wt);
                obj.VV[i] = multiplyMatrix(obj.VW[i], Vt);
                obj.VS[i] = multiplyMatrix(obj.VV[i], St);
            }

            draw(obj.VS, obj.E);
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            setRowMatrix(ref Wt, 0, 1, 0, 0, 0);
            setRowMatrix(ref Wt, 1, 0, 1, 0, 0);
            setRowMatrix(ref Wt, 2, 0, 0, 1, 0);
            setRowMatrix(ref Wt, 3, 0, 0, 0, 1);

            setRowMatrix(ref St, 0, 100, 0, 0, 0);
            setRowMatrix(ref St, 1, 0, -100, 0, 0);
            setRowMatrix(ref St, 2, 0, 0, 0, 0);
            setRowMatrix(ref St, 3, 200, 200, 0, 1);

            setRowMatrix(ref Pr2, 0, 1, 0, 0, 0);
            setRowMatrix(ref Pr2, 1, 0, 1, 0, 0);
            setRowMatrix(ref Pr2, 2, 0, 0, 0, 0);
            setRowMatrix(ref Pr2, 3, 0, 0, 0, 1);

            display();
        }
    }
}
