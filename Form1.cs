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

        public struct TSurface
        {
            public int p1, p2, p3;
            public Color c;
        }

        public class TObject
        {
            public int id;
            public TPoint[] P;
            public TPoint[] VW;
            public TPoint[] VV;
            public TPoint[] VS;
            public TSurface[] S;
            public double[,] Wt = new double[4, 4] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } };
            public double[,] Translate = new double[4, 4] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } };
            public double[,] Rotate = new double[4, 4] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } };
            public TObject(int pNum, int sNum)
            {
                P = new TPoint[pNum];
                VW = new TPoint[pNum];
                VV = new TPoint[pNum];
                VS = new TPoint[pNum];
                S = new TSurface[sNum];
            }
        }

        
        















        public void setPoint(ref TPoint V, double x, double y, double z)
        {
            V.x = x;
            V.y = y;
            V.z = z;
            V.w = 1;
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





























        Bitmap bmp;
        Graphics g;
        TPoint VRP, VPN, VUP, COP, N, upUnit, upVec, v, u, CW, DOP = new TPoint();
        double windowUmin, windowVmin, windowUmax, windowVmax, FP, BP;
        public double[,] T1 = new double[4, 4];
        public double[,] T2 = new double[4, 4];
        public double[,] T3 = new double[4, 4];
        public double[,] T4 = new double[4, 4];
        public double[,] T5 = new double[4, 4];
        public double[,] T7 = new double[4, 4];
        public double[,] T8 = new double[4, 4];
        public double[,] T9 = new double[4, 4];
        public double[,] Pr1 = new double[4, 4];
        public double[,] Pr2 = new double[4, 4] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 1 } };
        public double[,] Vt = new double[4, 4];
        public double[,] St = new double[4, 4] { { 100, 0, 0, 0 }, { 0, -100, 0, 0 }, { 0, 0, 0, 0 }, { 200, 200, 0, 1 } };




        public void display()
        {
            // 1. Prepare the objects
            TObject obj = new TObject(4, 4);
            setPoint(ref obj.P[0], -1, -1, 1);
            setPoint(ref obj.P[1], 1, -1, 1);
            setPoint(ref obj.P[2], 0, 1, 0);
            setPoint(ref obj.P[3], 0, -1, -1);
            setSurface(ref obj.S[0], 0, 1, 2, Color.Red);
            setSurface(ref obj.S[1], 1, 3, 2, Color.Yellow);
            setSurface(ref obj.S[2], 3, 0, 2, Color.Green);
            setSurface(ref obj.S[3], 3, 0, 1, Color.Blue);

            // 2. Prepare the perspective projection parameters
            setPoint(ref VRP, 0, 0, 0);
            setPoint(ref VPN, 0, 0, 1);
            setPoint(ref VUP, 0, 1, 0);
            setPoint(ref COP, 0, 0, 4);
            windowUmin = -2;
            windowVmin = -2;
            windowUmax = 2;
            windowVmax = 2;
            FP = 2;
            BP = -2;

            // 3. Calculate the perspective projection derivative parameters
            // N
            N = unitVector(VPN);
            // v
            upUnit = unitVector(VUP);
            double temp;
            TPoint tempPoint = new TPoint();
            temp = dotProduct(upUnit, N);
            tempPoint.x = temp * N.x;
            tempPoint.y = temp * N.y;
            tempPoint.z = temp * N.z;
            setPoint(ref upVec, upUnit.x - tempPoint.x, upUnit.y - tempPoint.y, upUnit.z - tempPoint.z);
            v = unitVector(upVec);
            // u
            u = crossProduct(v, N);
            // CW
            setPoint(ref CW, (windowUmax + windowUmin) / 2, (windowVmax + windowVmin) / 2, 0);
            // DOP
            setPoint(ref DOP, (CW.x - COP.x), (CW.y - COP.y), (CW.z - COP.z));

            // 4. Calculate the perspective projection matrix
            // T1
            double rx = VRP.x;
            double ry = VRP.y;
            double rz = VRP.z;
            setRowMatrix(ref T1, 0, 1, 0, 0, 0);
            setRowMatrix(ref T1, 1, 0, 1, 0, 0);
            setRowMatrix(ref T1, 2, 0, 0, 1, 0);
            setRowMatrix(ref T1, 3, -rx, -ry, -rz, 1);
            // T2
            setRowMatrix(ref T2, 0, u.x, v.x, N.x, 0);
            setRowMatrix(ref T2, 1, u.y, v.y, N.y, 0);
            setRowMatrix(ref T2, 2, u.z, v.z, N.z, 0);
            setRowMatrix(ref T2, 3, 0, 0, 0, 1);
            // T3
            setRowMatrix(ref T3, 0, 1, 0, 0, 0);
            setRowMatrix(ref T3, 1, 0, 1, 0, 0);
            setRowMatrix(ref T3, 2, 0, 0, 1, 0);
            setRowMatrix(ref T3, 3, -COP.x, -COP.y, -COP.z, 1);
            // T4
            double shx = -DOP.x / DOP.z;
            double shy = -DOP.y / DOP.z;
            setRowMatrix(ref T4, 0, 1, 0, 0, 0);
            setRowMatrix(ref T4, 1, 0, 1, 0, 0);
            setRowMatrix(ref T4, 2, shx, shy, 1, 0);
            setRowMatrix(ref T4, 3, 0, 0, 0, 1);
            // T5
            double w, h;
            w = ((COP.z - BP) * (windowUmax - windowUmin)) / (2 * COP.z);
            h = ((COP.z - BP) * (windowVmax - windowVmin)) / (2 * COP.z);
            double sx = 1 / w;
            double sy = 1 / h;
            double sz = -1 / (BP - COP.z);
            setRowMatrix(ref T5, 0, sx, 0, 0, 0);
            setRowMatrix(ref T5, 1, 0, sy, 0, 0);
            setRowMatrix(ref T5, 2, 0, 0, sz, 0);
            setRowMatrix(ref T5, 3, 0, 0, 0, 1);
            // T7
            setRowMatrix(ref T7, 0, 1, 0, 0, 0);
            setRowMatrix(ref T7, 1, 0, 1, 0, 0);
            setRowMatrix(ref T7, 2, 0, 0, 1, 0);
            setRowMatrix(ref T7, 3, 0, 0, -(-COP.z / (COP.z - BP)), 1);
            // T8
            setRowMatrix(ref T8, 0, (COP.z - BP) / COP.z, 0, 0, 0);
            setRowMatrix(ref T8, 1, 0, (COP.z - BP) / COP.z, 0, 0);
            setRowMatrix(ref T8, 2, 0, 0, 1, 0);
            setRowMatrix(ref T8, 3, 0, 0, 0, 1);
            // T9
            setRowMatrix(ref T9, 0, 1, 0, 0, 0);
            setRowMatrix(ref T9, 1, 0, 1, 0, 0);
            setRowMatrix(ref T9, 2, 0, 0, 1, (-1 / (COP.z / (COP.z - BP))));
            setRowMatrix(ref T9, 3, 0, 0, 0, 1);

            Pr1 = matrixMultiplication(matrixMultiplication(matrixMultiplication(matrixMultiplication(matrixMultiplication(matrixMultiplication(matrixMultiplication(T1, T2), T3), T4), T5), T7), T8), T9);
            Vt = matrixMultiplication(Pr1, Pr2);

            for (int i=0; i<obj.P.Length; i++)
            {
                obj.VW[i] = multiplyMatrix(obj.P[i], obj.Wt);
                obj.VV[i] = multiplyMatrix(obj.VW[i], Vt);
                obj.VS[i] = multiplyMatrix(obj.VV[i], St);
            }

            draw(obj.S, obj.VS);
        }

        public void draw(TSurface[] S, TPoint[] VS)
        {
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bmp);
            Pen redPen = new Pen(Color.Red);
            Pen blackPen = new Pen(Color.Black);
            TPoint p1, p2, p3;
            for(int i=0; i<S.Length; i++)
            {
                p1 = VS[S[i].p1];
                p2 = VS[S[i].p2];
                p3 = VS[S[i].p3];
                g.DrawLine(blackPen, new Point(Convert.ToInt32(p1.x), Convert.ToInt32(p1.y)), new Point(Convert.ToInt32(p2.x), Convert.ToInt32(p2.y)));
                g.DrawLine(blackPen, new Point(Convert.ToInt32(p2.x), Convert.ToInt32(p2.y)), new Point(Convert.ToInt32(p3.x), Convert.ToInt32(p3.y)));
                g.DrawLine(blackPen, new Point(Convert.ToInt32(p3.x), Convert.ToInt32(p3.y)), new Point(Convert.ToInt32(p1.x), Convert.ToInt32(p1.y)));
            }
            pictureBox1.Image = bmp;
        }


















        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            translateRB.Checked = true;

            display();
        }


    }
}
