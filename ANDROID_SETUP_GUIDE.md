# Инструкция: Запуск Easy-ice на Android эмуляторе

**Проблема:** Сборка успешна, но развертывание не удаётся из-за отсутствия Android SDK.

---

## Диагностика проблемы

✅ .NET workloads установлены (android, maui)  
❌ Android SDK не найден в стандартном расположении  
❌ ADB (Android Debug Bridge) недоступен  
❌ Эмуляторы не созданы

---

## Решение 1: Установка через Android Studio (Рекомендуется)

### Шаг 1: Скачайте Android Studio
1. Перейдите на https://developer.android.com/studio
2. Скачайте последнюю версию для Windows
3. Запустите установщик

### Шаг 2: Установите Android Studio
1. Выберите **Standard** установку
2. Дождитесь загрузки компонентов:
   - Android SDK Platform-Tools
   - Android SDK Build-Tools
   - Android Emulator
   - Android SDK Platform (API 34 или выше)

### Шаг 3: Настройте переменные среды
1. Нажмите `Win + R`, введите `sysdm.cpl`, нажмите Enter
2. Перейдите: **Дополнительно** → **Переменные среды**
3. В **Системных переменных** создайте:
   ```
   Имя: ANDROID_HOME
   Значение: C:\Users\HONOR\AppData\Local\Android\Sdk
   ```
4. В переменной **Path** добавьте:
   ```
   %ANDROID_HOME%\platform-tools
   %ANDROID_HOME%\emulator
   %ANDROID_HOME%\tools
   %ANDROID_HOME%\tools\bin
   ```
5. Нажмите **ОК** во всех окнах
6. **ПЕРЕЗАПУСТИТЕ Visual Studio и PowerShell**

### Шаг 4: Создайте Android Virtual Device (AVD)
1. Откройте Android Studio
2. Перейдите: **Tools** → **Device Manager**
3. Нажмите **Create Device**
4. Выберите устройство: **Pixel 5** или **Pixel 7**
5. Выберите образ системы: **API 34** (Android 14) или **API 33** (Android 13)
6. Скачайте образ, если требуется
7. Нажмите **Finish**
8. **Запустите эмулятор** (нажмите ▶️)

### Шаг 5: Проверьте подключение
Откройте PowerShell (закройте старую, откройте новую) и выполните:

```powershell
adb devices
```

**Ожидаемый результат:**
```
List of devices attached
emulator-5554   device
```

Если видите `emulator-5554   device` — всё работает!

---

## Решение 2: Ручная установка Android SDK (Только командная строка)

Если Android Studio не нужен:

### Шаг 1: Скачайте Command Line Tools
1. https://developer.android.com/studio#command-line-tools-only
2. Скачайте **Command line tools only** для Windows
3. Разархивируйте в `C:\Android\cmdline-tools\latest\`

### Шаг 2: Установите SDK компоненты
```powershell
cd C:\Android\cmdline-tools\latest\bin

# Установка базовых компонентов
.\sdkmanager.bat "platform-tools" "platforms;android-34" "build-tools;34.0.0" "emulator" "system-images;android-34;google_apis;x86_64"

# Создание эмулятора
.\avdmanager.bat create avd -n "EasyIce_Pixel5" -k "system-images;android-34;google_apis;x86_64" -d "pixel_5"
```

### Шаг 3: Настройте переменные среды
```
ANDROID_HOME = C:\Android
Path += C:\Android\platform-tools
Path += C:\Android\emulator
```

### Шаг 4: Запустите эмулятор
```powershell
emulator -avd EasyIce_Pixel5
```

---

## Запуск приложения после настройки

### Через Visual Studio:
1. Запустите Android эмулятор вручную (через AVD Manager)
2. Дождитесь полной загрузки Android (домашний экран)
3. В Visual Studio выберите целевое устройство: **emulator-5554**
4. Нажмите **F5** или **Debug** → **Start Debugging**

### Через командную строку:
```powershell
cd c:\Dev\Easy-ice\src\Frontend\EasyIce.Mobile

# Убедитесь, что эмулятор запущен
adb devices

# Разверните приложение
dotnet build -t:Run -f net9.0-android
```

---

## Альтернатива: Запуск на реальном устройстве

Если эмулятор работает медленно:

1. **Включите режим разработчика на телефоне:**
   - Настройки → О телефоне → Нажмите 7 раз на "Номер сборки"
2. **Включите USB-отладку:**
   - Настройки → Для разработчиков → USB-отладка
3. **Подключите телефон через USB**
4. **Разрешите отладку** (всплывающее окно на телефоне)
5. **Проверьте подключение:**
   ```powershell
   adb devices
   # Должно показать: <serial>   device
   ```
6. **Запустите из Visual Studio**, выбрав ваше устройство

**Важно для реального устройства:**
- В `ApiService.cs` измените `BaseUrl` с `10.0.2.2` на IP вашего ПК (например `192.168.1.100`)
- Узнать IP: `ipconfig | findstr "IPv4"`

---

## Проверка установки

Выполните все команды по очереди:

```powershell
# 1. Проверка ADB
adb version

# 2. Проверка эмулятора
emulator -list-avds

# 3. Проверка устройств
adb devices

# 4. Проверка Android SDK
dir $env:ANDROID_HOME
```

Все команды должны выполниться без ошибок.

---

## Частые проблемы

### "adb is not recognized"
- ❌ Переменная `ANDROID_HOME` не настроена
- ❌ Path не содержит `%ANDROID_HOME%\platform-tools`
- ✅ **Решение:** Перезапустите PowerShell/Visual Studio после настройки переменных

### "No emulators are running"
- ❌ Эмулятор не запущен или ещё загружается
- ✅ **Решение:** Запустите эмулятор вручную через AVD Manager

### "Deployment failed" в Visual Studio
- ❌ Эмулятор не виден для Visual Studio
- ✅ **Решение:** 
  1. Закройте Visual Studio
  2. Запустите эмулятор через AVD Manager
  3. Проверьте `adb devices`
  4. Откройте Visual Studio заново

### Эмулятор медленный/зависает
- Включите аппаратное ускорение (HAXM/WHPX)
- Увеличьте RAM эмулятора в настройках AVD (4096 MB+)
- Используйте образы x86_64, а не ARM

---

## Следующие шаги

После успешной настройки:
1. ✅ Эмулятор запущен и виден в `adb devices`
2. ✅ Visual Studio видит устройство в списке
3. ✅ Можно запускать приложение через F5

**Приложение работает в мок-режиме** — бэкенд не требуется.
