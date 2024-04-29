using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Utils;


namespace HyperGraphExportedCurvesProcessor
{
    public class Program
    {
        static string inputFile = @"29.04_AccelOverCattleGrids_Rear.csv";
        static string outPutFile = @"29.04_accelMagnitudesOut1111111111.csv";

        static string inputFolder = @"Input\";
        static string outputFolder = @"Output\";
        static string templateFile = outputFolder + @"template.csv";

        static void Main(string[] args)
        {
            string rootPath = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.FullName + @"\";
            Console.WriteLine(rootPath);

            string[] allText = readFile(rootPath + inputFolder +inputFile);
            Console.WriteLine(StringUtils.getInterval(allText[8],18,18+10 + 1 + 4));
            Console.WriteLine(StringUtils.expNotationToFloat(StringUtils.getInterval(allText[8],0,10+1+4)));
            Console.WriteLine(StringUtils.expNotationToFloat(StringUtils.getInterval(allText[15],0,10+1+4)));
            Console.WriteLine(StringUtils.getInterval("0123456789",9, 9));


            // // read
            int curveCount = 0;
            float[][] timeAndMax=new float[200][];
            int[] maxPosArr = new int[200];
            {
                int j = 0;
                while (j < allText.Length)
                {
                    bool MAG = false;
                    float max = 0;
                    int maxPos = 0;
                    float maxTime = 0;
                    int i = 0;
                    int k = i + j;
                    while (k < allText.Length && allText[k] != "")
                    {
                        string expText = StringUtils.getInterval(allText[k], 18, allText[k].Length);
                        string timeText = StringUtils.getInterval(allText[k], 0, 18);
                        float time = StringUtils.expNotationToFloat(timeText);
                        //short isNegative = 0;
                        /*if (expText[0] == '-')
                        {
                            isNegative = 1;
                            Console.WriteLine("E");
                        }*/
                        if (StringUtils.expNotationToFloat(expText) > max && time > 0.1f && curveCount%4==0) // if MAG curve
                        {
                            max = StringUtils.expNotationToFloat(expText);
                            maxPos = i;
                            maxTime = time;
                        }
                        if (curveCount % 4 == 0)
                        {
                            MAG = true;
                        }

                        i++;
                        k++;
                    }
                    // if not MAG curve
                    if (!MAG)
                    {
                        maxPos = maxPosArr[curveCount-1];
                        timeAndMax[curveCount] = new float[2];
                        timeAndMax[curveCount][0] =timeAndMax[curveCount-1][0];
                        timeAndMax[curveCount][1] = StringUtils.expNotationToFloat(StringUtils.getInterval(allText[maxPos + j], 18, allText[maxPos + j].Length));
                    }
                    else
                    {
                        timeAndMax[curveCount] = new float[] { maxTime, max };
                    }
                    maxPosArr[curveCount] = maxPos;
                    Console.WriteLine("max, maxPos, maxVal | i: " + max + ", " + maxPos + ", " + allText[maxPos] + " | " + i);
                    j = k + 1;
                    curveCount++;
                }
            }

            { // // write
                string outPutFilePath = rootPath + outputFolder + outPutFile;
                File.Copy(rootPath + templateFile, outPutFilePath);

                for (int i =0; i <curveCount; i+=4)
                {
                    File.AppendAllLines(outPutFilePath, new string[] { "RR" +(i/4+1)+"," +timeAndMax[i][0]+","+timeAndMax[i][1]+","+
                        timeAndMax[i + 1][1]+","+timeAndMax[i +2][1]+","+timeAndMax[i +3][1] });

                }
            }
        }

        static string[] readFile(string path)
        {
            return File.ReadAllLines(path);
        }
    }
}
