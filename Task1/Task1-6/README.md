# Задача 6. Добавьте OAuth 2.0 от Яндекс ID

## Настройка

### 1. Регистрация приложения в Яндекс ID
   - Открыть консоль разработчика: https://oauth.yandex.ru/
   - Создайте первое приложение - Для авторизации пользователей
   - Тип: Web service
   - Redirect URI: http://localhost:8082/auth/yandex/callback
   - Права доступа (Scopes):
      - login:email
      - login:info

При создании будут получены:
   - Client ID: 3fc4b067df62432396139160c4967445
   - Client Secret: 72cb9a140c6443f09b7196b6f0dd3357
   
Данные сохранены в [bionicpro-auth/appsettings.json](/architecture-bionicpro/Task1/Task1-6/bionicpro-auth/appsettings.json)
   

### 2. OAuth flow

Код интеграции с Yandex реализован в бекенд-приложении [bionicpro-auth/BionicProAuth.sln](/bionicpro-auth/BionicProAuth.sln)
   - Реализован /auth/yandex/login
   - Реализован /auth/yandex/callback
   - РЕализовано хранение пользователей в БД (пока это redis)

## Решение

1. Создаyj Web OAuth приложение в Яндекс ID
![screen1](/architecture-bionicpro/Task1/Task1-6/Screenshot_ya1.png)


При авторизации:
1. Можно войти по логину-паролю, и можено через Yandex
![screen2](/architecture-bionicpro/Task1/Task1-6/Screenshot_ya2.png)

2. При первом входе предлагается создать учетную запись
![screen3](/architecture-bionicpro/Task1/Task1-6/Screenshot_ya3.png)

3. Яндекс запрашивает необходимые права
![screen4](/architecture-bionicpro/Task1/Task1-6/Screenshot_ya4.png)

4. Выполняется авторизация
Пример har-лога с полным прохождением flow: [ya-oauth2-sample.har](/ya-oauth2-sample.har)
