# Задание 1. Повышение безопасности системы

## Задача 1. Aрхитектурное решение и доработка диаграммы C4 

[BionicPRO_C4_model.drawio.xml](/Task1/Task1-1/BionicPRO_C4_model.drawio.xml)

![tobe](/Task1/Task1-1/BionicPRO_C4_model.drawio_tobe.png)

## Задача 2. Улучшите безопасность существующего приложения, заменив Code Grant на PKCE

- Код с изменениями frontend-приложения расположен здесь: [App.tsx](/Task1/Task1-2/frontend/src/App.tsx)
- Обновленные настройки keycloak здесь: [realm-export.json](/Task1/Task1-2/keycloak/realm-export.json)
- PKCE работает, пример har-лога: [pkce-sample.har](/Task1/Task1-2/pkce-sample.har)

## Задача 3. Обеспечьте безопасное получение и хранение access-и refresh-токенов

1. Реализована схема обмена токенов через backend-приложение "bionicpro-auth" согласно схеме:
```
Browser
  ↓ (cookie)
Frontend (React)
  ↓
BFF / Auth Server (bionicpro-auth)
  ↓ (client_secret)
Keycloak
```
2. Код backend-приложения представлен здесь: [BionicProAuth.sln C# solution](/Task1/Task1-3/bionicpro-auth/BionicProAuth.sln)
3. Авторизация работает, токены продляются. Пример har-лога: [backeend-auth-sample.har](/Task1/Task1-3/backeend-auth-sample.har)

