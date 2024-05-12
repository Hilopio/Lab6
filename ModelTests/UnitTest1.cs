using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Task1;

namespace ModelTests
{
    public class UnitTests
    {
        [Fact]
        public void CheckDataArray()
        {
            double[] x1 = { 1, 2, 4, 10, 7 };
            static void FValues_F(double x, ref double y1, ref double y2)
            {
                y1 = x + 3;
                y2 = 0;
            }
            DataArray arr1 = new DataArray("x1", DateTime.Today, x1, FValues_F);
            
            arr1.Grid.Should().Equal(x1);
            arr1.Fields[0].Should().Equal(4, 5, 7, 13, 10);
            arr1.Fields[1].Should().OnlyContain(n => n == 0.0);
            arr1.MaxDistance.Should().BeApproximately(9.0F, 0.0001F);
            arr1[0].Should().Equal(4, 5, 7, 13, 10);
            arr1[1].Should().OnlyContain(n => n == 0.0);

        }

        [Fact]
        public void CheckSpline()
        {
            SplineData splinedata;
            int len = 100;
            double[] x = new double[len + 1];
            for( int i = 0; i < len + 1; i++ )
            {
                x[i] = (double)i / len;
            }
            static void FValues_F(double x, ref double y1, ref double y2)
            {
                y1 = x * x;
                y2 = 0;
            }
            DataArray arr = new DataArray("x", DateTime.Today, x, FValues_F);

            splinedata = new SplineData(arr, 10, 100, 21, 1e-4);
            SplineData.SplineBuilding(splinedata);
            List<double[]>? ans;
            ans = splinedata.Nodes;
            foreach (double[] node in ans)
                node[1].Should().BeApproximately(node[0] * node[0], 1e-3);
        }
    }
}