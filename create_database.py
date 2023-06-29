# Информация о каждом сотруднике должна храниться в базе данных и
# содержать следующие данные: 
# ФИО, Должность, Дата приема на работу; 
# Размер заработной платы; У каждого сотрудника есть 1 начальник;


import datetime
import random
from datetime import timedelta
from russian_names import RussianNames
import sqlite3
import time



class Staff:
    def __init__(self):  
        self.number_employees = 100     # constanta
        self.employe_index = None       # сквозной счётчик всех сотрудников, c 0
        self.immediate_supervisor = ''  # непосредственный начальник текущего сотрудника
        self.name = ''                  # Данные для внесения в БД
        self.position = ''
        self.date = None
        self.salary = None
        self.photo_path = ''
        self.current_chief = ''
        self.current_chief_deputy = ''
        self.current_department_head = ''
        self.current_senior_programmer = ''
        self.chief_set = []
        self.chief_deputy_set = []
        self.department_head_set = []
        self.senior_programmer_set = []
        self.database_path = r'SimpleAuth\Database\UsersDB.db'
        

    # Нужна функция (типа логарфмической) для количества персонала, в зависимости от общей цифры
    # Пока что все данные захардкожены в код - это временно
    def employees(self):   
        self.number_chief = 1
        self.number_chief_deputy = 3 
        self.number_department_head = 9 
        self.number_senior_programmer = 27 
        self.number_programmer = (self.number_employees -
                                 self.number_chief -
                                 self.number_chief_deputy -
                                 self.number_department_head -
                                 self.number_senior_programmer)
        res = [self.number_chief,
               self.number_chief_deputy,
               self.number_department_head,
               self.number_senior_programmer,
               self.number_programmer]
        return res


     # -------------------------database-----------------------------
    def create_db(self):
        sqliteConnection = sqlite3.connect(self.database_path)
        cursor = sqliteConnection.cursor()
        cursor.execute("""CREATE TABLE IF NOT EXISTS usersdb(
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        name TEXT,
        position TEXT,
        date TEXT, 
        salary INTEGER,
        immediate_supervisor TEXT,
        photo_path TEXT,
        email TEXT,
        password TEXT,
        roleid INTEGER,
        role TEXT)
                        """)
        # add admin row            
        add_to_table = "INSERT INTO usersdb (name, position, date, salary, immediate_supervisor, photo_path, email, password, roleid, role) values (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)"       
        cursor.execute(add_to_table,('admin', 'admin', '2023-01-11',
                                      250000, 'Генеральный директор', '',
                                      'admin@google.com', 'password', 0, 'AdminRole'))
        sqliteConnection.commit()
        cursor.close()


    def insert_date_db(self, _name, _position, _date,
                       _salary, _immediate_supervisor,
                       _photo_path, _email, _password, roleid, role):
        sqliteConnection = sqlite3.connect(self.database_path)
        cursor = sqliteConnection.cursor()
        add_to_table = "INSERT INTO usersdb (name, position, date, salary, immediate_supervisor, photo_path, email, password, roleid, role) values (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)"
        cursor.execute(add_to_table, (_name, _position, _date, _salary,
                                      _immediate_supervisor, _photo_path, _email,
                                      _password, 1, "UserRole"))
        sqliteConnection.commit()
        cursor.close()


                                 
    # -------------- data creation --------------------    
    # Индекс сквозной нумерации
    def index_func(self):
        if self.employe_index == None:
            self.employe_index = 1               
        else:
            self.employe_index += 1 
        return self.employe_index    
        
    
    # ФИО
    def generate_name(self, employe_index):
        name = RussianNames().get_person()
        # ИОФ --> ФИО
        splitted = name.split(' ')
        self.name = splitted[2] + ' ' + splitted[0] + ' ' + splitted[1]
        if self.employe_index == 1:
            self.chief_set.append(self.name)
        elif 1 < self.employe_index < 5:
            self.chief_deputy_set.append(self.name) 
        elif 4 < self.employe_index < 14:
            self.department_head_set.append(self.name)
        elif 13 < self.employe_index < 41:
            self.senior_programmer_set.append(self.name)
        # не надо добавлять программеров в список, т.к. они не 'непосредственные начальники'
        #elif 39 < self.employe_index:
        #    self.position = 'programmer'

   
    # должность
    # Должны быть не числа, а переменные из employees(), потом поправлю (эти - для 100 сотрудников)
    # + потом сделать current_position для вычисления текущего начальника
    def generate_position(self, employe_index):
        if self.employe_index == 1:
            self.position = 'Генеральный директор' 
        elif 1 < self.employe_index < 5:
            self.position = 'Заместитель генерального директора' 
        elif 4 < self.employe_index < 14:
            self.position = 'Начальник отдела' 
        elif 13 < self.employe_index < 41:
            self.position = 'Старший программист'
        elif 40 < self.employe_index:
            self.position = 'Программист' 

    
    # дата поступления на службу: чем выше должность тем раньше поступил
    # дата генерится случайно из вилки между двумя значениями
    def generate_date(self, position):
        if self.position == 'Генеральный директор': 
            start_dt = datetime.date(2010, 1, 1)
            end_dt = datetime.date(2011, 1, 1)
        elif self.position == 'Заместитель генерального директора': 
            start_dt = datetime.date(2011, 1, 1)
            end_dt = datetime.date(2013, 1, 1)
        elif self.position == 'Начальник отдела': 
            start_dt = datetime.date(2013, 1, 1)
            end_dt = datetime.date(2018, 1, 1)
        elif self.position == 'Старший программист': 
            start_dt = datetime.date(2018, 1, 1)
            end_dt = datetime.date(2021, 1, 1)
        else:
            start_dt = datetime.date(2021, 1, 1)
            end_dt = datetime.date(2023, 1, 1)
        time_between_dates = end_dt - start_dt
        days_between_dates = time_between_dates.days
        random_number_of_days = random.randrange(days_between_dates)
        self.date = str(start_dt + datetime.timedelta(days=random_number_of_days))


    # чем выше должность тем выше зарплата
    # Значение генерится случайно из вилки
    def generate_salary(self, position):
        salary = 0
        if self.position == 'Генеральный директор': 
            self.salary = random.randrange(800000, 1000000, 20000)
        elif self.position == 'Заместитель генерального директора': 
            self.salary = random.randrange(500000, 800000, 10000)
        elif self.position == 'Начальник отдела': 
            self.salary = random.randrange(300000, 500000, 5000)
        elif self.position == 'Старший программист': 
            self.salary = random.randrange(150000, 300000, 5000)
        else:
            self.salary = random.randrange(50000, 100000, 1000)


    # ----------------------- Генерация поля "непосредственный начальник" -----------------------------
    def generate_dict(start1, stop1, step1, stop2, stop3):
        keys_list = [str(x) for x in range(start1, stop1, step1)]                         
        values_list = [i for i in range(0, stop2, 1) for j in range(0, stop3, 1)] 
        res = dict(zip(keys_list, values_list))
        return res


    level_3_dict = generate_dict(5,14,1,3,3)
    level_4_dict = generate_dict(14,41,1,9,3)
    level_5_dict = generate_dict(41,176,1,135,5)


    def generate_immediate_supervisor(self, employe_index):
        # level_1 chief
        if self.employe_index == 1:
            self.immediate_supervisor = '---'
        # level_2 chief deputy 
        elif 1 < self.employe_index < 5:
            self.immediate_supervisor = self.chief_set[0]
        # level_3 department_head
        elif 4 < self.employe_index < 14:
            self.chief_deputy_set[ self.level_3_dict[str(self.employe_index)] ]
            self.immediate_supervisor = self.chief_deputy_set[ self.level_3_dict[str(self.employe_index)] ]
        # level_4 senior programmer        
        elif 13 < self.employe_index < 41:
            self.immediate_supervisor = self.department_head_set[ self.level_4_dict[str(self.employe_index)] ]     
        # level_5 programmer  
        else:
            self.immediate_supervisor = self.senior_programmer_set[ self.level_5_dict[str(self.employe_index)]]    
   # ------------------------------------------------------------------------------------------------------ 
            


    # Собираем результат
    def main(self):
        self.create_db()
        time.sleep(1)
        for i in range(1,176,1):
            self.index_func()
            self.generate_name(self.employe_index)
            self.generate_position(self.employe_index)
            self.generate_date(self.position)
            self.generate_salary(self.position)
            self.generate_immediate_supervisor(self.employe_index)
            self.insert_date_db(self.name, self.position, self.date,
                                self.salary, self.immediate_supervisor,
                                self.photo_path, '--', '---', 1, '')
        print('done')
        


my_var = Staff()
my_var.main()

