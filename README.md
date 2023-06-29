Приложение сделано на основе "Тестовое задание на позицию Junior Python Developer.pdf".


Отличия от задания
1) не Flask\Django, а .NET Core MVC + Entity Fr
2) БД не postgresql, а SQLight
3) заполнение фейковыми данными (seeding) сделано по другому
4) обычный bootstrap, не твитерный
5) количество записей намного меньше (<200 вместо 50k)
7) поиск только по полю 'имя' 
6) нет фото thumbnail на общей странице
7) не сделано:
   - "возможность перераспределения сотрудников в случае изменения начальника" 
   - "ленивая загрузка для дерева" 
   - "менять начальника сотрудника через drag-n-drop в дереве"  



Как пользоваться
1. Создание БД 
1.1 Установить python пакет: pip install russian-names
1.2 Запустить файл create_database.py, время выполнения ~6 секунд

2. Запустить проект



В wwwroot/userpics только одна картинка 'anonim'.
Папка userpics_issue имитирует внешний источник. Юзерпики оттуда копируются в wwwroot. Путь 
зашит в контроллере.

В поле DataTime тип данных string.

Роли\группы отсутствуют. Авторизация через куки.


логин: admin@google.com, пароль: password