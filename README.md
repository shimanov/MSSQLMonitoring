# Database Maintenance

### ToDo
- [x] sa авторизация
- [x] Windows авторизация
- [ ] Сохранение пароля 
- [x] Список БД на сервере
    * Имя БД
    * Статус БД
    * Размер
- [x] Общий объем ОЗУ
- [x] Использованный MS SQL Server объем ОЗУ 
- [x] Данные о подключенном сервере 
    * Логическое имя сервера
    * Версия продукта
    * Редакция
    * Сервис пак
- [x] Подключение к новому экземпляру сервера 
- [ ] Проверка БД на наличие ошибок без исправлений
	* Сохранение результата в файл
- [ ] Исправление ошибок в БД (repair_allow_data_loss)
    * Перевод БД из статуса Suspect
- [ ] Обслуживание БД
    * Перестроение индексов
    * Очистка статистики 
- [ ] Освобождение не используемой памяти MS SQL
- [x] Окно About
- [ ] Window в цвет приложения
- [ ] Логгирование
- [ ] Инсталлятор
- [ ] Автоматическое обновление
- [ ] Библиотека классов для хранения скриптов
- [x] Сохранение временных файлв в изолированное хранилище
- [x] Очистка изолированного хранилища
- [ ] График нагрузки на ЦП 
- [ ] Библиотека для работы со временными файлами 

### Используемые скрипты 

Использованная память сервером
```sql
select (physical_memory_in_use_kb/1024)Phy_Memory_usedby_Sqlserver_MB 
from sys. dm_os_process_memory
```

Общий объем памяти сервера
```sql
select (total_physical_memory_kb/1024) 
from sys.dm_os_sys_memory
```

Получение сведений о выпуске MS SQL Server
```sql
SELECT 
	SERVERPROPERTY('ServerName') AS [Сервер]
	,SERVERPROPERTY('ProductVersion') AS [Версия продукта]
	,SERVERPROPERTY('Edition') AS [Редакция]
	,SERVERPROPERTY('ProductLevel') AS [SP];
```

Данные о выполнении команды DBCC
```sql
SELECT
    [command]
    ,[start_time]
    ,[percent_complete]
    ,[estimated_completion_time] / 60000. AS [estimated_completion_time_min]
FROM sys.dm_exec_requests
WHERE [command] LIKE '%DBCC%'
```
