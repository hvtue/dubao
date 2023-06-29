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
using Microsoft.ML.Probabilistic.Factors;
using System.Collections.ObjectModel;
using System.Security.Policy;
using System.Net;
using System.Xml;
using Newtonsoft.Json;
using System.Globalization;
using System.Data.OleDb;
using System.Data;
using Microsoft.ML;
using Microsoft.ML.Data;
using System.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearRegression;
using Accord.Statistics.Models.Regression.Linear;
using Accord.MachineLearning;
using Accord.MachineLearning.VectorMachines;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Math.Optimization.Losses;
using Microsoft.ML.Probabilistic;
using Range = Microsoft.ML.Probabilistic.Models.Range;
using static Microsoft.ML.DataOperationsCatalog;

using Microsoft.ML.AutoML;
using System.IO;



namespace dubao
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,INotifyPropertyChanged
    {
        
        private string scon = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=data.accdb;Persist Security Info=False;";
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            listDai = new ObservableCollection<modelDai>();
            listDai.Add(new modelDai()
            {
                Id = 1,
                Name = "Quảng Bình",
                Url = "https://xskt.com.vn/rss-feed/quang-binh-xsqb.rss"
            });
            listDai.Add(new modelDai()
            {
                Id = 2,
                Name = "Ninh Thuận",
                Url = "https://xskt.com.vn/rss-feed/ninh-thuan-xsnt.rss"
            });

            OnChangeProperty(nameof(listDai));
            try
            {
                var path = System.IO.Path.Combine(Environment.CurrentDirectory);

                var dt = new DataTable();
                using (var con = new OleDbConnection(scon))
                {
                    if (ConnectionState.Closed == con.State)
                    {
                        con.Open();
                    }

                    var sql = "SELECT * FROM nhadai";

                    using (var cmd = new OleDbCommand(sql, con))
                    {
                        using (var adapter = new OleDbDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
                var list = dt.AsEnumerable().Select(s => new modelDai()
                {
                    Id = int.Parse(s["ID"].ToString()),
                    Name = s["ten"].ToString(),
                    Url = s["url"].ToString()
                }).ToList();
                listDai = new ObservableCollection<modelDai>(list);
            }
            catch
            {
                //
            }
            xstrung = sxchacnui();
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
        private double _xstrung;
        public double xstrung { get { return _xstrung; }
            set { _xstrung = value;
                OnChangeProperty(nameof(xstrung));
            }
        }
        public ObservableCollection<modelDai> listDai { get; set; }
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {              
                foreach (var selectvalue in listDai)
                {
                    if (selectvalue == null) continue;
                    //var selectvalue=cobdai.SelectedItem as modelDai;
                    using (var webclient = new WebClient())
                    {
                        var uri = new Uri(selectvalue.Url);
                        var data = webclient.DownloadString(uri);
                        var docxml = new XmlDocument();
                        docxml.LoadXml(data);
                        var listNote = docxml.SelectNodes(@"rss/channel/item");
                        //var json =JsonConvert.SerializeXmlNode(docxml);
                        var formatDate = "ddd, dd MMM yyyy HH:mm:ss 'GMT'";
                        if (listNote.Count > 0)
                        {
                            foreach (var note in listNote)
                            {
                                var json = JsonConvert.SerializeObject(note);
                                var obj = JsonConvert.DeserializeObject<modelItem>(json);
                                if (obj == null) { continue; }

                                DateTimeOffset dateOffset = DateTimeOffset.ParseExact(obj.item.pubDate, formatDate, CultureInfo.InvariantCulture);

                                var dateInLinkArray = obj.item.link.Split("ngay-");// obj.item.link.Substring(obj.item.link.IndexOf("-") + 1);
                                if (dateInLinkArray.Length < 2)
                                    continue;
                                var dateInLink = dateInLinkArray[1];
                                var doffset = DateTimeOffset.ParseExact(dateInLink, "d-M-yyyy", CultureInfo.InvariantCulture);
                                var date = doffset.DateTime.Date;

                            var cktontai = await Task<DataTable>.Run(() => { return getdtKq(selectvalue.Id, date); });
                                if (cktontai != null && cktontai.Rows.Count > 0)
                                    continue;

                                var arrayNum = obj.item.description.Split('\n');
                                if (arrayNum.Length > 0)
                                {
                                    foreach (var item in arrayNum)
                                    {
                                        var stNum = item.Substring(item.IndexOf(":") + 1);
                                        if (stNum.Length > 0)
                                        {
                                            var arraySt = stNum.Split("-");
                                            if (arraySt.Length > 0)
                                            {
                                                foreach (string st in arraySt)
                                                {
                                                    var tmp = st.Replace(" ", "").Trim();
                                                    if (tmp.Length > 0)
                                                    {
                                                        var result = getNumberLast(tmp);
                                                        var numfull = 0;
                                                        int.TryParse(tmp, out numfull);
                                                        var num = 0;
                                                        if (int.TryParse(result, out num))
                                                        {
                                                        await Task.Run(() => { insertKq(selectvalue.Id, date, numfull, num); });
                                                        }
                                                        foreach (var ch in tmp)
                                                        {
                                                            var numone = 0;
                                                            if (int.TryParse(ch.ToString(), out numone))
                                                            {
                                                            await Task.Run(() => { insertNumOne(selectvalue.Id, date, numone, 1); });// tue fig github
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }
                }             
        }

        private DataTable getdtKq(int iddai, DateTime date)
        {
            try
            {
                var dt = new DataTable();
                using(var con =new OleDbConnection(scon))
                {
                   if(con.State==ConnectionState.Closed) con.Open();

                    var sql = "SELECT * FROM kq WHERE iddai=@iddai and ngay =@ngay";
                    using (var cmd = new OleDbCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@iddai", iddai);
                        cmd.Parameters.AddWithValue("@ngay", date);
                        //var para = new OleDbParameter("@iddai", OleDbType.Numeric);
                        //para.Value = iddai;
                        //para.Direction = ParameterDirection.Input;
                        //cmd.Parameters.Add(para);
                        //para = new OleDbParameter("@ngay", OleDbType.Date);
                        //para.Value = date;
                        //para.Direction = ParameterDirection.Input;
                        using(var dtadapter= new OleDbDataAdapter(cmd))
                        {
                            dtadapter.Fill(dt);
                        }
                    }

                }
                return dt;

            }
            catch(Exception ex)
            {

                return new DataTable();
            }
        }

        private DataTable getdtKqFull()
        {
            try
            {
                var dt = new DataTable();
                using (var con = new OleDbConnection(scon))
                {
                    if (con.State == ConnectionState.Closed) con.Open();

                    var sql = "SELECT * FROM kq";
                    using (var cmd = new OleDbCommand(sql, con))
                    {
                        //cmd.Parameters.AddWithValue("@iddai", iddai);
                        //cmd.Parameters.AddWithValue("@ngay", date);
                        //var para = new OleDbParameter("@iddai", OleDbType.Numeric);
                        //para.Value = iddai;
                        //para.Direction = ParameterDirection.Input;
                        //cmd.Parameters.Add(para);
                        //para = new OleDbParameter("@ngay", OleDbType.Date);
                        //para.Value = date;
                        //para.Direction = ParameterDirection.Input;
                        using (var dtadapter = new OleDbDataAdapter(cmd))
                        {
                            dtadapter.Fill(dt);
                        }
                    }

                }
                return dt;

            }
            catch (Exception ex)
            {

                return new DataTable();
            }
        }
        private DataTable getdtNumOneFull()
        {
            try
            {
                var dt = new DataTable();
                using (var con = new OleDbConnection(scon))
                {
                    if (con.State == ConnectionState.Closed) con.Open();

                    var sql = "SELECT * FROM numone";
                    using (var cmd = new OleDbCommand(sql, con))
                    {                        
                        using (var dtadapter = new OleDbDataAdapter(cmd))
                        {
                            dtadapter.Fill(dt);
                        }
                    }

                }
                return dt;

            }
            catch (Exception ex)
            {

                return new DataTable();
            }
        }

        private void deleteKq()
        {
            try
            {

                using (var con = new OleDbConnection(scon))
                {
                    if (con.State == ConnectionState.Closed) con.Open();

                    var sql = "delete from kq";
                    using (var cmd = new OleDbCommand(sql, con))
                    {                            
                        cmd.ExecuteNonQuery();
                    }

                }


            }
            catch
            {
                // return new DataTable();
            }
        }
        private void insertKq(int iddai, DateTime date, int numfull, int num)
        {
            try
            {
               
                using (var con = new OleDbConnection(scon))
                {
                    if (con.State == ConnectionState.Closed) con.Open();

                    var sql = "insert into kq(numfull,num,iddai,ngay) values(@numfull,@num,@iddai,@ngay)";
                    using (var cmd = new OleDbCommand(sql, con))
                    {
                        //var para = new OleDbParameter("@iddai", OleDbType.Integer);
                        //para.Value = iddai;
                        //para.Direction = ParameterDirection.Input;
                        //cmd.Parameters.Add(para);
                        //para = new OleDbParameter("@ngay", OleDbType.Date);
                        //para.Value = date;
                        //para.Direction = ParameterDirection.Input;
                        //para = new OleDbParameter("@numfull", OleDbType.Integer); 
                        //para.Value = numfull;
                        //para.Direction = ParameterDirection.Input;
                        //cmd.Parameters.Add(para);
                        //para = new OleDbParameter("@num", OleDbType.Integer);
                        //para.Value = num;
                        //para.Direction = ParameterDirection.Input;
                        //cmd.Parameters.Add(para);

                        cmd.Parameters.AddWithValue("@numfull", numfull);
                        cmd.Parameters.AddWithValue("@num", num);
                        cmd.Parameters.AddWithValue("@iddai", iddai);
                        cmd.Parameters.AddWithValue("@ngay", date);
                        cmd.ExecuteNonQuery();
                    }

                }
                

            }
            catch
            {
               // return new DataTable();
            }
        }

        private void insertNumOne(int iddai, DateTime date, int num, int sl)
        {
            try
            {

                using (var con = new OleDbConnection(scon))
                {
                    if (con.State == ConnectionState.Closed) con.Open();

                    var sql = "INSERT INTO numone(iddai,num,sl,ngay) VALUES(@iddai,@num,@sl,@ngay)";
                    using (var cmd = new OleDbCommand(sql, con))
                    {                       
                        cmd.Parameters.AddWithValue("@sl", sl);
                        cmd.Parameters.AddWithValue("@num", num);
                        cmd.Parameters.AddWithValue("@iddai", iddai);
                        cmd.Parameters.AddWithValue("@ngay", date);
                        cmd.ExecuteNonQuery();
                    }

                }


            }
            catch
            {
                // return new DataTable();
            }
        }
        private string getNumberLast(string stnumber)
        {
            if (string.IsNullOrEmpty(stnumber)) return "";

            return stnumber.Substring(stnumber.Length - 2);
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
            //sxchachan();
            sxchacnui();
            //processManual();
            //sxcanthiet();
            //sxcanthietManual();
    }

        private async void quickstart()
        {
            // Initialize MLContext
            MLContext ctx = new MLContext();

            // Define data path
            var dataPath = System.IO.Path.GetFullPath(@"..\..\..\..\Data\taxi-fare-train.csv");

            // Infer column information
            ColumnInferenceResults columnInference =
                ctx.Auto().InferColumns(dataPath, labelColumnName: "fare_amount", groupColumns: false);

            // Create text loader
            TextLoader loader = ctx.Data.CreateTextLoader(columnInference.TextLoaderOptions);

            // Load data into IDataView
            IDataView data = loader.Load(dataPath);

            // Split into train (80%), validation (20%) sets
            TrainTestData trainValidationData = ctx.Data.TrainTestSplit(data, testFraction: 0.2);

            //Define pipeline
            SweepablePipeline pipeline =
                ctx.Auto().Featurizer(data, columnInformation: columnInference.ColumnInformation)
                    .Append(ctx.Auto().Regression(labelColumnName: columnInference.ColumnInformation.LabelColumnName));

            // Create AutoML experiment
            AutoMLExperiment experiment = ctx.Auto().CreateExperiment();

            // Configure experiment
            experiment
                .SetPipeline(pipeline)
                .SetRegressionMetric(RegressionMetric.RSquared, labelColumn: columnInference.ColumnInformation.LabelColumnName)
                .SetTrainingTimeInSeconds(60)
                .SetDataset(trainValidationData);

            // Log experiment trials
            ctx.Log += (_, e) => {
                if (e.Source.Equals("AutoMLExperiment"))
                {
                    Console.WriteLine(e.RawMessage);
                }
            };

            // Run experiment
            TrialResult experimentResults = await experiment.RunAsync();

            // Get best model
            var model = experimentResults.Model;
        }
        private double sxchacnui()
        {
            int n = 18; // Số lần lấy và trả lại
            //int k = 1; // Số lần ít nhất để lấy được số 13

            int totalOutcomes = 100; // Tổng số bi
            int targetOutcomes = 1; // Số bi số 13

            double probability = (double)targetOutcomes / totalOutcomes; // Xác suất lấy được số 13 trong một lần lấy

            double requiredProbability = 1 - Math.Pow(1 - probability, n); // Xác suất ít nhất một lần lấy được số 13 trong n lần lấy


            result += "Xác suất chắc chắn (đạt gần 100%) để lấy được số 13 ít nhất một lần trong 30 lần lấy và trả lại: " + requiredProbability;
            return Math.Round(requiredProbability, 5);
        }
        private double xs(int k,int n=18)
        {
            try
            {
                //int n = 30; // Số lần lấy và trả lại
                //int k = 3; // Số lần lấy được số 13

                double p = 1.0 / 100.0; // Xác suất lấy được số 13 trong mỗi lần thử

                Variable<int> successCount = Variable.Binomial(n, p).Named("successCount");
                Variable<bool> atLeastThreeSuccesses = (successCount >= k);

                InferenceEngine engine = new InferenceEngine();
                double probability = engine.Infer<Bernoulli>(atLeastThreeSuccesses).GetProbTrue();
                return Math.Round(probability, 5);
            }
            catch
            {
                return 0.0;
            }
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

        private string _songay;
        public string Songay
        {
            get { return _songay; }
            set { _songay = value; OnChangeProperty(nameof(Songay)); }
        }

        public ObservableCollection<BiData> ListPrediction { get; set; }
        public ObservableCollection<modelKq> listKq { get; set; }
        private ListCollectionView _listcolection;
        public ListCollectionView listcolection
        {
            get { return _listcolection; }
            set { _listcolection = value;
                OnChangeProperty(nameof(listcolection));
            }
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                var listfullFuLL = getdtKqFull().AsEnumerable().Select(s => new modelKq()
                {
                    iddai = int.Parse(s["iddai"].ToString()),
                    num = int.Parse(s["num"].ToString()),
                    ngay = DateTime.Parse(s["ngay"].ToString()),
                    xs = 0.0
                }).ToList();
                var objDai=cobdai.SelectedItem as modelDai;
                if (objDai == null)
                    throw new Exception("Vui lòng chọn đài");
                var countDate = listfullFuLL.Where(w => w.iddai == objDai.Id).GroupBy(g => g.ngay).Count();
                Songay = $"Số Ngày: {countDate}";

               var listfull = listfullFuLL.Where(w => w.iddai == objDai.Id).GroupBy(g => g.num).Select(s => new modelKq()
                {                     
                    num=s.Key,
                    count=s.Count(),
                    xs=Math.Round(1.0*s.Count()/(18*countDate),5)//xs(s.Count(),18*countDate)
                }).OrderByDescending(o => o.xs).ToList();

                listKq = new ObservableCollection<modelKq>(listfull);
                OnChangeProperty(nameof(listKq));
                var colectionsoure = new CollectionViewSource();
                colectionsoure.Source = listKq;
                listcolection = (ListCollectionView)colectionsoure.View;
                //var listbi = listfull.Select(s => new BiData() { Bi = s.num,P=(float)s.xs }).ToList();
                var doubles=listfull.Select(s=>(double)s.num).ToArray();
                //var P=listfull.Select(s=>s.xs).ToArray();
                double[]? P=new double[listfull.Count];
                var arrcount= listfull.Select(s => (double)s.count).ToArray();
                double[,] numFutre= new double[listfull.Count,2];
                var j = 0;
                foreach(var s in listfull) {
                    numFutre[j,0]=s.num;
                    numFutre[j,1]=s.count;

                    P[j]= s.xs;
                    j++;
                }

                var listdata = listfull.Select(s => new DataPoint() { Number = s.num, Count = s.count, P = (float)s.xs }).ToList();//new List<DataPoint>();
                //var predictionEnginer = abc(listdata);
                var listPrediction = new List<BiData>();

                for (float i = 0; i < 100; i++)
                {
                    //var result = ML_Ai(listbi, new BiData() { Bi = i });
                    //var result=MathNett(doubles,P,i);
                    //double[] newSample = { i, 1 };
                    //var result = mathnet(numFutre, P, newSample);
                    //listPrediction.Add(new BiData() { Bi = (float)i, P = result });
                    var mau = new DataPoint() { Number = i, Count = 5 };
                    var predictionEnginer = abc(listdata);
                    var prediction = predictionEnginer.Predict(mau);
                    listPrediction.Add(new BiData() { Bi = mau.Number, P =Math.Round(prediction.P,6) });
                }

                ListPrediction = new ObservableCollection<BiData>(listPrediction.OrderByDescending(o=>o.P));
                OnChangeProperty(nameof(ListPrediction));
               
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        static void main()
        {
            // The winner and loser in each of 6 samples games
            var winnerData = new[] { 0, 0, 0, 1, 3, 4 };
            var loserData = new[] { 1, 3, 4, 2, 1, 2 };

            // Define the statistical model as a probabilistic program
            var game = new Range(winnerData.Length);
            var player = new Range(winnerData.Concat(loserData).Max() + 1);
            var playerSkills = Variable.Array<double>(player);
            playerSkills[player] = Variable.GaussianFromMeanAndVariance(0.166, 9).ForEach(player);

            var winners = Variable.Array<int>(game);
            var losers = Variable.Array<int>(game);

            using (Variable.ForEach(game))
            {
                // The player performance is a noisy version of their skill
                var winnerPerformance = Variable.GaussianFromMeanAndVariance(playerSkills[winners[game]], 1.0);
                var loserPerformance = Variable.GaussianFromMeanAndVariance(playerSkills[losers[game]], 1.0);

                // The winner performed better in this game
                Variable.ConstrainTrue(winnerPerformance > loserPerformance);
            }

            // Attach the data to the model
            winners.ObservedValue = winnerData;
            losers.ObservedValue = loserData;

            // Run inference
            var inferenceEngine = new InferenceEngine();
            var inferredSkills = inferenceEngine.Infer<Gaussian[]>(playerSkills);

            // The inferred skills are uncertain, which is captured in their variance
            var orderedPlayerSkills = inferredSkills
        .Select((s, i) => new { Player = i, Skill = s })
        .OrderByDescending(ps => ps.Skill.GetMean());

            foreach (var playerSkill in orderedPlayerSkills)
            {
                Console.WriteLine($"Player {playerSkill.Player} skill: {playerSkill.Skill}");
            }
        }
        private PredictionEngine<DataPoint, Prediction> abc(List<DataPoint> listdata)
        {

            // Tạo đối tượng MLContext
            var mlContext = new MLContext();

            // Đọc dữ liệu huấn luyện từ file CSV
            //var trainingData = mlContext.Data.LoadFromTextFile<DataPoint>("abc.csv", separatorChar: ',');
            var trainingData = mlContext.Data.LoadFromEnumerable<DataPoint>(listdata);
            // Tạo pipeline dự đoán
            var pipeline = mlContext.Transforms.Concatenate("Features", "Number", "Count")
                .Append(mlContext.Transforms.NormalizeMinMax("Features"))
                //.Append(mlContext.Transforms.Conversion.MapValueToKey("P"))
                //.Append(mlContext.Transforms.Conversion.MapKeyToValue("P"))
                .Append(mlContext.Regression.Trainers.Sdca(labelColumnName: "P"));//, maximumNumberOfIterations: 100));
            //var pipeline = mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "P")
            //    .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "NumberEncode", inputColumnName: "Number"))
            //.Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "CountEncoded", inputColumnName: "Count"))             
            //    .Append(mlContext.Transforms.Concatenate("Features", "NumberEncode", "CountEncoded"))
            //    .Append(mlContext.Regression.Trainers.FastTree());
            //// Huấn luyện mô hình
            var model = pipeline.Fit(trainingData);

            // Dự đoán P cho mẩu dữ liệu mới
            var predictionEngine = mlContext.Model.CreatePredictionEngine<DataPoint, Prediction>(model);
            return predictionEngine;

            var newDataPoint = new DataPoint { Number = 55, Count = 1 };
            var prediction = predictionEngine.Predict(newDataPoint);

            Console.WriteLine("Predicted P: " + prediction.P);
        }
        private double ML_Ai(List<BiData> listbi, BiData newBi)
        {
            MLContext mlContext = new MLContext();
            IDataView trainingData = mlContext.Data.LoadFromEnumerable(listbi);
                var dataSplit=mlContext.Data.TrainTestSplit(trainingData);
            // 2. Specify data preparation and model training pipeline
            var pipeline = mlContext.Transforms.Concatenate("Features", new[] { "Bi" })
                .Append(mlContext.Regression.Trainers.Sdca(labelColumnName: "P",maximumNumberOfIterations: 100));

            // 3. Train model
            var model = pipeline.Fit(trainingData);

            // 4. Make a prediction
            
            var result = mlContext.Model.CreatePredictionEngine<BiData, BiPrediction>(model).Predict(newBi);

            return result.P;

            //// Khởi tạo môi trường ML.NET
            //var context = new MLContext();

            //// Đọc dữ liệu từ các bi đã xuất hiện trong 4 ngày
            //var data = context.Data.LoadFromEnumerable(listbi); //context.Data.LoadFromTextFile<BiData>("path/to/data.csv", separatorChar: ',');

            //// Chia dữ liệu thành hai tập train và test
            //var dataSplit = context.Data.TrainTestSplit(data);

            //// Xây dựng pipeline tiền xử lý và huấn luyện mô hình
            ////       var pipeline = context.Transforms.Conversion.MapValueToKey("Bi")
            ////.Append(context.Transforms.Categorical.OneHotEncoding("Bi"))
            ////.Append(context.Transforms.Concatenate("Features",new[] { "Bi" }))
            ////.Append(context.Transforms.NormalizeMinMax("Features"))
            //////.Append(context.Transforms.Conversion.MapKeyToValue("PredictedLabel"))
            ////.Append(context.Transforms.Conversion.MapKeyToValue("Bi"))
            ////.Append(context.Regression.Trainers.Sdca(labelColumnName: "Bi", maximumNumberOfIterations: 100));
            //var pipeline = context.Transforms.Concatenate("Features", new[] { "Bi" })
            //          .Append(context.Regression.Trainers.Sdca(labelColumnName: "Bi", maximumNumberOfIterations: 100));

            //// Huấn luyện mô hình sử dụng thuật toán Decision Tree Classification
            //var model = pipeline.Fit (dataSplit.TrainSet);

            ////// Dự đoán bi tiếp theo trong ngày thứ 5
            ////var predictionEngine = context.Model.CreatePredictionEngine<BiData, BiPrediction>(model);
            ////var nextBiPrediction = predictionEngine.Predict(new BiData { Bi = 0 });

            ////Console.WriteLine($"Next predicted bi: {nextBiPrediction.PredictedBi}");

            //// Dự đoán bi tiếp theo trong ngày thứ 5
            //var predictionEngine = context.Model.CreatePredictionEngine<BiData, BiPrediction>(model);
            //var nextBiPrediction = predictionEngine.Predict(new BiData() { Bi=55});

            //Console.WriteLine($"Next predicted bi: {nextBiPrediction.PredictedBi}");

            //var listb=new List<BiData>() { new BiData() { Bi = 0, P = 0.001F }, new BiData() { Bi = 5, P = 0.12F }, new BiData() { Bi = 55, P = 0.12F }, new BiData() { Bi = 56, P = 0.12F } };

        }

        private double mathnet(double[,] features, double[] labels, double[] newSample)
        {
            //// Dữ liệu huấn luyện
            //double[,] features = {
            //    {1, 2},
            //    {2, 3},
            //    {3, 4},
            //    {4, 5},
            //    {5, 6}
            //};

            //double[] labels = { 0, 0, 1, 1, 1 };

            // Số lượng mẫu và số lượng đặc trưng
            int numSamples = features.GetLength(0);
            int numFeatures = features.GetLength(1);

            // Thêm cột 1 vào ma trận đặc trưng để tính toán w0
            Matrix<double> X = Matrix<double>.Build.Dense(numSamples, numFeatures + 1);
            for (int i = 0; i < numSamples; i++)
            {
                X[i, 0] = 1;
                for (int j = 1; j <= numFeatures; j++)
                {
                    X[i, j] = features[i, j - 1];
                }
            }

            // Ma trận nhãn
            MathNet.Numerics.LinearAlgebra.Vector<double> y = MathNet.Numerics.LinearAlgebra.Vector<double>.Build.Dense(labels);

            // Số lần lặp và learning rate
            int numIterations = 1000;
            double learningRate = 0.01;

            // Khởi tạo vector trọng số
            MathNet.Numerics.LinearAlgebra.Vector<double> w = MathNet.Numerics.LinearAlgebra.Vector<double>.Build.Dense(numFeatures + 1, 0.0);

            // Huấn luyện Logistic Regression
            for (int iteration = 0; iteration < numIterations; iteration++)
            {
                // Tính giá trị dự đoán
                MathNet.Numerics.LinearAlgebra.Vector<double> scores = X * w;
                MathNet.Numerics.LinearAlgebra.Vector<double> predictions = Sigmoid(scores);

                // Tính gradient
                MathNet.Numerics.LinearAlgebra.Vector<double> gradient = X.Transpose() * (predictions - y) / numSamples;

                // Cập nhật trọng số
                w -= learningRate * gradient;
            }

            // In trọng số đã học
            Console.WriteLine("Trọng số đã học:");
            Console.WriteLine(w);

            // Dự đoán một mẫu mới
            //double[] newSample = { 6, 7 };
            MathNet.Numerics.LinearAlgebra.Vector<double> newX = MathNet.Numerics.LinearAlgebra.Vector<double>.Build.Dense(numFeatures + 1, 1.0);
            for (int i = 1; i <= numFeatures; i++)
            {
                newX[i] = newSample[i - 1];
            }
            double prediction = Sigmoid(newX * w);

            // Console.WriteLine($"Dự đoán cho mẫu mới {string.Join(", ", newSample)}: {prediction}");
            return Math.Round(prediction,6);
        }

        // Hàm sigmoid
        static double Sigmoid(double x)
        {
            return 1 / (1 + Math.Exp(-x));
        }

        // Hàm sigmoid cho vector
        static MathNet.Numerics.LinearAlgebra.Vector<double> Sigmoid(MathNet.Numerics.LinearAlgebra.Vector<double> x)
        {
            return x.Map(Sigmoid);
        }

        private double MathNett(double[] numbers, double[] probabilities, double newNumber=52)
        {  
            //double[] numbers = { 56, 78, 12, 35, 15, 48 };
            //double[] probabilities = { 0.01, 0.024, 0.0574, 0.0147, 0.0234, 0.041 };

            // Tính toán hệ số hồi quy
            double xMean = numbers.Average();
            double yMean = probabilities.Average();

            double numerator = 0;
            double denominator = 0;

            for (int i = 0; i < numbers.Length; i++)
            {
                numerator += (numbers[i] - xMean) * (probabilities[i] - yMean);
                denominator += Math.Pow(numbers[i] - xMean, 2);
            }

            double slope = numerator / denominator;
            double intercept = yMean - (slope * xMean);

            // Dự đoán xác suất cho số mới
            //double newNumber = 52;
            double predictedProbability = (slope * newNumber) + intercept;

           // Console.WriteLine($"Dự đoán xác suất cho số {newNumber}: {predictedProbability}");
           return Math.Round(predictedProbability,6);
        }
    }
    public class BiData
    {
        [LoadColumn(0)]
        public float Bi { get; set; }
        public double P { get; set; }
    }
    public class BiPrediction
    {
        [ColumnName("Score")]
        public double P { get; set; }
    }

    // Tạo một lớp để đại diện cho dữ liệu đầu vào
    public class DataPoint
    {
        [LoadColumn(0)] public float Number { get; set; }
        [LoadColumn(1)] public float Count { get; set; }
        [LoadColumn(2)] public float P { get; set; }
    }

    // Tạo một lớp để đại diện cho dữ liệu đầu ra
    public class Prediction
    {
        [ColumnName("Score")]
        public float P;
    }

}
