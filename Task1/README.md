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

## Задача 4. Добавьте LDAP для возможности получения данных о пользователях представительства BionicPRO в другой стране

[Настройка](/Task1-4/README.md)

После синхронизации пользователи получат роли из групп LDAP:
![screen1](/Task1-4/syncScreen.png)

![screen2](/Task1-4/syncScreen2.png)

## Задача 5. Настройте MFA

[Настройка](/Task1-5/README.md)

1. Пользователь логинится через LDAP
2. Если OTP ещё не настроен - Keycloak попросит отсканировать QR
   Har-Лог с примером: [mfa-sample.har](/mfa-sample.har)

   ![screen1](/Screenshot_mfa1.png)

3. После настройки OTP — каждый вход требует код
   Har-Лог с примером: [mfa-sample2.har](/mfa-sample2.har)

   ![screen1](/Screenshot_mfa2.png)

## Задача 6. Добавьте OAuth 2.0 от Яндекс ID

[Описание реализации](/Task1-6/README.md)

