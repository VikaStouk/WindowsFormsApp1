using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UDP_server
{
    class Command
    {
        private int[] set_parameters;
        private int[] color = new int[3];
        private int[] color0 = new int[3] { 0, 0, 0 };
        private int[] set_parameters0 = new int[2] { 0, 0 };
        private int[] set_parameters0_2 = new int[4] { 0, 0, 0, 0 };
        private char[] command1 = new char[5] { '3', '4', '5', '6', '7' }; 
        private int CommandNumber;
        int j;
        public (int CommandNumber, int[] parameters, int[] color) Parser(string command)
        {
            string[] ArrayCommand;
            char[] CharCommandArray = new char[command.Length];
            CharCommandArray = command.ToCharArray();
            CommandNumber = Convert.ToInt32(CharCommandArray[1].ToString());
            var tuple_parse0 = (0, set_parameters, color0);
            if (ValidCharsFound(command) == true)
            {
                if (CharCommandArray[0] == '#' && CharCommandArray[2] == '#' && CharCommandArray[CharCommandArray.Length - 1] == '$' && CommandNumber <9)
                {
                    ArrayCommand = split_string(command, 3);
                    if (CharCommandArray[1] == '1')
                    {
                        j = 0;
                        tuple_parse0 = (0, set_parameters, color0);
                        for (int index = 1; index < ArrayCommand.Length - 1; index++)
                        {
                            color[j] = Convert.ToInt16(ArrayCommand[index]);
                            j++;
                        }
                        foreach (var color_num in color)
                        {
                            if (color_num < 0 || color_num > 255)
                            {
                                //Console.WriteLine("Ошибка!Номер цвета должен находиться в диапазоне от 0 до 255");
                               return tuple_parse0;
                            }
                        }
                    }
                    else
                        if (CharCommandArray[1] == '2')
                    {
                        j = 0;
                        tuple_parse0 = (0, set_parameters0, color0);
                        set_parameters = new int[2];
                        for (int index = 1; index < ArrayCommand.Length - 4; index++)
                        {
                            set_parameters[j] = Convert.ToInt16(ArrayCommand[index]);
                            j++;
                        }
                        j = 0;
                        for (int index = 3; index < ArrayCommand.Length - 1; index++)
                        {
                            color[j] = Convert.ToInt16(ArrayCommand[index]);
                            j++;
                        }
                        foreach (var parameter in set_parameters)
                        {
                            if (parameter < 0 || parameter > 900)
                            {
                                //Console.WriteLine("Ошибка!Параметры должны находиться в диапазоне от 0 до 255");
                                return tuple_parse0;
                            }
                        }
                        foreach (var color_num in color)
                        {
                            if (color_num < 0 || color_num > 255)
                            {
                                //Console.WriteLine("Ошибка!Номер цвета должен находиться в диапазоне от 0 до 255");
                               return tuple_parse0;
                            }
                        }
                    }
                    else
                        if(CharCommandArray[1] == '8' )
                    {
                        set_parameters = new int[4];
                        tuple_parse0 = (0, set_parameters0_2, color0);
                        j = 0;
                        for (int index = 1; index < ArrayCommand.Length - 4; index++)
                        {
                            set_parameters[j] = Convert.ToInt16(ArrayCommand[index]);
                            j++;
                        }
                        j = 0;
                        for (int index = 5; index < ArrayCommand.Length - 1; index++)
                        {
                            color[j] = Convert.ToInt16(ArrayCommand[index]);
                            j++;
                        }
                        foreach (var parameter in set_parameters)
                        {
                            if (parameter < 0 || parameter > 900)
                            {
                                //Console.WriteLine("Ошибка!Параметры должны находиться в диапазоне от 0 до 255");
                                return tuple_parse0;
                            }
                        }
                        foreach (var color_num in color)
                        {
                            if (color_num < 0 || color_num > 255)
                            {
                                // Console.WriteLine("Ошибка!Номер цвета должен находиться в диапазоне от 0 до 255");
                                return tuple_parse0;
                            }
                        }
                    }
                    else
                    {
                        set_parameters = new int[4];
                        tuple_parse0 = (0, set_parameters0_2, color0);
                        j = 0;
                        for (int index = 1; index < ArrayCommand.Length - 4; index++)
                        {
                            set_parameters[j] = Convert.ToInt16(ArrayCommand[index]);
                            j++;
                        }
                        j = 0;
                        for (int index = 5; index < ArrayCommand.Length - 1; index++)
                        {
                            color[j] = Convert.ToInt16(ArrayCommand[index]);
                            j++;
                        }
                        foreach (var parameter in set_parameters)
                        {
                            if (parameter < 0 || parameter > 900)
                            {
                                //Console.WriteLine("Ошибка!Параметры должны находиться в диапазоне от 0 до 255");
                                return tuple_parse0;
                            }
                        }
                        if(set_parameters[2]>64)
                        {
                            return tuple_parse0;
                        }
                        else
                        foreach (var color_num in color)
                        {
                            if (color_num < 0 || color_num > 255)
                            {
                                // Console.WriteLine("Ошибка!Номер цвета должен находиться в диапазоне от 0 до 255");
                                return tuple_parse0;
                            }
                        }
                    }
                    var tuple_parse = (CommandNumber, set_parameters, color);
                    return tuple_parse;
                }
                else
                {
                    //Console.WriteLine("Ошибка! Введены неправильные символы начала/конца команды!");
                   return tuple_parse0;
                }
            }
            else
            {
                //Console.WriteLine("Ошибка! Введены недопустимые символы в команде!");
                return tuple_parse0;
            }
        }
        string[] split_string(string str, int interval)
        {
            List<string> substrs = new List<string>();
            int i;
            for (i = 0; i < str.Length - interval; i += interval)
            {
                substrs.Add(str.Substring(i, interval));
            }
            substrs.Add(str.Substring(i));
            return substrs.ToArray();
        }
        static bool ValidCharsFound(string str)
        {
            return Regex.IsMatch(str, @"^[0-9#$a-z]+$");
        }
    }
    
}
