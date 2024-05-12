using ViewModel;
using FluentAssertions;


namespace ViewModelTests
{
    public class UnitTests
    {
        public ViewData viewdata;

        [Fact]
        public void CheckValidation()
        {
            /*
            проверяем выполнение и не выполнение следующих условий
             
            "Mesh_Nodes_Number", "FrequentNodesNum", "Input_Boundaries", "SplineNodesNum"

            Spline_Nodes_Number < 2 || Spline_Nodes_Number > Mesh_Nodes_Number
            Input_Boundaries[0] >= Input_Boundaries[1]
            Frequent_Mesh_Nodes_Number < 3 ?
            Mesh_Nodes_Number < 3 ?
            */
            viewdata = new ViewData();

            viewdata["Input_Boundaries"].Should().BeNull();
            viewdata.Input_Boundaries = [10, 0];
            viewdata["Input_Boundaries"].Should().NotBeNull();

            
            viewdata["FrequentNodesNum"].Should().BeNull();
            viewdata.Frequent_Mesh_Nodes_Number = 2;
            viewdata["FrequentNodesNum"].Should().NotBeNull();
            

            viewdata["Mesh_Nodes_Number"].Should().BeNull();
            viewdata.Mesh_Nodes_Number = 2; 
            viewdata["Mesh_Nodes_Number"].Should().NotBeNull();

            viewdata["SplineNodesNum"].Should().NotBeNull();//нарушается Spline_Nodes_Number > Mesh_Nodes_Number

            viewdata["Input_Boundaries"].Should().NotBeNull();
            viewdata["FrequentNodesNum"].Should().NotBeNull();
            viewdata["Mesh_Nodes_Number"].Should().NotBeNull();
            viewdata["SplineNodesNum"].Should().NotBeNull();
        }

        [Fact]
        public void CheckSpline()
        {
            // проверяем разные функции на разных отрезках
            viewdata = new ViewData();
            viewdata.Input_Boundaries = [0, 1];
            viewdata.Spline_Nodes_Number = 40;
            viewdata.Mesh_Nodes_Number = 100;
            List<double[]>? ans;

            viewdata.Function_In_List = 2; // x^3
            viewdata.GetDataArray();
            viewdata.GetSplineData();
            viewdata.GetSpline();

            ans = viewdata.splinedata.Nodes;
            foreach(double[] node in ans)
                node[1].Should().BeApproximately(node[0] * node[0] * node[0], 1e-3);

            viewdata.Function_In_List = 1; // x^2
            viewdata.Input_Boundaries = [-1, 1];
            viewdata.GetDataArray();
            viewdata.GetSplineData();
            viewdata.GetSpline();

            ans = viewdata.splinedata.Nodes;
            foreach (double[] node in ans)
                node[1].Should().BeApproximately(node[0] * node[0], 1e-3);

            viewdata.Function_In_List = 0; // x
            viewdata.Input_Boundaries = [-10, 10];
            viewdata.GetDataArray();
            viewdata.GetSplineData();
            viewdata.GetSpline();

            ans = viewdata.splinedata.Nodes;
            foreach (double[] node in ans)
                node[1].Should().BeApproximately(node[0], 1e-3);

            viewdata.Function_In_List = 3; // cosx
            viewdata.Input_Boundaries = [0, 1];
            viewdata.GetDataArray();
            viewdata.GetSplineData();
            viewdata.GetSpline();

            ans = viewdata.splinedata.Nodes;
            foreach (double[] node in ans)
                node[1].Should().BeApproximately(Math.Cos(node[0]), 1e-3);
        }
    }
}