Команды для запуска

IPFilter -i input.txt -o out.txt

IPFilter -i input.txt -o out.txt -s 200.200.200.0 -e 3594967295

Если проапгрейдить версию для того чтобы сохранялась дата и время обращения. То можно получить такие результаты<br/>
Количество строк IP дата для теста 1 000 000
![](https://github.com/Millton8/IPFilter./blob/master/bench.jpg)

<p>FilterDict. Считываем файл через File.Open в словарь записываем только те события что попадают в заданный промежуток времени и затем фильтруем этот словарь по IP</p>
<p>ReadFromFileAndFilter Считываем файл через File.Open. Фильтруем на промежуток времени и IP и только потом записываем в словарь</p>
<p>FilterHashSet. Тоже самое что и ReadFromFileAndFilter но даты храним в HashSet</p>
<p>IPFilter. Та версия что представлена в гит. Открываем файл через StreamReader и фильтруем каждую строку на дату и IP. Только дата в той версии для теста также записывается в HashSet.</p></br>
<p>Попробовал написать такую же версию на python. </p>
<p>Одно лишь преобразование строки в дату используя strptime занимает 9.5 секунд.</p>
<p>Разделив строку по точкам через split(".") и время через через split(":") можно создать дату:
  date=datetime(int(date_mas[2]),int(date_mas[1]),int(date_mas[0]),int(time_mas[0]),int(time_mas[1]),int(time_mas[2]))
Это вариант уже занимает 2.8 секунды.
  </p>
  <p>Как итог со всеми улучшениями версия на python обрабатывает тот же самый файл с теми же условиями за 5.6 секунды. Выделяемая память 19 MB</p>

