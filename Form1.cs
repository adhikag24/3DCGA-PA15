using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
            public string objectName;
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
        public class TPolygon
        {
            public int polygon_id;
            public double A, B, C, D;
            public List<TPoint> P;
            public Color c;
            public bool flag;
        }
        public class Edge
        {
            public int ymax, xofymin, dx, dy, carrier = 0, polygon_id;
        }

        List<TPolygon> polygon_list = new List<TPolygon>();
        public bool firstLoad = true;
        public int nObj = 2;
        Bitmap bmp;
        Graphics g;
        public List<TObject> obj = new List<TObject>();
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
        public double[,] St = new double[4, 4] { { 128, 0, 0, 0 }, { 0, -128, 0, 0 }, { 0, 0, 1, 0 }, { 512, 256, 0, 1 } };



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

            for (int i = 0; i < obj.Count; i++)
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

            debug = "";
            if (drawRB.Checked)
            {
                g.Clear(Color.White);
                Draw();
            }
            else if (warnockRB.Checked)
            {
                g.Clear(Color.White);
                GeneratePolygonList();
                Warnock(0, 0, pictureBox1.Width, pictureBox1.Height);
            }
            else if (scanlineRB.Checked)
            {
                g.Clear(Color.White);
                List<TPolygon> scanline_polygon_list = GenerateScanlinePolygon();
                Scanline(scanline_polygon_list);
            }
            debugTextBox.Text = debug;
        }

        

        public void Draw()
        {
            TPoint[] P = new TPoint[3];
            for (int i = 0; i < obj.Count; i++)
            {
                for (int j = 0; j < obj[i].S.Length; j++)
                {
                    if (frontSurfaceCB.Checked)
                    {
                        if (obj[i].visibleSurfaceIndex.Contains(j))
                        {
                            Pen pen = new Pen(obj[i].S[j].c);
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
                                TPoint[] pp = new TPoint[3];
                                SetPoint(ref pp[0], P[0].x, P[0].y, P[0].z);
                                SetPoint(ref pp[1], P[1].x, P[1].y, P[1].z);
                                SetPoint(ref pp[2], P[2].x, P[2].y, P[2].z);
                                PolygonFill(pen, pp);
                            }
                        }
                    }
                    else
                    {
                        Pen pen = new Pen(obj[i].S[j].c);
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
                            TPoint[] pp = new TPoint[3];
                            SetPoint(ref pp[0], P[0].x, P[0].y, P[0].z);
                            SetPoint(ref pp[1], P[1].x, P[1].y, P[1].z);
                            SetPoint(ref pp[2], P[2].x, P[2].y, P[2].z);
                            PolygonFill(pen, pp);
                        }
                    }
                }
            }
            pictureBox1.Image = bmp;
        }

        public void Warnock(int xmin, int ymin, int xmax, int ymax)
        {
            if ((xmax == xmin) && (ymax == ymin))
            {
                bmp.SetPixel(xmin - 1, ymin - 1, GetClosestPixelColor(xmax, ymax));
            }
            else
            {
                int nS = 0, nIC = 0;
                List<TPolygon> clipped_polygon_list = new List<TPolygon>();
                List<TPolygon> surrounding_polygon_list = new List<TPolygon>();
                clipped_polygon_list.Clear();
                surrounding_polygon_list.Clear();
                for (int i = 0; i < polygon_list.Count; i++)
                {
                    TPolygon tempPolygon = polygon_list[i];
                    var clippedPolygon = polygonClip(tempPolygon, xmin, ymin, xmax, ymax);
                    if (clippedPolygon.Item1.P.Count == 0) continue;
                    else if (clippedPolygon.Item2)
                    {
                        nS++;
                        clipped_polygon_list.Add(clippedPolygon.Item1);
                        surrounding_polygon_list.Add(clippedPolygon.Item1);
                    }
                    else
                    {
                        nIC++;
                        clipped_polygon_list.Add(clippedPolygon.Item1);
                    }
                }
                // Case 1: All polygons are disjoint.
                if (nS == 0 && nIC == 0)
                {
                    Pen pen = new Pen(Color.White);
                    DrawRect(pen, xmin - 1, ymin - 1, xmax, ymax);
                }
                // Case 2: Only one intersecting or one contained polygon is the area.
                else if (nS == 0 && nIC == 1)
                {
                    Pen pen = new Pen(clipped_polygon_list[0].c);
                    TPoint[] P = new TPoint[clipped_polygon_list[0].P.Count];
                    for (int i = 0; i < clipped_polygon_list[0].P.Count; i++)
                    {
                        SetPoint(ref P[i], clipped_polygon_list[0].P[i].x, clipped_polygon_list[0].P[i].y, 0);
                    }
                    PolygonFill(pen, P);
                }
                //Case 3: Only one surrounding polygon in the area.
                else if (nS == 1 && nIC == 0)
                {
                    Pen pen = new Pen(clipped_polygon_list[0].c);
                    DrawRect(pen, xmin - 1, ymin - 1, xmax, ymax);
                }
                //Case 4: More than one polygon is intersecting, contained in, or surroundingthe area, with sorrounding polygon wholly in front.
                else if (nS > 0 && frontPolygonCheck(clipped_polygon_list, surrounding_polygon_list) != null)
                {
                    TPolygon temp = frontPolygonCheck(clipped_polygon_list, surrounding_polygon_list);
                    Pen pen = new Pen(temp.c);
                    DrawRect(pen, xmin - 1, ymin - 1, xmax, ymax);
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
            pictureBox1.Image = bmp;
        }

        public void GeneratePolygonList()
        {
            polygon_list.Clear();
            for (int i = 0; i < obj.Count; i++)
            {
                for (int j = 0; j < obj[i].S.Length; j++)
                {
                    if (obj[i].visibleSurfaceIndex.Contains(j))
                    {
                        List<TPoint> P = new List<TPoint>();
                        TPoint p1 = obj[i].VS[obj[i].S[j].p1];
                        TPoint p2 = obj[i].VS[obj[i].S[j].p2];
                        TPoint p3 = obj[i].VS[obj[i].S[j].p3];
                        P.Add(p1);
                        P.Add(p2);
                        P.Add(p3);
                        Color c = obj[i].S[j].c;
                        TPoint p1p2 = FindVector(p1, p2);
                        TPoint p2p3 = FindVector(p2, p3);
                        TPoint N = CrossProduct(p1p2, p2p3);
                        double D = -(N.x * p1.x + N.y * p1.y + N.z * p1.z);
                        TPolygon tempPolygon = new TPolygon();
                        tempPolygon.A = N.x;
                        tempPolygon.B = N.y;
                        tempPolygon.C = N.z;
                        tempPolygon.D = D;
                        tempPolygon.c = c;
                        tempPolygon.P = P;
                        polygon_list.Add(tempPolygon);
                    }
                }
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
        public void DrawRect(Pen pen, int xmin, int ymin, int xmax, int ymax)
        {
            for(int i=ymin; i<ymax; i++)
            {
                g.DrawLine(pen, new Point(xmin, i), new Point(xmax, i));
            }
        }
        public Tuple<TPolygon, bool> polygonClip(TPolygon polygon, int xmin, int ymin, int xmax, int ymax)
        {
            TPolygon tempPolygon = new TPolygon();
            List<TPoint> polygon_points = new List<TPoint>();
            List<TPoint> temp_list;
            List<int> inIndex = new List<int>();
            List<int> outIndex = new List<int>();

            // Insert all polygon points to the list
            for (int i = 0; i < polygon.P.Count; i++)
            {
                polygon_points.Add(polygon.P[i]);
            }

            bool surrounding = IsPolygonSurroundArea(polygon_points, xmin, ymin, xmax, ymax);

            // Left
            inIndex.Clear();
            outIndex.Clear();
            for (int i = 0; i < polygon_points.Count; i++)
            {
                if (polygon_points[i].x < xmin) outIndex.Add(i);
                else inIndex.Add(i);
            }
            temp_list = clip(polygon_points, inIndex, outIndex, "left", xmin-1);
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
            temp_list.Clear();

            // Bottom
            inIndex.Clear();
            outIndex.Clear();
            for (int i = 0; i < polygon_points.Count; i++)
            {
                if (polygon_points[i].y < ymin) outIndex.Add(i);
                else inIndex.Add(i);
            }
            temp_list = clip(polygon_points, inIndex, outIndex, "bottom", ymin-1);
            polygon_points.Clear();
            polygon_points.AddRange(temp_list);
            temp_list.Clear();

            tempPolygon.c = polygon.c;
            tempPolygon.P = polygon_points;

            return new Tuple<TPolygon, bool>(tempPolygon, surrounding);
        }
        public Color GetClosestPixelColor(int x, int y)
        {
            double zmax = -9999;
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
                }
            }
            return zmaxColor;
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
        public bool IsPolygonSurroundArea(List<TPoint> polygon_points, int xmin, int ymin, int xmax, int ymax) // Fungsi tidak penting, ada alternatif lain
        {
            TPoint p1 = polygon_points[0];
            TPoint p2 = polygon_points[1];
            TPoint p3 = polygon_points[2];

            TPoint p1p2 = FindVector(p1, p2);
            TPoint p2p3 = FindVector(p2, p3);
            TPoint p3p1 = FindVector(p3, p1);

            TPoint A = new TPoint();
            TPoint B = new TPoint();
            TPoint C = new TPoint();
            TPoint D = new TPoint();
            SetPoint(ref A, xmin, ymin, 0);
            SetPoint(ref B, xmax, ymin, 0);
            SetPoint(ref C, xmax, ymax, 0);
            SetPoint(ref D, xmin, ymax, 0);

            TPoint p1A = FindVector(p1, A);
            TPoint p2A = FindVector(p2, A);
            TPoint p3A = FindVector(p3, A);
            double cp1p2A = CrossProduct2D(p1A, p1p2);
            double cp2p3A = CrossProduct2D(p2A, p2p3);
            double cp3p1A = CrossProduct2D(p3A, p3p1);
            bool AIn = ((cp1p2A > 0) && (cp2p3A > 0) && (cp3p1A > 0));

            TPoint p1B = FindVector(p1, B);
            TPoint p2B = FindVector(p2, B);
            TPoint p3B = FindVector(p3, B);
            double cp1p2B = CrossProduct2D(p1B, p1p2);
            double cp2p3B = CrossProduct2D(p2B, p2p3);
            double cp3p1B = CrossProduct2D(p3B, p3p1);
            bool BIn = ((cp1p2B > 0) && (cp2p3B > 0) && (cp3p1B > 0));

            TPoint p1C = FindVector(p1, C);
            TPoint p2C = FindVector(p2, C);
            TPoint p3C = FindVector(p3, C);
            double cp1p2C = CrossProduct2D(p1C, p1p2);
            double cp2p3C = CrossProduct2D(p2C, p2p3);
            double cp3p1C = CrossProduct2D(p3C, p3p1);
            bool CIn = ((cp1p2C > 0) && (cp2p3C > 0) && (cp3p1C > 0));

            TPoint p1D = FindVector(p1, D);
            TPoint p2D = FindVector(p2, D);
            TPoint p3D = FindVector(p3, D);
            double cp1p2D = CrossProduct2D(p1D, p1p2);
            double cp2p3D = CrossProduct2D(p2D, p2p3);
            double cp3p1D = CrossProduct2D(p3D, p3p1);
            bool DIn = ((cp1p2D > 0) && (cp2p3D > 0) && (cp3p1D > 0));
            return (AIn && BIn && CIn && DIn);
        }
        TPolygon frontPolygonCheck(List<TPolygon> clipped_polygon_list, List<TPolygon> surrounding_polygon_list)
        {
            List<double> surroundzmin = new List<double>();
            List<double> surroundzmax = new List<double>();
            List<double> surroundzmaxy = new List<double>();
            List<double> surroundzminy = new List<double>();
            for (int i = 0; i < surrounding_polygon_list.Count; i++)
            {
                double ztempMin = 9999;
                double ztempMax = -9999;
                double zmaxy = 0;
                double zminy = 0;
                for (int j = 0; j < surrounding_polygon_list[i].P.Count; j++)
                {
                    if (ztempMin > surrounding_polygon_list[i].P[j].z) zminy = surrounding_polygon_list[i].P[j].y;
                    ztempMin = Math.Min(ztempMin, surrounding_polygon_list[i].P[j].z);
                    if (ztempMax < surrounding_polygon_list[i].P[j].z) zmaxy = surrounding_polygon_list[i].P[j].y;
                    ztempMax = Math.Max(ztempMax, surrounding_polygon_list[i].P[j].z);
                }
                surroundzmin.Add(ztempMin);
                surroundzmax.Add(ztempMax);
                surroundzmaxy.Add(zmaxy);
                surroundzminy.Add(zminy);
            }

            List<double> clippedzmax = new List<double>();
            for (int i = 0; i < clipped_polygon_list.Count; i++)
            {
                double ztemp = -9999;
                if (!surrounding_polygon_list.Contains(clipped_polygon_list[i]))
                {
                    for (int j = 0; j < clipped_polygon_list[i].P.Count; j++)
                    {
                        ztemp = Math.Max(ztemp, clipped_polygon_list[i].P[j].z);
                    }
                }
                else continue;
                clippedzmax.Add(ztemp);
            }

            List<double> final = new List<double>();
            for (int i = 0; i < surroundzmin.Count; i++)
            {
                bool greater = true;
                for (int j = 0; j < clippedzmax.Count; j++)
                {
                    if (surroundzmin[i] < clippedzmax[j]) greater = false;
                }
                if (greater) final.Add(surroundzmin[i]);
            }
            if (final.Count == 0) return null;
            else if (final.Count == 1) return surrounding_polygon_list[0];

            else
            {
                bool intersect = false;
                double initSlope = (surroundzmaxy[0] - surroundzminy[0]) / (surroundzmax[0] - final[0]);
                for (int i = 1; i < final.Count; i++)
                {
                    double tempSlope = (surroundzmaxy[i] - surroundzminy[i]) / (surroundzmax[i] - final[i]);
                    if (tempSlope != initSlope) intersect = true;
                    if (intersect) return null;
                }

                double tempZmax = -9999;
                double tempZmin = -9999;
                int indexZmax = 99;
                int indexZmin = 99;
                for (int i = 0; i < final.Count; i++)
                {
                    if (tempZmax < surroundzmax[i])
                    {
                        tempZmax = surroundzmax[i];
                        indexZmax = i;
                    }
                    if (tempZmin < surroundzmin[i])
                    {
                        tempZmin = surroundzmin[i];
                        indexZmin = i;
                    }
                }
                return surrounding_polygon_list[indexZmax];
            }
        }

        List<TPolygon> GenerateScanlinePolygon()
        {
            List<TPolygon> scanline_list_temp = new List<TPolygon>();
            int id = 0;
            for (int i = 0; i < obj.Count; i++)
            {
                for (int j = 0; j < obj[i].S.Length; j++)
                {
                    if (obj[i].visibleSurfaceIndex.Contains(j))
                    {
                        List<TPoint> P = new List<TPoint>();
                        TPoint p1 = obj[i].VS[obj[i].S[j].p1];
                        TPoint p2 = obj[i].VS[obj[i].S[j].p2];
                        TPoint p3 = obj[i].VS[obj[i].S[j].p3];
                        P.Add(p1);
                        P.Add(p2);
                        P.Add(p3);
                        Color c = obj[i].S[j].c;
                        TPoint p1p2 = FindVector(p1, p2);
                        TPoint p2p3 = FindVector(p2, p3);
                        TPoint N = CrossProduct(p1p2, p2p3);
                        double D = -(N.x * p1.x + N.y * p1.y + N.z * p1.z);
                        TPolygon scanlinePolygonTemp = new TPolygon();
                        scanlinePolygonTemp.polygon_id = id;
                        scanlinePolygonTemp.A = N.x;
                        scanlinePolygonTemp.B = N.y;
                        scanlinePolygonTemp.C = N.z;
                        scanlinePolygonTemp.D = D;
                        scanlinePolygonTemp.c = c;
                        scanlinePolygonTemp.P = P;
                        scanlinePolygonTemp.flag = false;
                        scanline_list_temp.Add(scanlinePolygonTemp);
                        id++;
                    }
                }
            }
            return scanline_list_temp;
        }

        public List<Edge>[] GenerateLocalSET(TPolygon polygon, int wholeymax)
        {
            int dx, dy, ymin, ymax, xofymin;
            int wholeymin = 9999;
            List<Edge>[] SET = new List<Edge>[wholeymax + 1];
            for (int i = 0; i <= wholeymax; i++)
            {
                SET[i] = new List<Edge>();
            }
            for (int i = 0; i < polygon.P.Count; i++)
            {
                TPoint first_vertices, second_vertices;
                int x1, y1, x2, y2;
                Edge edgeTemp = new Edge();
                if (i == polygon.P.Count - 1)
                {
                    first_vertices = polygon.P[i];
                    second_vertices = polygon.P[0];
                    x1 = Convert.ToInt32(first_vertices.x);
                    y1 = Convert.ToInt32(first_vertices.y);
                    x2 = Convert.ToInt32(second_vertices.x);
                    y2 = Convert.ToInt32(second_vertices.y);
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
                        edgeTemp.ymax = ymax;
                        edgeTemp.xofymin = xofymin;
                        edgeTemp.dx = dx;
                        edgeTemp.dy = dy;
                        SET[ymin].Add(edgeTemp);
                    }
                }
                else
                {
                    first_vertices = polygon.P[i];
                    second_vertices = polygon.P[i + 1];
                    x1 = Convert.ToInt32(first_vertices.x);
                    y1 = Convert.ToInt32(first_vertices.y);
                    x2 = Convert.ToInt32(second_vertices.x);
                    y2 = Convert.ToInt32(second_vertices.y);
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
                        edgeTemp.ymax = ymax;
                        edgeTemp.xofymin = xofymin;
                        edgeTemp.dx = dx;
                        edgeTemp.dy = dy;
                        SET[ymin].Add(edgeTemp);
                    }
                }
            }
            return SET;
        }

        public List<Edge>[] GenerateGlobalSET(List<Edge>[][] polygonSET, int wholeymax)
        {
            List<Edge>[] GlobalSET = new List<Edge>[wholeymax + 1];
            for (int i = 0; i <= wholeymax; i++)
            {
                GlobalSET[i] = new List<Edge>();
            }
            for (int i = 0; i < polygonSET.GetLength(0); i++)
            {
                for (int j = 0; j < wholeymax; j++)
                {
                    for (int k = 0; k < polygonSET[i][j].Count; k++)
                    {
                        polygonSET[i][j][k].polygon_id = i;
                        GlobalSET[j].Add(polygonSET[i][j][k]);
                    }
                }
            }
            return GlobalSET;
        }

        public List<Edge> CheckYmax(List<Edge> currentList, int ymin)
        {
            List<Edge> result = new List<Edge>(currentList);
            List<Edge> result1 = new List<Edge>();
            for (int i = 0; i < result.Count; i++)
            {
                if (result[i].ymax == ymin) result[i] = null;
            }
            for (int i = 0; i < result.Count; i++)
            {
                if (result[i] != null) result1.Add(result[i]);
            }
            return result1;
        }

        public List<Edge> SortList(List<Edge> ymaxCheckedList)
        {
            List<Edge> result = new List<Edge>(ymaxCheckedList);
            int n = result.Count;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    double x1 = Convert.ToDouble(result[j].xofymin) + (Convert.ToDouble(result[j].carrier) / Convert.ToDouble(result[j].dy));
                    double x2 = Convert.ToDouble(result[j + 1].xofymin) + (Convert.ToDouble(result[j + 1].carrier) / Convert.ToDouble(result[j + 1].dy));
                    if (x1 > x2)
                    {
                        Edge temp = result[j];
                        result[j] = result[j + 1];
                        result[j + 1] = temp;
                    }
                }
            }
            return result;
        }

        public List<Edge> ProcessList(List<Edge> sortedList)
        {
            List<Edge> result = new List<Edge>();
            for (int i = 0; i < sortedList.Count; i++)
            {
                Edge tempEdge = new Edge();
                tempEdge.ymax = sortedList[i].ymax;
                tempEdge.xofymin = sortedList[i].xofymin;
                tempEdge.dx = sortedList[i].dx;
                tempEdge.dy = sortedList[i].dy;
                tempEdge.carrier = sortedList[i].carrier;
                tempEdge.polygon_id = sortedList[i].polygon_id;
                result.Add(tempEdge);
            }
            for (int i = 0; i < result.Count; i++)
            {
                result[i].carrier += result[i].dx;
                if (result[i].carrier >= result[i].dy)
                {
                    while (result[i].carrier >= result[i].dy)
                    {
                        result[i].xofymin++;
                        result[i].carrier -= result[i].dy;
                    }
                }
                else if (result[i].carrier < 0)
                {
                    while (result[i].carrier < 0)
                    {
                        result[i].xofymin--;
                        result[i].carrier += result[i].dy;
                    }
                }
            }
            return result;
        }

        public List<Edge>[] GenerateAEL(List<Edge>[] globalSET, int wholeymax)
        {
            List<Edge>[] AEL = new List<Edge>[wholeymax + 1];
            for (int i = 0; i <= wholeymax; i++)
            {
                AEL[i] = new List<Edge>();
            }
            List<Edge> currentList = new List<Edge>();
            for (int i = 0; i < globalSET[0].Count; i++)
            {
                Edge tempEdge = new Edge();
                tempEdge.ymax = globalSET[0][i].ymax;
                tempEdge.xofymin = globalSET[0][i].xofymin;
                tempEdge.dx = globalSET[0][i].dx;
                tempEdge.dy = globalSET[0][i].dy;
                tempEdge.carrier = globalSET[0][i].carrier;
                tempEdge.polygon_id = globalSET[0][i].polygon_id;
                currentList.Add(tempEdge);
            }
            List<Edge> tempListInit = new List<Edge>(currentList);
            AEL[0] = tempListInit;
            for (int i = 1; i < globalSET.Length; i++)
            {
                List<Edge> ymaxCheckedList, processedList, sortedList;
                // Check y
                ymaxCheckedList = CheckYmax(currentList, i);
                // Process
                processedList = ProcessList(ymaxCheckedList);
                // Add new item list
                for (int j = 0; j < globalSET[i].Count; j++)
                {
                    Edge tempEdge = new Edge();
                    tempEdge.ymax = globalSET[i][j].ymax;
                    tempEdge.xofymin = globalSET[i][j].xofymin;
                    tempEdge.dx = globalSET[i][j].dx;
                    tempEdge.dy = globalSET[i][j].dy;
                    tempEdge.carrier = globalSET[i][j].carrier;
                    tempEdge.polygon_id = globalSET[i][j].polygon_id;
                    processedList.Add(tempEdge);
                }
                // Sort
                sortedList = SortList(processedList);
                currentList = new List<Edge>(sortedList);

                List<Edge> tempList = new List<Edge>(currentList);
                AEL[i] = tempList;
            }

            return AEL;
        }

        public double FindIntersection(TPolygon polygon1, TPolygon polygon2, double y)
        {
            double a1, b1, c1, a2, b2, c2, xi, zi;
            a1 = polygon1.A;
            b1 = polygon1.C;
            c1 = -polygon1.B * y - polygon1.D;
            a2 = polygon2.A;
            b2 = polygon2.C;
            c2 = -polygon2.B * y - polygon2.D;
            zi = (c1 * a2 - c2 * a1) / (b1 * a2 - b2 * a1);
            xi = (c1 - b1 * zi) / a1; // This is the intersection
            if (xi == double.PositiveInfinity) xi = 0.01;
            return xi;
        }

        public void VisibleSurfaceDetection(int index, List<Edge> AEL, List<TPolygon> scanline_polygon_list)
        {
            // Pseudocode:
            // 1. Set each polygons flag to false v
            // 2. Set the initial flag v
            // 3. Loop until the AEL.length-1 v
            // 3.1. Create a list of true flagged polygon v
            // 3.2. If the polygon flag is true, the add the polygon to the true flagged polygon list v
            // 3.3. Count the number of true flagged polygon v
            // 3.4. If there is no true flagged polygon, then continue/skip v
            // 3.5. Else If there are only 1 flagged polygon, then draw the span with the color of the polygon v
            // 3.6. Else v
            // 3.6.1. Create a list of sorted polygon based on z, the polygon with the biggest z is in the top of the list for the first x of the span v
            // 3.6.2. Create a list of sorted polygon based on z, the polygon with the biggest z is in the top of the list for the second x of the span v
            // 3.6.3. If the top of the sorted polygon for the first x is the same as the top of the sorted polygon for the second x, then draw the span with the color of the top polygon v
            // 3.6.4. Else (intersection) v
            // 3.6.4.1. Loop while first x sorted polygon length > 0 v
            // 3.6.4.1.1. Find the polygon with zmax and set it to the top v
            // 3.6.4.1.2. Find the intersection of the top polygon with the other polygon v
            // 3.6.4.1.3. If there is no intersection, then draw the top polygon from x1 to x2 v
            // 3.6.4.1.4. Else, then draw the top polygon from x1 to xi where xi is the minimum xi and xi >= x1 v
            // 3.6.4.1.5. Remove the top polygon from the list v
            // 3.6.4.1.6. Set x1 = xi v
            // 4. Update the polygon flag v

            // ** MAIN CODE **
            List<TPolygon> tempList = new List<TPolygon>();
            for (int i = 0; i < scanline_polygon_list.Count; i++)
            {
                TPolygon temp = new TPolygon();
                temp.polygon_id = scanline_polygon_list[i].polygon_id;
                temp.A = scanline_polygon_list[i].A;
                temp.B = scanline_polygon_list[i].B;
                temp.C = scanline_polygon_list[i].C;
                temp.D = scanline_polygon_list[i].D;
                temp.c = scanline_polygon_list[i].c;
                temp.P = scanline_polygon_list[i].P;
                temp.flag = scanline_polygon_list[i].flag;
                tempList.Add(temp);
            }
            // 1
            for (int i = 0; i < tempList.Count; i++)
                tempList[i].flag = false;
            // 2
            tempList[AEL[0].polygon_id].flag = true;
            // 3
            for (int i = 0; i < AEL.Count - 1; i++)
            {
                // 3.1
                List<TPolygon> flagged_polygon_list = new List<TPolygon>();
                // 3.2
                for (int j = 0; j < tempList.Count; j++) if (tempList[j].flag == true) flagged_polygon_list.Add(tempList[j]);
                // 3.3
                int flaggedCount = flagged_polygon_list.Count;
                // 3.4
                if (flaggedCount == 0)
                {
                    // Do nothing
                }
                // 3.5
                else if (flaggedCount == 1)
                {
                    Pen pen = new Pen(flagged_polygon_list[0].c);
                    g.DrawLine(pen, new Point(AEL[i].xofymin, index), new Point(AEL[i + 1].xofymin, index));
                }
                // 3.6
                else
                {
                    // 3.6.1 -> we copy the value to the list to avoid reference
                    List<TPolygon> first_x_sorted_polygon_list = new List<TPolygon>();
                    for (int j = 0; j < flagged_polygon_list.Count; j++)
                    {
                        TPolygon temp = new TPolygon();
                        temp.polygon_id = scanline_polygon_list[j].polygon_id;
                        temp.A = flagged_polygon_list[j].A;
                        temp.B = flagged_polygon_list[j].B;
                        temp.C = flagged_polygon_list[j].C;
                        temp.D = flagged_polygon_list[j].D;
                        temp.c = flagged_polygon_list[j].c;
                        temp.P = flagged_polygon_list[j].P;
                        temp.flag = flagged_polygon_list[j].flag;
                        first_x_sorted_polygon_list.Add(temp);
                    }
                    // 3.6.2 -> we copy the value to the list to avoid reference
                    List<TPolygon> second_x_sorted_polygon_list = new List<TPolygon>();
                    for (int j = 0; j < flagged_polygon_list.Count; j++)
                    {
                        TPolygon temp = new TPolygon();
                        temp.polygon_id = scanline_polygon_list[j].polygon_id;
                        temp.A = flagged_polygon_list[j].A;
                        temp.B = flagged_polygon_list[j].B;
                        temp.C = flagged_polygon_list[j].C;
                        temp.D = flagged_polygon_list[j].D;
                        temp.c = flagged_polygon_list[j].c;
                        temp.P = flagged_polygon_list[j].P;
                        temp.flag = flagged_polygon_list[j].flag;
                        second_x_sorted_polygon_list.Add(temp);
                    }
                    // Calculate z
                    double x1 = Convert.ToDouble(AEL[i].xofymin);
                    double x2 = Convert.ToDouble(AEL[i + 1].xofymin);
                    double y = Convert.ToDouble(index);
                    List<double> first_x_zlist = new List<double>();
                    for (int j = 0; j < first_x_sorted_polygon_list.Count; j++)
                    {
                        double A = first_x_sorted_polygon_list[j].A;
                        double B = first_x_sorted_polygon_list[j].B;
                        double C = first_x_sorted_polygon_list[j].C;
                        double D = first_x_sorted_polygon_list[j].D;
                        double z = -(A * x1 + B * y + D) / C;
                        first_x_zlist.Add(z);
                    }
                    List<double> second_x_zlist = new List<double>();
                    for (int j = 0; j < second_x_sorted_polygon_list.Count; j++)
                    {
                        double A = second_x_sorted_polygon_list[j].A;
                        double B = second_x_sorted_polygon_list[j].B;
                        double C = second_x_sorted_polygon_list[j].C;
                        double D = second_x_sorted_polygon_list[j].D;
                        double z = -(A * x2 + B * y + D) / C;
                        second_x_zlist.Add(z);
                    }
                    // Calculate the zmax
                    int firstXMaxZIndex = 99;
                    double firstXMaxZ = -9999;
                    for (int j = 0; j < first_x_zlist.Count; j++)
                    {
                        if (firstXMaxZ < first_x_zlist[j])
                        {
                            firstXMaxZIndex = j;
                            firstXMaxZ = first_x_zlist[j];
                        }
                    }
                    int secondXMaxZIndex = 99;
                    double secondXMaxZ = -9999;
                    for (int j = 0; j < second_x_zlist.Count; j++)
                    {
                        if (secondXMaxZ < second_x_zlist[j])
                        {
                            secondXMaxZIndex = j;
                            secondXMaxZ = second_x_zlist[j];
                        }
                    }
                    // Set the top of the list to the most highest z
                    TPolygon tempPolygon1 = first_x_sorted_polygon_list[firstXMaxZIndex];
                    first_x_sorted_polygon_list.RemoveAt(firstXMaxZIndex);
                    first_x_sorted_polygon_list.Insert(0, tempPolygon1);
                    TPolygon tempPolygon2 = second_x_sorted_polygon_list[secondXMaxZIndex];
                    second_x_sorted_polygon_list.RemoveAt(secondXMaxZIndex);
                    second_x_sorted_polygon_list.Insert(0, tempPolygon2);
                    // 3.6.3
                    if (first_x_sorted_polygon_list[0].polygon_id == second_x_sorted_polygon_list[0].polygon_id)
                    {
                        Pen pen = new Pen(first_x_sorted_polygon_list[0].c);
                        g.DrawLine(pen, new Point(AEL[i].xofymin, index), new Point(AEL[i + 1].xofymin, index));
                    }
                    // 3.6.4 (intersection exist)
                    else
                    {
                        // 3.6.4.1
                        while (first_x_sorted_polygon_list.Count > 0)
                        {
                            // 3.6.4.1.1
                            List<double> z_list = new List<double>();
                            for (int j = 0; j < first_x_sorted_polygon_list.Count; j++)
                            {
                                double A = first_x_sorted_polygon_list[j].A;
                                double B = first_x_sorted_polygon_list[j].B;
                                double C = first_x_sorted_polygon_list[j].C;
                                double D = first_x_sorted_polygon_list[j].D;
                                double z = -(A * x1 + B * y + D) / C;
                                z_list.Add(z);
                            }
                            int maxZIndex = 99;
                            double maxZ = -9999;
                            for (int j = 0; j < z_list.Count; j++)
                            {
                                if (maxZ < z_list[j])
                                {
                                    maxZIndex = j;
                                    maxZ = z_list[j];
                                }
                            }
                            TPolygon tempPolygon = first_x_sorted_polygon_list[maxZIndex];
                            first_x_sorted_polygon_list.RemoveAt(maxZIndex);
                            first_x_sorted_polygon_list.Insert(0, tempPolygon);
                            // 3.6.4.1.2
                            List<double> xiList = new List<double>();
                            for (int j = 1; j < first_x_sorted_polygon_list.Count; j++)
                            {
                                double xi = FindIntersection(first_x_sorted_polygon_list[0], first_x_sorted_polygon_list[j], y);
                                if (xi >= x1) xiList.Add(xi);
                            }
                            // 3.6.4.1.3
                            if (xiList.Count == 0)
                            {
                                Pen pen = new Pen(first_x_sorted_polygon_list[0].c);
                                g.DrawLine(pen, new Point(Convert.ToInt32(x1), index), new Point(Convert.ToInt32(x2), index));
                                // 3.6.4.1.5
                                first_x_sorted_polygon_list.RemoveAt(0);
                            }
                            // 3.6.4.1.4
                            else
                            {
                                double xiMin = 9999;
                                for (int j = 0; j < xiList.Count; j++) xiMin = Math.Min(xiMin, xiList[j]);
                                Pen pen = new Pen(first_x_sorted_polygon_list[0].c);
                                g.DrawLine(pen, new Point(Convert.ToInt32(x1), index), new Point(Convert.ToInt32(xiMin), index));
                                // 3.6.4.1.5
                                first_x_sorted_polygon_list.RemoveAt(0);
                                // 3.6.4.1.6
                                x1 = xiMin;
                            }
                        }
                    }
                }
                // 4
                if (tempList[AEL[i + 1].polygon_id].flag == true) tempList[AEL[i + 1].polygon_id].flag = false;
                else tempList[AEL[i + 1].polygon_id].flag = true;
            }
        }

        public void Scanline(List<TPolygon> scanline_polygon_list)
        {
            int wholeymax = -9999;
            for (int i = 0; i < scanline_polygon_list.Count; i++)
            {
                for (int j = 0; j < scanline_polygon_list[i].P.Count; j++)
                {
                    wholeymax = Math.Max(wholeymax, Convert.ToInt32(scanline_polygon_list[i].P[j].y));
                }
            }
            List<Edge>[][] polygonSET = new List<Edge>[scanline_polygon_list.Count][];
            for (int i = 0; i < scanline_polygon_list.Count; i++)
            {
                polygonSET[i] = GenerateLocalSET(scanline_polygon_list[i], wholeymax);
            }
            List<Edge>[] globalSET = GenerateGlobalSET(polygonSET, wholeymax);
            List<Edge>[] AEL = GenerateAEL(globalSET, wholeymax);
            for (int i = 0; i < AEL.Length; i++)
            {
                if (AEL[i].Count == 0) continue;
                else VisibleSurfaceDetection(i, AEL[i], scanline_polygon_list);
            }
            pictureBox1.Image = bmp;
        }

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
            obj[index].TranslateM = new double[4, 4] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } };
            obj[index].RotateM = new double[4, 4] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } };
        }
        public void TranslateObject(int index, double dx = 0, double dy = 0, double dz = 0)
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
            double[,] minTranslate = new double[4, 4] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { -dx, -dy, -dz, 1 } };
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
                p2 = V[S[i].p2];
                p3 = V[S[i].p3];
                p1p2 = FindVector(p1, p2);
                p2p3 = FindVector(p2, p3);
                N = CrossProduct(p1p2, p2p3);
                SetPoint(ref VN, 0, 0, -1);
                res = DotProduct(VN, N);
                if (res < 0) obj[index].visibleSurfaceIndex.Add(i);
            }
        }
        public void PolygonFill(Pen pen, TPoint[] P)
        {
            int dx, dy, ymin, ymax, xofymin; 
            int wholeymin = 9999; 
            int wholeymax = -9999; 

            var indexItemCount = new Int16[pictureBox1.Width];
            for (int i = 0; i < pictureBox1.Width; i++)
            {
                indexItemCount[i] = 0;
            }

            var SET = new Edge[pictureBox1.Height, pictureBox1.Width];
            for (int i = 0; i < P.Length; i++)
            {
                TPoint first_vertices, second_vertices;
                int x1, y1, x2, y2;
                Edge se = new Edge();
                if (i == P.Length - 1)
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
                        SET[ymin, indexItemCount[ymin]] = se;
                        indexItemCount[ymin]++;
                    }
                }
                else
                {
                    first_vertices = P[i];
                    second_vertices = P[i + 1];
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
                        SET[ymin, indexItemCount[ymin]] = se;
                        indexItemCount[ymin]++;
                    }
                }
            }
            ConstructAEL(SET, wholeymin, wholeymax, pen);
        }
        public Edge[] YmaxCheck(Edge[] cr, int index, int currentY)
        {
            int new_index = 0;
            var coppied_row = new Edge[pictureBox1.Width];
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
            var checkedCurrentRow = new Edge[pictureBox1.Width];
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
        public Edge[] ProcessCurrentRow(Edge[] cr, int index)
        {
            var processed = new Edge[pictureBox1.Width];
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
        public Edge[] SortCurrentRow(Edge[] cr, int index)
        {
            var sorted = new Edge[pictureBox1.Width];
            cr.CopyTo(sorted, 0);
            Edge temp = new Edge();
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
        public void ConstructAEL(Edge[,] se, int wholeymin, int wholeymax, Pen pen)
        {
            var current_row = new Edge[pictureBox1.Width];
            int current_row_index = 0;
            for (int i = wholeymin; i <= wholeymax - 1; i++)
            {

                current_row = YmaxCheck(current_row, current_row_index, i);

                current_row = ProcessCurrentRow(current_row, current_row_index);

                current_row = SortCurrentRow(current_row, current_row_index);

                var temp_row = new Edge[current_row_index + 1];
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


        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }

        private void importObjectBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog // Create an object from the OpenFileDialog Class.
            {
                InitialDirectory = @"D:\", // Initial directory when the dialogbox open for the first time.
                Title = "Browse Text Files", // Dialogbox title.

                CheckFileExists = true, // Check if the file exist.
                CheckPathExists = true, // Check if the path exist.

                DefaultExt = "txt", // Default file extension.
                Filter = "txt files (*.txt)|*.txt", // File extension filter.

                ReadOnlyChecked = true, // Read file only.
                ShowReadOnly = true // Only for reading the file.
            };
            if (ofd.ShowDialog() == DialogResult.OK) // If the user click open.
            {
                var sr = new StreamReader(ofd.FileName); // make a variable from the StreamReader Class.
                string line; // this variable will hold the current line from the file.

                //obj = new TObject[nObj];
                int PNum = 0, SNum = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] lineSplit = line.Split(',');
                    if (lineSplit[0] == "PNum")
                    {
                        PNum = Convert.ToInt32(lineSplit[1]);
                    }
                    else if (lineSplit[0] == "SNum")
                    {
                        SNum = Convert.ToInt32(lineSplit[1]);
                    }
                }
                //obj[nObj - 1] = new TObject(PNum, SNum);
                TObject tempObj = new TObject(PNum, SNum);
                sr = new StreamReader(ofd.FileName); // make a variable from the StreamReader Class.
                while ((line = sr.ReadLine()) != null)
                {
                    string[] lineSplit = line.Split(',');
                    if (lineSplit[0] == "ObjectName")
                    {
                        tempObj.objectName = lineSplit[1];
                    }
                    else if (lineSplit[0] == "Point")
                    {
                        SetPoint(ref tempObj.P[Convert.ToInt32(lineSplit[1])], Convert.ToDouble(lineSplit[2]), Convert.ToDouble(lineSplit[3]), Convert.ToDouble(lineSplit[4]));
                    }
                    else if (lineSplit[0] == "Surface")
                    {
                        SetSurface(ref tempObj.S[Convert.ToInt32(lineSplit[1])], Convert.ToInt32(lineSplit[2]), Convert.ToInt32(lineSplit[3]), Convert.ToInt32(lineSplit[4]), Color.FromName(lineSplit[5]));
                    }
                }
                obj.Add(tempObj);
            }
            ResetTransM(obj.Count-1);
            selectListBox.Items.Add(obj[obj.Count-1].objectName);
            Display();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            //obj = new TObject[nObj];

            // 1. Prepare the objects
            TObject objTemp = new TObject(4, 4);
            objTemp.objectName = "Triangular Pyramid";
            SetPoint(ref objTemp.P[0], -1, -1, 1);
            SetPoint(ref objTemp.P[1], 1, -1, 1);
            SetPoint(ref objTemp.P[2], 0, 1, 0);
            SetPoint(ref objTemp.P[3], 0, -1, -1);
            SetSurface(ref objTemp.S[0], 0, 1, 2, Color.Red);
            SetSurface(ref objTemp.S[1], 1, 3, 2, Color.Yellow);
            SetSurface(ref objTemp.S[2], 3, 0, 2, Color.Green);
            SetSurface(ref objTemp.S[3], 0, 3, 1, Color.Blue);
            obj.Add(objTemp);

            TObject objTemp1 = new TObject(4, 4);
            objTemp1 = new TObject(5, 6);
            objTemp1.objectName = "Square Pyramid";
            SetPoint(ref objTemp1.P[0], -1, -1, 1);
            SetPoint(ref objTemp1.P[1], 1, -1, 1);
            SetPoint(ref objTemp1.P[2], 1, -1, -1);
            SetPoint(ref objTemp1.P[3], -1, -1, -1);
            SetPoint(ref objTemp1.P[4], 0, 1, 0);
            SetSurface(ref objTemp1.S[0], 0, 1, 4, Color.Purple);
            SetSurface(ref objTemp1.S[1], 1, 2, 4, Color.Orange);
            SetSurface(ref objTemp1.S[2], 2, 3, 4, Color.Magenta);
            SetSurface(ref objTemp1.S[3], 3, 0, 4, Color.Black);
            SetSurface(ref objTemp1.S[4], 3, 2, 0, Color.Gray);
            SetSurface(ref objTemp1.S[5], 2, 1, 0, Color.Aqua);
            obj.Add(objTemp1);

            for (int i = 0; i < obj.Count; i++)
            {
                selectListBox.Items.Add(obj[i].objectName);
                ResetTransM(i);
            }


            TranslateObject(0, -2, 0, 0);
            RotateObjectOnX(0, 45);
            TranslateObject(1, 2, 0, 0);
            RotateObjectOnY(1, -45);

            //TranslateObject(0, 0, 0, -0.1);
            //TranslateObject(1, 0, 0, 0);
            //RotateObjectOnX(0, 45);
            //RotateObjectOnY(0, 45);

            selectListBox.SetSelected(0, true);

            translateRB.Checked = true;
            frontSurfaceCB.Checked = true;
            scanlineRB.Checked = true;

            Display();
        }



        int keycount = 0;
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (keycount % 100 == 0) debug = "Key pressed: " + Environment.NewLine;
            selectedObjectIndex = selectListBox.SelectedIndex;
            debug += e.KeyCode + " ";
            keycount++;

            if (e.KeyCode == Keys.W)
            {
                upBtn.BackColor = Color.Yellow;
                if (translateRB.Checked) TranslateObject(selectedObjectIndex, 0, 0.1, 0);
                else RotateObjectOnX(selectedObjectIndex, -1);
            }
            else if (e.KeyCode == Keys.S)
            {
                downBtn.BackColor = Color.Yellow;
                if (translateRB.Checked) TranslateObject(selectedObjectIndex, 0, -0.1, 0);
                else RotateObjectOnX(selectedObjectIndex, 1);
            }
            else if (e.KeyCode == Keys.A)
            {
                leftBtn.BackColor = Color.Yellow;
                if (translateRB.Checked) TranslateObject(selectedObjectIndex, -0.1, 0, 0);
                else RotateObjectOnZ(selectedObjectIndex, 1);
            }
            else if (e.KeyCode == Keys.D)
            {
                rightBtn.BackColor = Color.Yellow;
                if (translateRB.Checked) TranslateObject(selectedObjectIndex, 0.1, 0, 0);
                else RotateObjectOnZ(selectedObjectIndex, -1);
            }
            else if (e.KeyCode == Keys.Q)
            {
                frontBtn.BackColor = Color.Yellow;
                if (translateRB.Checked) TranslateObject(selectedObjectIndex, 0, 0, 0.1);
                else RotateObjectOnY(selectedObjectIndex, -1);
            }
            else if (e.KeyCode == Keys.E)
            {
                backBtn.BackColor = Color.Yellow;
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
            else if (e.KeyCode == Keys.Up)
            {
                int index = selectListBox.SelectedIndex;
                if (index > 0) index -= 1;
                selectListBox.SetSelected(index, true);
            }
            else if (e.KeyCode == Keys.Down)
            {
                int index = selectListBox.SelectedIndex;
                if (index < selectListBox.Items.Count - 1) index += 1;
                selectListBox.SetSelected(index, true);
            }
            else if (e.KeyCode == Keys.R)
            {
                resetBtn.BackColor = Color.Yellow;
                int index = selectListBox.SelectedIndex;
                ResetTransM(index);
            }
            Display();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                upBtn.BackColor = Color.White;
            }
            else if (e.KeyCode == Keys.S)
            {
                downBtn.BackColor = Color.White;
            }
            else if (e.KeyCode == Keys.A)
            {
                leftBtn.BackColor = Color.White;
            }
            else if (e.KeyCode == Keys.D)
            {
                rightBtn.BackColor = Color.White;
            }
            else if (e.KeyCode == Keys.Q)
            {
                frontBtn.BackColor = Color.White;
            }
            else if (e.KeyCode == Keys.E)
            {
                backBtn.BackColor = Color.White;
            }
            else if (e.KeyCode == Keys.R)
            {
                resetBtn.BackColor = Color.White;
            }
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

        private void drawRB_CheckedChanged(object sender, EventArgs e)
        {
            filledCB.Enabled = true;
            frontSurfaceCB.Enabled = true;
            Display();
        }

        private void warnockRB_CheckedChanged(object sender, EventArgs e)
        {
            filledCB.Enabled = false;
            frontSurfaceCB.Enabled = false;
            Display();
        }

        private void scanlineRB_CheckedChanged(object sender, EventArgs e)
        {
            filledCB.Enabled = false;
            frontSurfaceCB.Enabled = false;
            Display();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            label5.Text = "x = " + e.X.ToString();
            label6.Text = "y = " + e.Y.ToString();
        }
    }
}
