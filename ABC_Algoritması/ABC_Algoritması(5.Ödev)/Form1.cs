using System.Windows.Forms.DataVisualization.Charting;

namespace ABC_Algoritması_5.Ödev_
{
    public partial class Form1 : Form
    {
        public class ArıVerisi
        {
            public double x1 { get; set; }
            public double x2 { get; set; }
            public double fx { get; set; }
            public double fitness { get; set; }
            public int iyilestirilememisAdim { get; set; }
        }

        int CS = 5;
        int D = 2;
        int L = 0;

        List<ArıVerisi> arilarListesi = new List<ArıVerisi>();
        List<double> yakinsamaListesi = new List<double>();

        Chart chart1 = new Chart();
        public Form1()
        {
            InitializeComponent();
            GrafikOlustur();
        }

        // BAŞLATMA
        private void button1_Click(object sender, EventArgs e)
        {
            iyilestirme_hesap(); //1


            for (int i = 0; i < CS; i++) //PROBLEM BOYUTU BURADA BELİRLENİYOR ONA GÖRE x1, x2 değerleri alınıyor gibi düşünülebilir
            {
                double x1 = RastgeleBesin();
                double x2 = RastgeleBesin();
                double fx = (Math.Pow(x1, 2) + Math.Pow(x2, 2)); // (x1^2 + x2^2) PROBLEM FORMÜLÜ
                double fitx = fitness_hesap(fx); //3

                AriEkle(x1, x2, fx, fitx);
                MessageBox.Show($"x1: {x1} x2: {x2}");
            }

            for (int i = 0; i < CS; i++)
            {
                isci_ari_fazi_hesap(i);
            }

            for (int i = 0; i < CS; i++)
            {
                gozcu_ari_fazi_hesap();
            }

            grafikCiz();
        }

        // 1
        public void iyilestirme_hesap()
        {
            CS = (int)numericUpDown1.Value; // KOLONİDEKİ ARI SAYISI
            D = 2;                          // PROBLEM BOYUTU x1 ve x2 -> 2 sayısı sabittir
            L = ((CS * D) / 2);             // İYİLEŞTİRİLEMEME SAYISI (Limit)
        }

        // 2.1
        public double RastgeleBesin()
        {
            Random random = new Random();
            double rnd = random.NextDouble() * 10 - 5; // -5 < x1,x2 < 5 Minimizasyon
            return rnd;
        }

        // 2.2
        public void AriEkle(double x1, double x2, double fx, double fitness)
        {
            ArıVerisi veri = new ArıVerisi()
            {
                x1 = x1,
                x2 = x2,
                fx = fx,
                fitness = fitness,
                iyilestirilememisAdim = 0
            };

            arilarListesi.Add(veri);
        }


        // 3
        public double fitness_hesap(double fx)
        {
            if (fx >= 0)
            {
                return (1 / (1 + fx));
            }
            else
            {
                return (1 + Math.Abs(fx));
            }
        }

        #region İŞÇİ ARILAR -----
        //4 
        public void isci_ari_fazi_hesap(int x)
        {
            double enIyiFx = arilarListesi.Min(a => a.fx);
            yakinsamaListesi.Add(enIyiFx);

            double x1 = arilarListesi[x].x1;
            double fi1 = FiRastgele();
            double rndX1 = KomsuRastgele();

            double x2 = arilarListesi[x].x2;
            double fi2 = FiRastgele();
            double rndX2 = KomsuRastgele();

            double v1 = x1 - fi1 * (x1 - rndX1);
            double v2 = x2 - fi2 * (x2 - rndX2);

            double fv = (Math.Pow(v1, 2) + Math.Pow(v2, 2)); // f(v) = (v1^2 + v2^2)
            double fitv = fitness_hesap(fv); // v değerinin fitness değeri hesaplanıyor

            if (fitv < arilarListesi[x].fitness)
            {
                kasif_ari_fazi_hesap(x);
            }
            else
            {
                MessageBox.Show($"{x} Indexli Arı İyileştirilmiştir.");

                arilarListesi[x].x1 = v1;           // V daha büyük olduğu için X yerine
                arilarListesi[x].x2 = v2;           //tüm V değerlerini yazıyoruz 
                arilarListesi[x].fx = fv;
                arilarListesi[x].fitness = fitv;
                arilarListesi[x].iyilestirilememisAdim = 0;
            }
        }

