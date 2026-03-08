# Задание 1. Повышение безопасности системы

## Настройка

```bash
docker compose up --build

curl -X POST http://localhost:8083/connectors \
-H "Content-Type: application/json" \
-d '{
"name": "crm-postgres-connector",
"config": {
"connector.class": "io.debezium.connector.postgresql.PostgresConnector",
"database.hostname": "crm-db",
"database.port": "5432",
"database.user": "crm",
"database.password": "crm",
"database.dbname": "crm",
"database.server.name": "crm",
"table.include.list": "public.clients",
"plugin.name": "pgoutput",
"slot.name": "debezium_slot",
"publication.autocreate.mode": "filtered"
}
}'
```



## Решение


```sql
INSERT INTO clients(full_name, birth_date, city)
VALUES ('Mike Brown','1993-02-02','Rome');
```