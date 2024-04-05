using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPFilter
{
    //Настройка вводных значений из командной строки
    public class CommandLineOptions
    {
        [Option(shortName: 'i', longName: "file-log", Required = true, HelpText = "Путь к исходному фалу.")]
        public string fileLog { get; set; } = "1.txt";

        [Option(shortName: 'o', longName: "file-output", Required = true, HelpText = "Путь к файлу с отфильтрованными данными.")]
        public string fileOutput { get; set; } = "output.txt";

        [Option(shortName: 's', longName: "address-start", Required = false, HelpText = "Минимальный IP.")]
        public string addressStart { get; set; } = null;

        [Option(shortName: 'e', longName: "address-mask", Required = false, HelpText = "Максимальный IP.")]
        public uint? addressMask { get; set; } = null;


    }
}
