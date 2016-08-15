using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication5
{
    public class ReportPoint
    {
        private double _dblX;
        private double _dblY;

        public ReportPoint(double X_Coordinate, double Y_Coordinate)
        {
            _dblX = X_Coordinate;
            _dblY = Y_Coordinate;
        }

        public ReportPoint() { 
        }

        public double X_Coord
        {
            get { return _dblX; }
            set { _dblX = value; }
        }
        public double Y_Coord
        {
            get { return _dblY; }
            set { _dblY = value; }
        }


        //method for return slope and rsqrd
        public string calcValues(ArrayList alPoints)
        {
            double sumOfX = 0;
            double sumOfY = 0;
            double sumOfXSq = 0;
            double sumOfYSq = 0;
            double ssX = 0;
            double ssY = 0;
            double sumCodeviates = 0;
            double sCo = 0;


            for (int ctr = 0; ctr < alPoints.Count; ctr++)
            {
                ReportPoint objPoint = (ReportPoint)alPoints[ctr];
                double x = double.Parse(objPoint.X_Coord.ToString());
                double y = double.Parse(objPoint.Y_Coord.ToString());
                sumCodeviates += (x * y);
                sumOfX += x;
                sumOfY += y;
                sumOfXSq = sumOfXSq + (x * x);
                sumOfYSq = sumOfYSq + (y * y);
            }

            sumOfXSq = Math.Round(sumOfXSq, 2);
            sumOfYSq = Math.Round(sumOfYSq, 2);
            ssX = sumOfXSq - ((sumOfX * sumOfX) / alPoints.Count);
            ssY = sumOfYSq - ((sumOfY * sumOfY) / alPoints.Count);
            double RNumerator = (alPoints.Count * sumCodeviates) - (sumOfX * sumOfY);
            double RDenom = (alPoints.Count * sumOfXSq - (Math.Pow(sumOfX, 2))) * (alPoints.Count * sumOfYSq - (Math.Pow(sumOfY, 2)));
            sCo = sumCodeviates - ((sumOfX * sumOfY) / alPoints.Count);
            double dblSlope = sCo / ssX;
            double meanX = sumOfX / alPoints.Count;
            double meanY = sumOfY / alPoints.Count;
            double dblYintercept = meanY - (dblSlope * meanX);
            double dblR = RNumerator / Math.Sqrt(RDenom);
       
         //   Console.WriteLine("Y-Intercept: {0}", dblYintercept);
         //   double slopeWithoutIntercept = sumOfY / sumOfX;
            double slopeWithoutIntercept = sumCodeviates / sumOfXSq;

            return slopeWithoutIntercept + "," + Math.Pow(dblR, 2);
        } 



    }
}
