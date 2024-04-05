using CommandLine;
using Microsoft.Extensions.Configuration;

namespace IPFilter
{

    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
            //Для работы через конфигурацию вставьте appsettings.json в название файла ниже:
            if (File.Exists(""))
            {
                IConfiguration Configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables()
               .Build();

                var fileLog= Configuration.GetSection("FileInput").Value;
                var fileOutput = Configuration.GetSection("FileOutput").Value;
                var addressStart = Configuration.GetSection("lowerIP").Value;
                var addressMask = Convert.ToUInt32(Configuration.GetSection("maxIP").Value);

                var ipFilter = new IPFilter(fileLog, fileOutput, addressStart, addressMask);
                ipFilter.EnterDate();
                ipFilter.Filter();


            }
            else
            await Parser.Default.ParseArguments<CommandLineOptions>(args)
                .MapResult(async (CommandLineOptions opts) =>
                {
                    var ipFilter = new IPFilter(opts.fileLog, opts.fileOutput, opts.addressStart, opts.addressMask);
                    ipFilter.EnterDate();
                    ipFilter.Filter();


                },
                errs => Task.FromResult(-1)); 
            Console.ReadKey();
            return 1;

        }
    }
}
