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
        public double[,] Pr2 = new double[4, 4] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 1 } };
        public double[,] Wt = new double[4, 4];
        public double[,] Vt = new double[4, 4];
        public double[,] St = new double[4, 4] { { 100, 0, 0, 0 }, { 0, -100, 0, 0 }, { 0, 0, 0, 0 }, { 400, 200, 0, 1 } };


















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





        public void resetTransM(int index)
        {
            obj[index].TranslateM = new double[4,4] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } };
            obj[index].RotateM = new double[4,4] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } };
        }

        public void translateObject(int index, double dx=0, double dy=0, double dz=0)
        {
            double[,] temp = new double[4, 4] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { dx, dy, dz, 1 } };
            obj[index].TranslateM = matrixMultiplication(obj[index].TranslateM, temp);
        }
        public void rotateObjectOnX(int index, double angle)
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
            double[,] res1 = matrixMultiplication(matrixMultiplication(minTranslate, temp), obj[index].TranslateM);
            obj[index].RotateM = matrixMultiplication(obj[index].RotateM, res1);
        }
        public void rotateObjectOnY(int index, double angle)
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
            double[,] res1 = matrixMultiplication(matrixMultiplication(minTranslate, temp), obj[index].TranslateM);
            obj[index].RotateM = matrixMultiplication(obj[index].RotateM, res1);
        }
        public void rotateObjectOnZ(int index, double angle)
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
            double[,] res1 = matrixMultiplication(matrixMultiplication(minTranslate, temp), obj[index].TranslateM);
            obj[index].RotateM = matrixMultiplication(obj[index].RotateM, res1);
        }





        public void backfaceCulling(int index, TSurface[] S, TPoint[] V)
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
                p1p2 = findVector(p1, p2);
                //debug += "p1p2 => " + p1p2.x.ToString() + "  " + p1p2.y.ToString() + "  " + p1p2.z.ToString() + Environment.NewLine;
                p2p3 = findVector(p2, p3);
                //debug += "p2p3 => " + p2p3.x.ToString() + "  " + p2p3.y.ToString() + "  " + p2p3.z.ToString() + Environment.NewLine;
                N = crossProduct(p1p2, p2p3);
                //debug += "N => " + N.x.ToString() + "  " + N.y.ToString() + "  " + N.z.ToString() + Environment.NewLine;
                setPoint(ref VN, 0, 0, -1);
                res = dotProduct(VN, N);
                //debug += res.ToString() + Environment.NewLine;
                //debug += Environment.NewLine;
                if (res < 0) obj[index].visibleSurfaceIndex.Add(i);
            }
        }
        public void polygonFill(TPoint[] P, Pen pen)
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
            constructAEL(SET, wholeymin, wholeymax, pen);
        }

        public SETElement[] ymaxCheck(SETElement[] cr, int index, int currentY)
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

        public SETElement[] processCurrentRow(SETElement[] cr, int index)
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

        public SETElement[] sortCurrentRow(SETElement[] cr, int index)
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

        public void constructAEL(SETElement[,] se, int wholeymin, int wholeymax, Pen pen)
        {
            var current_row = new SETElement[pictureBox1.Width];
            int current_row_index = 0;
            for (int i = wholeymin; i <= wholeymax; i++)
            {

                current_row = ymaxCheck(current_row, current_row_index, i);

                current_row = processCurrentRow(current_row, current_row_index);

                current_row = sortCurrentRow(current_row, current_row_index);

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

        public void display()
        {
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bmp);

            // 2. Prepare the perspective projection parameters
            setPoint(ref VRP, 0, 0, 0);
            setPoint(ref VPN, 0, 0, 1);
            setPoint(ref VUP, 0, 1, 0);
            setPoint(ref COP, 0, 0, 4);
            windowUmin = -2;
            windowVmin = -2;
            windowUmax = 2;
            windowVmax = 2;
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

            for (int i = 0; i < obj.Length; i++)
            {
                Wt = matrixMultiplication(obj[i].TranslateM, obj[i].RotateM);
                for (int j = 0; j < obj[i].P.Length; j++)
                {
                    obj[i].VW[j] = multiplyMatrix(obj[i].P[j], Wt);
                    obj[i].VPr1[j] = multiplyMatrix(obj[i].VW[j], Pr1);
                }
                backfaceCulling(i, obj[i].S, obj[i].VPr1);
                for (int j = 0; j < obj[i].P.Length; j++)
                {
                    obj[i].VV[j] = multiplyMatrix(obj[i].VPr1[j], Pr2);
                    obj[i].VS[j] = multiplyMatrix(obj[i].VV[j], St);
                }
                //for (int j = 0; j < obj[i].visibleSurfaceIndex.Count; j++) debug += obj[i].visibleSurfaceIndex[j] + Environment.NewLine;
            }
            //debug = "";
            //debug += "Translate: " + Environment.NewLine;
            //for(int i=0; i<4; i++)
            //{
            //    for (int j = 0; j < 4; j++)
            //    {
            //        debug += obj[0].TranslateM[i, j] + "   ";
            //    }
            //    debug += Environment.NewLine;
            //}
            //debug += "Rotate: " + Environment.NewLine;
            //for (int i = 0; i < 4; i++)
            //{
            //    for (int j = 0; j < 4; j++)
            //    {
            //        debug += obj[0].RotateM[i, j] + "   ";
            //    }
            //    debug += Environment.NewLine;
            //}
            //debug += selectedObjectIndex + Environment.NewLine;
            draw();
            debugTextBox.Text = debug;
        }




        public void warnock()
        {

        }









        public void draw()
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
                                //polygonFill(P, pen);
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
                            //polygonFill(P, pen);
                            Point[] pp = new Point[3];
                            pp[0] = new Point(Convert.ToInt32(P[0].x), Convert.ToInt32(P[0].y));
                            pp[1] = new Point(Convert.ToInt32(P[1].x), Convert.ToInt32(P[1].y));
                            pp[2] = new Point(Convert.ToInt32(P[2].x), Convert.ToInt32(P[2].y));
                            g.FillPolygon(brush, pp);
                        }
                    }
                }
            }
            pictureBox1.Image = bmp;
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
            setPoint(ref obj[0].P[0], -1, -1, 1);
            setPoint(ref obj[0].P[1], 1, -1, 1);
            setPoint(ref obj[0].P[2], 0, 1, 0);
            setPoint(ref obj[0].P[3], 0, -1, -1);
            setSurface(ref obj[0].S[0], 0, 1, 2, Color.Red);
            setSurface(ref obj[0].S[1], 1, 3, 2, Color.Yellow);
            setSurface(ref obj[0].S[2], 3, 0, 2, Color.Green);
            setSurface(ref obj[0].S[3], 0, 3, 1, Color.Blue);

            obj[1] = new TObject(5, 6);
            setPoint(ref obj[1].P[0], -1, -1, 1);
            setPoint(ref obj[1].P[1], 1, -1, 1);
            setPoint(ref obj[1].P[2], 1, -1, -1);
            setPoint(ref obj[1].P[3], -1, -1, -1);
            setPoint(ref obj[1].P[4], 0, 1, 0);
            setSurface(ref obj[1].S[0], 0, 1, 4, Color.Purple);
            setSurface(ref obj[1].S[1], 1, 2, 4, Color.Orange);
            setSurface(ref obj[1].S[2], 2, 3, 4, Color.Magenta);
            setSurface(ref obj[1].S[3], 3, 0, 4, Color.Black);
            setSurface(ref obj[1].S[4], 3, 2, 0, Color.Gray);
            setSurface(ref obj[1].S[5], 2, 1, 0, Color.Aqua);

            for (int i = 0; i < obj.Length; i++)
            {
                selectListBox.Items.Add(i);
                resetTransM(i);
            }

            translateObject(0, -2, 0, 0);
            rotateObjectOnX(0, 45);
            translateObject(1, 2, 0, 0);
            rotateObjectOnY(1, -45);

            selectListBox.SetSelected(0, true);

            translateRB.Checked = true;
            frontSurfaceCB.Checked = true;

            display();
        }




        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            selectedObjectIndex = selectListBox.SelectedIndex;
            if(e.KeyCode == Keys.W)
            {
                if (translateRB.Checked) translateObject(selectedObjectIndex, 0, 0.1, 0);
                else rotateObjectOnX(selectedObjectIndex, -1);
            }
            else if(e.KeyCode == Keys.S)
            {
                if (translateRB.Checked) translateObject(selectedObjectIndex, 0, -0.1, 0);
                else rotateObjectOnX(selectedObjectIndex, 1);
            }
            else if (e.KeyCode == Keys.A)
            {
                if (translateRB.Checked) translateObject(selectedObjectIndex, -0.1, 0, 0);
                else rotateObjectOnZ(selectedObjectIndex, 1);
            }
            else if(e.KeyCode == Keys.D)
            {
                if (translateRB.Checked) translateObject(selectedObjectIndex, 0.1, 0, 0);
                else rotateObjectOnZ(selectedObjectIndex, -1);
            }
            else if(e.KeyCode == Keys.Q)
            {
                if (translateRB.Checked) translateObject(selectedObjectIndex, 0, 0, 0.1);
                else rotateObjectOnY(selectedObjectIndex, -1);
            }
            else if(e.KeyCode == Keys.E)
            {
                if (translateRB.Checked) translateObject(selectedObjectIndex, 0, 0, -0.1);
                else rotateObjectOnY(selectedObjectIndex, 1);
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
            display();
        }



        private void upBtn_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = selectListBox.SelectedIndex;
            if (translateRB.Checked) translateObject(selectedObjectIndex, 0, 1, 0);
            else rotateObjectOnX(selectedObjectIndex, -1);
            display();
        }

        private void downBtn_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = selectListBox.SelectedIndex;
            if (translateRB.Checked) translateObject(selectedObjectIndex, 0, -1, 0);
            else rotateObjectOnX(selectedObjectIndex, 1);
            display();
        }

        private void leftBtn_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = selectListBox.SelectedIndex;
            if (translateRB.Checked) translateObject(selectedObjectIndex, -1, 0, 0);
            else rotateObjectOnZ(selectedObjectIndex, 1);
            display();
        }

        private void rightBtn_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = selectListBox.SelectedIndex;
            if (translateRB.Checked) translateObject(selectedObjectIndex, 1, 0, 0);
            else rotateObjectOnZ(selectedObjectIndex, -1);
            display();
        }

        private void frontBtn_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = selectListBox.SelectedIndex;
            if (translateRB.Checked) translateObject(selectedObjectIndex, 0, 0, 1);
            else rotateObjectOnY(selectedObjectIndex, -1);
            display();
        }

        private void backBtn_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = selectListBox.SelectedIndex;
            if (translateRB.Checked) translateObject(selectedObjectIndex, 0, 0, -1);
            else rotateObjectOnY(selectedObjectIndex, 1);
            display();
        }

        private void resetBtn_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = selectListBox.SelectedIndex;
            resetTransM(selectedObjectIndex);
            display();
        }

        private void filledCB_CheckedChanged(object sender, EventArgs e)
        {
            frontSurfaceCB.Checked = true;
            if (filledCB.Checked) frontSurfaceCB.Enabled = false;
            else frontSurfaceCB.Enabled = true;
            display();
        }

        private void frontSurfaceCB_CheckedChanged(object sender, EventArgs e)
        {
            display();
        }
    }
}
