# suai-bot-api-schedule
## Использование
Чтобы использовать данный сервис, достаточно отправить GET-запрос в следующей форме
```
https://wedonthavedomain.com/api.[serviceName].[methodName]?param1=value1&param2=value2/
```
## Сервисы
1. [TimeTable](https://github.com/callmemars1/suai-bot-api-gateway/blob/main/README.md#TimeTable)
2. [TeacherInfo](https://github.com/callmemars1/suai-bot-api-gateway/blob/main/README.md#TeacherInfo)
_________
### TimeTable
Данный сервис позволяет получить расписание нужного вам университета.  
1. [Пример](https://github.com/callmemars1/suai-bot-api-gateway/blob/main/README.md#Пример)
2. [Аргументы](https://github.com/callmemars1/suai-bot-api-gateway/blob/main/README.md#Аргументы)
3. [JSON-ключи](https://github.com/callmemars1/suai-bot-api-gateway/blob/main/README.md#Результат)
#### Пример
Запрос: 
```
https://wedonthavedomain.com/api.timeTable.get?university=SUAI&group=1001&teacher="Иванов И.И."&building="Пр-кт Победы д.28"&classRoom=12-02/
```
Результат:
```yaml
{
  "university": "SUAI",
  "lessons": 
    [
      {
        "group": "1011",
        "teacher": "Иванов И.И.",
        "classRoom": "12-02",
        "building": "Пр-кт Победы д.28",
        "university": "SUAI",
        "lessonName": "Экономика",
        "startTime": "9:30", 
        "endTime": "11:00",
        "order": 1,
        "weekDay": 0,
        "weekType": 1,
      },
      {
        "group": "1011",
        "teacher": "Иванов И.И.",
        "classRoom": "12-02",
        "building": "Пр-кт Победы д.28",
        "lessonName": "Бухгалтерский учет",
        "startTime": "11:10", 
        "endTime": "12:40",
        "order": 2,
        "weekDay": 0,
        "weekType": 1,
      }
    ]
}
```
#### Аргументы 
- *university* - Название университета (обязательно)
- *group* - Номер группы в текстовом формате
- *teacher* - Имя преподавателя
- *building* - Корпус университета
- *classRoom* - номер аудитории
#### Результат
В результате запроса возвращается JSON.  
Значение ключей:
- *university* - Название университета
- *group* - Номер группы
- *lessonName* - Название предмета
- *startTime* - Время начала пары
- *endTime* - Время окончания пары
- *order* - Порядковый номер пары
- *weekDay* - День недели
  - 0 - Понедельник
  - 1 - Вторник
  - 2 - Среда
  - 3 - Четверг
  - 4 - Пятница
  - 5 - Суббота
  - 6 - Воскресенье
- *weekType* - Тип недели
  - 0 - Нижняя неделя
  - 1 - Верхняя неделя
### TeacherInfo
in dev
