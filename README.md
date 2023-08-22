CRUD-приложение сделано на основе "Тестовое задание на позицию 'Python Developer.pdf'".
https://drive.google.com/file/d/1qUzx0m_Koj83k_G8BScCNK7opazbuDzk/view




Отличия от задания
1) не Flask\Django, а .NET Core MVC + Entity Fr
2) БД не postgresql, а SQLight
3) заполнение фейковыми данными (seeding) сделано по другому
4) обычный bootstrap, не твитерный
5) количество записей меньше
7) поиск по полю 'имя' 
6) нет фото thumbnail на общей странице



Как пользоваться
1. Создание БД (python)
- Установить пакет 'pip install russian-names'. 
  Устанавливать виртуальное окружение из-за одного небольшого пакета не обязательно.
- Запустить файл .net-mvc-entity-project\create_database.py, время выполнения ~10 секунд.
  Созданная база появится в .net-mvc-entity-project\SimpleAuth\Database.
3. Запустить проект через Visual Studio.
   VS подхватит проект и сама установит версии библиотек из файла проекта.


Visual Studio Version 17, 2022;
net6.0, version 4.8.4084.04; 
Web browser Google Chrome.

В wwwroot/userpics только одна картинка 'anonim', дефолтная для всех пользователей.
Папка userpics_issue имитирует внешний источник. При добавлении юзерпиков 
они копируются оттуда в wwwroot. Путь зашит в контроллере.

В поле DataTime тип данных string.

Роли\группы отсутствуют. Авторизация через куки.

Входные данные для входа:
логин: admin@google.com, пароль: password