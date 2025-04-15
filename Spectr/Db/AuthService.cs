using Spectr.Data;


public class AuthService
{
    private readonly ApplicationContext _context;

    public AuthService(ApplicationContext context)
    {
        _context = context;
    }

    // Метод авторизации
    public object AuthenticateUser(string userName, string password, UserType userType)
    {
        switch (userType)
        {
            case UserType.Администратор:
                return _context.Administrators.FirstOrDefault(a => a.AdministratorLogin == userName && a.AdministratorPassword == password);

            case UserType.Оператор:
                return _context.Operators.FirstOrDefault(o => o.OperatorLogin == userName && o.OperatorPassword == password);

            case UserType.Заказчик:
                return _context.Customers.FirstOrDefault(c => c.CustomerLogin == userName && c.CustomerPassword == password);

            case UserType.Аналитик:
                return _context.Analysts.FirstOrDefault(an => an.AnalystLogin == userName && an.AnalystPassword == password);

            default:
                return null;
        }
    }

}
