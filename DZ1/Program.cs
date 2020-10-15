using System;
using System.Numerics;

namespace ConsoleApplication1
{
    class Program
    {

        public static int[] GetInfoVector()
        {
            var infoVectorString = "111110100010000";  
            var infoVector = new int[15];
            Console.Write("Информационный вектор: 11111010001\n");
            var buf = infoVectorString.ToCharArray();
            for (var i = 0; i < 15; i++)
            {
                switch (buf[i])
                {
                    case '0':
                        infoVector[i] = 0;
                        break;
                    case '1':
                        infoVector[i] = 1;
                        break;
                }
            }
            return infoVector;
        }

        public static int[] GetCodeVector(int[] infoVector)
        {   
            var codeArr = new int[4] { 0, 0, 0, 0 };
            codeArr = GetCodeArr(infoVector);
            var codeVector = new int[] { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
            codeVector[11] = codeArr[0];
            codeVector[12] = codeArr[1];
 /*0011*/           codeVector[13] = codeArr[2];
 /*0100*/           codeVector[14] = codeArr[3];
 /*0101*/           codeVector[0] = infoVector[0];
/*0110*/            codeVector[1] = infoVector[1];
/*0111*/            codeVector[2] = infoVector[2];
/*1000*/            codeVector[3] = infoVector[3];
/*1001*/            codeVector[4] = infoVector[4];
/*1010*/            codeVector[5] = infoVector[5];
/*1011*/            codeVector[6] = infoVector[6];
/*------1100-----*/ codeVector[7] = infoVector[7];
/*1101*/            codeVector[8] = infoVector[8];
/*1110*/            codeVector[9] = infoVector[9];
/*1111*/            codeVector[10] = infoVector[10];
            return codeVector;
        }

        public static int[] GetCodeArr(int[] infoVector)
        {
            int infoNum = 0;
            var codeArr = new int[4] { 0, 0, 0, 0 };
            for (int i = infoVector.Length - 1; i > -1; i--)
            {
                infoNum += infoVector[i] * Int32.Parse(Math.Pow(2, infoVector.Length-1-i).ToString());
            }
            var strCodeArr = GetString2Mod(infoNum, 19);
            while (strCodeArr.Length < 4)
                    strCodeArr ="0"+strCodeArr;   
            for (int i=0;i<4;i++)
                {
                    codeArr[i] = Int32.Parse(strCodeArr[i].ToString());
                }
            return codeArr;
        }

        public static string GetString2Mod(int a, int b)
        {
            int c;
            while (Convert.ToString(a, 2).Length - Convert.ToString(b, 2).Length >= 0)
            {
                c = Int32.Parse(Math.Pow(2, Convert.ToString(a, 2).Length - Convert.ToString(b, 2).Length).ToString());
                a = a ^ (b * c);
            }
            return Convert.ToString(a,2);
        }

        public static string ToString2NumSystem(int a)
        {
            if (a == 0) return "0";
            return (a == 1) ? "1" : ToString2NumSystem(a / 2) + a % 2;
        }

        public static int[] ToIntPtr(string a)    
        {
            var buf = a.ToCharArray();
            var bufIntPtr = new int[15];
            for (var i = 0; i < a.Length; i++)
            {
                switch (buf[i])
                {
                    case '0':
                        bufIntPtr[i] = 0;
                        break;
                    case '1':
                        bufIntPtr[i] = 1;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("bufIntPtr", "Info Vector must contain just 0 or 1");
                }
            }
            return bufIntPtr;
        }

        public static int[] GetErrorSyndrome(int[] resultVector)
        {
            var resultNum = 0;
            var errorSyndrome = new int[4] { 0,0,0,0};
            for (int i = resultVector.Length - 1; i > -1; i--)
            {
                resultNum += resultVector[i] * Int32.Parse(Math.Pow(2, resultVector.Length - 1 - i).ToString());
            }
            var strErrorSyndrome = GetString2Mod(resultNum,19);
            while(strErrorSyndrome.Length<4)
            {
                strErrorSyndrome = "0" + strErrorSyndrome;
            }
            errorSyndrome[0] = Int32.Parse(strErrorSyndrome[0].ToString());
            errorSyndrome[1] = Int32.Parse(strErrorSyndrome[1].ToString());
            errorSyndrome[2] = Int32.Parse(strErrorSyndrome[2].ToString());
            errorSyndrome[3] = Int32.Parse(strErrorSyndrome[3].ToString());
            return errorSyndrome;
        }

        public static bool IsSyndromNull(int[] errorSyndrom)
        {
            for (var i = 0; i < errorSyndrom.Length; i++)
            {
                if (errorSyndrom[i] == 1)
                {
                    return false;
                }
            }
            return true;
        }

        public static int GetCountOf1(int[] vector)
        {
            var count = 0;
            for (var i = 0; i < vector.Length; i++)
                if (vector[i] == 1)
                    count++;
            return count;
        }

        public static BigInteger GetCombination( int n, int i)
        {
            return Fact(n)/(Fact(n - i) * Fact(i));
        }

        public static BigInteger Fact(long n) { return recfact(1, n); }

        public static BigInteger recfact(long start, long n)
        {
            long i;
            if (n <= 16)
            {
                BigInteger r = new BigInteger(start);
                for (i = start + 1; i < start + n; i++) r *= i;
                return r;
            }
            i = n / 2;
            return recfact(start, i) * recfact(start + i, n - i);
        }
        
        public static float GetDetectingAbility(float combinations, float foundcount)
        {
            return (foundcount / combinations);
        }


        static void Main()
        {
            var mainInfoVector = GetInfoVector();
          
            var mainCodeVector=GetCodeVector(mainInfoVector);
            var detectedErrors = new int[15];
              for (var i = 1; i < 32768; i++)
              {
                    var resultVector = new int[15];
                    var stringerror = ToString2NumSystem(i);
                        for (var j = stringerror.Length; j < 15; j++)
                        {
                            stringerror = "0"+stringerror;
                        }
            var errorVector = ToIntPtr(stringerror);
                  
                    for (var j = 0; j < 15; j++)
                    {
                resultVector[j] = (mainCodeVector[j]^errorVector[j]);
                    }
                       
                    var errorSyndrom = GetErrorSyndrome(resultVector);
                if (!IsSyndromNull(errorSyndrom))
                   {
                        detectedErrors[GetCountOf1(errorVector) - 1]++;
                   }  
                }
                Console.WriteLine('\n'+ "==========================================================");
                Console.WriteLine("||  i\t||\tСочетания\t||\tN0\t||C0");
                Console.WriteLine("==========================================================");
                for (var i = 0; i < 15; i++)
                {
                    long comb = long.Parse(GetCombination(15, i + 1).ToString());
                    var ability = GetDetectingAbility(comb, detectedErrors[i]);
                    Console.Write("||  " + (i + 1) + "\t||\t" + comb);
                    Console.Write("\t\t||\t" + detectedErrors[i]);
                    Console.Write("\t||"+ability.ToString("n3")+"\t||");
                    Console.WriteLine("\n==========================================================");
                }
            Console.Read();
        }
    }
}
