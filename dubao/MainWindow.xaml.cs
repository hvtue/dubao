using Microsoft.ML.Probabilistic.Distributions;
using Microsoft.ML.Probabilistic.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.ML.Probabilistic.Models;
using Microsoft.ML.Probabilistic.Distributions;
using Microsoft.ML.Probabilistic.Algorithms;
using Microsoft.ML.Probabilistic.Math;


namespace dubao
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnChangeProperty(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private string _nhapso;
        public string nhapso
        {
            get { return _nhapso; }
            set
            {
                _nhapso = value;
                OnChangeProperty(nameof(nhapso));
            }
        }
        private string _result;
        public string result
        {
            get { return _result; }
            set
            {
                _result = value;
                OnChangeProperty(nameof(result));
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
           


                int n = 30; // Số lần lấy và trả lại
                int k = 3; // Số lần lấy được số 13

                double p = 1.0 / 90.0; // Xác suất lấy được số 13 trong mỗi lần thử

                Variable<int> successCount = Variable.Binomial(n, p).Named("successCount");
                Variable<bool> atLeastThreeSuccesses = (successCount >= k);

                InferenceEngine engine = new InferenceEngine();
                double probability = engine.Infer<Bernoulli>(atLeastThreeSuccesses).GetProbTrue();

            //Console.WriteLine("Xác suất lấy được ít nhất 3 lần số 13 trong 30 lần lấy và trả lại: " + probability);
            result = "Xác suất lấy được ít nhất 3 lần số 13 trong 30 lần lấy và trả lại: " + probability;
            sxchachan();
            //processManual();
            //sxcanthiet();
            //sxcanthietManual();
    }

        private void sxchachan()
        {
            int n = 30; // Số lần lấy và trả lại
            int k = 3; // Số lần lấy được số 13

            Variable<double> p = Variable.Beta(1, 1).Named("p");
            Variable<int> successCount = Variable.Binomial(n, p).Named("successCount");
            Variable<bool> atLeastThreeSuccesses = (successCount >= k);

            InferenceEngine engine = new InferenceEngine();
            engine.ShowProgress = false;

            // Tính xác suất chắc chắn (đạt đến gần 100%) để có ít nhất 3 lần số 13 trong 30 lần lấy và trả lại
            double requiredProbability = engine.Infer<Bernoulli>(atLeastThreeSuccesses).GetProbTrue();

            result += "Xác suất chắc chắn (đạt gần 100%) để có ít nhất 3 lần số 13 trong 30 lần lấy và trả lại: " + requiredProbability;
        }

        private void processManual()
            {
                int n = 30; // Số lần lấy và trả lại
                int k = 3; // Số lần lấy được số 13

                double p = 1.0 / 90.0; // Xác suất lấy được số 13 trong mỗi lần thử
                double probability = 0.0;

                for (int i = k; i <= n; i++)
                {
                    double combination = CalculateCombination(n, i);
                    double successProbability = Math.Pow(p, i);
                    double failureProbability = Math.Pow(1 - p, n - i);

                    probability += combination * successProbability * failureProbability;
                }

            //Console.WriteLine("Xác suất lấy được ít nhất 3 lần số 13 trong 30 lần lấy và trả lại: " + probability);
            result += "Xác suất lấy được ít nhất 3 lần số 13 trong 30 lần lấy và trả lại: " + probability;
            }

            private double CalculateCombination(int n, int k)
            {
                double numerator = Factorial(n);
                double denominator = Factorial(k) * Factorial(n - k);
                return numerator / denominator;
            }

            private double Factorial(int n)
            {
                if (n <= 1)
                    return 1;

                double result = 1;
                for (int i = 2; i <= n; i++)
                {
                    result *= i;
                }
                return result;
            }

        private void sxcanthiet()
        {

            int n = 30; // Số lần lấy và trả lại
            int k = 3; // Số lần lấy được số 13

            int numParticles = 10000; // Số lượng hạt trong Importance Sampling

            Variable<double> p = Variable.Beta(1, 1).Named("p");
            Variable<int> successCount = Variable.Binomial(n, p).Named("successCount");
            Variable<bool> atLeastThreeSuccesses = (successCount >= k);

            // Sử dụng Importance Sampling để xấp xỉ xác suất
            double requiredProbability = EstimateCumulativeProbability(atLeastThreeSuccesses, numParticles);

            Console.WriteLine("Xác suất cần thiết để có ít nhất 3 lần số 13 trong 30 lần lấy và trả lại: " + requiredProbability);
        }

        // Hàm xấp xỉ giá trị tích lũy bằng phương pháp Importance Sampling
        static double EstimateCumulativeProbability(Variable<bool> variable, int numParticles)
        {
            double cumulativeWeight = 0;

            for (int i = 0; i < numParticles; i++)
            {
                double weight = ImportanceWeight(variable);
                bool sample = Sample(variable);
                if (sample)
                    cumulativeWeight += weight;
            }

            return cumulativeWeight / numParticles;
        }

        // Hàm tính trọng số quan trọng (importance weight)
        static double ImportanceWeight(Variable<bool> variable)
        {
            InferenceEngine engine = new InferenceEngine();
            double weight = engine.Infer<Bernoulli>(variable).GetProbTrue();
            return weight;
        }

        // Hàm lấy mẫu từ phân phối Bernoulli
        static bool Sample(Variable<bool> variable)
        {
            InferenceEngine engine = new InferenceEngine();
            bool sample = engine.Infer<Bernoulli>(variable).Sample();
            return sample;
        }
        private void sxcanthietManual()
        {
            int n = 30; // Số lần lấy và trả lại
            int k = 3; // Số lần lấy được số 13

            double p = 1.0 / 90.0; // Xác suất lấy được số 13 trong mỗi lần thử
            double requiredProbability = 0.0;

            for (int i = k; i <= n; i++)
            {
                double combination = CalculateCombination1(n, i);
                double successProbability = Math.Pow(p, i);
                double failureProbability = Math.Pow(1 - p, n - i);

                double probability = combination * successProbability * failureProbability;

                requiredProbability += probability;
            }

            result += "Xác suất cần thiết để có ít nhất 3 lần số 13 trong 30 lần lấy và trả lại: " + requiredProbability;
        }

        private double CalculateCombination1(int n, int k)
        {
            double numerator = Factorial1(n);
            double denominator = Factorial1(k) * Factorial1(n - k);
            return numerator / denominator;
        }

        private double Factorial1(int n)
        {
            if (n <= 1)
                return 1;

            double result = 1;
            for (int i = 2; i <= n; i++)
            {
                result *= i;
            }
            return result;
        }
    }
}
