# Задание 2. Разработка сервиса отчётов

## Архитектура решения

[BionicPRO_C4_model.drawio.xml](/Task2/BionicPRO_C4_model.drawio.xml)

![BionicPRO_C4_model.drawio-ETL.png](/Task2/BionicPRO_C4_model.drawio-ETL.png)

## Airflow DAG

За основу взят [пример из теории](https://github.com/Yandex-Practicum/architecture-DWH-pipeline).

### Архитектруа текущего примера работы с Airflow

- Источники данных:
    - crm-clients-data.csv
    - clients-telemetry-data.csv
- OLAP: ClickHouse
- Оркестратор: Apache Airflow
- Executor: LocalExecutor
- DAG: по расписанию

### DAG Pipeline

1. Extract
    - Читаем CSV
    - Load (staging)
2. Загружаем:
    - stg_clients
    - stg_telemetry
3. Transform
    - Агрегируем телеметрию
    - JOIN с клиентами
4. Load (Data Mart)
    - Пишем в dm_clients_telemetry

### Запуск

1. Сгенерировать данные для примера
```bash
python ./data/generate_data.py
```

2. Запустить докер
```bash
docker compose down -v
docker compose up --build
```

3. Открыть http://localhost:8085 

4. Включить DAG
    - Найти etl_clickhouse_pipeline
    - Включить переключатель
    - Нажать Trigger

### Проверка

1. Открыть Clickhouse play ( http://localhost:8123/play)
2. Выполнить
```sql
SELECT * FROM dm_clients_telemetry FINAL;
```

![airflowDag_1.png](/Task2/airflowDag_1.png)

## Report service Backend Api

[ReportService.sln](/Task2/data-services/report-service/ReportService.sln) сервис реализован.

Проверка:
GET [http://localhost:8086/api/reports?clientId=user1](http://localhost:8086/api/reports?clientId=user1)

![reports-api-response.png](/Task2/reports-api-response.png)

