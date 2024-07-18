## Сборка

Скопировать и настроить файл конфигурации:
```bash
cp appsettings.json appsettings.Staging.json
nano appsettings.Staging.json
```

Для сборки выполнить команду:
```bash
dotnet publish -r linux-x64 -c Release --property:PublishDir=./publish /p:EnvironmentName=Staging --self-contained
```

## Сервис

Скопировать в директорию с сервисом:
```bash
rm -rf /var/www/oxi.ms.karousel.ru/web && mv ./publish /var/www/oxi.ms.karousel.ru/web
```

```bash
cp kp-ms-oxi.Staging.service /etc/systemd/system/kp-ms-oxi.service
```

## Запуск
```bash
sudo systemctl enable kp-ms-oxi.service
sudo systemctl start kp-ms-oxi.service
sudo systemctl status kp-ms-oxi.service
sudo systemctl disable kp-ms-oxi.service
sudo systemctl stop kp-ms-oxi.service

systemctl daemon-reload
```

## Среды
* Development
* Staging
* Production

Меняем среду на нужную в командах там где она указывается