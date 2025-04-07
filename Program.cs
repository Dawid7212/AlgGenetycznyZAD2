using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgGenetyczny
{
    internal class Program
    {
        private static Random rand = new Random();
        static Dictionary<double, double> bazaProbek = new Dictionary<double, double>
        {
            { -1.00000,  0.59554 },
            { -0.80000,  0.58813 },
            { -0.60000,  0.64181 },
            { -0.40000,  0.68587 },
            { -0.20000,  0.44783 },
            {  0.00000,  0.40836 },
            {  0.20000,  0.38241 },
            {  0.40000, -0.05933 },
            {  0.60000, -0.12478 },
            {  0.80000, -0.36847 },
            {  1.00000, -0.39935 },
            {  1.20000, -0.50881 },
            {  1.40000, -0.63435 },
            {  1.60000, -0.59979 },
            {  1.80000, -0.64107 },
            {  2.00000, -0.51808 },
            {  2.20000, -0.38127 },
            {  2.40000, -0.12349 },
            {  2.60000, -0.09624 },
            {  2.80000,  0.27893 },
            {  3.00000,  0.48965 },
            {  3.20000,  0.33089 },
            {  3.40000,  0.70615 },
            {  3.60000,  0.53342 },
            {  3.80000,  0.43321 },
            {  4.00000,  0.64790 },
            {  4.20000,  0.48834 },
            {  4.40000,  0.18440 },
            {  4.60000, -0.02389 },
            {  4.80000, -0.10261 },
            {  5.00000, -0.33594 },
            {  5.20000, -0.35101 },
            {  5.40000, -0.62027 },
            {  5.60000, -0.55719 },
            {  5.80000, -0.66377 },
            {  6.00000, -0.62740 }
        };
        public static double Przystosowanie(double[] Osobnik)
        {
            double pa = Osobnik[0];
            double pb = Osobnik[1];
            double pc = Osobnik[2];
            double przystosowanie = 0.0;
            foreach (var probka in bazaProbek)
            {
                double x = probka.Key;
                double y = probka.Value;
                double yObliczone = pa * Math.Sin(pb * x + pc);
                przystosowanie += Math.Pow(y - yObliczone, 2);
            }
            return przystosowanie;
        }
        public static int[][] StworzPule2(int ilosc, int chromosomy)
        {
            //Random rand = new Random();
            int[][] Pula = new int[ilosc][];
            for (int i = 0; i < ilosc; i++)
            {
                int[] osobnikTymczasowy = new int[chromosomy];
                for (int j = 0; j < chromosomy; j++)
                {
                    int bitlosowy = rand.Next(0, 2);
                    osobnikTymczasowy[j] = bitlosowy;
                }
                Pula[i] = osobnikTymczasowy;
            }
            return Pula;
        }
        public static double[][] StworzPule(int ilosc, int lParametrow, int Min, int Max)
        {
            //Random rand = new Random();
            double[][] Pula = new double[ilosc][];
            for (int i = 0; i < ilosc; i++)
            {
                double[] osobnikTymczasowy = new double[lParametrow];
                for (int j = 0; j < lParametrow; j++)
                {
                    double PARAMETRlosowy = Min + (Max - Min) * rand.NextDouble();
                    osobnikTymczasowy[j] = PARAMETRlosowy;
                }
                Pula[i] = osobnikTymczasowy;
            }
            return Pula;
        }
        public static int[] Kodowanie(double pm, float Max, float Min, int liczbaCh)
        {
            float ZD = Max - Min;
            int[] cb = new int[liczbaCh];
            if (pm < Min)
            {
                pm = Min;
            }
            else if (pm > Max)
            {
                pm = Max;
            }
            double ctmp = Math.Round(((pm - Min) / ZD) * (Math.Pow(2, liczbaCh) - 1));
            for (int b = 0; b <= liczbaCh - 1; b++)
            {
                cb[b] = (int)Math.Floor(ctmp / Math.Pow(2, b)) % 2;
            }
            return cb;
        }
        public static double Dekodowanie(int[] cb, float Min, float Max, int LBnP)
        {
            float ZD = Max - Min;
            double ctmp = 0;
            for (int b = 0; b <= LBnP - 1; b++)
            {
                ctmp += cb[b] * Math.Pow(2, b);
            }
            double pm = Min + (ctmp / (Math.Pow(2, LBnP) - 1)) * ZD;
            return pm;
        }
        public static int[] OperatorSelTurniejowej(int[][] pulaOsobnikow, double[] ocenaOsobnikow)
        {
            int RozmiarTurnieju = 3;
            int[] skladTurnieju = new int[RozmiarTurnieju];
            //Random rand = new Random();
            for (int i = 0; i < RozmiarTurnieju; i++)
            {
                skladTurnieju[i] = rand.Next(pulaOsobnikow.Length);
            }

            int najI = skladTurnieju[0];
            for (int i = 1; i < RozmiarTurnieju ; i++)
            {
                if (ocenaOsobnikow[skladTurnieju[i]] < ocenaOsobnikow[najI])
                {
                    najI = skladTurnieju[i];
                }
            }
            return pulaOsobnikow[najI].ToArray();
        }
        public static (int[], int[]) OperatorKrzyżowania(int[] cbr1, int[] cbr2)
        {
            int LBnOs = cbr1.Length;
            // Random rand = new Random();
            int bCiecie = rand.Next(0, LBnOs - 2);
            //Console.WriteLine("Wylosowane miejsce przeciecia: " + bCiecie);
            int[] cbp1 = new int[cbr1.Length];
            int[] cbp2 = new int[cbr2.Length];
            for (int i = 0; i < bCiecie; i++)
            {
                cbp1[i] = cbr1[i];
                cbp2[i] = cbr2[i];
            }
            for (int j = bCiecie; j < LBnOs; j++)
            {
                cbp1[j] = cbr2[j];
                cbp2[j] = cbr1[j];
            }
            return (cbp1, cbp2);
        }
        public static int[] OperatorMutacji(int[] cb)
        {
            int LBnOs = cb.Length;
            //Random rand = new Random();
            int bPunkt = rand.Next(0, LBnOs - 1);
            // Console.WriteLine("Wylosowany punkt mutacji: " + bPunkt);
            int[] cbwy = new int[LBnOs];
            for (int i = 0; i < LBnOs; i++)
            {
                cbwy[i] = cb[i];
            }
            if (cbwy[bPunkt] == 0)
            {
                cbwy[bPunkt] = 1;
            }
            else if (cbwy[bPunkt] == 1)
            {
                cbwy[bPunkt] = 0;
            }
            return cbwy;
        }
        public static int[] OperatorHotDeck(int[][] pulaOsobnikow, double[] ocenaOsobnikow)
        {
            int najlepszy = 0;
            for (int i = 1; i < pulaOsobnikow.Length; i++)
            {
                if (ocenaOsobnikow[i] < ocenaOsobnikow[najlepszy])
                {
                    najlepszy = i;
                }
            }
            return pulaOsobnikow[najlepszy].ToArray();
        }
        static void Main(string[] args)
        {

            int LBnP = 4;
            int lParametrow = 3;
            int lChromosomow = LBnP * lParametrow;
            int lOsobnikow = 13;
            float Min = 0;
            float Max = 3;
            //1.Tworze losową pulę osobników
            int[][] Pula = StworzPule2(lOsobnikow, lChromosomow);
            double[][] PulaDekodowana = new double[lOsobnikow][];
            int[] chromosomytymczasowe = new int[Pula[0].Length / lParametrow];
            double[] przystosowanie = new double[lOsobnikow];

            //2.Dekodowanie chormosomów wylosowanych osobników i tworzenie przystosowania 
            for (int i = 0; i < Pula.Length; i++)
            {
                double[] ParametryTymczasowe = new double[lParametrow];
                int y = 0;
                for (int z = 0; z < lParametrow; z++)
                {
                    for (int j = 0; j < Pula[0].Length / lParametrow; j++)
                    {
                        chromosomytymczasowe[j] = Pula[i][y];
                        y++;
                    }
                    ParametryTymczasowe[z] = Dekodowanie(chromosomytymczasowe, Min, Max, LBnP);
                }
                PulaDekodowana[i] = ParametryTymczasowe;
                przystosowanie[i] = Przystosowanie(ParametryTymczasowe);
            }
            // Wypisuje najlepszą oraz średnią wartość funkcji przystosowania osobników w puli
            Console.WriteLine("Srednia przystosowania pierwotnej puli: " + przystosowanie.Average());
            Console.WriteLine("Najlepsze przystosowanie w pierwotnej puli: " + przystosowanie.Min());
            for (int i=0;i<100;i++)
            {
                //1.Tworze nowa pulę osobników
                int[][] nowapula = new int[Pula.Length][];
                for (int j =0;j<Pula.Length-1;j++)
                {
                    nowapula[j] = OperatorSelTurniejowej(Pula, przystosowanie);
                }
                //2. Stosuje operator krzyżowania na wybranych osobnikach
                (nowapula[0],nowapula[1]) = OperatorKrzyżowania(nowapula[0], nowapula[1]);
                (nowapula[2], nowapula[3]) = OperatorKrzyżowania(nowapula[2], nowapula[3]);
                (nowapula[8], nowapula[9]) = OperatorKrzyżowania(nowapula[8], nowapula[9]);
                (nowapula[nowapula.Length-3], nowapula[nowapula.Length - 2]) = OperatorKrzyżowania(nowapula[nowapula.Length - 3], nowapula[nowapula.Length - 2]);
                //3.Urzywam operatora mutacji na osobnikach 5 - ostatni
                for (int j = 4;j<nowapula.Length-1;j++)
                {
                    nowapula[j] = OperatorMutacji(nowapula[j]);
                }
                //4.Do nowej puli dodaje najlepszego ze starej
                nowapula[nowapula.Length - 1] = OperatorHotDeck(Pula,przystosowanie);
                //5.Dekoduje osobniki nowej puli
                double[][] nowaPulaDekodowana = new double[nowapula.Length][];
                int[] chromosomytmp = new int[Pula[0].Length / lParametrow];
                double[] noweprzystosowanie = new double[nowapula.Length];
                for (int k = 0; k < nowapula.Length; k++)
                {
                    double[] ocenaTymczasowa = new double[lParametrow];
                    int y = 0;
                    for (int z = 0; z < lParametrow; z++)
                    {
                        for (int j = 0; j < nowapula[0].Length / lParametrow; j++)
                        {
                            chromosomytmp[j] = nowapula[k][y];
                            y++;
                        }
                        ocenaTymczasowa[z] = Dekodowanie(chromosomytmp, Min, Max, LBnP);
                    }
                    nowaPulaDekodowana[k] = ocenaTymczasowa;
                    //Licze funkcje przystosowania 
                    noweprzystosowanie[k] = Przystosowanie(ocenaTymczasowa);
                }
                Console.WriteLine("Srednia przystosowania w "+ (i+1) +" puli: " + noweprzystosowanie.Average());
                Console.WriteLine("Najlepsze przystosowanie w "+ (i+1) +" puli: " + noweprzystosowanie.Min());
                Pula = nowapula.Select(a => a.ToArray()).ToArray();
                przystosowanie = noweprzystosowanie.ToArray();


            }



            int[] najlepszyOsobnikNaKoniec = OperatorHotDeck(Pula, przystosowanie);
            double[] najlepszeParametry = new double[lParametrow];
            int[] chromosomyt = new int[Pula[0].Length / lParametrow];
            int y2 = 0;
            for (int z = 0; z < lParametrow; z++)
            {
                for (int j = 0; j < Pula[0].Length / lParametrow; j++)
                {
                    chromosomyt[j] = najlepszyOsobnikNaKoniec[y2];
                    y2++;
                }
                najlepszeParametry[z] = Dekodowanie(chromosomyt, Min, Max, LBnP);
            }
            Console.WriteLine("Parametry najlepszego osobnika:");
            Console.WriteLine("pa = " + najlepszeParametry[0]);
            Console.WriteLine("pb = " + najlepszeParametry[1]);
            Console.WriteLine("pc = " + najlepszeParametry[2]);


            Console.ReadKey();
        }
    }
}
