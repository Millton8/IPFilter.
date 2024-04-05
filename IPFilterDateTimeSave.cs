using System.Net;


namespace IPFilter
{
    /// <summary>
    /// Фильтрует и сохраняет в файл дату обращения с адреса
    /// В формате:
    /// Адрес Число обращений
    ///     дата1
    ///     дата2
    ///     дата3
    /// </summary>
    public class IPFilterDateTimeSave
    {
        IPAddress lowerIP;
        IPAddress maxIP;
        IPAddress fileIP;
        DateTime startDate = new();
        DateTime endDate = new();
        DateTime fileDate = new();
        string fileLog;
        string fileOutput;
        public IPFilterDateTimeSave(string fileLog, string fileOutput = null, string addressStart = null, uint? addressMask = null)
        {
            if (!File.Exists(fileLog))
            {
                Console.WriteLine("Исходный файл не найден");
                Console.ReadKey();
                throw new FileNotFoundException();
            }

            try
            {
                using (var writer = new StreamWriter(fileOutput))
                {
                    writer.WriteLine($"Start");
                }
            }
            catch
            {
                Console.WriteLine("Невозможно создать выходной файл по пути: " + fileOutput);
                Console.ReadKey();
                throw new FileNotFoundException();
            }

            if (addressMask != null && addressStart == null)
            {
                Console.WriteLine("Нельзя задать верхнюю границу без указания нижней");
                Console.ReadKey();
                throw new Exception("Нельзя задать верхнюю границу без указания нижней");

            }

            if (addressStart != null && addressMask == null)
            {
                if (!IPAddress.TryParse(addressStart, out lowerIP))
                {
                    Console.WriteLine("Неверный нижний IP. Введите адрес формата IPv4");
                    Console.ReadKey();
                    throw new Exception("Неправильный нижний ip");
                }
            }
            if (addressStart != null && addressMask != null)
            {
                if (!IPAddress.TryParse(addressStart, out lowerIP))
                {
                    Console.WriteLine("Неверный нижний IP. Введите адрес формата IPv4");
                    Console.ReadKey();
                    throw new Exception("Неправильный нижний ip");
                }

                // Преобразование десятичного представления в битовое
                byte[] binaryMask = BitConverter.GetBytes(addressMask.Value).Reverse().ToArray();
                maxIP = new IPAddress(binaryMask);
                if (lowerIP.CompareTo(maxIP) > 0)
                {
                    Console.WriteLine("Нижняя граница не может быть больше верхней");
                    Console.ReadKey();
                    throw new Exception("Нижняя граница не может быть больше верхней");

                }


            }
            this.fileLog = fileLog;
            this.fileOutput = fileOutput;
        }

        //Проверка даты
        public void EnterDate()
        {
            bool stDate = true;
            bool enDate = true;
            Console.WriteLine("Введите временной интервал\nВведите начальную дату");
            while (stDate)
            {
                if (!DateTime.TryParse(Console.ReadLine(), out startDate))
                {
                    Console.WriteLine("Неверная дата. Введите дату в формате dd.mm.yyyy");
                    continue;
                }
                stDate = false;
            }
            Console.WriteLine("Введите конечную дату");
            while (enDate)
            {
                if (!DateTime.TryParse(Console.ReadLine(), out endDate))
                {
                    Console.WriteLine("Неверная дата. Введите дату в формате dd.mm.yyyy");
                    continue;
                }
                enDate = false;
            }
            if (startDate > endDate)
            {
                Console.Clear();
                Console.WriteLine("Ошибка стартовое время не может быть больше конечного\n" +
                    "Введите даты заново\n");
                EnterDate();
            }

        }

        //Проверка ссчитываемых данных
        public bool FilterLineFromFile(string line)
        {
            var parts = line.Split(' ');
            if (parts.Length < 3)
            {
                Console.WriteLine("Ошибка. В файле отсутствует один из параметров.Пропускаю.");
                return false;
            }
            if (!IPAddress.TryParse(parts[0], out fileIP))
            {
                Console.WriteLine($"Ошибка. В файле неправильный формат IP адреса: {parts[0]}. Пропускаю.");
                return false;
            }
            if (!DateTime.TryParse(parts[1] + " " + parts[2], out fileDate))
            {
                Console.WriteLine($"Ошибка. В файле неправильный формат Даты: {parts[1] + " " + parts[2]}. Пропускаю.");
                return false;
            }

            if (fileDate < startDate || fileDate > endDate)
                return false;
            return true;
        }
        public void Filter()
        {
            var filteredDict = new Dictionary<IPAddress, HashSet<DateTime>>();
            bool isRight = false;
            string line;
            if (lowerIP != null && maxIP == null)
            {
                using (StreamReader sr = new StreamReader(fileLog))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        isRight = FilterLineFromFile(line);
                        if (!isRight)
                            continue;

                        if (fileIP.CompareTo(lowerIP) < 0)
                            continue;
                        addToDict();

                    }
                }
            }

            else if (lowerIP != null && maxIP != null)
            {
                using (StreamReader sr = new StreamReader(fileLog))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        isRight = FilterLineFromFile(line);
                        if (!isRight)
                            continue;

                        if (fileIP.CompareTo(lowerIP) < 0 || fileIP.CompareTo(maxIP) > 0)
                            continue;
                        addToDict();

                    }
                }
            }
            else
            {
                using (StreamReader sr = new StreamReader(fileLog))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        isRight = FilterLineFromFile(line);
                        if (!isRight)
                            continue;
                        addToDict();
                    }
                }
            }

            using (var writer = new StreamWriter(fileOutput))
            {
                if (filteredDict.Count == 0)
                {
                    writer.WriteLine($"Нет подходящих данных");
                    Console.WriteLine("За этот период не было совпадений");
                    Console.ReadKey();
                    return;
                }

                foreach (var ipAddress in filteredDict)
                {
                    writer.WriteLine($"{ipAddress.Key} Обращений {ipAddress.Value.Count}");
                    foreach (var date in ipAddress.Value)
                    {
                        writer.WriteLine($"\t {date}");
                    }
                    writer.WriteLine($"");
                }
            }
            Console.WriteLine($"Фильтрация завершена. Найдено IP адресов {filteredDict.Count}");

            void addToDict()
            {
                if (!filteredDict.ContainsKey(fileIP))
                    filteredDict[fileIP] = new HashSet<DateTime>();

                filteredDict[fileIP].Add(fileDate);
            }

        }
    }
}
