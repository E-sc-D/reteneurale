using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
namespace retentype1v2
{
    class Program
    {

        static double[,] data = new double[10, 3];
        static Random rnd = new Random();
        static double[] condS = new double[3];
        static void Main(string[] args)
        {
            Console.WriteLine("                                        ErdSra: NeuralnetType1 alpha 2.00('working through,ReLu L test')");
            double[,] workdata;
            int n;
            Console.WriteLine("vuoi inserire un dataset?");
            if (Console.ReadLine() == "y")
            {
                using (StreamReader datas = new StreamReader("data.txt"))
                {
                    for (int i = 0; i < data.GetLength(0); i++)
                    {
                        for (int j = 0; j < data.GetLength(1); j++)
                        {
                            data[i, j] = Convert.ToDouble(datas.ReadLine());
                        }
                    }
                }
                condS = train();
            }
            else
            {
                Console.WriteLine("vuoi caricare dei parametri?");
                if (Console.ReadLine() == "y")
                {
                    using (StreamReader weights = new StreamReader("weights.txt"))
                    {

                        for (int j = 0; j < condS.Length; j++)
                        {
                            condS[j] = Convert.ToInt64(weights.ReadLine());
                        }

                    }

                }
                else
                {
                    Console.WriteLine("parametri o dataset non presenti");
                    return;
                }
            }
            Console.WriteLine("settaggio parametri concluso pronto per l'esecuzione:inserire la quantità delle coppie di file ");
            do { Console.WriteLine("inserici un valore numerico"); } while (int.TryParse(Console.ReadLine(), out n) == false);
            workdata = new double[n, 2];
            Console.WriteLine("schegli la tipologia di inserimeto manuale (m) oppure da file");
            if (Console.ReadLine() == "m")
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        do { Console.WriteLine("inserici un valore numerico"); } while (double.TryParse(Console.ReadLine(), out workdata[i, j]) == false);
                    }
                }
            }
            else
            {
                Console.WriteLine("caricamento automatico inizializzato");
                using (StreamReader datas = new StreamReader("Wdata.txt"))
                {
                    for (int i = 0; i < n; i++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            workdata[i, j] = Convert.ToDouble(datas.ReadLine());
                        }
                    }
                }
            }
            Console.WriteLine("scegliere il risultato per valori vicini a 1");
            string one = Console.ReadLine();
            Console.WriteLine("scegliere il risultati per valori vicini a 0");
            string zero = Console.ReadLine();
            for (int i = 0; i < n; i++)
            {
                /* if (Neuralnet(GetColunms(workdata, i), condS) > 0.5)
                 {
                     Console.WriteLine(one);
                 }
                 else
                 {
                     Console.WriteLine(zero);
                 }*/
                Console.WriteLine(Neuralnet(GetColunms(workdata, i), condS));
            }

            Console.WriteLine("END");
        }

        static double[] train()
        {
            double w1 = rnd.NextDouble(), w2 = rnd.NextDouble(), b = rnd.NextDouble(), Lrate = 0.01, z, costo = 1, Pred, dcost_dpred, dpred_dz, dz_dw1, dz_dw2, dz_db, dcost_dw1, dcost_dw2, dcost_db, dcost_dz, target;
            int repetition = 10000, i, ri;
            double[] point = new double[3];
            while (costo > 0.00001)
            {
                for (i = 0; i < repetition; i++)
                {
                    ri = rnd.Next(0, data.GetLength(0) - 1);
                    for (int j = 0; j < 3; j++) { point[j] = data[ri, j]; }
                    target = point[2];
                    z = point[0] * w1 + point[1] * w2 + b;
                    Pred = ReLu(z);
                    costo = Math.Pow((Pred - target), 2);
                    dcost_dpred = 2 * (Pred - target);
                    dpred_dz = ReLu_d(z);
                    dz_dw1 = point[0];
                    dz_dw2 = point[1];
                    dz_db = 1;
                    dcost_dz = dcost_dpred * dpred_dz;
                    dcost_dw1 = dcost_dz * dz_dw1;
                    dcost_dw2 = dcost_dz * dz_dw2;
                    dcost_db = dcost_dz * dz_db;

                    w1 = w1 - Lrate * dcost_dw1;
                    w2 = w2 - Lrate * dcost_dw2;
                    b = b - Lrate * dcost_db;
                }
            }

            Console.WriteLine("Costo finale " + costo);
            point[0] = w1;
            point[1] = w2;
            point[2] = b;
            return point;
        }
        static double sigmoide(double t)
        {
            return 1 / (1 + Math.Exp(-t));
        }
        static double sigmoide_d(double t)
        {
            return sigmoide(t) * (1 - sigmoide(t));
        }
        static double Neuralnet(double[] ins, double[] ChgVal)
        {
            return ins[0] * ChgVal[0] + ins[1] * ChgVal[1] + ChgVal[2];//sigmoide removed
        }
        static double[] GetColunms(double[,] matrix, int row)
        {
            double[] output = new double[matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                output[i] = matrix[row, i];
            }
            return output;
        }
        static double ReLu(double t)
        {
            if (t < 0)
            { return (t*0.01); }
                else
            { return t; }
        }
        static double ReLu_d(double t)
        {
           if(t>=0)
            { return 1; }
           else
            { return 0.01; }
        }


            

    }
}
