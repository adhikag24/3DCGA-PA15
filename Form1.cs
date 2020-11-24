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
            public TPoint[] VPr1;
            public TPoint[] VV;
            public TPoint[] VS;
            public TSurface[] S;
            public List<int> visibleSurfaceIndex = new List<int>();
            public double[,] TranslateM;
            public double[,] RotateM;
            public TObject(int pNum, int sNum)
            {
                P = new TPoint[pNum];
                VW = new TPoint[pNum];
                VPr1 = new TPoint[pNum];
                VV = new TPoint[pNum];
                VS = new TPoint[pNum];
                S = new TSurface[sNum];
            }
        }
        public class SETElement
        {
            public int ymax, xofymin, dx, dy, carrier = 0;
        }









        Bitmap bmp;
        Graphics g;
        public TObject[] obj = new TObject[2];
        public string debug = "";
        TPoint VRP, VPN, VUP, COP, N, upUnit, upVec, v, u, CW, DOP, VN = new TPoint();
        double windowUmin, windowVmin, windowUmax, windowVmax, BP;
        public int selectedObjectIndex;
        public double[,] T1 = new double[4, 4];
        public double[,] T2 = new double[4, 4];
        public double[,] T3 = new double[4, 4];
        public double[,] T4 = new double[4, 4];
        public double[,] T5 = new double[4, 4];
        public double[,] T7 = new double[4, 4];
        public double[,] T8 = new double[4, 4];
        public double[,] T9 = new double[4, 4];
        public double[,] Pr1 = new double[4, 4];
        public double[,] Pr2 = new double[4, 4] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } };
        public double[,] Wt = new double[4, 4];
        public double[,] Vt = new double[4, 4];
        public double[,] St = new double[4, 4] { { 100, 0, 0, 0 }, { 0, -100, 0, 0 }, { 0, 0, 1, 0 }, { 400, 200, 0, 1 } };


















        public void SetPoint(ref TPoint V, double x, double y, double z)
        {
            V.x = x;
            V.y = y;
            V.z = z;
            V.w = 1;
        }
        public void SetSurface(ref TSurface S, int p1, int p2, int p3, Color c)
        {
            S.p1 = p1;
            S.p2 = p2;
            S.p3 = p3;
            S.c = c;
        }
        public void SetRowMatrix(ref double[,] M, int row, double a, double b, double c, double d)
        {
            M[row, 0] = a;
            M[row, 1] = b;
            M[row, 2] = c;
            M[row, 3] = d;
        }
        public TPoint MultiplyMatrix(TPoint P, double[,] M)
        {
            TPoint temp;
            double w = P.x * M[0, 3] + P.y * M[1, 3] + P.z * M[2, 3] + P.w * M[3, 3];
            temp.x = (P.x * M[0, 0] + P.y * M[1, 0] + P.z * M[2, 0] + P.w * M[3, 0]) / w;
            temp.y = (P.x * M[0, 1] + P.y * M[1, 1] + P.z * M[2, 1] + P.w * M[3, 1]) / w;
            temp.z = (P.x * M[0, 2] + P.y * M[1, 2] + P.z * M[2, 2] + P.w * M[3, 2]) / w;
            temp.w = 1;
            return temp;
        }
        public double[,] MatrixMultiplication(double[,] M1, double[,] M2)
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
        public TPoint FindVector(TPoint p1, TPoint p2)
        {
            TPoint temp;
            temp.x = p2.x - p1.x;
            temp.y = p2.y - p1.y;
            temp.z = p2.z - p1.z;
            temp.w = 1;
            return temp;
        }
        public double DotProduct(TPoint P1, TPoint P2)
        {
            double temp;
            temp = P1.x * P2.x + P1.y * P2.y + P1.z * P2.z;
            return temp;
        }
        public TPoint CrossProduct(TPoint P1, TPoint P2)
        {
            TPoint tempPoint;
            tempPoint.x = (P1.y * P2.z) - (P2.y * P1.z);
            tempPoint.y = (P1.z * P2.x) - (P2.z * P1.x);
            tempPoint.z = (P1.x * P2.y) - (P2.x * P1.y);
            tempPoint.w = 1;
            return tempPoint;
        }
        public TPoint UnitVector(TPoint P)
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





        public void ResetTransM(int index)
        {
            obj[index].TranslateM = new double[4,4] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } };
            obj[index].RotateM = new double[4,4] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } };
        }

        public void TranslateObject(int index, double dx=0, double dy=0, double dz=0)
        {
            double[,] temp = new double[4, 4] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { dx, dy, dz, 1 } };
            obj[index].TranslateM = MatrixMultiplication(obj[index].TranslateM, temp);
        }
        public void RotateObjectOnX(int index, double angle)
        {
            double dx, dy, dz;
            dx = obj[index].TranslateM[3, 0];
            dy = obj[index].TranslateM[3, 1];
            dz = obj[index].TranslateM[3, 2];
            double[,] minTranslate = new double[4, 4] { { 1, 0, 0, 0}, { 0, 1, 0, 0}, { 0, 0, 1, 0}, { -dx, -dy, -dz, 1} };
            double[,] temp = new double[4, 4]
            {
            {1, 0, 0, 0 },
            {0, Math.Cos(angle * Math.PI/180), Math.Sin(angle * Math.PI/180), 0},
            {0, -Math.Sin(angle * Math.PI/180), Math.Cos(angle * Math.PI/180), 0},
            {0, 0, 0, 1 }
            };
            double[,] res1 = MatrixMultiplication(MatrixMultiplication(minTranslate, temp), obj[index].TranslateM);
            obj[index].RotateM = MatrixMultiplication(obj[index].RotateM, res1);
        }
        public void RotateObjectOnY(int index, double angle)
        {
            double dx, dy, dz;
            dx = obj[index].TranslateM[3, 0];
            dy = obj[index].TranslateM[3, 1];
            dz = obj[index].TranslateM[3, 2];
            double[,] minTranslate = new double[4, 4] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { -dx, -dy, -dz, 1 } };
            double[,] temp = new double[4, 4]
            {
                {Math.Cos(angle * Math.PI/180), 0,  -Math.Sin(angle * Math.PI/180), 0 },
                {0, 1, 0, 0 },
                {Math.Sin(angle * Math.PI/180), 0,Math.Cos(angle * Math.PI/180), 0 },
                { 0 ,0, 0, 1 }
            };
            double[,] res1 = MatrixMultiplication(MatrixMultiplication(minTranslate, temp), obj[index].TranslateM);
            obj[index].RotateM = MatrixMultiplication(obj[index].RotateM, res1);
        }
        public void RotateObjectOnZ(int index, double angle)
        {
            double dx, dy, dz;
            dx = obj[index].TranslateM[3, 0];
            dy = obj[index].TranslateM[3, 1];
            dz = obj[index].TranslateM[3, 2];
            double[,] minTranslate = new double[4, 4] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { -dx, -dy, -dz, 1 } };
            double[,] temp = new double[4, 4]
            {
                {Math.Cos(angle * Math.PI/180), Math.Sin(angle * Math.PI/180), 0, 0 },
                {-Math.Sin(angle * Math.PI/180), Math.Cos(angle * Math.PI/180), 0, 0 },
                {0, 0, 1, 0 },
                {0, 0, 0, 1 }
            };
            double[,] res1 = MatrixMultiplication(MatrixMultiplication(minTranslate, temp), obj[index].TranslateM);
            obj[index].RotateM = MatrixMultiplication(obj[index].RotateM, res1);
        }





        public void BackfaceCulling(int index, TSurface[] S, TPoint[] V)
        {
            obj[index].visibleSurfaceIndex.Clear();
            TPoint p1, p2, p3, p1p2, p2p3, N;
            double res;
            for (int i = 0; i < S.Length; i++)
            {
                p1 = V[S[i].p1];
                //debug += "p1 => " + p1.x.ToString() + "  " + p1.y.ToString() + "  " + p1.z.ToString() + Environment.NewLine;
                p2 = V[S[i].p2];
                //debug += "p2 => " + p2.x.ToString() + "  " + p2.y.ToString() + "  " + p2.z.ToString() + Environment.NewLine;
                p3 = V[S[i].p3];
                //debug += "p3 => " + p3.x.ToString() + "  " + p3.y.ToString() + "  " + p3.z.ToString() + Environment.NewLine;
                p1p2 = FindVector(p1, p2);
                //debug += "p1p2 => " + p1p2.x.ToString() + "  " + p1p2.y.ToString() + "  " + p1p2.z.ToString() + Environment.NewLine;
                p2p3 = FindVector(p2, p3);
                //debug += "p2p3 => " + p2p3.x.ToString() + "  " + p2p3.y.ToString() + "  " + p2p3.z.ToString() + Environment.NewLine;
                N = CrossProduct(p1p2, p2p3);
                //debug += "N => " + N.x.ToString() + "  " + N.y.ToString() + "  " + N.z.ToString() + Environment.NewLine;
                SetPoint(ref VN, 0, 0, -1);
                res = DotProduct(VN, N);
                //debug += res.ToString() + Environment.NewLine;
                //debug += Environment.NewLine;
                if (res < 0) obj[index].visibleSurfaceIndex.Add(i);
            }
        }
        public void PolygonFill(TPoint[] P, Pen pen)
        {
            int dx, dy, ymin, ymax, xofymin; // Declare the variable needed for polygon scanline fill
            int wholeymin = 9999; // Initiate and set the wholeymin
            int wholeymax = 0; // Initiate and set the wholeymax

            var indexItemCount = new Int16[pictureBox1.Width];
            for (int i = 0; i < pictureBox1.Width; i++)
            {
                indexItemCount[i] = 0;
            }

            var SET = new SETElement[pictureBox1.Height, pictureBox1.Width]; // Create 2d array of SET element

            for (int i=0; i<P.Length; i++)
            {
                TPoint first_vertices, second_vertices;
                int x1, y1, x2, y2;
                SETElement se = new SETElement();
                if(i == P.Length-1)
                {
                    first_vertices = P[i];
                    second_vertices = P[0];
                    x1 = Convert.ToInt32(first_vertices.x);
                    y1 = Convert.ToInt32(first_vertices.y);
                    x2 = Convert.ToInt32(second_vertices.x);
                    y2 = Convert.ToInt32(second_vertices.y);
                    g.DrawLine(pen, x1, y1, x2, y2);

                    dx = x2 - x1;
                    dy = y2 - y1;
                    if (y1 > y2)
                    {
                        ymax = y1;
                        ymin = y2;
                    }
                    else
                    {
                        ymax = y2;
                        ymin = y1;
                    }
                    if (ymin == y1) xofymin = x1;
                    else xofymin = x2;
                    if (dy < 0)
                    {
                        dy *= -1;
                        dx *= -1;
                    }
                    if (dy != 0)
                    {
                        if (ymin < wholeymin) wholeymin = ymin;

                        if (ymax > wholeymax) wholeymax = ymax;

                        se.ymax = ymax;
                        se.xofymin = xofymin;
                        se.dx = dx;
                        se.dy = dy;

                        try
                        {
                            SET[ymin, indexItemCount[ymin]] = se;
                            indexItemCount[ymin]++;
                        }
                        catch(Exception e)
                        {
                            MessageBox.Show("Polygon reached the screen limit!");
                            Console.WriteLine(e);
                        }
                    }
                }
                else
                {
                    first_vertices = P[i];
                    second_vertices = P[i+1];
                    x1 = Convert.ToInt32(first_vertices.x);
                    y1 = Convert.ToInt32(first_vertices.y);
                    x2 = Convert.ToInt32(second_vertices.x);
                    y2 = Convert.ToInt32(second_vertices.y);
                    g.DrawLine(pen, x1, y1, x2, y2);

                    dx = x2 - x1;
                    dy = y2 - y1;

                    if (y1 > y2)
                    {
                        ymax = y1;
                        ymin = y2;
                    }
                    else
                    {
                        ymax = y2;
                        ymin = y1;
                    }

                    if (ymin == y1) xofymin = x1;
                    else xofymin = x2;

                    if (dy < 0)
                    {
                        dy *= -1;
                        dx *= -1;
                    }

                    if (dy != 0)
                    {
                        if (ymin < wholeymin) wholeymin = ymin;

                        if (ymax > wholeymax) wholeymax = ymax;

                        se.ymax = ymax;
                        se.xofymin = xofymin;
                        se.dx = dx;
                        se.dy = dy;

                        try
                        {
                            SET[ymin, indexItemCount[ymin]] = se;
                            indexItemCount[ymin]++;
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Polygon reached the screen limit!");
                            Console.WriteLine(e);
                        }
                    }
                }
            }
            ConstructAEL(SET, wholeymin, wholeymax, pen);
        }

        public SETElement[] YmaxCheck(SETElement[] cr, int index, int currentY)
        {
            int new_index = 0;
            var coppied_row = new SETElement[pictureBox1.Width];
            cr.CopyTo(coppied_row, 0);
            for (int i = 0; i < index + 1; i++)
            {
                if (coppied_row[i] != null)
                {
                    if (coppied_row[i].ymax < currentY)
                    {
                        coppied_row[i] = null;
                    }
                }

            }

            var checkedCurrentRow = new SETElement[pictureBox1.Width];
            for (int i = 0; i < index + 1; i++)
            {
                if (coppied_row[i] != null)
                {
                    checkedCurrentRow[new_index] = coppied_row[i];
                    new_index++;
                }
            }

            return checkedCurrentRow;
        }

        public SETElement[] ProcessCurrentRow(SETElement[] cr, int index)
        {
            var processed = new SETElement[pictureBox1.Width];
            cr.CopyTo(processed, 0);
            for (int i = 0; i < index + 1; i++)
            {
                if (processed[i] != null)
                {
                    processed[i].carrier += processed[i].dx;

                    if (processed[i].carrier >= processed[i].dy)
                    {
                        while (processed[i].carrier >= processed[i].dy)
                        {
                            processed[i].xofymin++;
                            processed[i].carrier -= processed[i].dy;
                        }
                    }
                    else if (processed[i].carrier < 0)
                    {
                        while (processed[i].carrier < 0)
                        {
                            processed[i].xofymin--;
                            processed[i].carrier += processed[i].dy;
                        }
                    }
                }
            }
            return processed;
        }

        public SETElement[] SortCurrentRow(SETElement[] cr, int index)
        {
            var sorted = new SETElement[pictureBox1.Width];
            cr.CopyTo(sorted, 0);
            SETElement temp = new SETElement();
            for (int i = 0; i < index + 1; i++)
            {
                for (int j = 0; j < index; j++)
                {
                    if (sorted[j + 1] != null)
                    {
                        if (sorted[j].xofymin > sorted[j + 1].xofymin)
                        {
                            temp = sorted[j];
                            sorted[j] = sorted[j + 1];
                            sorted[j + 1] = temp;
                        }
                    }
                }
            }
            return sorted;
        }

        public void ConstructAEL(SETElement[,] se, int wholeymin, int wholeymax, Pen pen)
        {
            var current_row = new SETElement[pictureBox1.Width];
            int current_row_index = 0;
            for (int i = wholeymin; i <= wholeymax; i++)
            {

                current_row = YmaxCheck(current_row, current_row_index, i);

                current_row = ProcessCurrentRow(current_row, current_row_index);

                current_row = SortCurrentRow(current_row, current_row_index);

                var temp_row = new SETElement[current_row_index + 1];
                int temp_counter = 0;

                for (int k = 0; k < pictureBox1.Width; k++)
                {
                    if (current_row[k] != null)
                    {
                        temp_row[temp_counter] = current_row[k];
                        temp_counter++;
                    }
                }
                for (int j = 0; j < pictureBox1.Width; j++)
                {
                    if (se[i, j] != null)
                    {
                        current_row[current_row_index] = se[i, j];
                        current_row_index++;
                    }
                }
                for (int x = 0; x < temp_counter + 1; x++)
                {
                    if (x < temp_counter)
                    {
                        if (x % 2 == 0)
                        {
                            g.DrawLine(pen, temp_row[x].xofymin, i, temp_row[x + 1].xofymin, i);
                        }
                    }
                }
                pictureBox1.Image = bmp;
            }
        }

        public void Display()
        {
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bmp);

            // 2. Prepare the perspective projection parameters
            SetPoint(ref VRP, 0, 0, 0);
            SetPoint(ref VPN, 0, 0, 1);
            SetPoint(ref VUP, 0, 1, 0);
            SetPoint(ref COP, 0, 0, 4);
            windowUmin = -2;
            windowVmin = -2;
            windowUmax = 2;
            windowVmax = 2;
            BP = -2;

            // 3. Calculate the perspective projection derivative parameters
            // N
            N = UnitVector(VPN);
            // v
            upUnit = UnitVector(VUP);
            double temp;
            TPoint tempPoint = new TPoint();
            temp = DotProduct(upUnit, N);
            tempPoint.x = temp * N.x;
            tempPoint.y = temp * N.y;
            tempPoint.z = temp * N.z;
            SetPoint(ref upVec, upUnit.x - tempPoint.x, upUnit.y - tempPoint.y, upUnit.z - tempPoint.z);
            v = UnitVector(upVec);
            // u
            u = CrossProduct(v, N);
            // CW
            SetPoint(ref CW, (windowUmax + windowUmin) / 2, (windowVmax + windowVmin) / 2, 0);
            // DOP
            SetPoint(ref DOP, (CW.x - COP.x), (CW.y - COP.y), (CW.z - COP.z));

            // 4. Calculate the perspective projection matrix
            // T1
            double rx = VRP.x;
            double ry = VRP.y;
            double rz = VRP.z;
            SetRowMatrix(ref T1, 0, 1, 0, 0, 0);
            SetRowMatrix(ref T1, 1, 0, 1, 0, 0);
            SetRowMatrix(ref T1, 2, 0, 0, 1, 0);
            SetRowMatrix(ref T1, 3, -rx, -ry, -rz, 1);
            // T2
            SetRowMatrix(ref T2, 0, u.x, v.x, N.x, 0);
            SetRowMatrix(ref T2, 1, u.y, v.y, N.y, 0);
            SetRowMatrix(ref T2, 2, u.z, v.z, N.z, 0);
            SetRowMatrix(ref T2, 3, 0, 0, 0, 1);
            // T3
            SetRowMatrix(ref T3, 0, 1, 0, 0, 0);
            SetRowMatrix(ref T3, 1, 0, 1, 0, 0);
            SetRowMatrix(ref T3, 2, 0, 0, 1, 0);
            SetRowMatrix(ref T3, 3, -COP.x, -COP.y, -COP.z, 1);
            // T4
            double shx = -DOP.x / DOP.z;
            double shy = -DOP.y / DOP.z;
            SetRowMatrix(ref T4, 0, 1, 0, 0, 0);
            SetRowMatrix(ref T4, 1, 0, 1, 0, 0);
            SetRowMatrix(ref T4, 2, shx, shy, 1, 0);
            SetRowMatrix(ref T4, 3, 0, 0, 0, 1);
            // T5
            double w, h;
            w = ((COP.z - BP) * (windowUmax - windowUmin)) / (2 * COP.z);
            h = ((COP.z - BP) * (windowVmax - windowVmin)) / (2 * COP.z);
            double sx = 1 / w;
            double sy = 1 / h;
            double sz = -1 / (BP - COP.z);
            SetRowMatrix(ref T5, 0, sx, 0, 0, 0);
            SetRowMatrix(ref T5, 1, 0, sy, 0, 0);
            SetRowMatrix(ref T5, 2, 0, 0, sz, 0);
            SetRowMatrix(ref T5, 3, 0, 0, 0, 1);
            // T7
            SetRowMatrix(ref T7, 0, 1, 0, 0, 0);
            SetRowMatrix(ref T7, 1, 0, 1, 0, 0);
            SetRowMatrix(ref T7, 2, 0, 0, 1, 0);
            SetRowMatrix(ref T7, 3, 0, 0, -(-COP.z / (COP.z - BP)), 1);
            // T8
            SetRowMatrix(ref T8, 0, (COP.z - BP) / COP.z, 0, 0, 0);
            SetRowMatrix(ref T8, 1, 0, (COP.z - BP) / COP.z, 0, 0);
            SetRowMatrix(ref T8, 2, 0, 0, 1, 0);
            SetRowMatrix(ref T8, 3, 0, 0, 0, 1);
            // T9
            SetRowMatrix(ref T9, 0, 1, 0, 0, 0);
            SetRowMatrix(ref T9, 1, 0, 1, 0, 0);
            SetRowMatrix(ref T9, 2, 0, 0, 1, (-1 / (COP.z / (COP.z - BP))));
            SetRowMatrix(ref T9, 3, 0, 0, 0, 1);

            Pr1 = MatrixMultiplication(MatrixMultiplication(MatrixMultiplication(MatrixMultiplication(MatrixMultiplication(MatrixMultiplication(MatrixMultiplication(T1, T2), T3), T4), T5), T7), T8), T9);

            for (int i = 0; i < obj.Length; i++)
            {
                Wt = MatrixMultiplication(obj[i].TranslateM, obj[i].RotateM);
                for (int j = 0; j < obj[i].P.Length; j++)
                {
                    obj[i].VW[j] = MultiplyMatrix(obj[i].P[j], Wt);
                    obj[i].VPr1[j] = MultiplyMatrix(obj[i].VW[j], Pr1);
                }
                BackfaceCulling(i, obj[i].S, obj[i].VPr1);
                for (int j = 0; j < obj[i].P.Length; j++)
                {
                    obj[i].VV[j] = MultiplyMatrix(obj[i].VPr1[j], Pr2);
                    obj[i].VS[j] = MultiplyMatrix(obj[i].VV[j], St);
                }
            }
            Draw();
            debug = "";
            GeneratePolygonList();
            Warnock(0, 0, pictureBox1.Width, pictureBox1.Height);
            ////debug += GetClosestPixelColor(400, 200) + Environment.NewLine;
            //TPolygon temp1 = IsOnlyOneIntersecting(0, 0, pictureBox1.Width / 2, pictureBox1.Height / 2);
            //if(temp1 != null) debug += temp1.c + Environment.NewLine;
            //debug += Environment.NewLine;

            //TPolygon temp2 = IsOnlyOneContained(0, 0, pictureBox1.Width / 2, pictureBox1.Height / 2);
            //if (temp2 != null) debug += temp2.c + Environment.NewLine;
            //debug += Environment.NewLine;

            //TPolygon temp3 = IsOnlyOneSurrounding(250, 250, 275, 275);
            //if (temp3 != null) debug += temp3.c + Environment.NewLine;
            //debug += Environment.NewLine;
            //TPolygon temp1 = new TPolygon();
            //TPoint temp_p1 = new TPoint();
            //TPoint temp_p2 = new TPoint();
            //TPoint temp_p3 = new TPoint();
            //SetPoint(ref temp_p1, 2, 5, 1);
            //SetPoint(ref temp_p2, 5, 4, 1);
            //SetPoint(ref temp_p3, 6, 9, 1);
            //List<TPoint> tempPList = new List<TPoint>();
            //tempPList.Add(temp_p1);
            //tempPList.Add(temp_p2);
            //tempPList.Add(temp_p3);
            //temp1.P = tempPList;
            //temp1.c = Color.Red;
            //TPolygon clipped = polygonClip(temp1, 3, 3, 8, 7);
            //for (int i = 0; i < clipped.P.Count; i++)
            //{
            //    debug += i + " => (" + clipped.P[i].x + "    " + clipped.P[i].y + "    " + clipped.P[i].z + ")" + Environment.NewLine;
            //}

            //Warnock(0, 0, pictureBox1.Width, pictureBox1.Height);
            debugTextBox.Text = debug;
        }

        public class TPolygon
        {
            public List<TPoint> P;
            public Color c;
        }

        List<TPolygon> polygon_list = new List<TPolygon>();


        public void GeneratePolygonList()
        {
            polygon_list.Clear();
            for (int i=0; i<obj.Length; i++)
            {
                for(int j=0; j<obj[i].S.Length; j++)
                {
                    if(obj[i].visibleSurfaceIndex.Contains(j))
                    {
                        List<TPoint> P = new List<TPoint>();
                        P.Add(obj[i].VS[obj[i].S[j].p1]);
                        P.Add(obj[i].VS[obj[i].S[j].p2]);
                        P.Add(obj[i].VS[obj[i].S[j].p3]);
                        TPolygon tempPolygon = new TPolygon();
                        tempPolygon.P = P;
                        tempPolygon.c = obj[i].S[j].c;
                        polygon_list.Add(tempPolygon);
                    }
                }
            }

            debug = "";
            for (int i = 0; i < polygon_list.Count; i++)
            {
                debug += "Polygon " + (i + 1) + Environment.NewLine;
                for (int j = 0; j < polygon_list[i].P.Count; j++)
                {
                    debug += "P[" + (j + 1) + "] => (" + polygon_list[i].P[j].x + "    " + polygon_list[i].P[j].y + "    " + polygon_list[i].P[j].z + ")" + Environment.NewLine;
                }
                debug += Environment.NewLine;
            }
        }


        public double CrossProduct2D(TPoint p1, TPoint p2)
        {
            return (p1.x * p2.y) - (p2.x * p1.y);
        }

        public double Sign(TPoint p, TPoint p1, TPoint p2)
        {
            TPoint p1p = FindVector(p, p1);
            TPoint p1p2 = FindVector(p1, p2);
            double temp = CrossProduct2D(p1p, p1p2);
            return temp;
        }

        public bool IsPointInTriangle(TPoint pt, TPoint v1, TPoint v2, TPoint v3)
        {
            double d1, d2, d3;
            d1 = Sign(pt, v1, v2);
            d2 = Sign(pt, v2, v3);
            d3 = Sign(pt, v3, v1);
            return ((d1 < 0) && (d2 < 0) && (d3 < 0)) || ((d1 > 0) && (d2 > 0) && (d3 > 0));
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            label5.Text = e.X.ToString();
            label6.Text = e.Y.ToString();
        }

        public Color GetClosestPixelColor(int x, int y)
        {
            double zmax = 0;
            TPoint tempPoint = new TPoint();
            tempPoint.x = x;
            tempPoint.y = y;
            tempPoint.z = 0;
            Color zmaxColor = Color.White;
            for (int i = 0; i < polygon_list.Count; i++)
            {
                TPoint p1 = polygon_list[i].P[0];
                TPoint p2 = polygon_list[i].P[1];
                TPoint p3 = polygon_list[i].P[2];
                if (IsPointInTriangle(tempPoint, p1, p2, p3))
                {
                    //debug += obj[i].S[j].c + " => ";
                    TPoint p1p2 = FindVector(p1, p2);
                    TPoint p2p3 = FindVector(p2, p3);
                    TPoint N = CrossProduct(p1p2, p2p3);
                    double d = -(N.x * p1.x + N.y * p1.y + N.z * p1.z);
                    double t = -(d + N.x * x + N.y * y) / N.z * -1;
                    double newZ = -1 * t;
                    if (zmax < newZ)
                    {
                        zmax = newZ;
                        zmaxColor = polygon_list[i].c;
                    }
                    //debug += newZ + Environment.NewLine;
                }
            }
            //debug += Environment.NewLine + zmaxColor + " => " + zmax;
            return zmaxColor;
        }
        

        public TPolygon IsOnlyOneIntersecting(int xmin, int ymin, int xmax, int ymax)
        {
            TPolygon temp = new TPolygon();
            List<int> index = new List<int>();
            for (int i = 0; i < polygon_list.Count; i++)
            {
                TPoint p1 = polygon_list[i].P[0];
                TPoint p2 = polygon_list[i].P[1];
                TPoint p3 = polygon_list[i].P[2];
                if (IsPolygonIntersectArea(p1, p2, p3, xmin, ymin, xmax, ymax)) index.Add(i);
            }
            if (index.Count == 1)
            {
                int polygonIndex = index[0];
                List<TPoint> P = new List<TPoint>();
                P.Add(polygon_list[polygonIndex].P[0]);
                P.Add(polygon_list[polygonIndex].P[1]);
                P.Add(polygon_list[polygonIndex].P[2]);
                temp.P = P;
                temp.c = polygon_list[polygonIndex].c;
                return temp;
            }
            else return null;
        }



        public TPolygon IsOnlyOneContained(int xmin, int ymin, int xmax, int ymax)
        {
            TPolygon temp = new TPolygon();
            List<int> index = new List<int>();
            for (int i = 0; i < polygon_list.Count; i++)
            {
                TPoint p1 = polygon_list[i].P[0];
                TPoint p2 = polygon_list[i].P[1];
                TPoint p3 = polygon_list[i].P[2];
                if (IsPolygonInsideArea(p1, p2, p3, xmin, ymin, xmax, ymax)) index.Add(i);
            }
            if (index.Count == 1)
            {
                int polygonIndex = index[0];
                List<TPoint> P = new List<TPoint>();
                P.Add(polygon_list[polygonIndex].P[0]);
                P.Add(polygon_list[polygonIndex].P[1]);
                P.Add(polygon_list[polygonIndex].P[2]);
                temp.P = P;
                temp.c = polygon_list[polygonIndex].c;
                return temp;
            }
            else return null;
        }


        public TPolygon IsOnlyOneSurrounding(int xmin, int ymin, int xmax, int ymax)
        {
            TPolygon temp = new TPolygon();
            List<int> index = new List<int>();
            for (int i = 0; i < polygon_list.Count; i++)
            {
                TPoint p1 = polygon_list[i].P[0];
                TPoint p2 = polygon_list[i].P[1];
                TPoint p3 = polygon_list[i].P[2];
                if (IsAreaInsidePolygon(p1, p2, p3, xmin, ymin, xmax, ymax)) index.Add(i);
            }
            if (index.Count == 1)
            {
                int polygonIndex = index[0];
                List<TPoint> P = new List<TPoint>();
                P.Add(polygon_list[polygonIndex].P[0]);
                P.Add(polygon_list[polygonIndex].P[1]);
                P.Add(polygon_list[polygonIndex].P[2]);
                temp.P = P;
                temp.c = polygon_list[polygonIndex].c;
                return temp;
            }
            else return null;
        }

        public bool IsAllPolygonsAreDisjoint(int xmin, int ymin, int xmax, int ymax)
        {
            bool flag = true;
            for (int i = 0; i < polygon_list.Count; i++)
            {
                TPoint p1 = polygon_list[i].P[0];
                TPoint p2 = polygon_list[i].P[1];
                TPoint p3 = polygon_list[i].P[2];
                if (IsPolygonIntersectArea(p1, p2, p3, xmin, ymin, xmax, ymax) 
                    || IsPolygonInsideArea(p1, p2, p3, xmin, ymin, xmax, ymax)
                    || IsAreaInsidePolygon(p1, p2, p3, xmin, ymin, xmax, ymax)) flag = false;
            }
            return flag;
        }

        public TPolygon IsMoreThanOnePolygon(int xmin, int ymin, int xmax, int ymax) // Selesaikan
        {
            TPolygon temp = new TPolygon();
            List<TPolygon> surrounding_polygon_list = new List<TPolygon>();
            for(int i=0; i<polygon_list.Count; i++)
            {
                TPoint p1 = polygon_list[i].P[0];
                TPoint p2 = polygon_list[i].P[1];
                TPoint p3 = polygon_list[i].P[2];
                if (IsAreaInsidePolygon(p1, p2, p3, xmin, ymin, xmax, ymax)) surrounding_polygon_list.Add(polygon_list[i]);
            }
            if (surrounding_polygon_list.Count == 1) return surrounding_polygon_list[0];
            else if (surrounding_polygon_list.Count > 1)
            {
                List<double> zmax = new List<double>();
                List<double> zmin = new List<double>();
                for (int i = 0; i < surrounding_polygon_list.Count; i++)
                {
                    TPoint p1 = surrounding_polygon_list[i].P[0];
                    TPoint p2 = surrounding_polygon_list[i].P[1];
                    TPoint p3 = surrounding_polygon_list[i].P[2];
                    TPoint p1p2 = FindVector(p1, p2);
                    TPoint p2p3 = FindVector(p2, p3);
                    TPoint N = CrossProduct(p1p2, p2p3);
                    double d = -(N.x * p1.x + N.y * p1.y + N.z * p1.z);
                    double z1, z2, z3, z4;
                    double t1, t2, t3, t4;
                    t1 = -(d + N.x * xmin + N.y * ymin) / N.z * -1;
                    t2 = -(d + N.x * xmax + N.y * ymin) / N.z * -1;
                    t3 = -(d + N.x * xmax + N.y * ymax) / N.z * -1;
                    t4 = -(d + N.x * xmin + N.y * ymax) / N.z * -1;
                    z1 = t1 * -1;
                    z2 = t2 * -1;
                    z3 = t3 * -1;
                    z4 = t4 * -1;
                    zmax.Add(Math.Max(z1, Math.Max(z2, Math.Max(z3, z4))));
                    zmin.Add(Math.Min(z1, Math.Min(z2, Math.Min(z3, z4))));
                }
                int front_polygon_index = 0;
                for (int i = 0; i < zmin.Count; i++)
                {
                    int count = 0;
                    for (int j = 0; j < zmax.Count; j++)
                    {
                        if (i == j) continue;
                        else
                        {
                            if (zmin[i] > zmax[j]) count++;
                        }
                        front_polygon_index = i;
                    }
                }
                return surrounding_polygon_list[front_polygon_index];
            }
            else return null;
        }

        public void Warnock(int xmin, int ymin, int xmax, int ymax)
        {
            if (xmin == 301 && ymin == 151 && xmax == 400 && ymax == 200) 
                MessageBox.Show(xmin.ToString() + ymin.ToString() +  xmax.ToString() +  ymax.ToString());
            // Base case if there are only one pixel left
            if ((xmax - xmin == 1) || (ymax - ymin == 1)) // Width = 800 and Height = 400, 1 pixel?
            {
                bmp.SetPixel(xmax - xmin, ymax - ymin, GetClosestPixelColor(xmax - xmin, ymax - ymin));
            }
            // Base case if there are some polygons
            else
            {
                // Case 1: All polygons are disjoint.
                if(IsAllPolygonsAreDisjoint(xmin, ymin, xmax, ymax))
                {
                    Point[] P = new Point[4];
                    SolidBrush brush = new SolidBrush(Color.White);
                    P[0] = new Point(xmin, ymax);
                    P[1] = new Point(xmax, ymax);
                    P[2] = new Point(xmax, ymin);
                    P[3] = new Point(xmin, ymin);
                    g.FillPolygon(brush, P);
                }

                // Case 2: Only one intersecting or one contained polygon is the area.
                else if (IsOnlyOneIntersecting(xmin, ymin, xmax, ymax) != null || IsOnlyOneContained(xmin, ymin, xmax, ymax) != null)
                {
                    TPolygon temp = new TPolygon();
                    if(IsOnlyOneIntersecting(xmin, ymin, xmax, ymax) != null) temp = IsOnlyOneIntersecting(xmin, ymin, xmax, ymax);
                    else temp = IsOnlyOneContained(xmin, ymin, xmax, ymax);
                    TPolygon clippedTemp = polygonClip(temp, xmin, ymin, xmax, ymax);
                    SolidBrush brush = new SolidBrush(clippedTemp.c);
                    Point[] P = new Point[clippedTemp.P.Count];
                    for (int i=0; i< clippedTemp.P.Count; i++)
                    {
                        P[i] = new Point(Convert.ToInt32(clippedTemp.P[i].x), Convert.ToInt32(clippedTemp.P[i].y));
                    }
                    g.FillPolygon(brush, P);
                }

                // Case 3: Only one surrounding polygon in the area.
                else if (IsOnlyOneSurrounding(xmin, ymin, xmax, ymax) != null)
                {
                    TPolygon temp = IsOnlyOneSurrounding(xmin, ymin, xmax, ymax);
                    Point[] P = new Point[4];
                    SolidBrush brush = new SolidBrush(temp.c);
                    P[0] = new Point(xmin, ymax);
                    P[1] = new Point(xmax, ymax);
                    P[2] = new Point(xmax, ymin);
                    P[3] = new Point(xmin, ymin);
                    g.FillPolygon(brush, P);
                }

                // Case 4: More than one polygon is intersecting, contained in, or surroundingthe area, with sorrounding polygon wholly in front.
                else if(IsMoreThanOnePolygon(xmin, ymin, xmax, ymax) != null)
                {
                    TPolygon temp = IsMoreThanOnePolygon(xmin, ymin, xmax, ymax);
                    Point[] P = new Point[4];
                    SolidBrush brush = new SolidBrush(temp.c);
                    P[0] = new Point(xmin, ymax);
                    P[1] = new Point(xmax, ymax);
                    P[2] = new Point(xmax, ymin);
                    P[3] = new Point(xmin, ymin);
                    g.FillPolygon(brush, P);
                }

                else
                {
                    int xmid = (xmin + xmax) / 2;
                    int ymid = (ymin + ymax) / 2;
                    Warnock(xmin, ymin, xmid, ymid);
                    Warnock(xmid + 1, ymin, xmax, ymid);
                    Warnock(xmid + 1, ymid + 1, xmax, ymax);
                    Warnock(xmin, ymid + 1, xmid, ymax);
                }
            }
        }









        public void Draw()
        {
            TPoint[] P = new TPoint[3];
            for (int i = 0; i < obj.Length; i++)
            {
                for (int j = 0; j < obj[i].S.Length; j++)
                {
                    if (frontSurfaceCB.Checked)
                    {
                        if (obj[i].visibleSurfaceIndex.Contains(j))
                        {
                            Pen pen = new Pen(obj[i].S[j].c);
                            SolidBrush brush = new SolidBrush(obj[i].S[j].c);
                            P[0] = obj[i].VS[obj[i].S[j].p1];
                            P[1] = obj[i].VS[obj[i].S[j].p2];
                            P[2] = obj[i].VS[obj[i].S[j].p3];
                            if (!filledCB.Checked)
                            {
                                for (int k = 0; k < 3; k++)
                                {
                                    if (k == 2) g.DrawLine(pen, new Point(Convert.ToInt32(P[k].x), Convert.ToInt32(P[k].y)), new Point(Convert.ToInt32(P[0].x), Convert.ToInt32(P[0].y)));
                                    else g.DrawLine(pen, new Point(Convert.ToInt32(P[k].x), Convert.ToInt32(P[k].y)), new Point(Convert.ToInt32(P[k + 1].x), Convert.ToInt32(P[k + 1].y)));
                                }
                            }
                            else
                            {
                                //PolygonFill(P, pen);
                                Point[] pp = new Point[3];
                                pp[0] = new Point(Convert.ToInt32(P[0].x), Convert.ToInt32(P[0].y));
                                pp[1] = new Point(Convert.ToInt32(P[1].x), Convert.ToInt32(P[1].y));
                                pp[2] = new Point(Convert.ToInt32(P[2].x), Convert.ToInt32(P[2].y));
                                g.FillPolygon(brush, pp);
                            }
                        }
                    }
                    else
                    {
                        Pen pen = new Pen(obj[i].S[j].c);
                        SolidBrush brush = new SolidBrush(obj[i].S[j].c);
                        P[0] = obj[i].VS[obj[i].S[j].p1];
                        P[1] = obj[i].VS[obj[i].S[j].p2];
                        P[2] = obj[i].VS[obj[i].S[j].p3];
                        if (!filledCB.Checked)
                        {
                            for (int k = 0; k < 3; k++)
                            {
                                if (k == 2) g.DrawLine(pen, new Point(Convert.ToInt32(P[k].x), Convert.ToInt32(P[k].y)), new Point(Convert.ToInt32(P[0].x), Convert.ToInt32(P[0].y)));
                                else g.DrawLine(pen, new Point(Convert.ToInt32(P[k].x), Convert.ToInt32(P[k].y)), new Point(Convert.ToInt32(P[k + 1].x), Convert.ToInt32(P[k + 1].y)));
                            }
                        }
                        else
                        {
                            //PolygonFill(P, pen);
                            Point[] pp = new Point[3];
                            pp[0] = new Point(Convert.ToInt32(P[0].x), Convert.ToInt32(P[0].y));
                            pp[1] = new Point(Convert.ToInt32(P[1].x), Convert.ToInt32(P[1].y));
                            pp[2] = new Point(Convert.ToInt32(P[2].x), Convert.ToInt32(P[2].y));
                            g.FillPolygon(brush, pp);
                        }
                    }
                }
            }
            g.DrawLine(new Pen(Color.Blue), new Point(0, pictureBox1.Height / 2), new Point(pictureBox1.Width, pictureBox1.Height / 2));
            g.DrawLine(new Pen(Color.Blue), new Point(pictureBox1.Width / 2, 0), new Point(pictureBox1.Width / 2, pictureBox1.Height));
            //g.DrawRectangle(new Pen(Color.Green), new Rectangle(250, 250, 25, 25));
            pictureBox1.Image = bmp;
        }



















        public List<TPoint> clip(List<TPoint> polygon_points, List<int> inIndex, List<int> outIndex, string side, double p)
        {
            List<TPoint> temp_list = new List<TPoint>();

            for (int i = 0; i < polygon_points.Count; i++)
            {
                if (i == polygon_points.Count - 1)
                {
                    // Case 1: Out In => Out' In
                    if (outIndex.Contains(i) && inIndex.Contains(0))
                    {
                        if (side == "left" || side == "right")
                        {
                            TPoint tempPoint;
                            tempPoint.x = p;
                            double t = (tempPoint.x - polygon_points[i].x) / (polygon_points[0].x - polygon_points[i].x);
                            tempPoint.y = polygon_points[i].y + t * (polygon_points[0].y - polygon_points[i].y);
                            tempPoint.z = polygon_points[i].z + t * (polygon_points[0].z - polygon_points[i].z);
                            tempPoint.w = 1;
                            temp_list.Add(tempPoint);
                        }
                        else if (side == "top" || side == "bottom")
                        {
                            TPoint tempPoint;
                            tempPoint.y = p;
                            double t = (tempPoint.y - polygon_points[i].y) / (polygon_points[0].y - polygon_points[i].y);
                            tempPoint.x = polygon_points[i].x + t * (polygon_points[0].x - polygon_points[i].x);
                            tempPoint.z = polygon_points[i].z + t * (polygon_points[0].z - polygon_points[i].z);
                            tempPoint.w = 1;
                            temp_list.Add(tempPoint);
                        }
                        temp_list.Add(polygon_points[0]);
                    }

                    // Case 2: In Out => In'
                    if (inIndex.Contains(i) && outIndex.Contains(0))
                    {
                        if (side == "left" || side == "right")
                        {
                            TPoint tempPoint;
                            tempPoint.x = p;
                            double t = (tempPoint.x - polygon_points[i].x) / (polygon_points[0].x - polygon_points[i].x);
                            tempPoint.y = polygon_points[i].y + t * (polygon_points[0].y - polygon_points[i].y);
                            tempPoint.z = polygon_points[i].z + t * (polygon_points[0].z - polygon_points[i].z);
                            tempPoint.w = 1;
                            temp_list.Add(tempPoint);
                        }
                        else if (side == "top" || side == "bottom")
                        {
                            TPoint tempPoint;
                            tempPoint.y = p;
                            double t = (tempPoint.y - polygon_points[i].y) / (polygon_points[0].y - polygon_points[i].y);
                            tempPoint.x = polygon_points[i].x + t * (polygon_points[0].x - polygon_points[i].x);
                            tempPoint.z = polygon_points[i].z + t * (polygon_points[0].z - polygon_points[i].z);
                            tempPoint.w = 1;
                            temp_list.Add(tempPoint);
                        }
                    }

                    // Case 3: In In => In kedua
                    if (inIndex.Contains(i) && inIndex.Contains(0))
                    {
                        temp_list.Add(polygon_points[0]);
                    }

                    // Case 4: Out Out => Null
                    if (outIndex.Contains(i) && outIndex.Contains(0))
                    {
                        continue;
                    }
                }
                else
                {
                    // Case 1: Out In => Out' In
                    if (outIndex.Contains(i) && inIndex.Contains(i + 1))
                    {
                        if (side == "left" || side == "right")
                        {
                            TPoint tempPoint;
                            tempPoint.x = p;
                            double t = (tempPoint.x - polygon_points[i].x) / (polygon_points[i + 1].x - polygon_points[i].x);
                            tempPoint.y = polygon_points[i].y + t * (polygon_points[i + 1].y - polygon_points[i].y);
                            tempPoint.z = polygon_points[i].z + t * (polygon_points[i + 1].z - polygon_points[i].z);
                            tempPoint.w = 1;
                            temp_list.Add(tempPoint);
                        }
                        else if (side == "top" || side == "bottom")
                        {
                            TPoint tempPoint;
                            tempPoint.y = p;
                            double t = (tempPoint.y - polygon_points[i].y) / (polygon_points[i + 1].y - polygon_points[i].y);
                            tempPoint.x = polygon_points[i].x + t * (polygon_points[i + 1].x - polygon_points[i].x);
                            tempPoint.z = polygon_points[i].z + t * (polygon_points[i + 1].z - polygon_points[i].z);
                            tempPoint.w = 1;
                            temp_list.Add(tempPoint);
                        }
                        temp_list.Add(polygon_points[i + 1]);
                    }

                    // Case 2: In Out => In'
                    if (inIndex.Contains(i) && outIndex.Contains(i + 1))
                    {
                        if (side == "left" || side == "right")
                        {
                            TPoint tempPoint;
                            tempPoint.x = p;
                            double t = (tempPoint.x - polygon_points[i].x) / (polygon_points[i + 1].x - polygon_points[i].x);
                            tempPoint.y = polygon_points[i].y + t * (polygon_points[i + 1].y - polygon_points[i].y);
                            tempPoint.z = polygon_points[i].z + t * (polygon_points[i + 1].z - polygon_points[i].z);
                            tempPoint.w = 1;
                            temp_list.Add(tempPoint);
                        }
                        else if (side == "top" || side == "bottom")
                        {
                            TPoint tempPoint;
                            tempPoint.y = p;
                            double t = (tempPoint.y - polygon_points[i].y) / (polygon_points[i + 1].y - polygon_points[i].y);
                            tempPoint.x = polygon_points[i].x + t * (polygon_points[i + 1].x - polygon_points[i].x);
                            tempPoint.z = polygon_points[i].z + t * (polygon_points[i + 1].z - polygon_points[i].z);
                            tempPoint.w = 1;
                            temp_list.Add(tempPoint);
                        }
                    }

                    // Case 3: In In => In kedua
                    if (inIndex.Contains(i) && inIndex.Contains(i + 1))
                    {
                        temp_list.Add(polygon_points[i + 1]);
                    }

                    // Case 4: Out Out => Null
                    if (outIndex.Contains(i) && outIndex.Contains(i + 1))
                    {
                        continue;
                    }
                }
            }

            return temp_list;
        }

        public TPolygon polygonClip(TPolygon polygon, int xmin, int ymin, int xmax, int ymax)
        {
            TPolygon tempPolygon = new TPolygon();
            List<TPoint> polygon_points = new List<TPoint>();
            List<TPoint> temp_list = new List<TPoint>();
            List<int> inIndex = new List<int>();
            List<int> outIndex = new List<int>();

            // Insert all polygon points to the list
            for (int i = 0; i < polygon.P.Count; i++)
            {
                polygon_points.Add(polygon.P[i]);
            }

            // Left
            inIndex.Clear();
            outIndex.Clear();
            for (int i = 0; i < polygon_points.Count; i++)
            {
                if (polygon_points[i].x < xmin) outIndex.Add(i);
                else inIndex.Add(i);
            }
            temp_list = clip(polygon_points, inIndex, outIndex, "left", xmin);
            polygon_points.Clear();
            polygon_points.AddRange(temp_list);
            temp_list.Clear();

            // Right
            inIndex.Clear();
            outIndex.Clear();
            for (int i = 0; i < polygon_points.Count; i++)
            {
                if (polygon_points[i].x > xmax) outIndex.Add(i);
                else inIndex.Add(i);
            }
            temp_list = clip(polygon_points, inIndex, outIndex, "right", xmax);
            polygon_points.Clear();
            polygon_points.AddRange(temp_list);
            temp_list.Clear();

            // Top
            inIndex.Clear();
            outIndex.Clear();
            for (int i = 0; i < polygon_points.Count; i++)
            {
                if (polygon_points[i].y > ymax) outIndex.Add(i);
                else inIndex.Add(i);
            }
            temp_list = clip(polygon_points, inIndex, outIndex, "top", ymax);
            polygon_points.Clear();
            polygon_points.AddRange(temp_list);
            for (int i = 0; i < polygon_points.Count; i++)
                temp_list.Clear();

            // Bottom
            inIndex.Clear();
            outIndex.Clear();
            for (int i = 0; i < polygon_points.Count; i++)
            {
                if (polygon_points[i].y < ymin) outIndex.Add(i);
                else inIndex.Add(i);
            }
            temp_list = clip(polygon_points, inIndex, outIndex, "bottom", ymin);
            polygon_points.Clear();
            polygon_points.AddRange(temp_list);
            temp_list.Clear();

            tempPolygon.c = polygon.c;
            tempPolygon.P = polygon_points;

            return tempPolygon;
        }






        public bool IsPolygonInsideArea(TPoint p1, TPoint p2, TPoint p3, int xmin, int ymin, int xmax, int ymax)
        {
            TPoint A = new TPoint();
            TPoint B = new TPoint();
            TPoint C = new TPoint();
            TPoint D = new TPoint();
            SetPoint(ref A, xmin, ymax, 0);
            SetPoint(ref B, xmax, ymax, 0);
            SetPoint(ref C, xmax, ymin, 0);
            SetPoint(ref D, xmin, ymin, 0);
            TPoint AB = FindVector(A, B);
            TPoint BC = FindVector(B, C);
            TPoint CD = FindVector(C, D);
            TPoint DA = FindVector(D, A);
            double cAB1, cBC1, cCD1, cDA1, cAB2, cBC2, cCD2, cDA2, cAB3, cBC3, cCD3, cDA3;
            bool p1In, p2In, p3In;

            TPoint Ap1 = FindVector(A, p1);
            TPoint Bp1 = FindVector(B, p1);
            TPoint Cp1 = FindVector(C, p1);
            TPoint Dp1 = FindVector(D, p1);
            cAB1 = CrossProduct2D(Ap1, AB);
            cBC1 = CrossProduct2D(Bp1, BC);
            cCD1 = CrossProduct2D(Cp1, CD);
            cDA1 = CrossProduct2D(Dp1, DA);
            p1In = ((cAB1 > 0) && (cBC1 > 0) && (cCD1 > 0) && (cDA1 > 0));

            TPoint Ap2 = FindVector(A, p2);
            TPoint Bp2 = FindVector(B, p2);
            TPoint Cp2 = FindVector(C, p2);
            TPoint Dp2 = FindVector(D, p2);
            cAB2 = CrossProduct2D(Ap2, AB);
            cBC2 = CrossProduct2D(Bp2, BC);
            cCD2 = CrossProduct2D(Cp2, CD);
            cDA2 = CrossProduct2D(Dp2, DA);
            p2In = ((cAB2 > 0) && (cBC2 > 0) && (cCD2 > 0) && (cDA2 > 0));

            TPoint Ap3 = FindVector(A, p3);
            TPoint Bp3 = FindVector(B, p3);
            TPoint Cp3 = FindVector(C, p3);
            TPoint Dp3 = FindVector(D, p3);
            cAB3 = CrossProduct2D(Ap3, AB);
            cBC3 = CrossProduct2D(Bp3, BC);
            cCD3 = CrossProduct2D(Cp3, CD);
            cDA3 = CrossProduct2D(Dp3, DA);
            p3In = ((cAB3 > 0) && (cBC3 > 0) && (cCD3 > 0) && (cDA3 > 0));

            return (p1In && p2In && p3In);
        }

        public bool IsPolygonIntersectArea(TPoint p1, TPoint p2, TPoint p3, int xmin, int ymin, int xmax, int ymax)
        {
            TPoint A = new TPoint();
            TPoint B = new TPoint();
            TPoint C = new TPoint();
            TPoint D = new TPoint();
            SetPoint(ref A, xmin, ymax, 0);
            SetPoint(ref B, xmax, ymax, 0);
            SetPoint(ref C, xmax, ymin, 0);
            SetPoint(ref D, xmin, ymin, 0);
            TPoint AB = FindVector(A, B);
            TPoint BC = FindVector(B, C);
            TPoint CD = FindVector(C, D);
            TPoint DA = FindVector(D, A);
            double cAB1, cBC1, cCD1, cDA1, cAB2, cBC2, cCD2, cDA2, cAB3, cBC3, cCD3, cDA3;
            bool p1In, p2In, p3In;

            TPoint Ap1 = FindVector(A, p1);
            TPoint Bp1 = FindVector(B, p1);
            TPoint Cp1 = FindVector(C, p1);
            TPoint Dp1 = FindVector(D, p1);
            cAB1 = CrossProduct2D(Ap1, AB);
            cBC1 = CrossProduct2D(Bp1, BC);
            cCD1 = CrossProduct2D(Cp1, CD);
            cDA1 = CrossProduct2D(Dp1, DA);
            p1In = ((cAB1 > 0) && (cBC1 > 0) && (cCD1 > 0) && (cDA1 > 0));

            TPoint Ap2 = FindVector(A, p2);
            TPoint Bp2 = FindVector(B, p2);
            TPoint Cp2 = FindVector(C, p2);
            TPoint Dp2 = FindVector(D, p2);
            cAB2 = CrossProduct2D(Ap2, AB);
            cBC2 = CrossProduct2D(Bp2, BC);
            cCD2 = CrossProduct2D(Cp2, CD);
            cDA2 = CrossProduct2D(Dp2, DA);
            p2In = ((cAB2 > 0) && (cBC2 > 0) && (cCD2 > 0) && (cDA2 > 0));

            TPoint Ap3 = FindVector(A, p3);
            TPoint Bp3 = FindVector(B, p3);
            TPoint Cp3 = FindVector(C, p3);
            TPoint Dp3 = FindVector(D, p3);
            cAB3 = CrossProduct2D(Ap3, AB);
            cBC3 = CrossProduct2D(Bp3, BC);
            cCD3 = CrossProduct2D(Cp3, CD);
            cDA3 = CrossProduct2D(Dp3, DA);
            p3In = ((cAB3 > 0) && (cBC3 > 0) && (cCD3 > 0) && (cDA3 > 0));

            if (
                ((p1In == true) && (p2In == true) && (p3In == false)) ||
                ((p1In == true) && (p2In == false) && (p3In == true)) ||
                ((p1In == false) && (p2In == true) && (p3In == true)) ||
                ((p1In == true) && (p2In == false) && (p3In == false)) ||
                ((p1In == false) && (p2In == true) && (p3In == false)) ||
                ((p1In == false) && (p2In == false) && (p3In == true))
                ) return true;
            else return false;
        }

        public bool IsAreaInsidePolygon(TPoint p1, TPoint p2, TPoint p3, int xmin, int ymin, int xmax, int ymax)
        {
            TPoint A = new TPoint();
            TPoint B = new TPoint();
            TPoint C = new TPoint();
            TPoint D = new TPoint();
            SetPoint(ref A, xmin, ymax, 0);
            SetPoint(ref B, xmax, ymax, 0);
            SetPoint(ref C, xmax, ymin, 0);
            SetPoint(ref D, xmin, ymin, 0);
            TPoint p1p2 = FindVector(p1, p2);
            TPoint p2p3 = FindVector(p2, p3);
            TPoint p3p1 = FindVector(p3, p1);

            double cp1p2A, cp2p3A, cp3p1A, cp1p2B, cp2p3B, cp3p1B, cp1p2C, cp2p3C, cp3p1C, cp1p2D, cp2p3D, cp3p1D;
            bool AIn, BIn, CIn, DIn;

            TPoint p1A = FindVector(p1, A);
            TPoint p2A = FindVector(p2, A);
            TPoint p3A = FindVector(p3, A);
            cp1p2A = CrossProduct2D(p1A, p1p2);
            cp2p3A = CrossProduct2D(p2A, p2p3);
            cp3p1A = CrossProduct2D(p3A, p3p1);
            AIn = ((cp1p2A > 0) && (cp2p3A > 0) && (cp3p1A > 0));

            TPoint p1B = FindVector(p1, B);
            TPoint p2B = FindVector(p2, B);
            TPoint p3B = FindVector(p3, B);
            cp1p2B = CrossProduct2D(p1B, p1p2);
            cp2p3B = CrossProduct2D(p2B, p2p3);
            cp3p1B = CrossProduct2D(p3B, p3p1);
            BIn = ((cp1p2B > 0) && (cp2p3B > 0) && (cp3p1B > 0));

            TPoint p1C = FindVector(p1, C);
            TPoint p2C = FindVector(p2, C);
            TPoint p3C = FindVector(p3, C);
            cp1p2C = CrossProduct2D(p1C, p1p2);
            cp2p3C = CrossProduct2D(p2C, p2p3);
            cp3p1C = CrossProduct2D(p3C, p3p1);
            CIn = ((cp1p2C > 0) && (cp2p3C > 0) && (cp3p1C > 0));

            TPoint p1D = FindVector(p1, D);
            TPoint p2D = FindVector(p2, D);
            TPoint p3D = FindVector(p3, D);
            cp1p2D = CrossProduct2D(p1D, p1p2);
            cp2p3D = CrossProduct2D(p2D, p2p3);
            cp3p1D = CrossProduct2D(p3D, p3p1);
            DIn = ((cp1p2D > 0) && (cp2p3D > 0) && (cp3p1D > 0));

            return (AIn && BIn && CIn && DIn);
        }

















        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 1. Prepare the objects
            obj[0] = new TObject(4, 4);
            SetPoint(ref obj[0].P[0], -1, -1, 1);
            SetPoint(ref obj[0].P[1], 1, -1, 1);
            SetPoint(ref obj[0].P[2], 0, 1, 0);
            SetPoint(ref obj[0].P[3], 0, -1, -1);
            SetSurface(ref obj[0].S[0], 0, 1, 2, Color.Red);
            SetSurface(ref obj[0].S[1], 1, 3, 2, Color.Yellow);
            SetSurface(ref obj[0].S[2], 3, 0, 2, Color.Green);
            SetSurface(ref obj[0].S[3], 0, 3, 1, Color.Blue);

            obj[1] = new TObject(5, 6);
            SetPoint(ref obj[1].P[0], -1, -1, 1);
            SetPoint(ref obj[1].P[1], 1, -1, 1);
            SetPoint(ref obj[1].P[2], 1, -1, -1);
            SetPoint(ref obj[1].P[3], -1, -1, -1);
            SetPoint(ref obj[1].P[4], 0, 1, 0);
            SetSurface(ref obj[1].S[0], 0, 1, 4, Color.Purple);
            SetSurface(ref obj[1].S[1], 1, 2, 4, Color.Orange);
            SetSurface(ref obj[1].S[2], 2, 3, 4, Color.Magenta);
            SetSurface(ref obj[1].S[3], 3, 0, 4, Color.Black);
            SetSurface(ref obj[1].S[4], 3, 2, 0, Color.Gray);
            SetSurface(ref obj[1].S[5], 2, 1, 0, Color.Aqua);

            for (int i = 0; i < obj.Length; i++)
            {
                selectListBox.Items.Add(i);
                ResetTransM(i);
            }

            TranslateObject(0, -2, 0, 0);
            RotateObjectOnX(0, 45);
            TranslateObject(1, 2, 0, 0);
            RotateObjectOnY(1, -45);

            selectListBox.SetSelected(0, true);

            translateRB.Checked = true;
            frontSurfaceCB.Checked = true;

            Display();
        }




        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            selectedObjectIndex = selectListBox.SelectedIndex;
            if(e.KeyCode == Keys.W)
            {
                if (translateRB.Checked) TranslateObject(selectedObjectIndex, 0, 0.1, 0);
                else RotateObjectOnX(selectedObjectIndex, -1);
            }
            else if(e.KeyCode == Keys.S)
            {
                if (translateRB.Checked) TranslateObject(selectedObjectIndex, 0, -0.1, 0);
                else RotateObjectOnX(selectedObjectIndex, 1);
            }
            else if (e.KeyCode == Keys.A)
            {
                if (translateRB.Checked) TranslateObject(selectedObjectIndex, -0.1, 0, 0);
                else RotateObjectOnZ(selectedObjectIndex, 1);
            }
            else if(e.KeyCode == Keys.D)
            {
                if (translateRB.Checked) TranslateObject(selectedObjectIndex, 0.1, 0, 0);
                else RotateObjectOnZ(selectedObjectIndex, -1);
            }
            else if(e.KeyCode == Keys.Q)
            {
                if (translateRB.Checked) TranslateObject(selectedObjectIndex, 0, 0, 0.1);
                else RotateObjectOnY(selectedObjectIndex, -1);
            }
            else if(e.KeyCode == Keys.E)
            {
                if (translateRB.Checked) TranslateObject(selectedObjectIndex, 0, 0, -0.1);
                else RotateObjectOnY(selectedObjectIndex, 1);
            }
            else if (e.KeyCode == Keys.T)
            {
                if (translateRB.Checked)
                {
                    translateRB.Checked = false;
                    rotateRB.Checked = true;
                }
                else
                {
                    translateRB.Checked = true;
                    rotateRB.Checked = false;
                }
            }
            else if(e.KeyCode == Keys.Up)
            {
                int index = selectListBox.SelectedIndex;
                if (index > 0) index -= 1;
                selectListBox.SetSelected(index, true);
            }
            else if (e.KeyCode == Keys.Down)
            {
                int index = selectListBox.SelectedIndex;
                if (index < selectListBox.Items.Count-1) index += 1;
                selectListBox.SetSelected(index, true);
            }
            Display();
        }



        private void upBtn_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = selectListBox.SelectedIndex;
            if (translateRB.Checked) TranslateObject(selectedObjectIndex, 0, 1, 0);
            else RotateObjectOnX(selectedObjectIndex, -1);
            Display();
        }

        private void downBtn_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = selectListBox.SelectedIndex;
            if (translateRB.Checked) TranslateObject(selectedObjectIndex, 0, -1, 0);
            else RotateObjectOnX(selectedObjectIndex, 1);
            Display();
        }

        private void leftBtn_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = selectListBox.SelectedIndex;
            if (translateRB.Checked) TranslateObject(selectedObjectIndex, -1, 0, 0);
            else RotateObjectOnZ(selectedObjectIndex, 1);
            Display();
        }

        private void rightBtn_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = selectListBox.SelectedIndex;
            if (translateRB.Checked) TranslateObject(selectedObjectIndex, 1, 0, 0);
            else RotateObjectOnZ(selectedObjectIndex, -1);
            Display();
        }

        private void frontBtn_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = selectListBox.SelectedIndex;
            if (translateRB.Checked) TranslateObject(selectedObjectIndex, 0, 0, 1);
            else RotateObjectOnY(selectedObjectIndex, -1);
            Display();
        }

        private void backBtn_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = selectListBox.SelectedIndex;
            if (translateRB.Checked) TranslateObject(selectedObjectIndex, 0, 0, -1);
            else RotateObjectOnY(selectedObjectIndex, 1);
            Display();
        }

        private void resetBtn_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = selectListBox.SelectedIndex;
            ResetTransM(selectedObjectIndex);
            Display();
        }

        private void filledCB_CheckedChanged(object sender, EventArgs e)
        {
            frontSurfaceCB.Checked = true;
            if (filledCB.Checked) frontSurfaceCB.Enabled = false;
            else frontSurfaceCB.Enabled = true;
            Display();
        }

        private void frontSurfaceCB_CheckedChanged(object sender, EventArgs e)
        {
            Display();
        }
    }
}