        //4.1
        public double FiRastgele()
        {
            Random rnd = new Random();
            double fi = rnd.NextDouble() * 2 - 1; // fi sayısı için (-1,1) aralığı
            return fi;
        }

        //4.2
        public double KomsuRastgele()
        {
            if (arilarListesi.Count == 0)
                throw new InvalidOperationException("Arı listesi boş!");

            Random rnd = new Random();
            int index = rnd.Next(arilarListesi.Count);
            ArıVerisi veri = arilarListesi[index]; // komşu seçimi

            bool chooseX1 = rnd.Next(2) == 0; // x1 veya x2
            return chooseX1 ? veri.x1 : veri.x2;
        }

        #endregion

        #region GÖZCÜ ARILAR -----

        // 5
        public void gozcu_ari_fazi_hesap()
        {
            double enIyiFx = arilarListesi.Min(a => a.fx);
            yakinsamaListesi.Add(enIyiFx);

            int x = OlasilikliSec();

            double x1 = arilarListesi[x].x1;
            double fi1 = FiRastgele();
            double rndX1 = KomsuRastgele();

            double x2 = arilarListesi[x].x2;
            double fi2 = FiRastgele();
            double rndX2 = KomsuRastgele();

            double v1 = x1 - fi1 * (x1 - rndX1);
            double v2 = x2 - fi2 * (x2 - rndX2);

            double fv = (Math.Pow(v1, 2) + Math.Pow(v2, 2)); // f(v) = (v1^2 + v2^2)
            double fitv = fitness_hesap(fv); // v değerinin fitness değeri hesaplanıyor

            if (fitv < arilarListesi[x].fitness)
            {
                kasif_ari_fazi_hesap(x);
            }
            else
            {
                MessageBox.Show($"{x} Indexli Arı İyileştirilmiştir.");

                arilarListesi[x].x1 = v1;           // V daha büyük olduğu için X yerine
                arilarListesi[x].x2 = v2;           //tüm V değerlerini yazıyoruz 
                arilarListesi[x].fx = fv;
                arilarListesi[x].fitness = fitv;
                arilarListesi[x].iyilestirilememisAdim = 0;
            }
        }

        // 5.1 (RULET TEKERLEĞİ YÖNTEMİ)
        public int OlasilikliSec()
        {
            double toplamFitness = arilarListesi.Sum(a => a.fitness);

            Random rnd = new Random();
            double r = rnd.NextDouble();
            double kümülatif = 0;

            for (int i = 0; i < arilarListesi.Count; i++)
            {
                kümülatif += arilarListesi[i].fitness / toplamFitness;

                if (r < kümülatif)
                    return i;
            }

            return arilarListesi.Count - 1; // Toplam 1 olmayabilir, sonuncuyu döndür
        }

        #endregion

        #region KAŞİF ARI
        // 6
        public void kasif_ari_fazi_hesap(int x)
        {
            arilarListesi[x].iyilestirilememisAdim++;

            if (arilarListesi[x].iyilestirilememisAdim >= L)
            {
                arilarListesi[x].x1 = RastgeleBesin();
                arilarListesi[x].x2 = RastgeleBesin();
                arilarListesi[x].fx = Math.Pow(arilarListesi[x].x1, 2) + Math.Pow(arilarListesi[x].x2, 2);
                arilarListesi[x].fitness = fitness_hesap(arilarListesi[x].fx);
                arilarListesi[x].iyilestirilememisAdim = 0;

                MessageBox.Show($"{x} indexli arı scout (kaşif) arı gibi yeniden başlatıldı.");
            }
            else
            {
                MessageBox.Show($"{x} indexli arı iyileştirilemedi.");
            }
        }
        #endregion

        // 7
        private void grafikCiz()
        {
            chart1.Series.Clear();
            chart1.Series.Add("Yakınsama");

            var seri = chart1.Series["Yakınsama"];
            seri.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

            seri.BorderWidth = 3;
            seri.Color = Color.Red;            

            for (int i = 0; i < yakinsamaListesi.Count; i++)
            {
                seri.Points.AddXY(i + 1, yakinsamaListesi[i]);
            }
        }

        // 7.1
        public void GrafikOlustur()
        {
            chart1.Name = "chart1";
            chart1.Size = new Size(700, 300);
            chart1.Location = new Point(275, 10);
            chart1.BackColor = Color.Yellow;

            ChartArea chartArea = new ChartArea();
            chartArea.Name = "ChartArea1";
            chart1.ChartAreas.Add(chartArea);

            this.Controls.Add(chart1);
        }
    }
}