namespace SimpleAuth.Models
{
    public enum SortState
    {
        PositionAsc,   // по должности по возрастанию
        PositionDesc,  // по должности по убыванию
        NameAsc,       // по имени по возрастанию
        NameDesc,      // по имени по убыванию
        DateAsc,       // по дате приёма на работу по возрастанию
        DateDesc,      // по дате приёма на работу по убыванию
        SalaryAsc,     // по зарплате
        SalaryDesc     // по зарплате
    }


    // Для колонки Position нужна пользовательская сортировка
    // ДОДЕЛАТЬ
    public enum SortListPositions
    {
        ГенеральныйДиректор,
        ЗаместительГенеральногоДиректора,
        НачальникОтдела,
        СтаршийПрограммист,
        Программист
    }
}



