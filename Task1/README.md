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

Подключить базу LDAP:
```bash
docker exec -it openldap ldapadd -x \
  -D "cn=admin,dc=example,dc=com" \
  -w admin \
  -f /tmp/config.ldif

```

Проверить OpenLDAP:
```bash
ldapsearch -x -H ldap://localhost:389 -D "cn=admin,dc=example,dc=com" -w admin -b "dc=example,dc=com"
```

Подключить LDAP к Keycloak:

можно попробовать скриптом (но не сработало)
```bash
docker ps -a
docker exec -it task1-4-keycloak-1 /bin/bash
bash /opt/keycloak/setup-ldap-mappers.sh
```
или вручную:

### Добавить LDAP провайдер
- В меню слева выбрать User Federation
- Нажать Add provider → выбрать ldap
- Настроить параметры подключения:
 - Bind Credential	- admin
 - User Object Classes	inetOrgPerson
 - Import Users	- true
 - Save

Теперь Keycloak видит LDAP и может импортировать пользователей.

### Добавить LDAP mappers для групп → ролей
- Открыть User Federation → выбрать LDAP провайдер → вкладка Mappers
- Нажать Create → выбрать Mapper Type: group-ldap-mapper

Настроить mapper:
- Name	groups-mapper
- LDAP Groups DN	ou=Groups,dc=example,dc=com
- Group Name LDAP Attribute	cn
- Group Object Classes	groupOfNames
- Membership LDAP Attribute	member
- Membership Attribute Type	DN
- Mode	READ_ONLY
- User Roles Retrieve Strategy	LOAD_GROUPS_BY_MEMBER_ATTRIBUTE
- Groups Path	/
- Preserve Group Inheritance	true
- Ignore Missing Groups	false
- Drop Non Existing Groups During Sync	false
- Save

### Синхронизация LDAP
- В User Federation → LDAP провайдер → Sync Settings
- Можно запустить Full Sync или Changed Users Sync вручную

После синхронизации пользователи получат роли из групп LDAP:
![screen1](/Task1/Task1-4/syncScreen.png)

![screen1](/Task1/Task1-4/syncScreen2.png)


## Задача 5. Настройте MFA

