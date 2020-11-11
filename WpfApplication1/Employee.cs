namespace WpfApplication1
{
    public abstract class Employee
    {

        #region Конструкторы

        public Employee(string firstName, int id, int salary)
        {
            FirstName = firstName;
            Id = id;
            Salary = salary;
        }

        #endregion
        
        #region Поля

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }
       
        /// <summary>
        /// Уникальный номер
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Зарплата
        /// </summary>
        public int Salary { get; set; }

        /// <summary>
        /// Должность
        /// </summary>
        public abstract string Position { get; set; }
        
        #endregion

        #region Метода

        public void ChangeEmployee(string firstName, int salary)
        {
            FirstName = firstName;
            Salary = salary;
        }

        #endregion
    }
}